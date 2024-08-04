import React from "react";
import "../css/SurpriseCardModal.css"; // Ensure this CSS file exists and is correctly styled

const SurpriseCardModal = ({ card, onClose }) => {
  if (!card) return null;

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <h2>קלף הפתעה</h2>
        <p>Card ID: {card.cardId}</p>
        <p>{card.description}</p>
        <p>Amount: {card.amount}</p>
        <button onClick={onClose}>Close</button>
      </div>
    </div>
  );
};

export default SurpriseCardModal;
