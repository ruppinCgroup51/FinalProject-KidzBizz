import React, { useState } from "react";
import { useNavigate } from "react-router-dom";

export default function Register() {
  const navigate = useNavigate();
  const [errors, setErrors] = useState({});

  const [formData, setFormData] = useState({
    username: "",
    password: "",
    firstName: "",
    lastName: "",
    avatarPicture: "",
    dateOfBirth: "",
    gender: "",
  });

  function validateForm(formData) {
    const errors = {};

    // Username validation
    if (!formData.username) {
      errors.username = "Username is required";
    } else if (!/^[\u0590-\u05FFa-zA-Z0-9]{3,25}$/.test(formData.username)) {
      errors.username =
        "Username must be in English or Hebrew letters and numbers, a minimum of 3 characters up to 25";
    }

    // Password validation
    if (!formData.password) {
      errors.password = "Password is required";
    } else if (formData.password.length < 8) {
      errors.password = "Password must contain 8 characters or more";
    }

    // First name validation
    if (!formData.firstName) {
      errors.firstName = "First name is required";
    } else if (!/^[\u0590-\u05FFa-zA-Z]{3,25}$/.test(formData.firstName)) {
      errors.firstName =
        "First name must be in English or Hebrew letters, a minimum of 3 characters up to 25";
    }

    // Last name validation
    if (!formData.lastName) {
      errors.lastName = "Last name is required";
    } else if (!/^[\u0590-\u05FFa-zA-Z]{3,25}$/.test(formData.lastName)) {
      errors.lastName =
        "Last name must be in English or Hebrew letters, a minimum of 3 characters up to 25";
    }

    // Date of birth validation
    if (!formData.dateOfBirth) {
      errors.dateOfBirth = "Date of birth is required";
    } else {
      const birthDate = new Date(formData.dateOfBirth);
      const today = new Date();
      const age = today.getFullYear() - birthDate.getFullYear();
      if (age < 14) {
        errors.dateOfBirth = "User must be over 14 years old";
      }
    }

    // Gender validation
    if (!formData.gender) {
      errors.gender = "Gender is required";
    }

    return errors;
  }

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  {
    /*const handleSubmit = (e) => {
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
  */
  }

  const handleSubmit = async (e) => {
    e.preventDefault();

    const validationErrors = validateForm(formData);

    // Set errors in state
    setErrors(validationErrors);

    if (Object.keys(validationErrors).length > 0) {
      return;
    }

    //Navigate to the chooseAvater page
    navigate("/ChooseAvatar", { state: { formData } });
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
          {errors.username && <p style={{ color: "red" }}>{errors.username}</p>}
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
          {errors.password && <p style={{ color: "red" }}>{errors.password}</p>}
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
          {errors.firstName && (
            <p style={{ color: "red" }}>{errors.firstName}</p>
          )}
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
          {errors.lastName && <p style={{ color: "red" }}>{errors.lastName}</p>}
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
          {errors.dateOfBirth && (
            <p style={{ color: "red" }}>{errors.dateOfBirth}</p>
          )}
        </div>
        <div>
          <label htmlFor="gender">Gender:</label>
          <select
            id="gender"
            name="gender"
            value={formData.gender}
            onChange={handleChange}
          >
            <option value="">Select...</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
            <option value="Other">Other</option>
          </select>
          {errors.gender && <p style={{ color: "red" }}>{errors.gender}</p>}
        </div>
        <button class="button-29" type="submit">
          Proceed
        </button>
      </form>
    </div>
  );
}
