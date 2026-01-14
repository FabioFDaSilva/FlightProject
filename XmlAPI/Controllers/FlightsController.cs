using Microsoft.AspNetCore.Mvc;
using XmlAPI.Services;
using XmlAPI.Models;

[ApiController]
[Route("api/flights")]
public class FlightsController : ControllerBase
{
    private readonly FlightDataReader _reader;

    public FlightsController(FlightDataReader reader)
    {
        _reader = reader;
    }

    [HttpGet]
    public IActionResult GetFlights()
    {
        var flights = _reader.LoadFlights(); 
        return Ok(flights);

    }
}