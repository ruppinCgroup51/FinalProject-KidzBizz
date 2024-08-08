import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../css/home.css";
import "../css/Login.css";
import { TiArrowLeftThick } from "react-icons/ti";
import { Link } from "react-router-dom";
import { Button } from "@mui/material";
import { PersonAdd as PersonAddIcon } from "@mui/icons-material";
import { LockReset } from "@mui/icons-material";
import TextField from "@mui/material/TextField";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import swal from "sweetalert";

export default function Login({ onLogin }) {
  const theme = createTheme({
    palette: {
      primary: {
        main: "#d32f2f", // Change this to the color you want
      },
    },
  });
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    userId: 0,
    username: "",
    password: "",
    firstName: "",
    lastName: "",
    avatarPicture: "",
    dateOfBirth: new Date().toISOString(),
    gender: "",
    score: 0,
  });

  const handleChange = (event) => {
    setFormData({
      ...formData,
      [event.target.name]: event.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const setUserApi = () => {
      if (
        location.hostname === "localhost" ||
        location.hostname === "127.0.0.1"
      ) {
        return "https://localhost:7034/api/Users/login";
      } else {
        return "https://proj.ruppin.ac.il/cgroup51/test2/tar1/api/Users/login";
      }
    };

    const apiUrl = setUserApi();

    try {
      const response = await fetch(apiUrl, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(formData),
      });
      if (response.ok) {
        // Clear the local storage
        localStorage.removeItem("players");
        // Then perform the login operation...

        const formData = await response.json();
        console.log("userData : ", formData);
        console.log("User logged in successfully");
        onLogin(formData);
        navigate("/Lobi");
      } else {
        // Handle error response
        const errorData = await response.json();
        console.error("Error:", errorData);
        swal({
          title: "שגיאה",
          text: "אחד או יותר מהפרטים שהזנת שגויים. אנא נסה שוב.",
          icon: "error",
          dangerMode: true,
        }) // Display an error popup
          .then(() => {
            setFormData({
              userId: 0,
              username: "",
              password: "",
              firstName: "",
              lastName: "",
              avatarPicture: "",
              dateOfBirth: new Date().toISOString(),
              gender: "",
            }); // Reset the form
          });
      }
    } catch (error) {
      console.error("Error:", error);
      swal({
        title: "שגיאה",
        text: "אחד או יותר מהפרטים שהזנת שגויים. אנא נסה שוב.",
        icon: "error",
        dangerMode: true,
      }) // Display an error popup
        .then(() => {
          setFormData({
            userId: 0,
            username: "",
            password: "",
            firstName: "",
            lastName: "",
            avatarPicture: "",
            dateOfBirth: new Date().toISOString(),
            gender: "",
            score: 0,
          }); // Reset the form
        });
    }
  };

  const handleForgetPassword = () => {
    // Here you can handle the forget password action
    console.log("Forget password clicked");
  };

  return (
    <>
      <div>
        <Link to="/" className="arrow-button">
          <TiArrowLeftThick size={70} color="red" />
        </Link>
      </div>
      <div className="main-div">
        <h2 className="header">התחברות</h2>
        <br />
        <form className="form" onSubmit={handleSubmit}>
          <ThemeProvider theme={theme}>
            <TextField
              name="username"
              id="username"
              label="שם משתמש"
              variant="standard"
              value={formData.username}
              onChange={handleChange}
              sx={{
                fontSize: "16px",
                width: "20%",
              }}
            />
            <br />
            <ThemeProvider theme={theme}>
              <TextField
                name="password"
                id="password"
                label="סיסמא"
                type="password"
                variant="standard"
                value={formData.password}
                onChange={handleChange}
                sx={{
                  fontSize: "16px",
                  width: "20%",
                }}
              />
            </ThemeProvider>

            <br />
            <br />

            <Button startIcon={<LockReset />} onClick={handleForgetPassword}>
              שחזור סיסמא
            </Button>
          </ThemeProvider>
          <div>
            <br />
            <Button
              variant="contained"
              color="error"
              startIcon={<PersonAddIcon sx={{ fontSize: 40 }} />}
              type="submit"
              onClick={handleSubmit}
              sx={{
                fontSize: "20px",
                padding: "20px",
                "&:hover": {
                  backgroundColor: "#d32f2f",
                  boxShadow: "0 0 10px #d32f2f",
                },
              }}
            >
              התחבר
            </Button>
          </div>
        </form>
      </div>
    </>
  );
}
