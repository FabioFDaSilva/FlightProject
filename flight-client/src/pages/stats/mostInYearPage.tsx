import { useState } from "react";
import MostInYearBar from "../../components/stats/mostInYearBar";
import { mostInYear } from "../../services/flightService";

import MostInYearCard from "./mostInYearCard";

export default function MostInYear() {
  const [mostCommonDates, setmostCommonDates] = useState<globalThis.Date[]>([]);
  const [totalcount, setTotalcount] = useState<number>(0);
  const [error, setError] = useState<string | null>(null);
  
  const handleSearch = async (params: any) => {

    console.log("Handle search");
    if (params.targetYear === undefined) {
      setError("Please select a year");
      return;
    } 
    try {
      const { dates: datesFromApi, maxCount: flightCount } = await mostInYear(params);
    
      console.log(datesFromApi);
      console.log(flightCount);
      // Convert strings to Date objects
      const parsedDates = datesFromApi.map(d => new Date(d));
      
      setTotalcount(flightCount);
      setmostCommonDates(parsedDates);
      setError(null);
    } catch {
      setError("Failed to load flights");
    }
  };  

  return (
    <>
		  <MostInYearBar onSearch={handleSearch} />
		  {error && <p>{error}</p>}
		  <div>
		  	{mostCommonDates.map((item, index) => (
		  		<MostInYearCard key={index} date={item} count={totalcount} />
		  	))}
		  </div>
	  </>
  );
}