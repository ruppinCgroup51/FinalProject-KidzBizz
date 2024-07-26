import React, { useEffect, useRef } from "react";
import BoardSection from "./BoardSection";
import { SquareConfigData } from "/src/components/SquareData.jsx";
import { SquareInfo } from "/src/components/SquareInfo.jsx";
import SquareType from "./SquareType";
import "../css/gamesquare.css";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

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
    [SquareType.Present, "הפתעה"],
    [SquareType.Chance, "סיכוי"],
    [SquareType.Go, "passgo"],
    [SquareType.GoToJail, "go-to-jail"],
    [SquareType.Jail, "jail"],
    [SquareType.Property, "נכס"],
    [SquareType.DidYouKnow, "הידעת ?"],
  ]);
  const prevPlayerPositions = useRef(
    players.map((player) => player.currentPosition)
  );
  const getContainerClassName = () => {
    return "container container-" + sectionMap.get(section);
  };

  const getSquareClassName = () => {
    return "square " + squareTypeClass.get(squareType);
  };

  const getSquareId = () => {
    return "game-square-" + id;
  };

  useEffect(() => {
    players.forEach((player, index) => {
      if (
        player.currentPosition === id &&
        player.currentPosition !== prevPlayerPositions.current[index]
      ) {
        prevPlayerPositions.current[index] = player.currentPosition;
        /*if (squareType !== SquareType.Go) {
          toast(
            `שחקן מספר ${player.playerId} הגעת ל-תא מסוג ${squareTypeClass.get(
              squareType
            )}`,
            { type: "success" }
          );*/
        }
      //}
    });
  }, [players, id, squareType, squareTypeClass]);

  return (
    <div className={getSquareClassName()} id={getSquareId()}>
      <div className={getContainerClassName()}>
        <SquareInfo id={id} />
        <div className="player-container">
          {/*render the players on this square */}
          {players &&
            players.map((player) => (
              <div key={player["user"]["userId"]} className="player">
                {/* {player["user"]["userId"]} */}
                <img
                  src={player["user"]["avatarPicture"]}
                  alt="avatar"
                  className="avatar"
                />
              </div>
            ))}
        </div>
      </div>
    </div>
  );
};
