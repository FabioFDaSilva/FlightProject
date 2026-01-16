import { AirportCount, Flight, CarrierAveragePrice } from "../types";

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


type YearSearchParams = {
  targetYear: string;
};

interface MostInYearResponse {
  dates: string[];
  maxCount: number;
}

export async function mostInYear(params: YearSearchParams): Promise<MostInYearResponse> {

  const res = await fetch(`${API_BASE}/flights/most-flights-day?targetYear=${params.targetYear}`);
  if (!res.ok) throw new Error("API request failed");
  return res.json();
}

export async function availableYears(): Promise<number[]> {
  
  console.log("Fetching available years");
  const response = await fetch(`${API_BASE}/flights/available-years`);
  
  if (!response.ok) {
    throw new Error("Failed to fetch years");
  }
  
  return response.json();
}



export async function mostCommonAirport(): Promise<AirportCount> {
  
  console.log("Fetching Most Common Airports");
  const response = await fetch(`${API_BASE}/flights/most-common-airports`);
  
  if (!response.ok) {
    throw new Error("Failed to fetch airports");
  }
  
  return response.json();
}

export async function averagePricePerCarrier(): Promise<CarrierAveragePrice[]> {
  
  console.log("Fetching Average Prices");
  const response = await fetch(`${API_BASE}/flights/avg-price-per-carrier`);
  
  if (!response.ok) {
    throw new Error("Failed to fetch carriers");
  }
  
  return response.json();
}

export async function askAI(query: string): Promise<{ answer: string }> {
  
  console.log("Asking AI: " + query);
  const response = await fetch(`${API_BASE}/flights/ask-ai`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ query }),
  });

  if (!response.ok) {
    throw new Error("Failed to get AI answer");
  }

  return response.json();

}





