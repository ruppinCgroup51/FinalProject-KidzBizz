namespace KidzBizzServer.BL
{
    public class Player
    {
        int playerId;
        User user;
        int currentPosition;
        double currentBalance;
        string playerStatus;
        int lastDiceResult;
        int totalGamesPlayed;
        int totalPropertiesOwned;
        int totalWins;
        int totalLosses;

        List<Property> properties = new List<Property>();
     

        public Player(int playerId, User user, int currentPosition, double currentBalance, string playerStatus, int lastDiceResult, List<Property> properties)
        {
            this.playerId = playerId;
            this.user = user;
            this.currentPosition = currentPosition;
            this.currentBalance = currentBalance;
            this.playerStatus = playerStatus;
            this.lastDiceResult = lastDiceResult;
            this.totalGamesPlayed = 0;
            this.totalPropertiesOwned = 0;
            this.totalWins = 0;
            this.totalLosses = 0;

            this.properties = properties;
        }

        public Player()
        {

        }

        public int PlayerId { get => playerId; set => playerId = value; }
        public User User { get => user; set => user = value; }
        public int CurrentPosition { get => currentPosition; set => currentPosition = value; }
        public double CurrentBalance { get => currentBalance; set => currentBalance = value; }
        public string PlayerStatus { get => playerStatus; set => playerStatus = value; }
        public int LastDiceResult { get => lastDiceResult; set => lastDiceResult = value; }
        public int TotalGamesPlayed { get => totalGamesPlayed; set => totalGamesPlayed = value; }
        public int TotalPropertiesOwned { get => totalPropertiesOwned; set => totalPropertiesOwned = value; }
        public int TotalWins { get => totalWins; set => totalWins = value; }
        public int TotalLosses { get => totalLosses; set => totalLosses = value; }


        public List<Property> Properties { get => properties; set => properties = value; }


        public List<Player> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadPlayers();
        }
    }
}
