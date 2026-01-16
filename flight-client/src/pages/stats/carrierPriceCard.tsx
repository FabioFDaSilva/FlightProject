import styles from "./carrierPriceCard.module.css";

type Props = {
	carrier: string;
	averagePrice: number;
	position?: number; // optional if you want to highlight top carriers
    flightCount: number;
};

export default function CarrierPriceCard({ carrier, averagePrice, position, flightCount }: Props) {
	// Optional: trophy/star for top positions


	return (
		<div className={styles.card}>
			<div className={styles.header}>
				<span className={styles.name}>{carrier}</span>
			</div>
			<div className={styles.price}>{averagePrice.toFixed(2)} GBP</div>
            <div className={styles.airportCount}>Over {flightCount} Flights</div>
		</div>
	);
}