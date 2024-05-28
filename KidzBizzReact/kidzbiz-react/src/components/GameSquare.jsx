import React from "react";
import BoardSection from "./BoardSection";
import { SquareConfigData } from "/src/components/SquareData.jsx";
import { SquareInfo } from "/src/components/SquareInfo.jsx";
import SquareType from "./SquareType";
import "../css/gamesquare.css";

export const GameSquare = ({ id, players }) => {
  const section = SquareConfigData.get(id)?.section;
  const squareType = SquareConfigData.get(id)?.type;

  const sectionMap = new Map([
    [BoardSection.Top, "top"],
    [BoardSection.Right, "right"],
    [BoardSection.Left, "left"],
    [BoardSection.Bottom, "bottom"],
  ]);

  const squareTypeClass = new Map([
    [SquareType.Present, "present"],
    [SquareType.Chance, "chance"],
    [SquareType.Go, "passgo"],
    [SquareType.GoToJail, "go-to-jail"],
    [SquareType.Jail, "jail"],
    [SquareType.Property, "property"],
    [SquareType.DidYouKnow, "didYouKnow"]
  ]);

  const getContainerClassName = () => {
    return "container container-" + sectionMap.get(section);
  };

  const getSquareClassName = () => {
    return "square " + squareTypeClass.get(squareType);
  };

  const getSquareId = () => {
    return "game-square-" + id;
  };

  return (
    <div className={getSquareClassName()} id={getSquareId()}>
      <div className={getContainerClassName()}>
      
        <SquareInfo id={id} />
        
        {/* Add this block to render the players on this square */}
        {players &&
          players.map((player) => (
            <div key={player["user"]["userId"]} className="player">
              {player["user"]["userId"]}
            </div>
          ))}

        
      </div>

    </div>
  );
};
