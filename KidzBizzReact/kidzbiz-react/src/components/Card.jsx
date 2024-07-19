import React from "react";
import "../css/Card.css";

const Card = ({ onClose, card }) => {
  console.log(card);
  return (
    <div className="card-overlay">
      <div className="card">
        <div className="card-body">
          <p>
            <strong>Amount:</strong> {card.amount}
          </p>
          <p>
            <strong>Description:</strong> {card.description}
          </p>
          <p>
            <strong>Action:</strong> {card.action}
          </p>
        </div>
        <div className="card-footer">
          <button onClick={onClose} className="card-close-button">
            Close
          </button>
        </div>
      </div>
    </div>
  );
};

export default Card;
