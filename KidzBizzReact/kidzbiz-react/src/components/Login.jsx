import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../css/home.css";
import "../css/Login.css";
import { TiArrowLeftThick } from "react-icons/ti";
import { Link } from "react-router-dom";

export default function Login({ onLogin }) {
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
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await fetch("https://localhost:7034/api/Users/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(formData),
      });
      if (response.ok) {
        const formData = await response.json();
        console.log('userData : ', formData)
        console.log("User logged in successfully");
        onLogin(formData); 
        navigate("/Lobi");
      } else {
        // Handle error response
        const errorData = await response.json();
        console.error("Error:", errorData);
      }
    } catch (error) {
      console.error("Error:", error);
    }

    setFormData({
      userId: 0,
      username: "",
      password: "",
      firstName: "",
      lastName: "",
      avatarPicture: "",
      dateOfBirth: new Date().toISOString(),
      gender: "",
    });
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

      <h2>Login Page</h2>
      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="username">Username:</label>
          <input
            type="text"
            id="username"
            name="username"
            value={formData.username}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="password">Password:</label>
          <input
            type="password"
            id="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
          />
        </div>
        <div>
          <button className="main-button red" type="submit" onClick={handleSubmit}>
            Login
          </button>
          <br />

          <button id="forget-password-button" type="button" className="main-button red" onClick={handleForgetPassword}>
            Forget Password
          </button>
        </div>
      </form>
    </div>
    </>
  );
}
