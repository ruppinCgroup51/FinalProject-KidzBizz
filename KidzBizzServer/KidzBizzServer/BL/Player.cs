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
        int dice1 = 0;
        int dice2 = 0;


        public Player(int playerId, User user, int currentPosition, double currentBalance, string playerStatus, int lastDiceResult, List<Property> properties, int dice1, int dice2)
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
            dbs.UpdatePlayerPosition(this.PlayerId, this.CurrentPosition , this.LastDiceResult);
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



     
        // מיישמת את ההשפעות של כרטיס לפי סוגו
        public void ApplyCardEffect(Card card)
        {
            switch (card.Action)
            {
                case CardAction.Command:
                    ApplyCommandCardEffect(card);
                    break;
                case CardAction.Surprise:
                    ApplySurpriseEffect(card);
                    break;
                case CardAction.DidYouKnow:
                    ApplyDidYouKnowCardEffect(card);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // מיישמת את ההשפעות של כרטיס פקודה
        private void ApplyCommandCardEffect(Card card)
        {
            if (card.Description.Contains("התקדם למשבצת"))
            {
                if (card.Description.Contains("הידעת הקרובה"))
                {
                    int targetPosition = FindNearestPosition("ידעת");
                    MoveToPosition(targetPosition);
                }
                else if (card.Description.Contains("תיבת הפתעות הקרובה"))
                {
                    int targetPosition = FindNearestPosition("תיבת הפתעה");
                    MoveToPosition(targetPosition);
                    DrawSurpriseCard();
                }
                else if (card.Description.Contains("הפקודה הקרובה"))
                {
                    int targetPosition = FindNearestPosition("פקודה");
                    MoveToPosition(targetPosition);
                }
            }
            else if (card.Description.Contains("דלג"))
            {
                SkipNextTurn();
                if (card.Description.Contains("הרוויח"))
                {
                    this.currentBalance += card.Amount;
                }
            }
            else if (card.Description.Contains("הרוויח"))
            {
                this.currentBalance += card.Amount;
            }
            else if (card.Description.Contains("שלם"))
            {
                this.currentBalance -= card.Amount;
            }
            else if (card.Description.Contains("בחר שחקן כדי לדלג על התור הבא שלו"))
            {
                // יש לממש את הלוגיקה לבחירת שחקן ודילוג על התור הבא שלו
            }
            else if (card.Description.Contains("קלף הגנה"))
            {
                // יש לממש את הלוגיקה לקלף הגנה
            }
            else if (card.Description.Contains("אם תפתרו נכון את השאלה הרוויחו"))
            {
                // יש לממש את הלוגיקה לכרטיס מחקר ופיתוח
            }
        }

        // מיישמת את ההשפעות של כרטיס הפתעה
        private void ApplySurpriseEffect(Card card)
        {
            if (card.Description.Contains("קבל") || card.Description.Contains("הרוויח"))
            {
                this.currentBalance += card.Amount;
            }
            else if (card.Description.Contains("שלם") || card.Description.Contains("השקיעו") || card.Description.Contains("הפסדת"))
            {
                this.currentBalance -= card.Amount;
            }
            if (card.Description.Contains("ערכי הנכסים יורדים"))
            {
                int percentage = ExtractPercentage(card.Description);
                foreach (var property in properties)
                {
                    property.PropertyPrice -= property.PropertyPrice * percentage / 100;
                }
            }
            if (card.Description.Contains("בחר מתחרה אחד"))
            {
                PlayDiceWithRival();
            }
        }

        // מיישמת את ההשפעות של כרטיס הידעת
        private void ApplyDidYouKnowCardEffect(Card card)
        {
            // יש לממש את הלוגיקה לכרטיס הידעת
        }

        // מוצאת את המיקום הקרוב ביותר לפי סוג הכרטיס
        private int FindNearestPosition(string type)
        {
            int currentPosition = this.CurrentPosition;
            int[] positions;

            switch (type)
            {
                case "ידעת":
                    positions = new int[] { };
                    break;
                case "תיבת הפתעה":
                    positions = new int[] { 6, 15, 21, 30, 37 };
                    break;
                case "פקודה":
                    positions = new int[] { 2, 9, 13, 19, 32, 38 };
                    break;
                default:
                    positions = new int[] { };
                    break;
            }

            return positions.Where(p => p > currentPosition).OrderBy(p => p).FirstOrDefault();
        }

        // מזיז את השחקן למיקום החדש
        private void MoveToPosition(int targetPosition)
        {
            int steps = targetPosition - this.CurrentPosition;
            this.CurrentPosition = steps;
            UpdatePosition();
        }

        // מדלג על התור הבא
        private void SkipNextTurn()
        {
            // לוגיקה לדילוג על התור הבא
        }

        // שולף קלף הפתעה
        private void DrawSurpriseCard()
        {
            // לוגיקה לשליפת קלף הפתעה
        }

        // מחלץ אחוזים מהתיאור
        private int ExtractPercentage(string description)
        {
            var match = Regex.Match(description, @"\d+%");
            return int.Parse(match.Value.TrimEnd('%'));
        }

        // משחק קוביות עם יריב
        private void PlayDiceWithRival()
        {
            // לוגיקה להטלת קוביות עם יריב לפי חוקי המשחק
        }
    }
}





    

