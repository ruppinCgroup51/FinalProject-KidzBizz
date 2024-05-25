import React from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faQuestion } from "@fortawesome/free-solid-svg-icons";

const ChanceDisplay = ({ id }) => {
  return (
    <React.Fragment>
      <div className="blank"></div>
      <div className="icon">
        <FontAwesomeIcon icon={faQuestion} size="3x" color="#ff4500" />
      </div>
      <div className="square-name">סיכוי</div>
    </React.Fragment>
  );
};

export default ChanceDisplay;
