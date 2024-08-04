namespace KidzBizzServer.BL
{
    public class GameManagerWithAI
    {
        Player player = new Player();
        AIPlayer aiPlayer = new AIPlayer();
        int currentPlayerIndex;
        int diceRoll;
        Game game = new Game();

        public GameManagerWithAI(Player player, AIPlayer aiPlayer, int currentPlayerIndex, int diceRoll, Game game)
        {
            Player = player;
            AiPlayer = aiPlayer;
            CurrentPlayerIndex = currentPlayerIndex;
            DiceRoll = diceRoll;
            Game = game;
        }

        public GameManagerWithAI()
        {
        }

        public Player Player { get => player; set => player = value; }
        public AIPlayer AiPlayer { get => aiPlayer; set => aiPlayer = value; }
        public int CurrentPlayerIndex { get => currentPlayerIndex; set => currentPlayerIndex = value; }
        public int DiceRoll { get => diceRoll; set => diceRoll = value; }
        public Game Game { get => game; set => game = value; }

        // פעולה להפעלת משחק חדש
        public List<Player> StartNewGame(User user)
        {
            DBservices dbs = new DBservices();
            var (startingMoney, currentLocation) = dbs.GetGameSettings();

            // Create a new game instance
            Game game = new Game
            {
                NumberOfPlayers = 2,
                GameDuration = "00:00:00",  // no duration initially // Timespan made some problems later we will change it 
                GameStatus = "Active",
                GameTimestamp = DateTime.Now
            };

            // Insert the game into the database
            game.InsertGame(); // Assuming this method exists and returns game ID

            // Create players
            Player player = new Player
            {
                // how to convert decimal to double
                User = user,
                CurrentBalance = Convert.ToDouble(startingMoney),
                CurrentPosition = currentLocation,
                PlayerStatus = "Active",
                LastDiceResult = 0,
            };

            player.Insert();
            PlayerType aiPlayerType = DetermineAIPlayerType(player);
            AIPlayer aiPlayer = new AIPlayer(player.PlayerId, null, currentLocation, Convert.ToDouble(startingMoney), "Active", 0, new List<Property>(), 0, 0, aiPlayerType);
            aiPlayer.Insert();

            // Randomly decide who starts first
            currentPlayerIndex = new Random().Next(0, 2);
            return new List<Player> { player, aiPlayer };
        }

        // פונקציה לקביעת סוג השחקן AI על פי סטטיסטיקות השחקן
        private PlayerType DetermineAIPlayerType(Player player)
        {
            if (player.Statistics.TotalWins > player.Statistics.TotalLosses)
            {
                return PlayerType.Adventurous;
            }
            else if (player.Statistics.TotalWins < player.Statistics.TotalLosses)
            {
                return PlayerType.Conservative;
            }
            else
            {
                return PlayerType.Balanced;
            }
        }

        public void EndGame()
        {
            game.GameDuration = (DateTime.Now - game.GameTimestamp).ToString("g");
            game.GameStatus = "Completed";
            game.UpdateGame();

            player.PlayerStatus = "Not Active";
            aiPlayer.PlayerStatus = "Not Active";
            player.Update();
            aiPlayer.Update();

            string winner = DetermineWinner(); // Implement this method to decide based on game logic
            Console.WriteLine($"The game has ended. The winner is {winner}.");

            if (winner == "Player")
            {
                player.User.Score += 100; // Update score for human player
                player.Update();
            }
        }

        public string DetermineWinner()
        {
            if (player.CurrentBalance > aiPlayer.CurrentBalance)
            {
                return "Player";
            }
            else if (aiPlayer.CurrentBalance > player.CurrentBalance)
            {
                return "AIPlayer";
            }
            else
            {
                if (player.Properties.Count() > aiPlayer.Properties.Count())
                {
                    return "Player";
                }
                else if (aiPlayer.Properties.Count() > player.Properties.Count())
                {
                    return "AIPlayer";
                }
                else
                {
                    return "Draw";
                }
            }
        }

        // פעולה לזריקת קובייה
        public Player RollDice(Player player)
        {
            Random random = new Random();
            int dice1 = random.Next(1, 7);
            int dice2 = random.Next(1, 7);
            int diceRoll = dice1 + dice2;

            // Update the player's currentPosition
            player.CurrentPosition += diceRoll;
            player.CurrentPosition %= 40; // Wrap around to start if currentPosition >= 40
            player.LastDiceResult = diceRoll;
            player.Dice1 = dice1;
            player.Dice2 = dice2;

            // Save the new position in the database
            player.UpdatePosition();

            // Return the updated player
            return player;
        }

        public void PayRent(int playerId, int propertyOwnerId, int propertyId)
        {
            DBservices dbServices = new DBservices();
            decimal rentPrice = dbServices.GetRentPrice(propertyId);
            decimal payerBalance = dbServices.GetPlayerBalance(playerId);
            decimal ownerBalance = dbServices.GetPlayerBalance(propertyOwnerId);

            if (payerBalance >= rentPrice)
            {
                dbServices.UpdatePlayerBalance(playerId, payerBalance - rentPrice);  // Deduct rent from payer
                dbServices.UpdatePlayerBalance(propertyOwnerId, ownerBalance + rentPrice);  // Add rent to owner
            }
            else
            {
                throw new Exception("Player cannot afford to pay the rent.");
            }
        }

        //כל הפעולות שנבצע על לוח המשחק SWITCH CASE
        private void HandleSlotActions(int currentPos, string slot)
        {
            switch (currentPos)
            {
                case 0: // when enter the "GO" slot the user gets 200 NIS
                    // Add NIS 200 to the player's balance
                    if (currentPlayerIndex == 0)
                    {
                        player.CurrentBalance += 200;
                    }
                    else
                    {
                        aiPlayer.CurrentBalance += 200;
                    }
                    break;

                // Add other cases for different slot types as needed

                default:
                    // Handle other slot types
                    break;
            }
        }

        //פונקציה שמדלגת על 3 טורות
        private void SkipTurns(int turnsToSkip)
        {
            // Logic to skip turns for the current player
            // For example, you can increment currentPlayerIndex to move to the next player
            for (int i = 0; i < turnsToSkip; i++)
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % 2;
            }
        }

        //האם הנכס שייך למישהו אחר ? לעבור על מערך הנכסים ולבדוק אם קיים.
        private bool IsPropertyOwnedByOtherPlayer(Property property)
        {
            // Check if the property belongs to the other player
            if (currentPlayerIndex == 0)
            {
                // Check if the property exists in the AI player's list of properties
                foreach (var prop in aiPlayer.Properties)
                {
                    if (prop.PropertyId == property.PropertyId)
                    {
                        return true; // Property belongs to the AI player
                    }
                }
            }
            else
            {
                // Check if the property exists in the player's list of properties
                foreach (var prop in player.Properties)
                {
                    if (prop.PropertyId == property.PropertyId)
                    {
                        return true; // Property belongs to the player
                    }
                }
            }

            return false; // Property does not belong to the other player
        }

        // מימוש פונקציה הסתברותית ששחקן האיהיי יקנה את הנכס
        private void ActivateAIPlayerFunctionForProperty(Property property)
        {
            // Implement logic for AI player's actions when landing on an property slot
        }

        //מימוש פונקציה הסתברותית ששחקן האיהי יענה נכון על כרטיס הידעת
        private void ActivateAIPlayerFunctionForKnowledge()
        {
            // Implement logic for AI player's actions when landing on a knowledge slot
        }

        //בכל מקום שנבצע שינוי של כסף נקרא לפונקציה זו
        private void UpdatePlayerDetails(double price)
        {
            if (currentPlayerIndex == 0)
            {
                // Update player details
                player.CurrentBalance -= price; // For example, deduct 100 from the player's balance
            }
            else
            {
                // Update AIPlayer details
                aiPlayer.CurrentBalance -= price; // For example, deduct 100 from the AI player's balance
            }
        }

        public void ApplyCommandCardEffect(int cardId, int playerId)
        {
            var card = Card.GetCardById(cardId);
            var player = new Player().Read().FirstOrDefault(p => p.PlayerId == playerId);

            if (card is CommandCard commandCard)
            {
                if (commandCard.Description.Contains("הרוויח"))
                {
                    player.CurrentBalance += commandCard.Amount;
                }
                else if (commandCard.Description.Contains("שלם"))
                {
                    player.CurrentBalance -= commandCard.Amount;
                }
                player.Update();
            }
        }

        public void ApplySurpriseCardEffect(int cardId, int playerId)
        {
            var card = Card.GetCardById(cardId);
            var player = new Player().Read().FirstOrDefault(p => p.PlayerId == playerId);

            if (card is SurpriseCard surpriseCard)
            {
                if (surpriseCard.Description.Contains("קבל") || surpriseCard.Description.Contains("הרוויח"))
                {
                    player.CurrentBalance += surpriseCard.Amount;
                }
                else if (surpriseCard.Description.Contains("שלם") || surpriseCard.Description.Contains("השקיעו") || surpriseCard.Description.Contains("הפסדת"))
                {
                    player.CurrentBalance -= surpriseCard.Amount;
                }
                player.Update();
            }
        }

        public void ApplyDidYouKnowCardEffect(int cardId, int playerId, string selectedAnswer)
        {
            var card = Card.GetCardById(cardId);
            var player = new Player().Read().FirstOrDefault(p => p.PlayerId == playerId);

            if (card is DidYouKnowCard didYouKnowCard)
            {
                player.ApplyCardEffect(didYouKnowCard, selectedAnswer);
                player.Update(); // עדכון פרטי השחקן
            }
        }

        // פעולה להפסקת משחק
        public void PauseGame()
        {
            game.GameStatus = "Completed";
            Console.WriteLine("המשחק הסתיים.  !");
        }

        // פעולה להמשך משחק
        public void ContinueGame()
        {
            // לוגיקת המשך משחק
        }
    }
}
