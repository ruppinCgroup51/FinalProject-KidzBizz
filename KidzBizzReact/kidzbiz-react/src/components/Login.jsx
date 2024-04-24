import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Login() {
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
        console.log("User logged in successfully");
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
    <div>
      <button class="button-29" onClick={() => navigate("/")}>
        Back to home page
      </button>
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
          <button class="button-29" type="submit" onClick={handleSubmit}>
            Login
          </button>
          <br />

          <button type="button" onClick={handleForgetPassword}>
            Forget Password
          </button>
        </div>
      </form>
    </div>
  );
}
