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

    [HttpGet]
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
}