import React from "react";
import { SquareConfigData, squareGroupColorMap } from "../SquareData";

const ColorBar = ({ id }) => {
  const groupId = SquareConfigData.get(id)?.groupId;

  const getClassName = () => {
    return "square-color-bar " + squareGroupColorMap.get(groupId);
  };

  return <div className={getClassName()}></div>;
};

export default ColorBar;
