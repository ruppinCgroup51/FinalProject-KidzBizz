import React from "react";
import { Modal, Button } from "react-bootstrap";

const PropertyModal = ({ show, onHide, property, onBuy }) => {
  return (
    <Modal show={show} onHide={onHide}>
      <Modal.Header closeButton>
        <Modal.Title>Property Available</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <p>Property ID: {property.propertyId}</p>
        <p>Property Name: {property.propertyName}</p>
        <p>Property Price: {property.propertyPrice}</p>
        <p>Do you want to buy this property?</p>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>
          No
        </Button>
        <Button variant="primary" onClick={onBuy}>
          Yes
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default PropertyModal;
