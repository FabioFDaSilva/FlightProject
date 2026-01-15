
import React, { useEffect, useState } from "react";
import { Flight } from "../../types";
import { fetchFlights } from "../../services/flightService";
// import Stats from "../pages/stats/statspage";



export default function AllFlights() {
  const [flights, setFlights] = useState<Flight[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchFlights()
      .then(data => setFlights(data))
      .catch(err => setError(err.message))
      .finally(() => setLoading(false));
  }, []);

  if (loading) return <div>Loading flights...</div>;
  if (error) return <div>Error: {error}</div>;
  
  return (
      <div>
        <h1>Flights</h1>
        {flights.map(flight => (
          <div key={flight.id} style={{ border: "1px solid #ccc", margin: "1rem", padding: "1rem" }}>
            <h2>{flight.carrier} ({flight.depAir} → {flight.destAir})</h2>
            <p>Reservation: {flight.reservation} | Price: {flight.originalPrice} {flight.originalCurrency}</p>
            <h3>Segments:</h3>
            <ul>
              {flight.segments.map(seg => (
                <li key={seg.flightNo}>
                  {seg.flightNo}: {seg.departCode} ({seg.departDate} {seg.departTime}) → {seg.arrivalCode} ({seg.arrivalDate} {seg.arrivalTime}) | Class: {seg.flightClass}
                </li>
              ))}
            </ul>
          </div>
        ))}
      </div>
    );
}