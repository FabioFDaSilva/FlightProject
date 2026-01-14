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
        private const string filePath = "Data/flightdata_A.xml";

        public List<Flight> LoadFlights()
        {
            var serializer = new XmlSerializer(typeof(FlightList));

            using var stream = File.OpenRead(filePath);
            var result = (FlightList?)serializer.Deserialize(stream);

            Console.WriteLine($"Flights loaded: {result?.Flights.Count ?? 0}");
            return result?.Flights ?? new List<Flight>();
        }
    }
}