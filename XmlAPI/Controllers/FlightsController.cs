using Microsoft.AspNetCore.Mvc;
using XmlAPI.Services;
using XmlAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace XmlAPI.Controllers
{
    [ApiController]
    [Route("api/flights")]
    public class FlightsController : ControllerBase
    {
        private readonly FlightDataReader _reader;


        // Caching
        private static List<Flight> cachedFlights = new List<Flight>();
        private static List<int> cachedAvailableYears = new List<int>();
        
        private List<Flight> flights = new List<Flight>();

        public FlightsController(FlightDataReader reader)
        {
            _reader = reader;
        }


        private List<Flight> CacheFlights () 
        {
            flights = _reader.LoadFlights();
            cachedFlights = flights;

            return flights;
        }

        [HttpGet("getAll")]
        public IActionResult GetFlights()
        {

            flights = cachedFlights.Count > 0 ? cachedFlights : CacheFlights();

            return Ok(flights);

        }

        [HttpGet("search")]
        public IActionResult SearchFlights(string? fromAirport = null, string? toAirport = null, string? fromDate = null, string? toDate = null, string? fromPrice = null, string? toPrice = null)
        {

            DateTime? parsedFromDate = DateTime.TryParse(fromDate, out DateTime tempFromDate) ? parsedFromDate = tempFromDate : null;
            DateTime? parsedToDate = DateTime.TryParse(toDate, out DateTime tempToDate) ? parsedToDate = tempToDate : null;

            decimal? parsedFromPrice = decimal.TryParse(fromPrice, out decimal tempFromPrice) ? parsedFromPrice = tempFromPrice : null;
            decimal? parsedToPrice = decimal.TryParse(toPrice, out decimal tempToPrice) ? parsedToPrice = tempToPrice : null;

            flights = cachedFlights.Count > 0 ? cachedFlights : CacheFlights();


            var filteredFlights = flights
                .Where(f =>
                {
                    // Airport filters
                    bool matchesFrom = string.IsNullOrEmpty(fromAirport) || 
                                       f.DepAir.Equals(fromAirport, StringComparison.OrdinalIgnoreCase);
                    bool matchesTo = string.IsNullOrEmpty(toAirport) || 
                                     f.DestAir.Equals(toAirport, StringComparison.OrdinalIgnoreCase);

                    // Date filters
                    bool matchesFromDate = true;
                    if (parsedFromDate.HasValue)
                    {
                        matchesFromDate = DateTime.TryParse(f.InDepartDate, out var fFrom) &&
                                          fFrom >= parsedFromDate.Value;
                    }

                    bool matchesToDate = true;
                    if (parsedToDate.HasValue)
                    {
                        matchesToDate = DateTime.TryParse(f.OutDepartDate, out var fTo) &&
                                        fTo <= parsedToDate.Value;
                    }

                    if (matchesFrom == true && matchesTo == true && matchesToDate == false)
                    {
                        Console.WriteLine(
                            $"Flight {f.Id} matches airport criteria. matchesToDate={matchesToDate}, " +
                            $"OutDepartDate='{f.OutDepartDate}', parsedToDate={parsedToDate?.ToString("yyyy-MM-dd")}"
                        );
                    }
                    // Price filters
                    bool matchesFromPrice = true;
                    if (parsedFromPrice.HasValue)
                    {
                        matchesFromPrice = decimal.TryParse(f.OriginalPrice, out var pFrom) &&
                                           pFrom >= parsedFromPrice.Value;
                    }

                    bool matchesToPrice = true;
                    if (parsedToPrice.HasValue)
                    {
                        matchesToPrice = decimal.TryParse(f.OriginalPrice, out var pTo) &&
                                         pTo <= parsedToPrice.Value;
                    }

                    // Flight passes only if ALL conditions are true
                    return matchesFrom && matchesTo && matchesFromDate && matchesToDate &&
                           matchesFromPrice && matchesToPrice;
                })
                .ToList();

            Console.WriteLine($"Filtered flights count: {filteredFlights.Count}");

            return Ok(filteredFlights);
        }


        public class MostFlightsResult
        {
            public DateTime[] Dates { get; set; } = Array.Empty<DateTime>();
            public int MaxCount { get; set; }
        }
        [HttpGet("most-flights-day")]
        public IActionResult MostFlightsInYear(string targetYear)
        {
            
            Console.WriteLine($"Received targetYear: {targetYear}");
        
            if (string.IsNullOrEmpty(targetYear) || !int.TryParse(targetYear, out int year))
                return BadRequest("Invalid or missing year parameter.");
        
            // Use cached flights if available
            if (cachedFlights.Count > 0)
                flights = cachedFlights;
            else
                CacheFlights();
        
            // Collect all departure dates
            var allDepartureDates = flights
                .SelectMany(f =>
                {
                    var dates = new List<DateTime>();
                    if (DateTime.TryParse(f.InDepartDate, out var inDate)) dates.Add(inDate);
                    if (DateTime.TryParse(f.OutDepartDate, out var outDate)) dates.Add(outDate);
                    return dates;
                })
                .Where(d => d.Year == year); // filter by requested year
        
            if (!allDepartureDates.Any())
                return NotFound("No flights found for the given year.");
        
            // Group by date and count flights
            var groupedDates = allDepartureDates
                .GroupBy(d => d.Date)                // group by date only
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToList();                           // evaluate once
            
            var maxCount = groupedDates.Max(g => g.Count);  // find the maximum count
            
            var mostFlightsDays = groupedDates
                .Where(g => g.Count == maxCount)    // get all dates with the max count
                .Select(g => g.Date)                // just return the date
                .ToArray();            

            Console.WriteLine($"Most flights days in {year}: {string.Join(", ", mostFlightsDays.Select(d => d.ToString("yyyy-MM-dd")))} with {maxCount} flights.");
            return Ok(new MostFlightsResult
            {
                Dates = mostFlightsDays,
                MaxCount = maxCount
            });
        }

        [HttpGet("available-years")]
        public IActionResult GetYearsAvailable() 
        {
            flights = cachedFlights.Count > 0 ? cachedFlights : CacheFlights();


            var years = new HashSet<int>();

            Console.WriteLine($"avaibleYears count: {cachedAvailableYears.Count}");
            if (cachedAvailableYears.Count > 0)
            {
                Console.WriteLine($"Returning cached available years: {string.Join(", ", cachedAvailableYears)}");

                return Ok(cachedAvailableYears);
            }


            foreach (var flight in flights)
            {
                if (DateTime.TryParse(flight.InDepartDate, out var inDate))
                {
                    years.Add(inDate.Year);
                }

                if (DateTime.TryParse(flight.OutDepartDate, out var outDate))
                {
                    years.Add(outDate.Year);
                }
            }

            Console.WriteLine($"Available years: {string.Join(", ", years)}");
            var sortedYears = years.OrderBy(y => y).ToList();

            //Lets cache this result
            cachedAvailableYears = sortedYears;
            return Ok(sortedYears);
        }


        public class AirportCount
        {
            public string Name { get; set; } = string.Empty; // IATA code
            public int Count { get; set; }
        }


        public class MostCommonAirportsResponse
        {
            public List<AirportCount> Airports { get; set; } = new List<AirportCount>();
        }

        [HttpGet("most-common-airports")]
        public IActionResult MostCommonAirports() 
        {
            flights = cachedFlights.Count > 0 ? cachedFlights : CacheFlights();

            var airportCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var flight in flights)
            {
                // Count departure airport
                if (airportCounts.ContainsKey(flight.DepAir))
                    airportCounts[flight.DepAir]++;
                else
                    airportCounts[flight.DepAir] = 1;

                // Count destination airport
                if (airportCounts.ContainsKey(flight.DestAir))
                    airportCounts[flight.DestAir]++;
                else
                    airportCounts[flight.DestAir] = 1;
            }

            var mostCommonAirports = airportCounts
                .OrderByDescending(kv => kv.Value)
                .Take(5)
                .Select(kv => new AirportCount{
                    Name = kv.Key,
                    Count = kv.Value
                })
                .ToList();

            return Ok(new MostCommonAirportsResponse
            {
                Airports = mostCommonAirports
            });
        }

        [HttpGet("avg-price-per-carrier")]
        public IActionResult AveragePricePerCarrier()
        {
            flights = cachedFlights.Count > 0 ? cachedFlights : CacheFlights();

            var carrierPrices = new Dictionary<string, List<decimal>>(StringComparer.OrdinalIgnoreCase);

            foreach (var flight in flights)
            {
                if (decimal.TryParse(flight.OriginalPrice, out var price))
                {
                    if (!carrierPrices.ContainsKey(flight.Carrier))
                    {
                        carrierPrices[flight.Carrier] = new List<decimal>();
                    }
                    carrierPrices[flight.Carrier].Add(price);
                }
            }

            var avgPrices = carrierPrices
                .Select(kv => new 
                { 
                    Carrier = kv.Key, 
                    AveragePrice = kv.Value.Average() 
                })
                .OrderBy(x => x.Carrier)
                .ToList();

            return Ok(avgPrices);
        }

    }
}
