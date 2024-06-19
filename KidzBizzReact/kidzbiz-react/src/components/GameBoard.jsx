import React, { useState, useContext, useEffect } from "react";
import UserContext from "./UserContext";
import { GameSquare } from "./GameSquare";
import "../css/gameboard.css";
import { SquareConfigData} from "/src/components/SquareData.jsx"; // Already imported in GameSquare, ensure it's imported in GameBoard too
import { faDice, faDollarSign } from "@fortawesome/free-solid-svg-icons";
import SquareType from "/src/components/SquareType.jsx"
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
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';  // Make sure this line is also included to apply styles.
 // At the top level of your component or application


// This line is needed to bind the modal to your app element (set the ID accordingly)
Modal.setAppElement('#root'); // or whatever your root element's ID is
export default function GameBoard() {
  const numSquares = Array.from({ length: 40 }, (_, i) => i + 1); // 40 משבצות בלוח
  const user = useContext(UserContext); // המשתמש המחובר
  const [players, setPlayers] = useState([]);
  const [currentPlayerIndex, setCurrentPlayerIndex] = useState(0);
  const [isRollDiceDisabled, setIsRollDiceDisabled] = useState(false);
  const [isEndTurnDisabled, setisEndTurnDisabled] = useState(false);
  const [displayDice, setDisplayDice] = useState(null);
  // Add a new state variable for prop button.
  const [selectedPlayer, setSelectedPlayer] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  ////
  const [modalSquareIsOpen, setModalSquarIsOpen] = useState(false);
  const [modalContent, setModalContent] = useState('');


  useEffect(() => {
    setCurrentPlayerIndex(0);
    setIsRollDiceDisabled(false);
    setisEndTurnDisabled(false);

    const fetchData = async () => {
      if (!user || !user.userId) {
        console.error("User context is missing userId");
        return; // Optionally display an error message to the user
      }

      // התחלת משחק
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
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(user),
        });

        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();
        setPlayers(data);
        console.log(data);

        // Store the players array in local storage
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
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(players[currentPlayerIndex]),
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const data = await response.json();
      //Update the players array with the new player data
      const updatedPlayers = [...players];
      updatedPlayers[currentPlayerIndex] = data;
      setPlayers(updatedPlayers);
    } catch (error) {
      console.error("Error:", error);
    }
  };

  useEffect(() => {
    // If it's the AI's turn, make a move
    if (
      players[currentPlayerIndex] &&
      players[currentPlayerIndex].user.userId === 1016
    ) {
      rollDice().then(endTurn);
    }
  }, [currentPlayerIndex, players, rollDice]);

  //Update the localstorage every time players array change
  useEffect(() => {
    localStorage.setItem("players", JSON.stringify(players));
  }, [players]);

  const endTurn = () => {
    // Advance the current player index

    const nextPlayerIndex = (currentPlayerIndex + 1) % players.length;
    setCurrentPlayerIndex(nextPlayerIndex);
  };

  const handleRollDiceClick = async () => {
    setIsRollDiceDisabled(true);
    setisEndTurnDisabled(false);

    await rollDice();
    // Set the displayDice state to the current player's userId
    setDisplayDice(players[currentPlayerIndex].user.userId);
  };

  const handleEndTurnClick = () => {
    const updatedPlayers = [...players];
    if (updatedPlayers[currentPlayerIndex].user.userId === 1016) {
      // Only reset the dice for the AI player
      updatedPlayers[currentPlayerIndex].dice1 = 0;
      updatedPlayers[currentPlayerIndex].dice2 = 0;
    }
    setPlayers(updatedPlayers);
    endTurn();
    setIsRollDiceDisabled(false);
    setisEndTurnDisabled(true);
    setDisplayDice(1016);
  };

  const handleEndGame = () => {
    //call end game from server

    // go back to looby page
    Navigate("/Lobi");
  };

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

  const handleSquareLanding = async (position) => {
    const squareType = SquareConfigData.get(position)?.type; // Using the existing GameSquare logic to get type
  
    switch(squareType) {
      case SquareType.Property:
        /*try {
          const response = await fetch(`/api/Properties/CheckPropertyOwnership?propertyId=${position}`);
          const result = await response.json();
          const ownerId = result.owner; 
    
          if (ownerId === -1) {
            // No owner, ask player if they want to buy the property
            const wantToBuy = window.confirm('This property is available. Do you want to buy it?');
            if (wantToBuy) {
              const buyResponse = await fetch(`/api/Properties/BuyProperty`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ propertyId: position, playerId: currentPlayer.userId })
              });
              if (buyResponse.ok) {
                toast('You have successfully bought the property!', { type: 'success' });
              } else {
                toast('Failed to buy property.', { type: 'error' });
              }
            }
          } else {
            // Property has an owner, need to pay rent
            const rentResponse = await fetch(`/api/GameManagerWithAI/payRent`, {
              method: 'POST',
              headers: { 'Content-Type': 'application/json' },
              body: JSON.stringify({ playerId: currentPlayer.userId, propertyOwnerId: ownerId, propertyId: position })
            });
            if (rentResponse.ok) {
              toast('Rent paid successfully!', { type: 'info' });
            } else {
              toast('Failed to pay rent.', { type: 'error' });
            }
          }
        } catch (error) {
          console.error('Failed to check property ownership or handle transactions:', error);
          toast('Error handling property action.', { type: 'error' });
        }; */
        // Show modal to buy property or something similar
        showModal(`You landed on a property. Would you like to buy it?`);
        break;
      case SquareType.Surprise:
        // Trigger a surprise event
        showModal('Surprise! You have drawn a surprise card.');
        break;
      case SquareType.Chance:
        // Trigger a chance event
        showModal('Chance! Take a chance card and see what happens.');
        break;
      case SquareType.DidYouKnow:
        // Trigger a trivia question
        showModal('Did You Know? Time for a trivia question!');
        break;
      case SquareType.Go:
        // Perhaps increment player's money here as they pass go
        toast('You passed GO! Collect $200.', { type: "info" });
        break;
      case SquareType.Jail:
        // Handle jail logic
        showModal('You are just visiting jail this time.');
        break;
      case SquareType.GoToJail:
        // Move player to jail
        showModal('Go to Jail! Move directly to jail.');
        break;
      default:
        // Handle other types or do nothing
        toast(`You landed on a regular square.`, { type: "info" });
        break;
    }
  };
  
  // Function to show modal with a message
  const showModal = (message) => {
    setModalContent(message);
    setModalSquarIsOpen(true);  // Ensure this is the correct state variable
  };
  

useEffect(() => {
  if (players[currentPlayerIndex]) {
    const currentPosition = players[currentPlayerIndex].currentPosition;
    handleSquareLanding(currentPosition);
  }
}, [currentPlayerIndex, players]);

  



  

  return (
    <>
      <div className="frame">
        <div className="board">
          {numSquares.map((num) => {
            //Get the players on this square
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
                    <FontAwesomeIcon icon={faDollarSign} />
                    Current Money: {player.currentBalance}
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
                        {numberToDiceIcon(player.dice1, 50)}
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
        {/* Modal for displaying messages based on square landing */}
        <Modal isOpen={modalSquareIsOpen} onRequestClose={() => setModalSquarIsOpen(false)}>
        <h2>{modalContent}</h2>
        <button onClick={() => setModalSquarIsOpen(false)}>Close</button>
      </Modal>
    </>
  );
}
