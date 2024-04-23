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

  const handleSubmit = async (e) => {
    e.preventDefault();
  {/* setFormData((prevState) => ({
      ...prevState, 
      
    }))*/}
     try {
     {/*משהו פה בחיבור לא עובד, שמדבגים דרך הסוואגר השרת עובד טוב, אני לא מצליחה להבין מה הבעיה עם הקליינט */}
      const response = await fetch('https://localhost:7034/api/Users/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(formData),
      });
      if (response.ok) {
        // User created successfully
        console.log('User loged in successfully');
        navigate("/Lobi");
        // You can redirect the user to another page here if needed
      } else {
        // Handle error response
        console.error('Failed to login');
      }
    } catch (error) {
      // Handle network or other errors
      console.error('Error:', error);
    }

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
      <button class="button-29" onClick={() => navigate("/")}>Back to home page</button>
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
          <button class="button-29" type="submit">Login</button>
          <br />

          <button type="button" onClick={handleForgetPassword}>
            Forget Password
          </button>
        </div>
      </form>
    </div>
  );
}
