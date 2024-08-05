import React, {
  useState,
  useContext,
  useEffect,
  useCallback,
  useRef,
} from "react";
import UserContext from "./UserContext";
import { GameSquare } from "./GameSquare";
import "../css/gameboard.css";
import { SquareConfigData } from "./SquareData.jsx";
import PropertyModal from "./PropertyModal";
import { faDice, faDollarSign } from "@fortawesome/free-solid-svg-icons";
import SquareType from "./SquareType.jsx";
import SurpriseCardModal from "./SurpriseCardModal.jsx";
import ChanceCardModal from "./ChanceCardModal.jsx";
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
import Modal from "react-modal";
import { toast } from "react-toastify";
import getBaseApiUrl from "./GetBaseApi";
import "react-toastify/dist/ReactToastify.css";
import { useNavigate } from 'react-router-dom';


Modal.setAppElement("#root");

const GameBoard = () => {
  const numSquares = Array.from({ length: 40 }, (_, i) => i + 1);
  const user = useContext(UserContext);
  const navigate = useNavigate();
  const [countdown, setCountdown] = useState(5); // Countdown from 5 seconds
  const [gameStarted, setGameStarted] = useState(false);

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
  const [showPropertyModal, setShowPropertyModal] = useState(false);
  const [currentProperty, setCurrentProperty] = useState(null);

  const isHandlingSquareLanding = useRef(false);

  useEffect(() => {
    setCurrentPlayerIndex(0);
    setIsRollDiceDisabled(false);
    setIsEndTurnDisabled(false);

    const fetchData = async () => {
      if (!user || !user.userId) {
        console.error("User context is missing userId");
        return;
      }

      const apiUrl = getApiUrl("startnewgame");
      try {
        const response = await fetch(apiUrl, {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(user),
        });

        if (!response.ok)
          throw new Error(`HTTP error! status: ${response.status}`);

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

  const getApiUrl = useCallback((endpoint) => {
    if (
      location.hostname === "localhost" ||
      location.hostname === "127.0.0.1"
    ) {
      return `https://localhost:7034/api/GameManagerWithAI/${endpoint}`;
    } else {
      return `https://proj.ruppin.ac.il/cgroup51/test2/tar1/api/GameManagerWithAI/${endpoint}`;
    }
  }, []);

  const rollDice = useCallback(async () => {
    const apiUrl = getApiUrl("rolldice");
    try {
      const response = await fetch(apiUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(players[currentPlayerIndex]),
      });

      if (!response.ok)
        throw new Error(`HTTP error! status: ${response.status}`);

      const data = await response.json();
      const updatedPlayers = [...players];
      updatedPlayers[currentPlayerIndex] = data;
      setPlayers(updatedPlayers);
    } catch (error) {
      console.error("Error:", error);
    }
  }, [currentPlayerIndex, players, getApiUrl]);

  useEffect(() => {
    if (players[currentPlayerIndex]?.user.userId === 1016) {
      rollDice().then(endTurn);
    }
  }, [currentPlayerIndex, players, rollDice]);

  useEffect(() => {
    localStorage.setItem("players", JSON.stringify(players));
  }, [players]);

  const endTurn = useCallback(() => {
    setIsModalVisible(false);
    setShowCard(false);
    setModalSquareIsOpen(false);
    setModalContent("");
    setCardData(null);

    setCurrentPlayerIndex((prevIndex) => (prevIndex + 1) % players.length);
  }, [players.length]);

  const handleRollDiceClick = async () => {
    setIsRollDiceDisabled(true);
    setIsEndTurnDisabled(false);
    await rollDice();
    setDisplayDice(players[currentPlayerIndex].user.userId);
  };

  const handleEndTurnClick = () => {
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

  const handleEndGame = () => {
    navigate("/Lobi");  // Navigate to the "/Lobi" route when this function is called
  };

  const handleShowCard = (player, type, data) => {
    if (currentPlayerIndex !== players.indexOf(player)) return;
    setCardData(data);
    if (type === "surprise") setIsModalVisible(true);
    else if (type === "chance") setShowCard(true);
  };

  const handleCloseCard = () => {
    setIsModalVisible(false);
    setShowCard(false);
    setModalSquareIsOpen(false);
  };

  const numberToDiceIcon = (number, size) => {
    const diceIcons = {
      1: <FaDiceOne size={size} />,
      2: <FaDiceTwo size={size} />,
      3: <FaDiceThree size={size} />,
      4: <FaDiceFour size={size} />,
      5: <FaDiceFive size={size} />,
      6: <FaDiceSix size={size} />,
    };
    return diceIcons[number] || null;
  };

  const PlayerProperties = ({ player }) => {
    if (!player) return null;

    return (
      <Modal isOpen={isModalOpen} onRequestClose={() => setIsModalOpen(false)} style={{ maxWidth: '600px', width: '50%', margin: 'auto', position: 'relative' }}>
      <h2 style={{ position: 'relative',textAlign: 'right',color:'red', fontFamily: 'cursive'}}>{player.user.firstName} הנכסים של </h2>
      {player.properties && player.properties.length > 0 ? (
          player.properties.map((property, index) => (
              <div key={index} style={{ color:'black', textAlign:'right'}}>
                  <p>מזהה נכס: {property.propertyId}</p>
                  <p>שם נכס: {property.propertyName}</p>
                  <p>מחיר נכס: {property.propertyPrice}</p>
              </div>
          ))
      ) : (
          <p style={{ position: 'relative',textAlign: 'right',color:'black'}}>אין נכסים</p>
      )}
      <button style={{ position: 'absolute', left: '10px', bottom: '10px' , color:'white', backgroundColor:'red'}} onClick={() => setIsModalOpen(false)}>סגור</button>
  </Modal>
  
    );
  };

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

    if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

    const result = JSON.parse(responseText);
    const owner = result.owner;

    if (owner === -1) {
      // Property has no owner, fetch property details
      const propertyDetails = await fetchPropertyDetails(position);
      if (propertyDetails) {
        setCurrentProperty({
          propertyId: position,
          propertyName: propertyDetails.propertyName,
          propertyPrice: propertyDetails.propertyPrice,
          currentPlayer,
        });
        setShowPropertyModal(true);
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
        const updatedPlayerResponse = await fetchPlayerData(
          currentPlayer.playerId
        );
        if (updatedPlayerResponse.ok) {
          const updatedPlayerData = await updatedPlayerResponse.json();
          updatePlayerDataInState(updatedPlayerData);
        }
      } else {
        toast("Failed to pay rent.", { type: "error" });
      }
    }
  };

  const fetchPropertyDetails = async (propertyId) => {
    const apiUrl = getBaseApiUrl();
    const fullUrl = `${apiUrl}Properties/GetPropertyDetails?propertyId=${propertyId}`;
    try {
      const response = await fetch(fullUrl, {
        method: "GET",
        headers: { "Accept": "application/json" }
      });
  
      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }
  
      return await response.json();  // Directly parsing as JSON
    } catch (error) {
      console.error("Failed to fetch property details:", error.message);
      return null;  // Optionally return more specific error information
    }
  };
  

  const handleBuyProperty = async () => {
    const { propertyId, currentPlayer } = currentProperty;
    const apiUrl = getBaseApiUrl();
    const buyUrl = `${apiUrl}Properties/BuyProperty?PlayerId=${currentPlayer.playerId}&PropertyId=${propertyId}`;
    const buyResponse = await fetch(buyUrl, { method: "POST" });

    if (buyResponse.ok) {
      toast("You have successfully bought the property!", { type: "success" });
      const updatedPlayerResponse = await fetchPlayerData(
        currentPlayer.playerId
      );
      if (updatedPlayerResponse.ok) {
        const updatedPlayerData = await updatedPlayerResponse.json();
        updatePlayerDataInState(updatedPlayerData);
      }
    } else {
      toast("Failed to buy property.", { type: "error" });
    }

    setShowPropertyModal(false);
  };

  const fetchPlayerData = (playerId) => {
    const apiUrl = getBaseApiUrl();
    const fullUrl = `${apiUrl}Players/${playerId}`;
    return fetch(fullUrl, {
      method: "GET",
      headers: { Accept: "application/json" },
    });
  };

  const updatePlayerDataInState = (updatedPlayerData) => {
    setPlayers((prevPlayers) =>
      prevPlayers.map((player) =>
        player.playerId === updatedPlayerData.playerId
          ? updatedPlayerData
          : player
      )
    );
  };

  const handleSurpriseSquareType = async (position, currentPlayer) => {
    const apiUrl = getBaseApiUrl();
    const fullUrl = `${apiUrl}Cards/surprise`;
    const response = await fetch(fullUrl, {
      method: "GET",
      headers: { Accept: "application/json" },
    });
    const responseText = await response.text();

    if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

    const result = JSON.parse(responseText);
    setCardData(result);
    setIsModalVisible(true);
  };

  const handleChanceSquareType = async (position, currentPlayer) => {
    const apiUrl = getBaseApiUrl();
    const fullUrl = `${apiUrl}Cards/command`;
    const response = await fetch(fullUrl, {
      method: "GET",
      headers: { Accept: "application/json" },
    });
    const responseText = await response.text();

    if (!response.ok) throw new Error(`HTTP error! Status: ${response.status}`);

    const result = JSON.parse(responseText);
    setCardData(result);
    setShowCard(true);
  };

  const handleSquareLanding = useCallback(async (currentPlayer) => {
    if (isHandlingSquareLanding.current) return;

    isHandlingSquareLanding.current = true;
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
          toast("You are just visiting jail this time.");
          break;
        case SquareType.GoToJail:
          toast("Go to Jail! Move directly to jail.");
          break;
        default:
          toast(`You landed on a regular square.`, { type: "info" });
          break;
      }
    } catch (error) {
      console.error("Error during square landing actions:", error);
      toast("Error handling property action.", { type: "error" });
    } finally {
      isHandlingSquareLanding.current = false;
    }
  }, []);

  useEffect(() => {
    const currentPlayer = players[currentPlayerIndex];
    if (currentPlayer) {
      console.log("Current player:", currentPlayer);
      handleSquareLanding(currentPlayer);
    }
  }, [currentPlayerIndex, players, handleSquareLanding]);
  
  useEffect(() => {
    let timerId;
    if (countdown > 0 && !gameStarted) {
      timerId = setTimeout(() => setCountdown(countdown - 1), 1000);
    } else {
      setGameStarted(true);
      // Game initialization logic here
    }
    return () => clearTimeout(timerId);
  }, [countdown, gameStarted]);

  const renderCountdown = () => {
    const radius = 45;
    const circumference = 2 * Math.PI * radius;
    const strokeDashoffset = ((5 - countdown) / 5) * circumference;

    return (
      <div className="countdown-container">
        <svg width="100" height="100">
          <circle
            r={radius}
            cx="50"
            cy="50"
            fill="transparent"
            stroke="#4caf50"
            strokeWidth="10"
            strokeDasharray={circumference}
            strokeDashoffset={strokeDashoffset}
            transform="rotate(-90 50 50)"
          />
          <text x="50" y="50" fill="white" textAnchor="middle" dy="8" fontSize="20">{countdown}</text>
        </svg>
      </div>
    );
  };

  if (!gameStarted) {
    return renderCountdown();
  }


  return (
    <>
    <button class="endgameBTN" onClick={handleEndGame}>סיים משחק</button>
      <div className="frame">
        <div className="board">
          {numSquares.map((num) => {
            const playersOnThisSquare = players.filter(
              (player) => player.currentPosition === num
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
                   {player.user.firstName} - שחקן {index + 1}
                  </h3>
                  <p>
                    <FontAwesomeIcon icon={faDollarSign} /> כמות כסף:{" "}
                    {player.currentBalance}
                    <br/>
                  </p>
                  <button class="propertyBTN"
                    onClick={() => {
                      setSelectedPlayer(player);
                      setIsModalOpen(true);
                    }}
                  >
                    ראה נכסים
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
            <br/>
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
                סיים תור
              </button>
              <br />
              <br />
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
      {currentProperty && (
        <PropertyModal
          show={showPropertyModal}
          onHide={() => setShowPropertyModal(false)}
          property={currentProperty}
          onBuy={handleBuyProperty}
        />
      )}
    </>
  );
};

export default GameBoard;
