using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using XmlAPI.Models;

namespace XmlAPI.Services
{
    [XmlRoot("flights", Namespace = "")] // root element in your XML
    public class FlightList
    {
        [XmlElement("flight")] // repeated flight nodes
        public List<Flight> Flights { get; set; } = new();
    }

    

    public class FlightDataReader
    {
        private static List<Flight> _cachedFlights = new();
        
        private static readonly string filePath =
            Path.Combine(AppContext.BaseDirectory, "Data", "flightdata_A.xml");

        private readonly List<Flight> _flights;

        public FlightDataReader()
        {
            _flights = LoadFlights();
        }

        public List<Flight> LoadFlights()
        {

            if (_cachedFlights.Count > 0)
            {
                Console.WriteLine($"Returning cached flights: {_cachedFlights.Count}");
                return _cachedFlights;
            }
            var serializer = new XmlSerializer(typeof(FlightList));

            using var stream = File.OpenRead(filePath);
            var result = (FlightList?)serializer.Deserialize(stream);

            Console.WriteLine($"Flights loaded: {result?.Flights.Count ?? 0}");

            _cachedFlights = result?.Flights ?? new List<Flight>();
            return result?.Flights ?? new List<Flight>();
        }

        public List<Flight> GetAllFlights() => _flights;

        public List<Segment> GetSegmentsByFlightId(string flightId)
        {
            if (string.IsNullOrWhiteSpace(flightId))
                return new List<Segment>();

            var flight = _flights.FirstOrDefault(f => f.Id == flightId);
            return flight?.Segments ?? new List<Segment>();
        }
    }
}