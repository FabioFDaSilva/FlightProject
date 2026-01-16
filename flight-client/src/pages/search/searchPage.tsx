import { useState } from "react";
import SearchBar from "../../components/search/searchBar";
import { searchFlights } from "../../services/flightService";
import type { Flight } from "../../types";
import FlightCard from "./flightCard";
import Loader from "../../components/loader/loader";

export default function SearchPage() {
  const [flights, setFlights] = useState<Flight[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleSearch = async (params: any) => {
    setLoading(true);
    searchFlights(params)
      .then(data => setFlights(data))
      .catch(err => setError(err.message))
      .finally(() => setLoading(false));
  };

  return (
    <>
      <SearchBar onSearch={handleSearch} />
      {loading && <Loader message="Searching flights..." />}
      {error && <p>{error}</p>}
      <ul>
        {flights.map(f => (
					<FlightCard key={f.id} flight={f} />
				))}
      </ul>
    </>
  );
}