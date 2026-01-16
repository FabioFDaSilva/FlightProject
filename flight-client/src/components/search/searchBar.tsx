import React from "react";
import { useState } from "react";
import styles from "../../pages/search/searchBar.module.css";

type SearchParams = {
  fromAirport?: string;
  toAirport?: string;
  fromDate?: string;
  toDate?: string;
  fromPrice?: string;
  toPrice?: string;
};

type Props = {
  onSearch: (params: SearchParams) => void;
};

export default function SearchBar({ onSearch }: Props) {
const [fromAirport, setFromAirport] = useState("");
const [toAirport, setToAirport] = useState("");
const [fromDate, setFromDate] = useState("");
const [toDate, setToDate] = useState("");
const [fromPrice, setFromPrice] = useState("");
const [toPrice, setToPrice] = useState("");

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();   
        onSearch({
            fromAirport: fromAirport || undefined,
            toAirport: toAirport || undefined,
            fromDate: fromDate || undefined,
            toDate: toDate || undefined,
            fromPrice: fromPrice || undefined,
            toPrice: toPrice || undefined,
            });

        
    };  
    return (
      <form className={styles.form} onSubmit={handleSubmit}>
        <div className={styles.row}>
          <input
            placeholder="From airport (IATA code)"
            value={fromAirport}
            onChange={(e) => setFromAirport(e.target.value)}
          />
          <input
            placeholder="To airport (IATA code)"
            value={toAirport}
            onChange={(e) => setToAirport(e.target.value)}
          />
        </div>
    
        <div className={styles.row}>
          <input
            type="date"
            value={fromDate}
            onChange={(e) => setFromDate(e.target.value)}
          />
          <input
            type="date"
            value={toDate}
            onChange={(e) => setToDate(e.target.value)}
          />
        </div>
    
        <div className={styles.row}>
          <input
            placeholder="Min price (£)"
            value={fromPrice}
            onChange={(e) => setFromPrice(e.target.value)}
          />
          <input
            placeholder="Max price (£)"
            value={toPrice}
            onChange={(e) => setToPrice(e.target.value)}
          />
        </div>
    
        <button className={styles.submit} type="submit">
          Search
        </button>
      </form>
    );
}