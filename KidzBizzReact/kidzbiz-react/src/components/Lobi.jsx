import React, { useContext } from "react";
import { Link } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faCog,
  faBook,
  faUniversalAccess,
  faSearch,
  faFlagCheckered,
  faDice,
  faTrophy,
  faMedal,
} from "@fortawesome/free-solid-svg-icons";
import "../css/Lobi.css";
import UserContext from "./UserContext"; // Import the UserContext
import { Avatar, Box, Typography } from "@mui/material";
import EmojiEventsIcon from "@mui/icons-material/EmojiEvents";
import { useNavigate } from 'react-router-dom'; 

export default function Lobi() {
  const user = useContext(UserContext); // Access the user context
  const navigate = useNavigate(); // Use the navigate hook to navigate to a different page

  const handlePlayAlone = async () => {

    if (!user || !user.userId) {
      console.error("User context is missing userId");
      return; // Optionally display an error message to the user
    }
    
    try {
      const response = await fetch('https://localhost:7034/api/GameManagerWithAI/startnewgame', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
          // Include other headers as needed, e.g., authorization tokens
        },
        body: JSON.stringify({
          userId: user.userId
        })
      });

      if (response.ok) {
        // If the server responds that the game has started successfully
        navigate('/GameBoard'); // Navigate to the game board page
      } else {
        throw new Error('Failed to start the game');
      }
    } catch (error) {
      console.error('Error starting the game:', error);
    }
  };

  return (
    <div className="lobi-container">
      <nav className="navbar">
        <div className="navbar-left">
          <Link to="/game-settings" className="navbar-btn gray">
            <FontAwesomeIcon icon={faCog} />
          </Link>
          <Link to="/game-guide" className="navbar-btn darkpurple">
            <FontAwesomeIcon icon={faBook} />
          </Link>
          <Link to="/accessibility" className="navbar-btn">
            <FontAwesomeIcon icon={faUniversalAccess} />
          </Link>
          <Link to="/find-friends" className="navbar-btn deepblue">
            <FontAwesomeIcon icon={faSearch} />
          </Link>
        </div>
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            flexDirection: "row-reverse",
          }}
        >
          <Avatar alt="User avatar" src={user.avatarPicture} sx={{ ml: 2 }} />
          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              alignItems: "flex-end",
            }}
          >
            <Typography variant="h6">{user.username}</Typography>
            <Typography variant="subtitle2" color="text.secondary">
              שלום,
            </Typography>
            <Box
              sx={{
                display: "flex",
                alignItems: "center",
                flexDirection: "row-reverse",
              }}
            >
              <EmojiEventsIcon color="action" />
              <Typography variant="body2">מקום 36</Typography>
            </Box>
          </Box>
        </Box>
      </nav>
      <div className="main-buttons">
         <button onClick={handlePlayAlone} className="main-button blue">
        <FontAwesomeIcon icon={faFlagCheckered} /> משחק לבד
      </button>
        <Link to="/playing-with-friends" className="main-button green">
          <FontAwesomeIcon icon={faDice} /> משחק מול חברים
        </Link>
        <Link to="/MyAchievements" className="main-button purple">
          <FontAwesomeIcon icon={faTrophy} /> ההישגים שלי
        </Link>
        <Link to="/PlayersRating" className="main-button orange">
          <FontAwesomeIcon icon={faMedal} /> דירוגי שחקנים
        </Link>
      </div>
      <div className="money-background"></div>
    </div>
  );
}
