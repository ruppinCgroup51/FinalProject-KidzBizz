import React from "react";
import { useNavigate } from "react-router-dom";
import "../css/title.css";
import "../css/home.css";

export default function Home() {
  const navigate = useNavigate();
  return (
    <div className="home-container">
      <div className="title-container">
        <h1 className="title">KidzBizz</h1>
      </div>
      <button className="button-29" onClick={() => navigate("/Register")}>
        Register
      </button>
      <br />
      <br />
      <button className="button-29" onClick={() => navigate("/Login")}>
        Login
      </button>
    </div>
  );
}
