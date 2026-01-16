import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar from "./components/navbar/navbar";
import SearchPage from "./pages/search/searchPage";
import MostInYearPage from "./pages/stats/mostInYearPage";
import MostCommonAirpotPage from "./pages/stats/mostCommonAirportPage";
import AveragePricePerCarrier from "./pages/stats/averagePricePerCarrier";

function App() {
  return (
    <BrowserRouter>
      <Navbar />

      <Routes>
        <Route path="/" element={<SearchPage />} />
        <Route path="/mostInYear" element={<MostInYearPage />} />
        <Route path="/mostCommon" element={<MostCommonAirpotPage />} />
        <Route path="/avgCarrierPrice" element={<AveragePricePerCarrier />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;