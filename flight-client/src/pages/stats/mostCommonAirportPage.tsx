import React, { useEffect, useState } from "react";
import { mostCommonAirport } from "../../services/flightService";
import type { AirportCount } from "../../types";
import MostCommonAirportCard from "./mostCommonAirportCard";

export default function MostCommonAirpotPage() {
const [mostCommonAirports, setMostCommonAirports] = useState<AirportCount | null>(null);
  const [error, setError] = useState<string | null>(null);

  // Fetch available years on mount
  useEffect(() => {

    async function fetchCommonAirports() {
      try {
            const data = await mostCommonAirport();
            setMostCommonAirports(data);
            setError(null);
          } catch {
            setError("Failed to load available years");
          }
    }

    fetchCommonAirports();
  }, []);


  return (
    <div>
        {error && <p style={{ color: "red" }}>{error}</p>}

        {!error && !mostCommonAirports && <p>Loading airports...</p>}

        <div>
        	{mostCommonAirports?.airports.map((airport, index) => (
        		<MostCommonAirportCard
        			key={airport.name}
        			position={index}
        			name={airport.name}
        			count={airport.count}
        		/>
        	))}
        </div>
    </div>
  );
}