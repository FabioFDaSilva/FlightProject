import { Flight } from "../types";

const API_BASE = "http://localhost:5027/api";

export async function fetchFlights(): Promise<Flight[]> {
  const res = await fetch(`${API_BASE}/flights`);
  if (!res.ok) {
    throw new Error(`API error: ${res.status}`);
  }
  return res.json();
}