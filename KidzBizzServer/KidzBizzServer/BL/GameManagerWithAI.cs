
using System.Diagnostics.Metrics;

namespace KidzBizzServer.BL
{
    public class GameManagerWithAI
    {
        Player player = new Player();
        Player aiPlayer = new Player();
        int currentPlayerIndex;
        int diceRoll;
        Game game = new Game();
        bool skipNextTurn;
        bool extraTurn;

        public GameManagerWithAI(Player player, Player aiPlayer, int currentPlayerIndex, int diceRoll, Game game)
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
        public Player AiPlayer { get => aiPlayer; set => aiPlayer = value; }
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
                PlayerType = 0

            };

            player.Insert();


            Player aiPlayer = new Player
            {
                User = new User
                {
                    UserId = 1016,
                    Gender = "Not specified",
                    LastName = "AI",
                    Password = "password",
                    Username = "AIPlayer",
                    FirstName = "AI",
                    AvatarPicture = "https://robohash.org/avatar1"
                }, // Assuming AI player has user ID 2
                CurrentBalance = Convert.ToDouble(startingMoney),
                CurrentPosition = currentLocation,
                PlayerStatus = "Active",
                LastDiceResult = 0,
                PlayerType = new Random().Next(1, 4)
            };

            aiPlayer.Insert();

            // Randomly decide who starts first
            currentPlayerIndex = new Random().Next(0, 2);
            return new List<Player> { player, aiPlayer };
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
        public void HandleSlotActions(int currentPos, string slot)
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

                //case "מלונות": // when we know the index of hotel we will change it 
                //    // Check if the property is available for purchase
                //    if (currentPlayerIndex == 0)
                //    {
                //        // Determine the property the player landed on based on their current position
                //        Property property = GetPropertyAtPosition(currentPosition);

                //        // Check if the property belongs to the other player
                //        if (IsPropertyOwnedByOtherPlayer(property))
                //        {
                //            // Deduct 10% of the property value from the current player's balance
                //            int rentAmount = (int)(property.PropertyPrice * 0.1);
                //            player.CurrentBalance -= rentAmount;

                //            // Add the rent amount to the other player's balance
                //            aiPlayer.CurrentBalance += rentAmount;
                //        }
                //        else
                //        {
                //            // Prompt the player to buy or not
                //            // For example, you could implement a UI prompt or return a boolean from a function
                //            bool shouldBuy = AskPlayerToBuyProperty(property);
                //            if (shouldBuy)
                //            {
                //                // Deduct the property price from the player's balance and add the property to their list
                //                player.CurrentBalance -= property.Price;
                //                player.Properties.Add(property);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        // Activate AI player's functions for handling property
                //        // This logic should also include checking if the property belongs to the player
                //        // If not, AI player should decide whether to buy or not
                //        ActivateAIPlayerFunctionForProperty(currentPosition);
                //    }
                //    break;

                //case "הידעת":
                //    // Decide whether to answer a question
                //    if (currentPlayerIndex == 0)
                //    {
                //        // Prompt the player to answer a question
                //        // For example, you could implement a UI prompt or return a boolean from a function
                //        bool shouldAnswer = AskPlayerToAnswerQuestion();
                //        if (shouldAnswer)
                //        {
                //            // Display question to the player and handle correct answer
                //            bool answeredCorrectly = DisplayAndCheckQuestion();
                //            if (answeredCorrectly)
                //            {
                //                // Win money if answered correctly
                //                player.CurrentBalance += 100; // For example, add NIS 100
                //            }
                //        }
                //    }
                //    else
                //    {
                //        // Activate AI player's functions for handling knowledge slot
                //        ActivateAIPlayerFunctionForKnowledge();
                //    }
                //    break;

                //נניח שתא מס' 30 זה הכלא אז נפעיל את הפונקציה לעשות סקיפ ל3 טורות
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
        //private bool IsPropertyOwnedByOtherPlayer(Property property)
        //{
        //    // Check if the property belongs to the other player
        //    if (currentPlayerIndex == 0)
        //    {
        //        // Check if the property exists in the AI player's list of properties
        //        //foreach (var property in aiPlayer.Properties)
        //        //{
        //        //    if (property.PropertyId == property.PropertyId)
        //        //    {
        //        //        return true; // Property belongs to the AI player
        //        //    }
        //        //}
        //    }
        //    else
        //    {
        //        // Check if the property exists in the player's list of properties
        //        //foreach (var property in player.Properties)
        //        //{
        //        //    if (property.PropertyId == property.PropertyId)
        //        //    {
        //        //        return true; // Property belongs to the player
        //        //    }
        //        //}
        //    }

