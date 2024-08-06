//using System;
//using System.Collections.Generic;

//namespace KidzBizzServer.BL
//{
//    // קוד זה מגדיר אנומרציה לסוגי השחקנים
//    public enum PlayerType
//    {
//        Conservative,  // שחקן שמרני
//        Adventurous,   // שחקן הרפתקן
//        Balanced       // שחקן מאוזן
//    }

//    // מחלקת AIPlayer מייצגת שחקן אוטונומי במערכת
//    public class AIPlayer : Player
//    {
//        // תכונה ציבורית לקבלת סוג השחקן
//        public PlayerType PlayerType { get; private set; }
//        private static Random random = new Random();

//        public AIPlayer()
//        {
//            PlayerType = (PlayerType)random.Next(0, 3); // הגרלת סוג השחקן
//        }

//        public class GameState
//        {
//            public double CurrentPropertyPrice { get; set; }
//            public double CurrentRentPotential { get; set; }
//        }

//        public AIPlayer(int playerId, User user, int currentPosition, double currentBalance, string playerStatus, int lastDiceResult, List<Property> properties, int dice1, int dice2, PlayerType type)
//           : base(playerId, user ?? new User
//           {
//               Gender = "Not specified",
//               LastName = "AI",
//               Password = "password",
//               Username = "AIPlayer",
//               FirstName = "AI",
//               AvatarPicture = "https://robohash.org/avatar1"
//           }, currentPosition, currentBalance, playerStatus, lastDiceResult, properties, dice1, dice2)
//        {
//            PlayerType = type;
//        }

//        public AIPlayer(PlayerType type)
//        {
//            PlayerType = type;
//        }

//        // שיטה לביצוע פעולה בהתאם לסוג השחקן
//        public void PerformAction(PlayerType)
//        {
//            switch (PlayerType)
//            {
//                case PlayerType.Conservative:
                    
//                    break;
//                case PlayerType.Adventurous:
                   
//                    break;
//                case PlayerType.Balanced:
                    
//                    break;
//            }
//        }

//        // שיטה לביצוע פעולה עבור שחקן שמרני
//        private void PerformConservativeAction(GameState gameState, List<PlayerActionData> historicalData)
//        {
//            double baseProbability = 0.25; // סיכוי נמוך יותר לרכישת נכסים
//            MakeDecision(gameState, baseProbability, historicalData);
//        }

//        // שיטה לביצוע פעולה עבור שחקן הרפתקן
//        private void PerformAdventurousAction(GameState gameState, List<PlayerActionData> historicalData)
//        {
//            double baseProbability = 0.50; // סיכוי גבוה יותר לרכישת נכסים
//            MakeDecision(gameState, baseProbability, historicalData);
//        }

//        // שיטה לביצוע פעולה עבור שחקן מאוזן
//        private void PerformBalancedAction(GameState gameState, List<PlayerActionData> historicalData)
//        {
//            double baseProbability = 0.33; // סיכוי ממוצע לרכישת נכסים
//            MakeDecision(gameState, baseProbability, historicalData);
//        }

//        // שיטה להחלטה אם לקנות נכס בהתאם להסתברות שנקבעה
//        private void MakeDecision(GameState gameState, double baseProbability, List<PlayerActionData> historicalData)
//        {
//            double probability = CalculateBuyProbability(gameState, baseProbability, historicalData);
//            bool buyAsset = new Random().NextDouble() < probability;
//            Console.WriteLine($"{PlayerType} player decides to " + (buyAsset ? "buy an asset." : "save money."));
//        }

//        // שיטה לחישוב ההסתברות לרכישת נכס על בסיס מודל רגרסיה לוגיסטית
//        private double CalculateBuyProbability(GameState gameState, double baseProbability, List<PlayerActionData> historicalData)
//        {
//            double currentCash = this.CurrentBalance; // מזומן נוכחי של השחקן
//            int numberOfOwnedProperties = this.Properties.Count; // מספר הנכסים שבבעלות השחקן
//            double propertyCost = gameState.CurrentPropertyPrice; // עלות הנכס הנוכחי
//            double rentPotential = gameState.CurrentRentPotential; // פוטנציאל השכירות של הנכס הנוכחי

//            // יחס הנכסים למזומן
//            double assetToCashRatio = (numberOfOwnedProperties > 0) ? currentCash / numberOfOwnedProperties : currentCash;
//            // יחס עלות הפיתוח מול פוטנציאל השכירות
//            double developmentCostVsRentRatio = (propertyCost > 0) ? rentPotential / propertyCost : 0;

//            // חישוב x על פי הנתונים הקיימים
//            double x = baseProbability
//                       + 0.5 * (currentCash / 1000)
//                       + 0.2 * numberOfOwnedProperties
//                       + 0.3 * developmentCostVsRentRatio;

//            // אם קיימים נתונים היסטוריים, נוסיף אותם לחישוב ההסתברות
//            if (historicalData != null && historicalData.Count > 0)
//            {
//                foreach (var data in historicalData)
//                {
//                    // עדכון x על בסיס נתונים היסטוריים
//                    x += data.Beta0
//                        + data.Beta1 * currentCash
//                        + data.Beta2 * numberOfOwnedProperties
//                        + data.Beta3 * propertyCost
//                        + data.Beta4 * rentPotential;
//                }
//            }

//            // חישוב ההסתברות הסופית באמצעות מודל הרגרסיה הלוגיסטית
//            return 1 / (1 + Math.Exp(-x));
//        }
//    }
//}

//namespace KidzBizzServer.BL
//{
//    // מחלקת PlayerActionData מייצגת נתונים היסטוריים של פעולות שחקן
//    public class PlayerActionData
//    {
//        public double Beta0 { get; set; }
//        public double Beta1 { get; set; }
//        public double Beta2 { get; set; }
//        public double Beta3 { get; set; }
//        public double Beta4 { get; set; }
//    }
//}
