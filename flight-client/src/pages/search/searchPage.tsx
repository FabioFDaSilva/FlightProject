import { useState } from "react";
import SearchBar from "../../components/search/searchBar";
import { searchFlights } from "../../services/flightService";
import type { Flight } from "../../types";

export default function SearchPage() {
  const [flights, setFlights] = useState<Flight[]>([]);
  const [error, setError] = useState<string | null>(null);

  const handleSearch = async (params: any) => {
    try {
      setFlights(await searchFlights(params));
      setError(null);
    } catch {
      setError("Failed to load flights");
    }
  };

  return (
    <>
      <SearchBar onSearch={handleSearch} />
      {error && <p>{error}</p>}
      <ul>
        {flights.map(f => (
          <li key={f.id} style={{ marginBottom: '1.5rem', border: '1px solid #ccc', padding: '0.5rem' }}>
            {/* Flight summary */}
            <div>
              <strong>{f.carrier}</strong> {f.depAir} → {f.destAir} | 
              Out: {f.outDepartDate} | In: {f.inDepartDate} | Price: {f.originalPrice} {f.originalCurrency}
            </div>
        
            {/* Segments */}
            <ul style={{ marginLeft: '1rem' }}>
              {f.segments.map((s, index) => (
                <li key={index}>
                  <strong>Segment {index + 1} ({s.journey})</strong>: {s.departCode} ({s.departDate} {s.departTime}) → {s.arrivalCode} ({s.arrivalDate} {s.arrivalTime}) | Class: {s.flightClass} | Flight: {s.flightNo}
                </li>
              ))}
            </ul>
          </li>
        ))}
      </ul>
    </>
  );
}