        //    return false; // Property does not belong to the other player
        //}


        //בהתאם למיקום לקבל את הנכס שדרכתי עליו,להביא את הדירות מהדאטה בייס
        //private Property GetPropertyAtPosition(int position)
        //{
        //    // Placeholder logic to retrieve property based on position
        //    // You need to implement the actual logic to map positions to propertys
        //    // This could involve accessing a predefined list of propertys or querying a database

        //    // For demonstration purposes, let's assume propertys are predefined
        //    // and we have a list of propertys where each property corresponds to a specific position

        //    // Example hardcoded list of propertys (for demonstration only)
        //    //List<Property> propertys = new List<Property>
        //    //  {

        //    //         new Property { PropertyId = 1, Name = "Park Lane", Price = 350 },
        //    //           new Property { PropertyId = 2, Name = "Mayfair", Price = 400 },
        //    //        // Add more propertys as needed
        //    //     };

        //    // Retrieve the property at the specified position
        //    // Assuming position is 0-based index
        //    //int index = position % propertys.Count; // Modulo operation to handle circular board
        //    //return propertys[index];
        //}


        // מימוש פונקציה שתשאל את השחקן האם לקנות את הנכס
        //private bool AskPlayerToBuyProperty(Property property)
        //{
        //    // Implement logic to prompt the player to buy or not (e.g., show a UI dialog)
        //    // Return true if the player chooses to buy, false otherwise
        //}


        //private bool AskPlayerToAnswerQuestion()
        //{
        //    // Implement logic to prompt the player to answer a question (e.g., show a UI dialog)
        //    // Return true if the player chooses to answer, false otherwise
        //}

        //מימוש פונקציה שתראה על המסך את השאלה לשחקן
        //private bool DisplayAndCheckQuestion()
        //{
        //    // Implement logic to display a question to the player and check if they answered correctly
        //    // Return true if the player answers correctly, false otherwise
        //}

        // מימוש פונקציה הסתברותית ששחקן האיהיי יקנה את הנכס
        //private void ActivateAIPlayerFunctionForProperty(Property property)
        //{
        //    // Implement logic for AI player's actions when landing on an property slot
        //}


        ////מימוש פונקציה הסתברותית ששחקן האיהי יענה נכון על כרטיס הידעת
        //private void ActivateAIPlayerFunctionForKnowledge()
        //{
        //    // Implement logic for AI player's actions when landing on a knowledge slot
        //}


        //בכל מקום שנבצע שינוי של כסף נקרא לפונקציה זו


        // מתודה לטיפול בפתיחת כרטיס


        public void HandleCardAction(int cardId, int playerId, string selectedAnswer, int currentPosition)
        {
            var card = Card.GetCardById(cardId);
            var player = new Player().GetPlayerDetails(playerId);

            if (player == null)
            {
                throw new Exception("Player not found.");
            }

            if (card is CommandCard commandCard)
            {
                ApplyCommandCardEffect(commandCard, player, currentPosition);
            }
            else if (card is SurpriseCard surpriseCard)
            {
                ApplySurpriseCardEffect(surpriseCard, player, currentPosition);
            }
            else if (card is DidYouKnowCard didYouKnowCard)
            {
                ApplyDidYouKnowCardEffect(didYouKnowCard, player, selectedAnswer, currentPosition);
            }
        }

        private void UpdatePlayerDetails(double price)
        {
            if (currentPlayerIndex == 0)
            {
                player.CurrentBalance -= price;
            }
            else
            {
                aiPlayer.CurrentBalance -= price;
            }
        }

