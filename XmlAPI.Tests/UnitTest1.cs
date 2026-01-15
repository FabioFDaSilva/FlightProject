using Xunit;
using XmlAPI.Services;

namespace XmlAPI.Tests;

public class FlightDataReaderTests
{
    [Fact]
    public void LoadFlights_ShouldReturnFlightsList()
    {
        // Arrange
        var reader = new FlightDataReader();        // Act
        var flights = reader.LoadFlights();        // Assert
        Assert.NotNull(flights); // Should return a list
        Assert.True(flights.Count > 0); // Should contain at least one flight
    }    
    [Fact]
    public void GetSegmentsByFlightId_ShouldReturnSegments()
    {
        // Arrange
        var reader = new FlightDataReader();
        var flightId = "1803064"; // known ID in your XML        // Act
        var segments = reader.GetSegmentsByFlightId(flightId);        // Assert
        Assert.NotNull(segments);
        Assert.True(segments.Count > 0); // Flight should have segments
    }


}