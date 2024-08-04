using System.Diagnostics.Metrics;

namespace KidzBizzServer.BL
{
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

                string winner = DetermineWinner();
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

            // כל הפעולות שנבצע על לוח המשחק SWITCH CASE
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

                    case 15:
                        if ((currentPlayerIndex == 0 && player.CurrentPosition == 30) || (currentPlayerIndex == 1 && aiPlayer.CurrentPosition == 30))
                        {
                            SkipTurns(3);
                        }
                        break;

                    // Add other cases for different slot types as needed

                    default:
                        // Handle other slot types
                        break;
                }
            }

            // פונקציה שמדלגת על 3 טורות
            private void SkipTurns(int turnsToSkip)
            {
                // Logic to skip turns for the current player
                for (int i = 0; i < turnsToSkip; i++)
                {
                    currentPlayerIndex = (currentPlayerIndex + 1) % 2;
                }
            }

            // האם הנכס שייך למישהו אחר? לעבור על מערך הנכסים ולבדוק אם קיים.
            private bool IsPropertyOwnedByOtherPlayer(Property property)
            {
                if (currentPlayerIndex == 0)
                {
                    // Check if the property exists in the AI player's list of properties
                    //foreach (var prop in aiPlayer.Properties)
                    //{
                    //    if (prop.PropertyId == property.PropertyId)
                    //    {
                    //        return true; // Property belongs to the AI player
                    //    }
                    //}
                }
                else
                {
                    // Check if the property exists in the player's list of properties
                    //foreach (var prop in player.Properties)
                    //{
                    //    if (prop.PropertyId == property.PropertyId)
                    //    {
                    //        return true; // Property belongs to the player
                    //    }
                    //}
                }

                return false; // Property does not belong to the other player
            }

            // פונקציה להחלטה אם לקנות נכס על ידי AI
            private void ActivateAIPlayerFunctionForProperty(Property property)
            {
                // Implement logic for AI player's actions when landing on a property slot
            }

            // פונקציה להחלטה אם לענות על שאלה על ידי AI
            private void ActivateAIPlayerFunctionForKnowledge()
            {
                // Implement logic for AI player's actions when landing on a knowledge slot
            }

            // בכל מקום שנבצע שינוי של כסף נקרא לפונקציה זו
            private void UpdatePlayerDetails(double price)
            {
                if (currentPlayerIndex == 0)
                {
                    // Update player details
                    player.CurrentBalance -= price; // For example, deduct 100 from the player's balance
                                                    // Update other player details as needed
                }
                else
                {
                    // Update AIPlayer details
                    aiPlayer.CurrentBalance -= price; // For example, deduct 100 from the AI player's balance
                                                      // Update other AIPlayer details as needed
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
                game.GameStatus = "Paused";
                Console.WriteLine("המשחק הופסק.");
            }

            // פעולה להמשך משחק
            public void ContinueGame()
            {
                game.GameStatus = "Active";
                Console.WriteLine("המשחק נמשך.");
            }
        }

        // AIPlayer Class
        public enum PlayerType
        {
            Conservative,  // שחקן שמרני
            Adventurous,   // שחקן הרפתקן
            Balanced       // שחקן מאוזן
        }

        public class AIPlayer : Player
        {
            public PlayerType PlayerType { get; private set; }
            private static Random random = new Random();

            public AIPlayer()
            {
                PlayerType = (PlayerType)random.Next(0, 3); // הגרלת סוג השחקן
            }

            public class GameState
            {
                public double CurrentPropertyPrice { get; set; }
                public double CurrentRentPotential { get; set; }
            }

            public AIPlayer(int playerId, User user, int currentPosition, double currentBalance, string playerStatus, int lastDiceResult, List<Property> properties, int dice1, int dice2, PlayerType type)
               : base(playerId, user ?? new User
               {
                   Gender = "Not specified",
                   LastName = "AI",
                   Password = "password",
                   Username = "AIPlayer",
                   FirstName = "AI",
                   AvatarPicture = "https://robohash.org/avatar1"
               }, currentPosition, currentBalance, playerStatus, lastDiceResult, properties, dice1, dice2)
            {
                PlayerType = type;
            }

            public AIPlayer(PlayerType type)
            {
                PlayerType = type;
            }

            public void PerformAction(GameState gameState, List<PlayerActionData> historicalData = null)
            {
                switch (PlayerType)
                {
                    case PlayerType.Conservative:
                        PerformConservativeAction(gameState, historicalData);
                        break;
                    case PlayerType.Adventurous:
                        PerformAdventurousAction(gameState, historicalData);
                        break;
                    case PlayerType.Balanced:
                        PerformBalancedAction(gameState, historicalData);
                        break;
                }
            }

            private void PerformConservativeAction(GameState gameState, List<PlayerActionData> historicalData)
            {
                double baseProbability = 0.25; // סיכוי נמוך יותר לרכישת נכסים
                MakeDecision(gameState, baseProbability, historicalData);
            }

            private void PerformAdventurousAction(GameState gameState, List<PlayerActionData> historicalData)
            {
                double baseProbability = 0.50; // סיכוי גבוה יותר לרכישת נכסים
                MakeDecision(gameState, baseProbability, historicalData);
            }

            private void PerformBalancedAction(GameState gameState, List<PlayerActionData> historicalData)
            {
                double baseProbability = 0.33; // סיכוי ממוצע לרכישת נכסים
                MakeDecision(gameState, baseProbability, historicalData);
            }

            private void MakeDecision(GameState gameState, double baseProbability, List<PlayerActionData> historicalData)
            {
                double probability = CalculateBuyProbability(gameState, baseProbability, historicalData);
                bool buyAsset = new Random().NextDouble() < probability;
                Console.WriteLine($"{PlayerType} player decides to " + (buyAsset ? "buy an asset." : "save money."));
            }

            private double CalculateBuyProbability(GameState gameState, double baseProbability, List<PlayerActionData> historicalData)
            {
                double currentCash = this.CurrentBalance;
                int numberOfOwnedProperties = this.Properties.Count;
                double propertyCost = gameState.CurrentPropertyPrice;
                double rentPotential = gameState.CurrentRentPotential;

                double assetToCashRatio = (numberOfOwnedProperties > 0) ? currentCash / numberOfOwnedProperties : currentCash;
                double developmentCostVsRentRatio = (propertyCost > 0) ? rentPotential / propertyCost : 0;

                double x = baseProbability
                           + 0.5 * (currentCash / 1000)
                           + 0.2 * numberOfOwnedProperties
                           + 0.3 * developmentCostVsRentRatio;

                if (historicalData != null && historicalData.Count > 0)
                {
                    foreach (var data in historicalData)
                    {
                        x += data.Beta0
                            + data.Beta1 * currentCash
                            + data.Beta2 * numberOfOwnedProperties
                            + data.Beta3 * propertyCost
                            + data.Beta4 * rentPotential;
                    }
                }

                return 1 / (1 + Math.Exp(-x));
            }
        }

        public class PlayerActionData
        {
            public double Beta0 { get; set; }
            public double Beta1 { get; set; }
            public double Beta2 { get; set; }
            public double Beta3 { get; set; }
            public double Beta4 { get; set; }
        }
    }
}