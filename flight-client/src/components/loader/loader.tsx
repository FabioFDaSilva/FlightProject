import styles from "./loader.module.css";

type LoaderProps = {
	message?: string;
};

export default function Loader({ message = "Loading..." }: LoaderProps) {
	return (
		<div className={styles.container}>
			<div className={styles.spinner}></div>
			<p className={styles.message}>{message}</p>
		</div>
	);
}