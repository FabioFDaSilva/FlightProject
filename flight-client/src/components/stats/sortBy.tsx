import { useState } from "react";
import styles from "./sortBy.module.css";

type SortByProps = {
	options: string[]; // array of sorting options, e.g. ["Name", "Price", "Flights"]
	onChange: (selected: string) => void;
	defaultValue?: string;
    label?: string;
};

export default function SortBy({ options, onChange, defaultValue, label }: SortByProps) {
	const [selected, setSelected] = useState(defaultValue || options[0]);

	const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
		setSelected(e.target.value);
		onChange(e.target.value);
	};

	return (
		<div className={styles.container}>
            {label && <span className={styles.label}>{label}</span>}
			<select value={selected} onChange={handleChange} className={styles.select}>
				{options.map((option) => (
					<option key={option} value={option}>
						{option}
					</option>
				))}
			</select>
		</div>
	);
}