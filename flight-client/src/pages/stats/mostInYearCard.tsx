import styles from "./mostInYearCard.module.css";

type Props = {
	date: string | Date; // the most common date
	count: number;       // number of flights on that date
};

export default function MostInYearCard({ date, count }: Props) {
	return (
		<div className={styles.card}>
			<div className={styles.date}>
				{new Date(date).toLocaleDateString(undefined, {
					weekday: "short",
					month: "short",
					day: "numeric",
					year: "numeric"
				})}
			</div>
			<div className={styles.count}>
				{count} flights
			</div>
		</div>
	);
}