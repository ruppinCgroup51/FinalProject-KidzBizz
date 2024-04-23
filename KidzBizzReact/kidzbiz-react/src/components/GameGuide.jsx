import React from "react";
import { Link, useNavigate } from "react-router-dom";
import "../css/GameGuide.css" // Import your CSS file for styling

export default function GameGuide() {
  const navigate = useNavigate();

  const handleClose = () => {
    navigate("/Lobi"); // Navigate back to the Lobi page
  };

  return (
    <div className="game-guide-overlay"> {/* Overlay container */}
      <div className="game-guide-modal"> {/* Modal container */}
        <h1>Game Guide</h1>
        {/*הכנסתי מלל בנתיים לראות איך זה נראה*/}
        <p>
        The KidzBizz Monopoly game is a game where you learn about money, economics and finance.<br/>
        In the game you buy, rent or sell valuable assets, answer questions about finance, earn money and get rich. The richest participant of all - wins in the end.<br/><br/>

At any given moment in the game, you can see the game board, the position of the players on the board, the assets you have purchased and your current capital.<br/>
At the beginning of the game, 1,500 NIS will be distributed to each player.<br/>
Each player starts in the "through a plate" slot, and each round moves around the board according to the result of his dice roll.<br/>
<br/>
When a player reaches the "property" slot, he chooses whether to buy the property or not.<br/>
Note! Property owners collect rent from the other participants who stay on their properties, so you should purchase as many properties as possible.<br/>
When a player reaches the "pay taxes" slot he will have to pay taxes according to his assets.<br/>
When a player reaches the "share purchase" slot, he will have to purchase a share according to the shares available in the market.<br/>
When a player reaches a "prison" slot, he must enter the prison and stay there for a full turn.<br/>
<br/>
Surprise cards and command cards are scattered around the board, the player must always fill in what is written on the "surprise" cards and the "command" cards, even if it means being sent to prison 😣<br/>
"Knowledge" cards are also scattered around the board. These cards contain educational information about economics and finance.<br/>
After reading the card, a player can choose if they want to answer a question about it and win money.<br/>
Note  ! The more money you have, the more your chance of winning the game increases, so be focused and answer as many questions as possible 😀<br/>
When a player goes bankrupt, he is out of the game.<br/>
<br/>
The goal of the game is to be the last player to go bankrupt.<br/>
The richest player - wins the game 👏

        </p>
        <button onClick={handleClose}>Close</button> {/* Close button to navigate back to Lobi */}
      </div>
    </div>
  );
}
