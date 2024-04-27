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
import UserContext from "./UserContext" // Import the UserContext

export default function Lobi() {
  const user = useContext(UserContext); // Access the user context

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
        <div className="navbar-right">
          <div className="user-info">
            <span>{user.username} ,שלום</span>
            <div className="avatar-container">
              <img src={user.avatarPicture} alt="User avatar" />
            </div>
            <span className="user-rating"> <FontAwesomeIcon icon={faTrophy} /> מקום 36 </span>
            <i className="fas fa-trophy"></i>
          </div>
        </div>
      </nav>
      <div className="main-buttons">
        <Link to="/GameBoard" className="main-button blue">
          <FontAwesomeIcon icon={faFlagCheckered} /> משחק לבד 
        </Link>
        <Link to="/playing-with-friends" className="main-button green">
        <FontAwesomeIcon icon={faDice} /> משחק מול חברים
        </Link>
        <Link to="/my-achievements" className="main-button purple">
        <FontAwesomeIcon icon={faTrophy} /> ההישגים שלי 
        </Link>
        <Link to="/player-ratings" className="main-button orange">
       <FontAwesomeIcon icon={faMedal} /> דירוגי שחקנים 
        </Link>
      </div>
      <div className="money-background"></div>
    </div>
  );
}
