import React, { useEffect, useState } from "react";
import { Flight } from "./types";
import { fetchFlights } from "./services/flightService";
import { searchFlights } from "./services/flightService";

import { BrowserRouter, Routes, Route } from "react-router-dom";
import Navbar from "./components/navbar/navbar";
import SearchPage from "./pages/search/searchPage";
import MostInyearPage from "./pages/stats/mostInYearPage";
import AllFlights from "./pages/allFlights/allFlights";
// import Stats from "../pages/stats/statspage";



function App() {
  return (
    <BrowserRouter>
      <Navbar />

      <Routes>
        <Route path="/" element={<SearchPage />} />
        <Route path="/mostInYear" element={<MostInyearPage />} />
        {/* <Route path="/stats" element={<StatsPage />} /> */}
      </Routes>
    </BrowserRouter>
  );
}

export default App;