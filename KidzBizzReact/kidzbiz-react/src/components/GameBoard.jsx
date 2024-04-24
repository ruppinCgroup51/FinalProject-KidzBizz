import React from "react";
import { GameSquare } from "./GameSquare";
import "../css/gameboard.css";

export default function GameBoard() {
  const numSquares = Array.from({ length: 40 }, (_, i) => i + 1);

  return (
    <>
      <div class="frame">
        <div className="board">
          {numSquares.map((num) => (
            <GameSquare key={num} id={num} />
          ))}

          <div className="center-square square">
            <div className="center-txt"></div>
          </div>
        </div>
      </div>
    </>
  );
}
