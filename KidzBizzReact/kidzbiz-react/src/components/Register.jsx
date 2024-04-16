import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Register() {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    username: "",
    password: "",
    firstName: "",
    lastName: "",
    avatarPicture: "",
    dateOfBirth: "",
    gender:""
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  {/*const handleSubmit = (e) => {
    e.preventDefault();
    // Here you can handle form submission, like sending data to a server
    console.log(formData);
    setFormData({
      firstName: "",
      lastName: "",
      birthDate: "",
      userName: "",
      password: "",
      confirmPassword: "",
    });
  */}

    const handleSubmit = async (e) => {
      e.preventDefault();
    
      try {
        const response = await fetch('https://localhost:7034/api/Users', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(formData),
        });
    
        if (response.ok) {
          // Registration successful, you can redirect the user or perform other actions
          console.log('User registered successfully!');
          navigate('/'); // Redirect to home page
        } else {
          // Handle registration error
          console.error('Failed to register user:', response.statusText);
        }
      } catch (error) {
        console.error('Failed to register user:', error.message);
      }
    };

  return (
    <div>
      <button class="button-29" onClick={() => navigate("/")}>
        Back to home page
      </button>
      <h2>Register</h2>
      <form onSubmit={handleSubmit}>
      <div>
          <label htmlFor="username">User Name:</label>
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
          <label htmlFor="firstName">First Name:</label>
          <input
            type="text"
            id="firstName"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="lastName">Last Name:</label>
          <input
            type="text"
            id="lastName"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="avatarPicture">Avatar Picture:</label>
          <input
            type="text"
            id="avatarPicture"
            name="avatarPicture"
            value={formData.avatarPicture}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="dateOfBirth">Date of Birth:</label>
          <input
            type="date"
            id="dateOfBirth"
            name="dateOfBirth"
            value={formData.dateOfBirth}
            onChange={handleChange}
          />
        </div>
        <div>
          <label htmlFor="gender">Gender:</label>
          <input
            type="text"
            id="gender"
            name="gender"
            value={formData.gender}
            onChange={handleChange}
          />
        </div>
        <button class="button-29" type="submit">
          Proceed
        </button>
      </form>
    </div>
  );
}

