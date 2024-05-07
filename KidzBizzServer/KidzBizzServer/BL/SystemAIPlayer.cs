//using System;

//namespace KidzBizzServer.BL
//{
//    public class SystemAIPlayer
//    {
//        private Random _random;

//        public SystemAIPlayer()
//        {
//            _random = new Random();
//        }

//        public bool ShouldBuyProperty(AIPlayer player)
//        {
//            double mean = DetermineMean(player.PlayerType);
//            double stddev = DetermineStdDev(player.PlayerType);
//            double moneyNormalized = (player.Money - mean) / stddev;
//            double probability = NormalDistribution(moneyNormalized);

//            // נקבע שהסיכוי לקנות נכס הוא אם ההסתברות גבוהה מ-0.5
//            return probability > 0.5;
//        }

//        private double NormalDistribution(double value)
//        {
//            return 0.5 * (1 + Math.Erf(value / Math.Sqrt(2)));
//        }

//        private double DetermineMean(PlayerType type)
//        {
//            switch (type)
//            {
//                case PlayerType.Conservative:
//                    return 3000; // שחקנים שמרנים יש להם סף גבוה יותר לקנייה
//                case PlayerType.Adventurous:
//                    return 1500; // שחקנים הרפתקנים מוכנים לקחת סיכון גם בכסף נמוך יותר
//                default:
//                    return 2000; // שחקנים מאוזנים נמצאים באמצע
//            }
//        }

//        private double DetermineStdDev(PlayerType type)
//        {
//            switch (type)
//            {
//                case PlayerType.Conservative:
//                    return 500;
//                case PlayerType.Adventurous:
//                    return 300;
//                default:
//                    return 400;
//            }
//        }
//    }
//}
