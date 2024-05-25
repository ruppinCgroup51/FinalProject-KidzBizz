import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faLightbulb, faQuestion } from "@fortawesome/free-solid-svg-icons";

const DidYouKnowDisplay = ({ id }) => {
  return (
    <React.Fragment>
      <div className="blank"></div>
      <div className="icon">
        <FontAwesomeIcon icon={faLightbulb} size="3x" color="#FFD700" />
      </div>
      <div className="square-name"> ?הידעת </div>
    </React.Fragment>
  );
};

export default DidYouKnowDisplay;