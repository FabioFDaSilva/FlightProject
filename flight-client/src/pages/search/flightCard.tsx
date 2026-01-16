import { useState } from "react";
import type { Flight } from "../../types";
import styles from "./flightCard.module.css";

type Props = {
	flight: Flight;
};

export default function FlightCard ({ flight }: Props) {
	const [open, setOpen] = useState(false);

	const outCount = flight.segments.filter(
        s => s.journey?.toUpperCase() === "OUT"
    ).length;

	const inCount = flight.segments.filter(
	    s => s.journey?.toUpperCase() === "IN"
    ).length;


    const hasSegments = flight.segments && flight.segments.length > 0;


	return (
		<div className={styles.card}>
			<div className={styles.summary}>
				<div>
					<strong>{flight.carrier}</strong>{" "}
					{flight.depAir} → {flight.destAir}
				</div>

				<div className={styles.meta}>
					<span>Out: {flight.outDepartDate}</span>
					<span>In: {flight.inDepartDate}</span>
					<span className={styles.price}>
						{flight.originalPrice} {flight.originalCurrency}
					</span>
				</div>

				{hasSegments && (<button
				    	className={styles.toggle}
				    	onClick={() => setOpen(!open)}
				    >
				    	Segments: OUT {outCount} / IN {inCount}
				    </button>
                )}
			</div>

			{open && (
				<div className={styles.segments}>
					{flight.segments.map((s, index) => (
						<div key={index} className={styles.segment}>
							<strong>{s.journey}</strong><br />
							{s.departCode} → {s.arrivalCode}<br />
							{s.departDate} {s.departTime} → {s.arrivalDate} {s.arrivalTime}<br />
							Class: {s.flightClass} | Flight: {s.flightNo}
						</div>
					))}
				</div>
			)}
		</div>
	);
}