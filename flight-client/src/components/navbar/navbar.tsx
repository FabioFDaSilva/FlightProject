import { NavLink } from "react-router-dom";

export default function Navbar() {
  return (
    <nav>
      <NavLink to="/">Search</NavLink>
      {" | "}
      <NavLink to="/mostInYear">Most Flights In Selected Year</NavLink>
    </nav>
  );
}