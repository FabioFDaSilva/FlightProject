using System;
using XmlAPI.Models;
using System.Collections.Generic;

namespace XmlAPI.Services
{
    public class FlightPrinter
    {
        private readonly FlightDataReader _reader;

        public FlightPrinter(FlightDataReader reader)
        {
            _reader = reader;
        }

        public void PrintFlights()
        {
            List<Flight> flights = _reader.LoadFlights();

            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight ID: {flight.Id} | Carrier: {flight.Carrier} | From: {flight.DepAir} -> To: {flight.DestAir}");
                Console.WriteLine($"  Oneway: {flight.OneWay} | Price: {flight.OriginalPrice} {flight.OriginalCurrency} | Reservation: {flight.Reservation}");
                Console.WriteLine("  Segments:");

                foreach (var seg in flight.Segments)
                {
                    Console.WriteLine($"    Flight: {seg.FlightNo} | {seg.DepartCode} ({seg.DepartDate} {seg.DepartTime}) -> {seg.ArrivalCode} ({seg.ArrivalDate} {seg.ArrivalTime}) | Class: {seg.FlightClass}");
                }

                Console.WriteLine(new string('-', 80)); // separator between flights
            }
        }
    }
}