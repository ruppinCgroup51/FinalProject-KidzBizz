using System.Text.RegularExpressions;

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
        int playerType;
        List<Property> properties = new List<Property>();
        int dice1 = 0;
        int dice2 = 0;



        public Player(int playerId, User user, int currentPosition, double currentBalance, string playerStatus, int lastDiceResult, List<Property> properties, int dice1, int dice2, int playerType)
        {
            this.playerId = playerId;
            this.user = user;
            this.currentPosition = currentPosition;
            this.currentBalance = currentBalance;
            this.playerStatus = playerStatus;
            this.lastDiceResult = lastDiceResult;
            this.properties = properties;
            this.dice1 = dice1;
            this.dice2 = dice2;
            this.playerType = playerType;
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
        public List<Property> Properties { get => properties; set => properties = value; }

        public int Dice1 { get => dice1; set => dice1 = value; }
        public int Dice2 { get => dice2; set => dice2 = value; }

        public int PlayerType { get => playerType; set => playerType = value; }





        public List<Player> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadPlayers();
        }
        public int Insert()
        {
            DBservices dbs = new DBservices();
            return dbs.InsertPlayer(this);

        }
        public void UpdatePosition()
        {
            DBservices dbs = new DBservices();
            dbs.UpdatePlayerPosition(this.PlayerId, this.CurrentPosition, this.LastDiceResult);
        }

        public Player GetPlayerDetails(int playerId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetPlayerById(playerId);

        }


        public Player Update()
        {
            DBservices dbs = new DBservices();
            return dbs.UpdatePlayer(this);

        }

        public decimal GetPlayerBalance(int playerId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetPlayerBalance(playerId);
        }

        public void UpdatePlayerBalance(int playerId, decimal newBalance)
        {
            DBservices dbs = new DBservices();
            dbs.UpdatePlayerBalance(playerId, newBalance);
        }

        public int AddPropertyToPlayer(int playerId, int propertyId)
        {
            DBservices dbs = new DBservices();
            return dbs.AddPropertyToPlayer(playerId, propertyId);
        }

        public List<Property> GetPlayerProperties(int playerId)
        {
            DBservices dbs = new DBservices();
            return dbs.ReadPropertiesByPlayerId(playerId);
        }



        // מזיז את השחקן למיקום החדש
        private void MoveToPosition(int targetPosition)
        {
            int steps = targetPosition - this.CurrentPosition;
            this.CurrentPosition = steps;
            UpdatePosition();
        }
        // מדלג על התור הבא


        public void GainExtraTurn()
        {
            // לוגיקה לקבלת תור נוסף
            Console.WriteLine("זכית בתור נוסף!");
        }

        public void SkipNextTurn()
        {
            // לוגיקה לדילוג על התור הבא
            Console.WriteLine($" הפסדת תור!");
        }

        // משחק קוביות עם יריב
        private void PlayDiceWithRival()
        {
            // לוגיקה להטלת קוביות עם יריב לפי חוקי המשחק
        }


      
    }
}









