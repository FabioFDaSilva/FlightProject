using Microsoft.AspNetCore.Mvc;
using XmlAPI.Services;
using XmlAPI.Models;

[ApiController]
[Route("api/flights")]
public class FlightsController : ControllerBase
{
    private readonly FlightDataReader _reader;

    var flights;
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
    public IActionResult SearchFlights(string? fromAirport = null, string? toAirport = null, string? fromDate = null, string? toDate = null, string? fromprice = null, string? toprice = null)
    {

        var parsedFromDate = DateTime.TryParse(fromDate, out DateTime tempFromDate) ? parsedFromDate = tempFromDate : null;
        var parsedToDate = DateTime.TryParse(toDate, out DateTime tempToDate) ? parsedToDate = tempToDate : null;

        var parsedFromPrice = decimal.TryParse(fromprice, out decimal tempFromPrice) ? parsedFromPrice = tempFromPrice : null;
        var parsedToPrice = decimal.TryParse(toprice, out decimal tempToPrice) ? parsedToPrice = tempToPrice : null;

        if (flights == null || flights.Count == 0) {
            flights = _reader.LoadFlights();
        }

        var filteredFlights = flights
            .Where(f => string.IsNullOrEmpty(depAir) || f.DepAir.Equals(depAir, StringComparison.OrdinalIgnoreCase))
            .Where(f => string.IsNullOrEmpty(destAir) || f.DestAir.Equals(destAir, StringComparison.OrdinalIgnoreCase))
            .Where(f => !parsedFromDate.HasValue || 
                        DateTime.TryParse(f.InDepartDate, out var fFrom) && fFrom >= parsedFromDate.Value)
            .Where(f => !parsedToDate.HasValue || 
                        DateTime.TryParse(f.OutDepartDate, out var fTo) && fTo <= parsedToDate.Value)
            .Where(f => !parsedFromPrice.HasValue || 
                        decimal.TryParse(f.OriginalPrice, out var pFrom) && pFrom >= parsedFromPrice.Value)
            .Where(f => !parsedToPrice.HasValue || 
                        decimal.TryParse(f.OriginalPrice, out var pTo) && pTo <= parsedToPrice.Value)
            .ToList();

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