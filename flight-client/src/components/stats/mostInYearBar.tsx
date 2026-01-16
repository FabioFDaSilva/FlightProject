import React, { useEffect, useState } from "react";
import { availableYears } from "../../services/flightService";

type SearchParams = {
  targetYear?: string;
};

type Props = {
  onSearch: (params: SearchParams) => void;
};

export default function SearchBar({ onSearch }: Props) {
  const [years, setYears] = useState<number[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [selectedYear, setSelectedYear] = useState<string>("");

  // Fetch available years on mount
  useEffect(() => {

    async function fetchYears() {
      try {
            setYears(await availableYears());
            setError(null);
          } catch {
            setError("Failed to load available years");
          }
    }

    fetchYears();
  }, []);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSearch({
      targetYear: selectedYear || undefined,
    });
  };



  return (
    <form onSubmit={handleSubmit}>
      <label htmlFor="year">Select Year:</label>
      <select
        id="year"
        value={selectedYear}
        onChange={(e) => setSelectedYear(e.target.value)}
      >
        <option value="">All Years</option>
        {years.map((year) => (
          <option key={year} value={year}>
            {year}
          </option>
        ))}
      </select>

      <button type="submit">Search</button>
    </form>
  );
}