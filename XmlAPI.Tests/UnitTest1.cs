using Xunit;
using XmlAPI.Controllers;
using XmlAPI.Models;
using System.Collections.Generic;
using System;
using XmlAPI.Services;
using Microsoft.AspNetCore.Mvc;

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

public class FlightControllerTests
{
    [Fact]
    public void FilterFlights_ByAll_Works()
    {
        // Arrange
        var reader = new FlightDataReader(); // or a mocked version
        var controller = new FlightsController(reader);
        // Act
        var result = controller.SearchFlights();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var flights = Assert.IsAssignableFrom<List<Flight>>(okResult.Value);
        Assert.True(flights.Count > 0);
    }

    [Fact]
    public void FilterFlights_FromAirport_Works()
    {
        // Arrange
        var reader = new FlightDataReader(); // or a mocked version
        var controller = new FlightsController(reader);
        // Act
        var result = controller.SearchFlights(fromAirport: "GLA"); //It's important this is a valid airport AND exists in the XML

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var flights = Assert.IsAssignableFrom<List<Flight>>(okResult.Value);
        Assert.True(flights.Count > 0);
    }

    [Fact]
    public void FilterFlights_ToAirport_Works()
    {
        // Arrange
        var reader = new FlightDataReader();
        var controller = new FlightsController(reader);
        // Act
        var result = controller.SearchFlights(toAirport: "GLA"); //It's important this is a valid airport AND exists in the XML

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var flights = Assert.IsAssignableFrom<List<Flight>>(okResult.Value);
        Assert.True(flights.Count > 0);
    }

    [Fact]
    public void FilterFlights_FromDate_Works()
    {
        // Arrange
        var reader = new FlightDataReader();
        var controller = new FlightsController(reader);
        // Act
        var result = controller.SearchFlights(fromDate: "2000-01-01");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var flights = Assert.IsAssignableFrom<List<Flight>>(okResult.Value);
        Assert.True(flights.Count > 0);
    }

    [Fact]
    public void FilterFlights_ToDate_Works()
    {
        // Arrange
        var reader = new FlightDataReader();
        var controller = new FlightsController(reader);
        // Act
        var result = controller.SearchFlights(toDate: "9999-01-01");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var flights = Assert.IsAssignableFrom<List<Flight>>(okResult.Value);
        Assert.True(flights.Count > 0);
    }

    [Fact]
    public void FilterFlights_FromPrice_Works()
    {
        // Arrange
        var reader = new FlightDataReader();
        var controller = new FlightsController(reader);
        // Act
        var result = controller.SearchFlights(fromPrice: "0");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var flights = Assert.IsAssignableFrom<List<Flight>>(okResult.Value);
        Assert.True(flights.Count > 0);
    }

    [Fact]
    public void MostFlightsInYear_Works()
    {
        // Arrange
        var reader = new FlightDataReader();
        var controller = new FlightsController(reader);
        // Act
        var result = controller.MostFlightsInYear("2018"); //Change this to a date you know exists in the XML

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        // Use dynamic to access properties
        dynamic data = okResult.Value;
        
        DateTime[] dates = data.Dates;
        int count = data.MaxCount;
        
        Assert.NotNull(dates);
        Assert.True(dates.Length > 0);
        Assert.True(count > 0);
    }

    [Fact]
    public void AvailableYears_Works()
    {
        // Arrange
        var reader = new FlightDataReader();
        var controller = new FlightsController(reader);
        // Act
        var result = controller.GetYearsAvailable();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var years = Assert.IsAssignableFrom<List<int>>(okResult.Value);
        Assert.True(years.Count > 0);
    }

}

