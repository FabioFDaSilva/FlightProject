import styles from "./AIAnswerCard.module.css";

type Props = {
	answer: string;
};

export default function AiAnswerCard({ answer }: Props) {
	return (
		<div className={styles.card}>
			<p>{answer}</p>
		</div>
	);
}