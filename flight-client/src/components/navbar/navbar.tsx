import { NavLink } from "react-router-dom";
import "../../styles/navbar.css";

export default function Navbar() {
  return (
    <nav className="navbar">
      <div className="navbar-inner">
        <NavLink to="/">Search</NavLink>
        <NavLink to="/mostInYear">Most Flights In Selected Year</NavLink>
        <NavLink to="/mostCommon">Most Common Airports</NavLink>
        <NavLink to="/avgCarrierPrice">Average Price Per Carrier</NavLink>
      </div>
    </nav>
  );
}