//namespace KidzBizzServer.BL
//{
//    public class GameState
//    {
//        // Define properties to represent the current state of the game
//        public int PlayerMoney { get; set; }
//        public int OpponentMoney { get; set; }
//        public int PlayerAssets { get; set; }
//        public int OpponentAssets { get; set; }
//        // Add more properties as needed

//        // Constructor
//        public GameState(int playerMoney, int opponentMoney, int playerAssets, int opponentAssets)
//        {
//            PlayerMoney = playerMoney;
//            OpponentMoney = opponentMoney;
//            PlayerAssets = playerAssets;
//            OpponentAssets = opponentAssets;
//        }
//    }


//    class Program
//    {
//        static void Main(string[] args)
//        {
//            // Create an instance of the game state

//            GameState gameState = new GameState(//need to insert the data from the game )

//            // Create AI players of different types
//            AIPlayer conservativePlayer = new AIPlayer(PlayerType.Conservative);
//            AIPlayer adventurousPlayer = new AIPlayer(PlayerType.Adventurous);
//            AIPlayer balancedPlayer = new AIPlayer(PlayerType.Balanced);

//            // Make decisions for each player
//            conservativePlayer.MakeDecision(gameState);
//            adventurousPlayer.MakeDecision(gameState);
//            balancedPlayer.MakeDecision(gameState);



//            //// Display decision history for each player
//            //Console.WriteLine("Conservative player decision history:");
//            //foreach (bool decision in conservativePlayer.DecisionHistory)
//            //{
//            //    Console.WriteLine(decision ? "Save money" : "Buy an asset");
//            //}
            
//            //Console.WriteLine("Adventurous player decision history:");
//            //foreach (bool decision in adventurousPlayer.DecisionHistory)
//            //{
//            //    Console.WriteLine(decision ? "Save money" : "Buy an asset");
//            //}
//            //Console.WriteLine()
//        }
//    }

//}
