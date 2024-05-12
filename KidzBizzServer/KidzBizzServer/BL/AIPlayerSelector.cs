//using KidzBizzServer.BL;

//public class AIPlayerSelector
//{
//    public AIPlayer DeterminePlayerType(PlayerStatistics stats)
//    {
//        if (stats.TotalGamesPlayed == 0)
//        {
//            return new AIPlayer(RandomPlayerType());
//        }
//        else
//        {
//            return new AIPlayer(SelectBasedOnStatistics(stats));
//        }
//    }

//    private PlayerType RandomPlayerType()
//    {
//        return (PlayerType)new Random().Next(3);
//    }

//    private PlayerType SelectBasedOnStatistics(PlayerStatistics stats)
//    {
//        // \\ מימוש אלגוריתמים לבחירת שחקן ע"פ נתונים
//        double winRate = (double)stats.TotalWins / stats.TotalGamesPlayed;
//        if (winRate > 0.5)
//            return PlayerType.Adventurous;
//        else
//            return PlayerType.Conservative;
//    }
//}
