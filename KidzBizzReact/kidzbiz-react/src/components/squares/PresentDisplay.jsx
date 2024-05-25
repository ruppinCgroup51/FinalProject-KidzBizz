import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faGift } from "@fortawesome/free-solid-svg-icons";

const PresentDisplay = ({ id }) => {
  return (
    <React.Fragment>
      <div className="blank"></div>
      <div className="icon">
        <FontAwesomeIcon icon={faGift} size="3x" color="#9932CC" />
      </div>
      <div className="square-name"> הפתעה</div>
    </React.Fragment>
  );
};

export default PresentDisplay;
