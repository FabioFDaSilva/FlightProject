import { useState } from "react";
import SearchBar from "../../components/search/searchBar";
import { searchFlights } from "../../services/flightService";
import type { Flight } from "../../types";
import FlightCard from "./flightCard";

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
					<FlightCard key={f.id} flight={f} />
				))}
      </ul>
    </>
  );
}