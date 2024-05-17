import React, { useState, useContext, useEffect } from "react";
import UserContext from "./UserContext";
import { GameSquare } from "./GameSquare";
import "../css/gameboard.css";

export default function GameBoard() {
  const numSquares = Array.from({ length: 40 }, (_, i) => i + 1);
  const user = useContext(UserContext);
  const [players, setPlayers] = useState([]);

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
            body: JSON.stringify(user), // corrected typo he),
          }
        );

        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        console.log("Hey" + user.userId);
        setPlayers(data);
        console.log(data);

        // Store the players array in local storage
        localStorage.setItem("players", JSON.stringify(data));
      } catch (error) {
        console.error("Error:", error);
      }
    };

    fetchData();
  }, [user]);

  // Load the players array from local storage when the component mounts
  useEffect(() => {
    const storedPlayers = localStorage.getItem("players");
    if (storedPlayers) {
      setPlayers(JSON.parse(storedPlayers));
    }
  }, []);

  useEffect(() => {
    console.log(players);
  }, [players]);

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
            <div className="center-txt"></div>
          </div>
        </div>
      </div>
    </>
  );
}
