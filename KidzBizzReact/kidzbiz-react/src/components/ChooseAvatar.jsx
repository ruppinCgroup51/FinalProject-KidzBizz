import React, { useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import "../css/ChooseAvatar.css";

const ChooseAvatar = () => {
  const [selectedAvatar, setSelectedAvatar] = useState(null);
  const navigate = useNavigate();
  const location = useLocation();
  const formData = location.state.formData;

  const avatars = [
    "https://robohash.org/avatar1",
    "https://robohash.org/avatar2",
    "https://robohash.org/avatar3",
    "https://robohash.org/avatar4",
    "https://robohash.org/avatar5",
    "https://robohash.org/avatar6",
    "https://robohash.org/avatar7",
    "https://robohash.org/avatar8",
    "https://robohash.org/avatar9",
  ];

  const handleAvatarClick = (avatar) => {
    setSelectedAvatar(avatar);
  };

  const handleSubmit = async () => {
    // Add avatar to form data
    console.log("Form data:", formData);
    formData.AvatarPicture = selectedAvatar;

    
  const setUserApi = () => {
    if (location.hostname === "localhost" || location.hostname === "127.0.0.1") {
      return "https://localhost:7034/api/Users";
    } else {
      return 'https://proj.ruppin.ac.il/cgroup51/test2/tar1/api/Users';
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
        console.log("User registered successfully!");
        navigate("/"); // Redirect to home page
      } else {
        console.error("Failed to register user:", response.statusText);
      }
    } catch (error) {
      console.error("Failed to register user:", error.message);
    }
  };

  return (
    <div className="avatar-selection">
      <h2>Select an Avatar</h2>
      <div className="avatar-grid">
        {avatars.map((avatar, index) => (
          <div
            key={index}
            className={`avatar-item ${
              avatar === selectedAvatar ? "selected" : ""
            }`}
            onClick={() => handleAvatarClick(avatar)}
          >
            <img src={avatar} alt={`Avatar ${index + 1}`} />
          </div>
        ))}
      </div>
      <button className="submit-button" onClick={handleSubmit}>
        Submit
      </button>
    </div>
  );
};

export default ChooseAvatar;
