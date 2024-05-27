import React, { useEffect, useState } from 'react';
import { FaStar } from 'react-icons/fa';  // Importing star icon
import { useContext } from "react";
import "../css/PlayersRating.css";
import UserContext from "./UserContext";
import { Link } from "react-router-dom";
import { TiArrowLeftThick } from "react-icons/ti";


function UsersScores() {
    const [userScores, setUserScores] = useState([]);
    const user = useContext(UserContext);
    const currentUser = user.username; // This should be dynamically set based on actual user data

    useEffect(() => {
        const fetchData = async () => {
            const setUserApi = () => {
                if (window.location.hostname === "localhost" || window.location.hostname === "127.0.0.1") {
                    return 'https://localhost:7034/api/Users';
                } else {
                    return 'https://proj.ruppin.ac.il/cgroup51/test2/tar1/api/Users';
                }
            };

            const apiUrl = setUserApi();
            try {
                const response = await fetch(apiUrl);
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                const data = await response.json();
                const filteredData = data.filter(user => user.username !== 'PlayerAI');
                filteredData.sort((a, b) => b.score - a.score);
                setUserScores(filteredData);
            } catch (error) {
                console.error('Failed to fetch data:', error);
            }
        };

        fetchData();
    }, []);
    return (
        <>
        <div className="playerRatingsPage"> {/* This is your unique class for this page */}
        <div>
        <Link to="/Lobi" className="arrow-button">
          <TiArrowLeftThick size={70} color="red" />
        </Link>
      </div>
            <div className="scoreContainer">
                {userScores.length > 0 ? (
                    <div>
                        {userScores.map((userScore, index) => (
                            <div key={index} className={`userScore ${userScore.username === currentUser ? 'current' : ''}`}>
                                <span>{userScore.username}</span>
                                <span className="score"><FaStar className="scoreIcon" /> {userScore.score}</span>
                            </div>
                        ))}
                    </div>
                ) : (
                    <p>No user scores available.</p>
                )}
            </div>
        </div>
        </>
    );
    
    
    
    
}

export default UsersScores;



