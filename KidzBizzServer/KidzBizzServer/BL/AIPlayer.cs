//using System;

//namespace KidzBizzServer.BL
//{
//    public enum PlayerType
//    {
//        Conservative,
//        Adventurous,
//        Balanced
//    }

//    public class AIPlayer
//    {
//        private PlayerType playerType;
//        private List<bool> decisionHistory;
        
//        public AIPlayer(PlayerType type)
//        {
//            playerType = type;
//            decisionHistory = new List<bool>();
//        }

//        // Method to set the player type randomly
//        public PlayerType SetPlayerType()
//        {
//            Random random = new Random();
//            int playerTypeIndex = random.Next(3);
//            playerType = (PlayerType)playerTypeIndex;
//            return playerType;
//        }

//        // Method to make a decision based on the player type
//        public void MakeDecision(GameState gameState)
//        {
//            switch (playerType)
//            {
//                case PlayerType.Conservative:
//                    MakeConservativeDecision(gameState);
//                    break;
//                case PlayerType.Adventurous:
//                    MakeAdventurousDecision(gameState);
//                    break;
//                case PlayerType.Balanced:
//                    MakeBalancedDecision(gameState);
//                    break;
//                default:
//                    Console.WriteLine("Invalid player type.");
//                    break;
//            }
//        }

//        private void MakeConservativeDecision(GameState gameState)
//        {
//            // Implement decision-making logic for a conservative player
//            Random random = new Random();
//            bool buyAsset = random.Next(4) == 0; // 25% chance of buying asset
//            decisionHistory.Add(buyAsset);
//            Console.WriteLine("Conservative player decides to " + (buyAsset ? "save money." : "buy an asset."));
//        }

//        private void MakeAdventurousDecision(GameState gameState)
//        {
//            // Implement decision-making logic for an adventurous player
//            Random random = new Random();
//            bool buyAsset = random.Next(2) == 0; // 50% chance of buying asset
//            decisionHistory.Add(buyAsset);
//            Console.WriteLine("Adventurous player decides to " + (buyAsset ? "buy an asset." : "save money."));
//        }

//        private void MakeBalancedDecision(GameState gameState)
//        {
//            // Implement decision-making logic for a balanced player
//            Random random = new Random();
//            bool buyAsset = random.Next(3) == 0; // 33% chance of buying asset
//            decisionHistory.Add(buyAsset);
//            Console.WriteLine("Balanced player decides to " + (buyAsset ? "buy an asset." : "save money."));
//        }


//        // Method to get the last n decisions
//        public List<bool> GetLastNDecisions(int n)
//        {
//            if (decisionHistory.Count <= n)
//            {
//                return decisionHistory;
//            }
//            else
//            {
//                int startIndex = decisionHistory.Count - n;
//                return decisionHistory.GetRange(startIndex, n);
//            }
//        }

//        public void PrintDecisionHistory()
//        {
//            Console.WriteLine("Decision history for " + playerType + " player:");
//            foreach (bool decision in decisionHistory)
//            {
//                Console.WriteLine(decision ? "Buy asset" : "Save money");
//            }
//        }   

      
//    }



//}


//}
