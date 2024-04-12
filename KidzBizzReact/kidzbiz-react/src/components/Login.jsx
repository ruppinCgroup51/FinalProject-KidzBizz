import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Login() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    username: "",
    password: "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // Here you can handle form submission, like sending data to a server
    console.log(formData);
    // Optionally, you can clear the form fields after submission
    // setFormData({ username: '', password: '' });
    setFormData({
      username: "",
      password: "",
    });
  };

  const handleForgetPassword = () => {
    // Here you can handle the forget password action
    console.log("Forget password clicked");
  };

  return (
    <div>
      <button onClick={() => navigate("/")}>Back to home page</button>
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
          <button type="submit">Login</button>
          <br />

          <button type="button" onClick={handleForgetPassword}>
            Forget Password
          </button>
        </div>
      </form>
    </div>
  );
}
