import React, { useState, useContext, useEffect } from "react";
import UserContext from "./UserContext";
import { GameSquare } from "./GameSquare";
import "../css/gameboard.css";
import { faDice } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export default function GameBoard() {
  const numSquares = Array.from({ length: 40 }, (_, i) => i + 1);
  const user = useContext(UserContext);
  const [players, setPlayers] = useState([]);
  const [currentPlayerIndex, setCurrentPlayerIndex] = useState(0);

  useEffect(() => {
    const fetchData = async () => {
      if (!user || !user.userId) {
        console.error("User context is missing userId");
        return; // Optionally display an error message to the user
      }

      try {
        const response = await fetch(
          "https://localhost:7034/api/GameManagerWithAI/startnewgame",
          {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify(user),
          }
        );

        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        setPlayers(data);
        console.log(data);

        // Store the players array in local storage
        localStorage.setItem("players", JSON.stringify(data));
      } catch (error) {
        console.error("Error:", error);
      }
    };

    const storedPlayers = localStorage.getItem("players");
    if (storedPlayers) {
      setPlayers(JSON.parse(storedPlayers));
    } else {
      fetchData();
    }
  }, []);

  const rollDice = async () => {
    try {
      
      const response = await fetch("https://localhost:7034/api/GameManagerWithAI/rolldice", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(players[currentPlayerIndex]),
      });
  
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
  
      const data = await response.json();
      // Update the players array with the new player data
      const updatedPlayers = [...players];
      updatedPlayers[currentPlayerIndex] = data;
      setPlayers(updatedPlayers);
  
      // Update the current player index
      setCurrentPlayerIndex((currentPlayerIndex + 1) % players.length);
    } catch (error) {
      console.error("Error:", error);
    }
  };

  return (
    <>
      <div className="frame">
        <div className="board">
          {numSquares.map((num) => {
            // Get the players on this square
            const playersOnThisSquare = players.filter(
              (player) => player["currentPosition"] === num
            );
            return (
              <GameSquare key={num} id={num} players={playersOnThisSquare} />
            );
          })}

          <div className="center-square square">
            <div className="center-txt">
              <button onClick={rollDice}>
                <FontAwesomeIcon icon={faDice} /> הגרל קוביות
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
