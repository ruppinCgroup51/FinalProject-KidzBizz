import React from "react";
import { useNavigate } from "react-router-dom";
import "../css/title.css";

export default function Home() {
  const navigate = useNavigate();
  return (
    <>
      <div class="title-container">
        <h1 class="title">KidzBizz</h1>
      </div>
      <button class="button-29" onClick={() => navigate("/Register")}>
        Register
      </button>
      <br />
      <br />
      <button class="button-29" onClick={() => navigate("/Login")}>
        Login
      </button>
    </>
  );
}
