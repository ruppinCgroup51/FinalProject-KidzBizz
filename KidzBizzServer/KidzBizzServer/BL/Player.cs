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
        //רשימה נכסים

        public Player(int playerId, User user, int currentPosition, double currentBalance, string playerStatus, int lastDiceResult)
        {
            this.playerId = playerId;
            this.user = user;
            this.currentPosition = currentPosition;
            this.currentBalance = currentBalance;
            this.playerStatus = playerStatus;
            this.lastDiceResult = lastDiceResult;
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


        public List<Player> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadPlayers();
        }
    }
}
