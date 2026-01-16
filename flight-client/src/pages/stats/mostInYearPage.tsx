import { useState } from "react";
import MostInYearBar from "../../components/stats/mostInYearBar";
import { mostInYear } from "../../services/flightService";
import Loader from "../../components/loader/loader";

import MostInYearCard from "./mostInYearCard";

export default function MostInYear() {
  const [mostCommonDates, setmostCommonDates] = useState<globalThis.Date[]>([]);
  const [totalcount, setTotalcount] = useState<number>(0);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  
  const handleSearch = async (params: any) => {
    if (params.targetYear === undefined) {
      setError("Please select a year");
      return;
    } 

    setLoading(true);
    mostInYear(params)
      .then(data => {
        const { dates: datesFromApi, maxCount: flightCount } = data;
        // Convert strings to Date objects
        const parsedDates = datesFromApi.map(d => new Date(d));
        setTotalcount(flightCount);
        setmostCommonDates(parsedDates);
        setError(null);
      }
      )
      .catch(err => setError(err.message))
      .finally(() => setLoading(false));
  };  

  return (
    <>
		  <MostInYearBar onSearch={handleSearch} />
		  {error && <p>{error}</p>}
      {loading && <Loader message="Loading most common dates..." />}
		  <div>
		  	{mostCommonDates.map((item, index) => (
		  		<MostInYearCard key={index} date={item} count={totalcount} />
		  	))}
		  </div>
	  </>
  );
}