import React, { useState, useContext, useEffect } from "react";
import UserContext from "./UserContext";
import { GameSquare } from "./GameSquare";
import "../css/gameboard.css";
import { faDice, faDollarSign } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

export default function GameBoard() {
  const numSquares = Array.from({ length: 40 }, (_, i) => i + 1);
  const user = useContext(UserContext);
  const [players, setPlayers] = useState([]);
  const [currentPlayerIndex, setCurrentPlayerIndex] = useState(0);
  const [isRollDiceDisabled, setIsRollDiceDisabled] = useState(false);
  const [isEndTurnDisabled, setisEndTurnDisabled] = useState(false);

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
      const response = await fetch(
        "https://localhost:7034/api/GameManagerWithAI/rolldice",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(players[currentPlayerIndex]),
        }
      );

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const data = await response.json();
      // Update the players array with the new player data
      const updatedPlayers = [...players];
      updatedPlayers[currentPlayerIndex] = data;
      setPlayers(updatedPlayers);
    } catch (error) {
      console.error("Error:", error);
    }
  };

  useEffect(() => {
    // If it's the AI's turn, make a move
    if (
      players[currentPlayerIndex] &&
      players[currentPlayerIndex].user.userId === 1016
    ) {
      rollDice().then(endTurn);
    }
  }, [currentPlayerIndex, players, rollDice]);

  //Update the localstorage every time players array change
  useEffect(() => {
    localStorage.setItem("players", JSON.stringify(players));
  }, [players]);

  const endTurn = () => {
    // Advance the current player index
    const nextPlayerIndex = (currentPlayerIndex + 1) % players.length;
    setCurrentPlayerIndex(nextPlayerIndex);
  };

  const handleRollDiceClick = async () => {
    setIsRollDiceDisabled(true);
    setisEndTurnDisabled(false);
    await rollDice();
  };

  const handleEndTurnClick = () => {
    endTurn();
    setIsRollDiceDisabled(false);
    setisEndTurnDisabled(true);
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
            <div className="players-info">
              {players.map((player, index) => (
                <div key={index} className="player-info">
                  <h3>
                    שחקן {index + 1} - {player.user.firstName}
                  </h3>
                  <p>
                    <FontAwesomeIcon icon={faDollarSign} />
                    Current Money: {player.currentBalance}
                  </p>
                </div>
              ))}
            </div>

            <div className="center-txt">
              <button
                onClick={handleRollDiceClick}
                disabled={isRollDiceDisabled}
              >
                <FontAwesomeIcon icon={faDice} /> הגרל קוביות
              </button>
              <br />
              <br />
              <button onClick={handleEndTurnClick} disabled={isEndTurnDisabled}>
                End Turn
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
