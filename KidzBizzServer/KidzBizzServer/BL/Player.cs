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
        List<Property> properties = new List<Property>();

        public Player(int playerId, User user, int currentPosition, double currentBalance, string playerStatus, int lastDiceResult, List<Property> properties)
        {
            this.playerId = playerId;
            this.user = user;
            this.currentPosition = currentPosition;
            this.currentBalance = currentBalance;
            this.playerStatus = playerStatus;
            this.lastDiceResult = lastDiceResult;
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

        public List<Property> Properties { get => properties; set => properties = value; }


        // מיישמת את ההשפעות של כרטיס הפתעה
        public void ApplySurpriseEffect(Card card)
        {
            // הפעולות המוסיפות כסף
            if (card.Description.Contains("קבל") || card.Description.Contains("הרווח"))
            {
                this.currentBalance += card.Amount;
            }
            // הפעולות המחסירות כסף
            else if (card.Description.Contains("שלם") || card.Description.Contains("השקיעו") || card.Description.Contains("הפסדת "))
            {
                this.currentBalance -= card.Amount;
            }
            // פעולות המשפיעות על ערך הנכסים
            if (card.Description.Contains("ערכי הנכסים יורדים"))
            {
                int percentage = ExtractPercentage(card.Description);
                foreach (var property in properties)
                {
                    property.PropertyPrice -= property.PropertyPrice * percentage / 100;
                }
            }
            // מיוחד: משחק קוביות עם מתחרה
            if (card.Description.Contains("בחר מתחרה אחד"))
            {
                PlayDiceWithRival();
            }
        }

        private int ExtractPercentage(string description)
        {
            var match = Regex.Match(description, @"\d+%");
            return int.Parse(match.Value.TrimEnd('%'));
        }

        private void PlayDiceWithRival()
        {
            // יש לממש את פונקציית ההטלות בהתאם ללוגיקה של המשחק
        }

        // קריאה ועדכון נתוני שחקן מהדאטאבייס

        public List<Player> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadPlayers();
        }
        public bool Insert()
        {
            DBservices dbs = new DBservices();
            if (!CheckPlayer(this.playerId))
            {
                Player insertedPlayer = dbs.InsertPlayer(this);  
                return insertedPlayer != null;  
            }
            return false;  
        }


        public bool UpdatePlayer(int playerId)
        {
            DBservices dbs = new DBservices();
            if (CheckPlayer(playerId))
            {
                Player updatedPlayer = dbs.UpdatePlayer(this);  
                return updatedPlayer != null; 
            }
            return false;  
        }


        private bool CheckPlayer(int playerId)
        {
            DBservices dbs = new DBservices();
            List<Player> players = dbs.ReadPlayers();
            foreach (Player player in players)
            {
                if (player.PlayerId == playerId)
                {
                    return true;  // Player exists
                }
            }
            return false;  // Player does not exist
        }
    }
}





    

