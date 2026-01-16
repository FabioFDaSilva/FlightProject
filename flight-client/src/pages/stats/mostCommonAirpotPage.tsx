import React, { useEffect, useState } from "react";
import { mostCommonAirport } from "../../services/flightService";
import type { AirportCount } from "../../types";



export default function MostCommonAirpotPage() {
const [mostCommonAirports, setMostCommonAirports] = useState<AirportCount | null>(null);
  const [error, setError] = useState<string | null>(null);

  // Fetch available years on mount
  useEffect(() => {

    async function fetchYears() {
      try {
            const data = await mostCommonAirport();
            setMostCommonAirports(data);
            setError(null);
          } catch {
            setError("Failed to load available years");
          }
    }

    fetchYears();
  }, []);


  return (
    <div>
        {error && <p style={{ color: "red" }}>{error}</p>}
    
        {!error && !mostCommonAirports && <p>Loading airports...</p>}
    
        {mostCommonAirports && (
          <ul>
            {mostCommonAirports.airports.map((airport) => (
              <li key={airport.name}>
                {airport.name}: {airport.count} flights
              </li>
            ))}
          </ul>
        )}
    </div>
  );
}