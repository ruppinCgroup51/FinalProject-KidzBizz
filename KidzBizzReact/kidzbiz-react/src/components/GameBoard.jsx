import React, { useState, useContext, useEffect } from "react";
import UserContext from "./UserContext";
import { GameSquare } from "./GameSquare";
import "../css/gameboard.css";
import { SquareConfigData } from "/src/components/SquareData.jsx";
import { faDice, faDollarSign } from "@fortawesome/free-solid-svg-icons";
import SquareType from "/src/components/SquareType.jsx";
import SurpriseCardModal from "/src/components/SurpriseCardModal.jsx";
import ChanceCardModal from "/src/components/ChanceCardModal.jsx";
import { Modal as BootstrapModal, Button } from "react-bootstrap";
import {
  FaDiceOne,
  FaDiceTwo,
  FaDiceThree,
  FaDiceFour,
  FaDiceFive,
  FaDiceSix,
} from "react-icons/fa";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { Navigate } from "react-router-dom";
import Modal from "react-modal";
import { toast } from "react-toastify";
import getBaseApiUrl from "./GetBaseApi";
import "react-toastify/dist/ReactToastify.css";

// Bind the modal to your app element
Modal.setAppElement("#root");

export default function GameBoard() {
  // Create an array representing the squares on the board
  const numSquares = Array.from({ length: 40 }, (_, i) => i + 1);
  // Get the user from the context
  const user = useContext(UserContext);

  // Define state variables
  const [players, setPlayers] = useState([]);
  const [currentPlayerIndex, setCurrentPlayerIndex] = useState(0);
  const [isRollDiceDisabled, setIsRollDiceDisabled] = useState(false);
  const [isEndTurnDisabled, setIsEndTurnDisabled] = useState(false);
  const [displayDice, setDisplayDice] = useState(null);
  const [selectedPlayer, setSelectedPlayer] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [modalSquareIsOpen, setModalSquareIsOpen] = useState(false);
  const [modalContent, setModalContent] = useState("");
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [cardData, setCardData] = useState(null);
  const [showCard, setShowCard] = useState(false);

  // Initialize the game when the component mounts
  useEffect(() => {
    setCurrentPlayerIndex(0);
    setIsRollDiceDisabled(false);
    setIsEndTurnDisabled(false);

    // Fetch data function to start the game
    const fetchData = async () => {
      if (!user || !user.userId) {
        console.error("User context is missing userId");
        return;
      }

      // Determine API URL based on environment
      const setUserApi = () => {
        if (
          location.hostname === "localhost" ||
          location.hostname === "127.0.0.1"
        ) {
          return "https://localhost:7034/api/GameManagerWithAI/startnewgame";
        } else {
          return "https://proj.ruppin.ac.il/cgroup51/test2/tar1/api/GameManagerWithAI/startnewgame";
        }
      };

      const apiUrl = setUserApi();

      try {
        const response = await fetch(apiUrl, {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(user),
        });

        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        setPlayers(data);
        localStorage.setItem("players", JSON.stringify(data));
      } catch (error) {
        console.error("Error:", error);
      }
    };

    const storedPlayers = localStorage.getItem("players");
    if (storedPlayers) {
      setPlayers(JSON.parse(storedPlayers));
    } else {
      fetchData();
    }
  }, [user]);

  // Function to roll the dice
  const rollDice = async () => {
    const setUserApi = () => {
      if (
        location.hostname === "localhost" ||
        location.hostname === "127.0.0.1"
      ) {
        return "https://localhost:7034/api/GameManagerWithAI/rolldice";
      } else {
        return "https://proj.ruppin.ac.il/cgroup51/test2/tar1/api/GameManagerWithAI/rolldice";
      }
    };

    const apiUrl = setUserApi();

    try {
      const response = await fetch(apiUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(players[currentPlayerIndex]),
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const data = await response.json();
      const updatedPlayers = [...players];
      updatedPlayers[currentPlayerIndex] = data;
      setPlayers(updatedPlayers);
    } catch (error) {
      console.error("Error:", error);
    }
  };

  // Handle AI's turn
  useEffect(() => {
    if (
      players[currentPlayerIndex] &&
      players[currentPlayerIndex].user.userId === 1016
    ) {
      rollDice().then(() => endTurn());
    }
  }, [currentPlayerIndex, players]);

  // Update local storage whenever the players array changes
  useEffect(() => {
    localStorage.setItem("players", JSON.stringify(players));
  }, [players]);

  // Function to end the current player's turn
  const endTurn = () => {
    console.log("Ending turn. Resetting modals.");
    // Reset modal state when turn ends
    setIsModalVisible(false);
    setShowCard(false);
    setModalSquareIsOpen(false);
    setModalContent("");
    setCardData(null);

    const nextPlayerIndex = (currentPlayerIndex + 1) % players.length;
    setCurrentPlayerIndex(nextPlayerIndex);
  };

  // Handle roll dice button click
  const handleRollDiceClick = async () => {
    setIsRollDiceDisabled(true);
    setIsEndTurnDisabled(false);
    await rollDice();
    setDisplayDice(players[currentPlayerIndex].user.userId);
  };

  // Handle end turn button click
  const handleEndTurnClick = () => {
    console.log("Handling end turn click.");
    const updatedPlayers = [...players];
    if (updatedPlayers[currentPlayerIndex].user.userId === 1016) {
      updatedPlayers[currentPlayerIndex].dice1 = 0;
      updatedPlayers[currentPlayerIndex].dice2 = 0;
    }
    setPlayers(updatedPlayers);
    endTurn();
    setIsRollDiceDisabled(false);
    setIsEndTurnDisabled(true);
    setDisplayDice(1016);
  };

  // Handle end game button click
  const handleEndGame = () => {
    Navigate("/Lobi");
  };

  // Show card modal
  const handleShowCard = (player, type, data) => {
    if (currentPlayerIndex !== players.indexOf(player)) {
      return; // Only show card for the current player
    }
    setCardData(data);
    if (type === "surprise") {
      setIsModalVisible(true);
    } else if (type === "chance") {
      setShowCard(true);
    }
  };

  // Close card modal
  const handleCloseCard = () => {
    setIsModalVisible(false);
    setShowCard(false);
    setModalSquareIsOpen(false);
  };

  // Map dice number to icon
  const numberToDiceIcon = (number, size) => {
    switch (number) {
      case 1:
        return <FaDiceOne size={size} />;
      case 2:
        return <FaDiceTwo size={size} />;
      case 3:
        return <FaDiceThree size={size} />;
      case 4:
        return <FaDiceFour size={size} />;
      case 5:
        return <FaDiceFive size={size} />;
      case 6:
        return <FaDiceSix size={size} />;
      default:
        return null;
    }
  };

  // Component to display player's properties in a modal
  const PlayerProperties = ({ player }) => {
    if (!player) {
      return null;
    }

    const hasProperties = player.properties && player.properties.length > 0;

    return (
      <Modal isOpen={isModalOpen} onRequestClose={() => setIsModalOpen(false)}>
        <h2>{player.user.firstName}'s Properties</h2>
        {hasProperties ? (
          player.properties.map((property, index) => (
            <div key={index}>
              <p>Property ID: {property.propertyId}</p>
              <p>Property Name: {property.propertyName}</p>
              <p>Property Price: {property.propertyPrice}</p>
            </div>
          ))
        ) : (
          <p>No properties</p>
        )}
        <button onClick={() => setIsModalOpen(false)}>Close</button>
      </Modal>
    );
  };

  // Handle landing on a property square
  const handlePropertySquareType = async (position, currentPlayer) => {
    const apiUrl = getBaseApiUrl();
    const fullUrl = `${apiUrl}Properties/CheckPropertyOwnership?propertyId=${position}&playerId=${
      currentPlayer.playerId
    }&playerAiId=${currentPlayer.playerId + 2}`;
    const response = await fetch(fullUrl, {
      method: "GET",
      headers: { Accept: "application/json" },
    });
    const responseText = await response.text();

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    const result = JSON.parse(responseText);
    const owner = result.owner;

    if (owner === -1) {
      const wantToBuy = window.confirm(
        `This property is available. Do you want to buy it?`
      );
      if (wantToBuy) {
        const buyUrl = `${apiUrl}Properties/BuyProperty?PlayerId=${currentPlayer.playerId}&PropertyId=${position}`;
        const buyResponse = await fetch(buyUrl, { method: "POST" });

        if (buyResponse.ok) {
          toast("You have successfully bought the property!", {
            type: "success",
          });
        } else {
          toast("Failed to buy property.", { type: "error" });
        }
      }
    } else if (owner !== currentPlayer.playerId) {
      const rentUrl = `${apiUrl}GameManagerWithAI/payRent`;
      const rentResponse = await fetch(rentUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          playerId: currentPlayer.playerId,
          propertyOwnerId: owner,
          propertyId: position,
        }),
      });

      if (rentResponse.ok) {
        toast("Rent paid successfully!", { type: "info" });
      } else {
        toast("Failed to pay rent.", { type: "error" });
      }
    }
  };

  // Handle landing on a surprise square
  const handleSurpriseSquareType = async (position, currentPlayer) => {
    const apiUrl = getBaseApiUrl();
    const fullUrl = `${apiUrl}Cards/surprise`;
    const response = await fetch(fullUrl, {
      method: "GET",
      headers: { Accept: "application/json" },
    });
    const responseText = await response.text();

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    const result = JSON.parse(responseText);
    setCardData(result);
    setIsModalVisible(true);
  };

  // Handle landing on a chance square
  const handleChanceSquareType = async (position, currentPlayer) => {
    const apiUrl = getBaseApiUrl();
    const fullUrl = `${apiUrl}Cards/command`;
    const response = await fetch(fullUrl, {
      method: "GET",
      headers: { Accept: "application/json" },
    });
    const responseText = await response.text();

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    const result = JSON.parse(responseText);
    setCardData(result);
    setShowCard(true);
  };

  // Handle the player landing on different types of squares
  const handleSquareLanding = async (currentPlayer) => {
    const position = currentPlayer.currentPosition;
    const squareType = SquareConfigData.get(position)?.type;

    try {
      switch (squareType) {
        case SquareType.Property:
          await handlePropertySquareType(position, currentPlayer);
          break;
        case SquareType.Present:
          await handleSurpriseSquareType(position, currentPlayer);
          break;
        case SquareType.Chance:
          await handleChanceSquareType(position, currentPlayer);
          break;
        case SquareType.DidYouKnow:
          console.log("Did You Know? Time for a trivia question!");
          break;
        case SquareType.Go:
          toast("You passed GO! Collect $200.", { type: "info" });
          break;
        case SquareType.Jail:
          showModal("You are just visiting jail this time.");
          break;
        case SquareType.GoToJail:
          showModal("Go to Jail! Move directly to jail.");
          break;
        default:
          toast(`You landed on a regular square.`, { type: "info" });
          break;
      }
    } catch (error) {
      console.error("Error during square landing actions:", error);
      toast("Error handling property action.", { type: "error" });
    }
  };

  // Show modal with a message
  const showModal = (message) => {
    setModalContent(message);
    setModalSquareIsOpen(true);
  };

  // Handle player actions when landing on a square
  useEffect(() => {
    const currentPlayer = players[currentPlayerIndex];
    if (currentPlayer) {
      console.log("Current player:", currentPlayer);
      handleSquareLanding(currentPlayer);
    }
  }, [currentPlayerIndex, players]);

  return (
    <>
      <div className="frame">
        <div className="board">
          {numSquares.map((num) => {
            const playersOnThisSquare = players.filter(
              (player) => player["currentPosition"] === num
            );
            return (
              <GameSquare key={num} id={num} players={playersOnThisSquare} />
            );
          })}
          <div className="center-square square">
            <div className="players-info">
              {players.map((player, index) => (
                <div key={index} className="player-info">
                  <h3>
                    שחקן {index + 1} - {player.user.firstName}
                  </h3>
                  <p>
                    <FontAwesomeIcon icon={faDollarSign} /> Current Money:{" "}
                    {player.currentBalance}
                  </p>
                  <button
                    onClick={() => {
                      setSelectedPlayer(player);
                      setIsModalOpen(true);
                    }}
                  >
                    View Properties
                  </button>
                  {player.user.userId === displayDice &&
                    player.dice1 > 0 &&
                    player.dice2 > 0 && (
                      <p>
                        {numberToDiceIcon(player.dice1, 50)}{" "}
                        {numberToDiceIcon(player.dice2, 50)}
                      </p>
                    )}
                </div>
              ))}
            </div>
            <div className="center-txt">
              <button
                onClick={handleRollDiceClick}
                disabled={isRollDiceDisabled}
              >
                <FontAwesomeIcon icon={faDice} /> הגרל קוביות
              </button>
              <br />
              <br />
              <button onClick={handleEndTurnClick} disabled={isEndTurnDisabled}>
                End Turn
              </button>
              <br />
              <br />
              <button onClick={handleEndGame}>End Game</button>
            </div>
          </div>
          <PlayerProperties player={selectedPlayer} />
        </div>
      </div>

      <BootstrapModal show={modalSquareIsOpen} onHide={handleCloseCard}>
        <h2>{modalContent}</h2>
        <button onClick={handleCloseCard}>Close</button>
      </BootstrapModal>

      {isModalVisible && (
        <SurpriseCardModal card={cardData} onClose={handleCloseCard} />
      )}
      {showCard && (
        <ChanceCardModal
          show={showCard}
          onHide={handleCloseCard}
          cardData={cardData}
        />
      )}
    </>
  );
}