        public void ApplyCommandCardEffect(CommandCard card, Player player, int currentPosition)
        {
            if (card.Description.Contains("הרווחת"))
            {
                player.CurrentBalance += card.Amount;
            }
            else if (card.Description.Contains("הרוויח"))
            {
                player.CurrentBalance -= card.Amount;
            }
            else if (card.Description.Contains("לך לכלא"))
            {
                SkipTurns(3);
                player.CurrentPosition = 30; 
                player.UpdatePosition();
            }
            else if (card.Description.Contains("שלם"))
            {
                player.CurrentBalance -= card.Amount;
            }
            else if (card.Description.Contains("הפסדת"))
            {
                var match = System.Text.RegularExpressions.Regex.Match(card.Description, @"(\d+(\.\d+)?)%");
                if (match.Success)
                {
                    double percentage = double.Parse(match.Groups[1].Value);
                    double lossAmount = player.CurrentBalance * (percentage / 100.0);
                    player.CurrentBalance -= lossAmount;
                }
            }
            else if (card.Description.Contains("התקדם למשבצת הפקודה הקרובה"))
            {
                int nearestCommandSlot = FindNearestSlot(currentPosition, "פקודה");
                player.CurrentPosition = nearestCommandSlot;
                player.UpdatePosition();
            }
            else if (card.Description.Contains("זכית בתור כפול") || card.Description.Contains("זכית בתור נוסף"))
            {
                GrantExtraTurn(player);
                if (currentPlayerIndex == 0)
                {
                    extraTurn = true;
                }
                else
                {
                    skipNextTurn = true;
                }
            }
            player.Update();
        }

        public void ApplySurpriseCardEffect(SurpriseCard card, Player player, int currentPosition)
        {
            if (card.Description.Contains("קבל") || card.Description.Contains("הרוויח") || card.Description.Contains("הרווחת") || card.Description.Contains("הרווח"))
            {
                player.CurrentBalance += card.Amount;
            }
            else if (card.Description.Contains("שלם"))
            {
                player.CurrentBalance -= card.Amount;
            }
            else if (card.Description.Contains("הפסדת"))
            {
                var match = System.Text.RegularExpressions.Regex.Match(card.Description, @"(\ד+(\.\ד+)?)%");
                if (match.Success)
                {
                    double percentage = double.Parse(match.Groups[1].Value);
                    double lossAmount = player.CurrentBalance * (percentage / 100.0);
                    player.CurrentBalance -= lossAmount;
                }
            }
            else if (card.Description.Contains("התקדם למשבצת הפתעה הקרובה"))
            {
                int nearestSurpriseSlot = FindNearestSlot(currentPosition, "הפתעה");
                player.CurrentPosition = nearestSurpriseSlot;
                player.UpdatePosition();
            }
            else if (card.Description.Contains("זכית בתור כפול") || card.Description.Contains("זכית בתור נוסף"))
            {

                GrantExtraTurn(player);
                if (currentPlayerIndex == 0)
                {
                    extraTurn = true;
                }
                else
                {
                    skipNextTurn = true;
                }
            }
            player.Update();
        }

        public void ApplyDidYouKnowCardEffect(DidYouKnowCard card, Player player, string selectedAnswer, int currentPosition)
        {
            if (selectedAnswer == card.CorrectAnswer)
            {
                player.CurrentBalance += 300;
            }
            else if (card.Description.Contains("התקדם למשבצת הידעת הקרובה"))
            {
                int nearestDidYouKnowSlot = FindNearestSlot(currentPosition, "ידעת");
                player.CurrentPosition = nearestDidYouKnowSlot;
                player.UpdatePosition();
            }
         
            player.Update();
        }

        public int FindNearestSlot(int currentPosition, string slotType)
        {
            int[] slots;

            switch (slotType)
            {
                case "פקודה":
                    slots = new int[] { 3, 13, 21, 28, 37 };
                    break;
                case "הפתעה":
                    slots = new int[] { 6, 16, 26, 36 };
                    break;
                case "ידעת":
                    slots = new int[] { 8, 18, 23, 26, 33, 39 };
                    break;
                default:
                    slots = new int[] { };
                    break;
            }

            return slots.Where(p => p > currentPosition).OrderBy(p => p).FirstOrDefault();
        }

        private void GrantExtraTurn(Player currentPlayer)
        {
            if (currentPlayer.PlayerId == player.PlayerId)
            {
                extraTurn = true;
                Console.WriteLine("קיבלת תור נוסף!");
            }
            else
            {
                extraTurn = true;
                Console.WriteLine("AI קיבל תור נוסף!");
            }
        }

        private void SkipNextTurn(Player currentPlayer)
        {
            if (currentPlayer.PlayerId == player.PlayerId)
            {
                skipNextTurn = true;
                Console.WriteLine("השחקן קיבל דילוג תור!");
            }
            else
            {
                skipNextTurn = true;
                Console.WriteLine("AI קיבל דילוג תור!");
            }
        }

        private void ExecuteTurn()
        {
            if (skipNextTurn)
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % 2;
                skipNextTurn = false;
            }
            else if (!extraTurn)
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % 2;
            }

            extraTurn = false;
        }

        // פעולה להפסקת משחקc
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