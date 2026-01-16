import React, { useEffect, useState } from "react";
import { averagePricePerCarrier } from "../../services/flightService";
import type { CarrierAveragePrice } from "../../types";
import styles from "./averagePricePerCarrier.module.css";
import CarrierPriceCard from "./carrierPriceCard";
import SortBy from "../../components/stats/sortBy";
import Loader from "../../components/loader/loader";


export default function AveragePricePerCarrier() {
    const [carriers, setCarriers] = useState<CarrierAveragePrice[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);

    const [sortOption, setSortOption] = useState("Price"); // default sort

    useEffect(() => {
        averagePricePerCarrier()
            .then(data => setCarriers(data))
            .catch(err => setError(err.message))
            .finally(() => setLoading(false));
    }, []);


    const sortedCarriers = [...carriers].sort((a, b) => {
		if (sortOption === "Price") return a.averagePrice - b.averagePrice;
		if (sortOption === "Name") return a.carrier.localeCompare(b.carrier);
		return 0;
	});


    return (
      <div>
            <SortBy
				options={["Name", "Price"]}
				defaultValue="Price"
				onChange={(selected) => setSortOption(selected)}
                label="Sort by:"
                />
            {error && <p style={{ color: "red" }}>{error}</p>}  
            {!error && !carriers.length && <Loader message="Loading Carriers..." />} 
            {sortedCarriers?.map((c, index) => (
	            <CarrierPriceCard
	            	key={c.carrier}
	            	carrier={c.carrier}
	            	averagePrice={c.averagePrice}
	            	position={index}
                    flightCount={c.flightCount} // optional: highlights top 3 with trophies
	            />
	        ))}
      </div>
    );
}