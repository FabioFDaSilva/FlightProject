import { Flight } from "../types";

const API_BASE = "http://localhost:5027/api";

export async function fetchFlights(): Promise<Flight[]> {
  const res = await fetch(`${API_BASE}/flights`);
  if (!res.ok) {
    throw new Error(`API error: ${res.status}`);
  }
  return res.json();
}


type SearchParams = {
    fromAirport?: string;
    toAirport?: string;
    fromDate?: string;
    toDate?: string;
    fromPrice?: string;
    toPrice?: string;
};

export async function searchFlights(params: SearchParams): Promise<Flight[]> {
    const query = new URLSearchParams(
      Object.entries(params).filter(([, v]) => v !== undefined) as [string, string][]
    );
    
    console.log(query.toString());
    const response = await fetch(`${API_BASE}/flights/search?${query.toString()}`);
    
    if (!response.ok) {
      throw new Error("Failed to fetch flights");
    }
    
    return response.json();
}
