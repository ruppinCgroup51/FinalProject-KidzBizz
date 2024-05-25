import React from "react";
import { SquareConfigData } from "./SquareData";
import SquareType from "/src/components/SquareType.jsx";
import ChanceDisplay from "./squares/ChanceDisplay";
import PropertyDisplay from "./squares/PropertyDisplay";
import GoDisplay from "./squares/GoDisplay";
import PresentDisplay from "./squares/PresentDisplay";
import DidYouKnowDisplay from "./squares/DidYouKnowDisplay";

export const SquareInfo = ({ id }) => {
  const type = SquareConfigData.get(id)?.type;

  const getInfo = () => {
    if (type === SquareType.Present) {
      return <PresentDisplay id={id} />;
    }
    if (type === SquareType.Chance) {
      return <ChanceDisplay id={id} />;
    }
    if (type === SquareType.DidYouKnow) {
      return <DidYouKnowDisplay id={id} />;
    }
    if (type === SquareType.Go) {
      return <GoDisplay id={id} />;
    }
  

    if (type === SquareType.Jail || type === SquareType.GoToJail) {
      return null;
    }

    return <PropertyDisplay id={id} />;
  };

  return getInfo();
};
