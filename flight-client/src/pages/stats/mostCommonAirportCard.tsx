import styles from "./mostCommonAirportCard.module.css";

type Props = {
	position: number; // 0-based index
	name: string;
	count: number;
};

export default function MostCommonAirportCard({ position, name, count }: Props) {
	// Determine trophy
	let trophy = "⭐"; // default
	if (position === 0) trophy = "🥇";
	else if (position === 1) trophy = "🥈";
	else if (position === 2) trophy = "🥉";

	return (
		<div className={styles.card}>
			<div className={styles.header}>
				<span className={styles.trophy}>{trophy}</span>
				<span className={styles.name}>{name}</span>
			</div>
			<div className={styles.count}>{count} flights</div>
		</div>
	);
}