import React, { useState } from "react";
import { Modal, Button } from "react-bootstrap";

const ChanceCardModal = ({ show, onHide, cardData }) => {
  const safeCardData = cardData || {};

  return (
    <Modal show={show} onHide={onHide}>
      <Modal.Header closeButton>
        <Modal.Title>Chance Card</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <p>
          <strong>Description:</strong> {safeCardData.description ?? "N/A"}
        </p>
        <p>
          <strong>Amount:</strong> {safeCardData.amount ?? "N/A"}
        </p>
        <p>
          <strong>Move To:</strong> {safeCardData.moveTo ?? "N/A"}
        </p>
        <p>
          <strong>Action:</strong> {safeCardData.action ?? "N/A"}
        </p>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>
          Close
        </Button>
      </Modal.Footer>
    </Modal>
  );
};
export default ChanceCardModal;
