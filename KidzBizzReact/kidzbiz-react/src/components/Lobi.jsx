import React from "react";
import { Link, useLocation } from "react-router-dom";
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

export default function Lobi() {
  const location = useLocation();
  console.log("location : ", location.state.user);
  const user = location.state.user;

  return (
    <div className="lobi-container">
      <nav className="navbar">
        <div className="navbar-left">
          <Link to="/game-settings" className="navbar-btn">
            <FontAwesomeIcon icon={faCog} />
          </Link>
          <Link to="/game-guide" className="navbar-btn">
            <FontAwesomeIcon icon={faBook} />
          </Link>
          <Link to="/accessibility" className="navbar-btn">
            <FontAwesomeIcon icon={faUniversalAccess} />
          </Link>
          <Link to="/find-friends" className="navbar-btn">
            <FontAwesomeIcon icon={faSearch} />
          </Link>
        </div>
        <div className="navbar-right">
          <div className="user-info">
            <span>Hello,{user.firstName + " " + user.lastName} </span>
            <div className="avatar-container">
              {" "}
              {/* Add this line */}
              <img src={user.avatarPicture} alt="User avatar" />
            </div>{" "}
            {/* Add this line */}
            <span className="user-rating">36th place</span>
            <i className="fas fa-trophy"></i>
          </div>
        </div>
      </nav>
      <div className="main-buttons">
        <Link to="/playing-alone" className="main-button blue">
          Playing alone <FontAwesomeIcon icon={faFlagCheckered} />
        </Link>
        <Link to="/playing-with-friends" className="main-button green">
          Playing with friends <FontAwesomeIcon icon={faDice} />
        </Link>
        <Link to="/my-achievements" className="main-button purple">
          My achievements <FontAwesomeIcon icon={faTrophy} />
        </Link>
        <Link to="/player-ratings" className="main-button orange">
          Player ratings <FontAwesomeIcon icon={faMedal} />
        </Link>
      </div>
      <div class="money-background"></div>
    </div>
  );
}
