import React, { useState } from "react";
import { GameSquare } from "./GameSquare";
import "../css/gameboard.css";

export default function GameBoard() {
  const numSquares = Array.from({ length: 40 }, (_, i) => i + 1);

  const [players, setPlayers] = useState([
    { id: 1, name: "Player 1", position: 1 },
    { id: 2, name: "Player 2", position: 5 },
  ]);

  return (
    <>
      <div className="frame">
        <div className="board">
          {numSquares.map((num) => {
            // Get the players on this square
            const playersOnThisSquare = players.filter(
              (player) => player.position === num
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
