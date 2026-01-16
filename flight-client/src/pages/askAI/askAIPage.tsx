import { useState } from "react";
import Loader from "../../components/loader/loader";
import AIAnswerCard from "./AIAnswerCard";
import styles from "./askAIPage.module.css";
import { askAI } from "../../services/flightService";

export default function AskAIPage() {
	const [question, setQuestion] = useState("");
	const [answer, setAnswer] = useState("");
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState("");

	const handleAsk = async () => {
		if (!question.trim()) return;

		setLoading(true);
		setError("");
		setAnswer("");

		askAI(question)
        .then((data) => { setAnswer(data.answer); })
        .catch((err) => { setError(err.message); })
        .finally(() => { setLoading(false); });
	};

	return (
		<div className={styles.container}>
			<h2>AI Flight Assistant</h2>

			<div className={styles.inputGroup}>
				<input
					type="text"
					placeholder="Ask a question about the flights..."
					value={question}
					onChange={(e) => setQuestion(e.target.value)}
					onKeyDown={(e) => e.key === "Enter" && handleAsk()}
				/>
				<button onClick={handleAsk}>Ask AI</button>
			</div>

			{loading && <Loader message="Thinking..." />}
			{error && <p className={styles.error}>{error}</p>}
			{answer && <AIAnswerCard answer={answer} />}
		</div>
	);
}