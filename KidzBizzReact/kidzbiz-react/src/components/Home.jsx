import React from "react";
import { useNavigate } from "react-router-dom";
import "../css/title.css";
import "../css/home.css";
import PersonAddIcon from "@mui/icons-material/PersonAdd";
import { Button } from "@mui/material";
import LoginSharpIcon from "@mui/icons-material/LoginSharp";

export default function Home() {
  const navigate = useNavigate();
  return (
    <div className="home-container">
      <div className="title-container">
        <h1 className="title">KidzBizz</h1>
      </div>
      <br />
      <br />
      <Button
        variant="contained"
        color="error"
        startIcon={<PersonAddIcon />} // Make the icon bigger
        onClick={() => navigate("/Register")}
        sx={{
          fontSize: "30px",
          padding: "20px",
          "&:hover": {
            backgroundColor: "#d32f2f",
            boxShadow: "0 0 10px #d32f2f",
          },
        }}
      >
        הרשמה
      </Button>
      <br />
      <br />

      <Button
        variant="contained"
        color="error"
        startIcon={<LoginSharpIcon />} // Make the icon bigger
        onClick={() => navigate("/Login")}
        sx={{
          fontSize: "30px",
          padding: "20px",
          "&:hover": {
            backgroundColor: "#d32f2f",
            boxShadow: "0 0 10px #d32f2f",
          },
        }}
      >
        התחברות
      </Button>
    </div>
  );
}
