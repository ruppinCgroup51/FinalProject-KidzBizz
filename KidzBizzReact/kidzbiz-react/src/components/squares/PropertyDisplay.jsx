import React from "react";
import { NyThemeData } from "../NyTheme";
import ColorBar from "./ColorBar";

const PropertyDisplay = ({ id }) => {
  const txt = NyThemeData.get(id)?.name;

  return (
    <React.Fragment>
      <ColorBar id={id} />
      <div className="square-name">{txt}</div>
    </React.Fragment>
  );
};

export default PropertyDisplay;
