using Microsoft.AspNetCore.Mvc;
using XmlAPI.Services;
using XmlAPI.Models;

[ApiController]
[Route("api/flights")]
public class FlightsController : ControllerBase
{
    private readonly FlightDataReader _reader;

    List<Flight> flights = new List<Flight>();

    public FlightsController(FlightDataReader reader)
    {
        _reader = reader;
    }

    [HttpGet]
    public IActionResult GetFlights()
    {
        if (flights == null || flights.Count == 0) {
            flights = _reader.LoadFlights();
        }
        return Ok(flights);

    }

    [HttpGet("search")]
    public IActionResult SearchFlights(string? fromAirport = null, string? toAirport = null, string? fromDate = null, string? toDate = null, string? fromPrice = null, string? toPrice = null, bool? withSegments = false)
    {

        DateTime? parsedFromDate = DateTime.TryParse(fromDate, out DateTime tempFromDate) ? parsedFromDate = tempFromDate : null;
        DateTime? parsedToDate = DateTime.TryParse(toDate, out DateTime tempToDate) ? parsedToDate = tempToDate : null;

        decimal? parsedFromPrice = decimal.TryParse(fromPrice, out decimal tempFromPrice) ? parsedFromPrice = tempFromPrice : null;
        decimal? parsedToPrice = decimal.TryParse(toPrice, out decimal tempToPrice) ? parsedToPrice = tempToPrice : null;

        if (flights == null || flights.Count == 0) {
            flights = _reader.LoadFlights();
        }
        
        Console.WriteLine($"Flights loaded for search: {flights.Count}");
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

                if (withSegments == true)
                {
                    f.Segments = _reader.GetSegmentsByFlightId(f.Id);
                }
                // Flight passes only if ALL conditions are true
                return matchesFrom && matchesTo && matchesFromDate && matchesToDate &&
                       matchesFromPrice && matchesToPrice;
            })
            .ToList();
        
        Console.WriteLine($"Filtered flights count: {filteredFlights.Count}");
        
        return Ok(filteredFlights);
    }

    [HttpGet("most-flights-day")]
    public IActionResult MostFlightsInYear(string? yearStr = null)
    {
        int? year = null;
        if (!string.IsNullOrEmpty(yearStr) && int.TryParse(yearStr, out int parsedYear))
        {
            year = parsedYear;
        }

        if (flights == null || flights.Count == 0) {
            flights = _reader.LoadFlights();
        }

        var allDepartureDates = flights
        .SelectMany(f =>
        {
            var dates = new List<DateTime>();

            if (DateTime.TryParse(f.InDepartDate, out var inDate))
                dates.Add(inDate);

            if (DateTime.TryParse(f.OutDepartDate, out var outDate))
                dates.Add(outDate);

            return dates;
        });

        // Optionally filter by year
        if (year.HasValue)
            allDepartureDates = allDepartureDates.Where(d => d.Year == year.Value);

        // Group by full date and count
        var mostFlightsDay = allDepartureDates
            .GroupBy(d => d.Date) // group by date only, ignore time
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .FirstOrDefault();

        if (mostFlightsDay == null)
            return NotFound("No flights found for the given criteria.");

        return NotFound("Something went wrong");
    }

    [HttpGet("available-years")]
    public IActionResult GetYearsAvailable() 
    {
        if (flights == null || flights.Count == 0) {
            flights = _reader.LoadFlights();
        }

        var years = new HashSet<int>();

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

        var sortedYears = years.OrderBy(y => y).ToList();
        return Ok(sortedYears);
    }

    [HttpGet("most-common-airports")]
    public IActionResult MostCommonAirports() 
    {
        if (flights == null || flights.Count == 0) {
            flights = _reader.LoadFlights();
        }

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
            .Select(kv => new { Airport = kv.Key, Count = kv.Value })
            .ToList();

        return Ok(mostCommonAirports);
    }

    [HttpGet("avg-price-per-carrier")]
    public IActionResult AveragePricePerCarrier()
    {
        if (flights == null || flights.Count == 0) {
            flights = _reader.LoadFlights();
        }

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