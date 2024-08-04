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
        PlayerStatistics statistics;



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
            this.statistics = new PlayerStatistics();

        }

        public Player()
        {
            this.statistics = new PlayerStatistics();

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

        public PlayerStatistics Statistics { get => statistics; set => statistics = value; } // *** הוספה ***




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

        public void ApplyCardEffect(Card card, string selectedAnswer = "")
        {
            switch (card.Action)
            {
                case CardAction.Command:
                    ApplyCommandCardEffect(card);
                    break;
                case CardAction.Surprise:
                    ApplySurpriseCardEffect(card);
                    break;
                case CardAction.DidYouKnow:
                    ApplyDidYouKnowCardEffect(card, selectedAnswer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // מיישמת את ההשפעות של כרטיס פקודה
        private void ApplyCommandCardEffect(Card card)
        {
            CommandCard commandCard = (CommandCard)card;
            if (commandCard.Description.Contains("התקדם למשבצת"))
            {
                if (commandCard.Description.Contains("הידעת הקרובה"))
                {
                    int targetPosition = FindNearestPosition("ידעת");
                    MoveToPosition(targetPosition);
                }
                else if (commandCard.Description.Contains("תיבת הפתעות הקרובה"))
                {
                    int targetPosition = FindNearestPosition("תיבת הפתעה");
                    MoveToPosition(targetPosition);
                    DrawSurpriseCard();
                }
                else if (commandCard.Description.Contains("הפקודה הקרובה"))
                {
                    int targetPosition = FindNearestPosition("פקודה");
                    MoveToPosition(targetPosition);
                }
            }
            else if (commandCard.Description.Contains("דלג"))
            {
                SkipNextTurn();
                if (commandCard.Description.Contains("הרוויח"))
                {
                    this.currentBalance += commandCard.Amount;
                }
            }
            else if (commandCard.Description.Contains("הרוויח"))
            {
                this.currentBalance += commandCard.Amount;
            }
            else if (commandCard.Description.Contains("שלם"))
            {
                this.currentBalance -= commandCard.Amount;
            }
            else if (commandCard.Description.Contains("בחר שחקן כדי לדלג על התור הבא שלו"))
            {
                // יש לממש את הלוגיקה לבחירת שחקן ודילוג על התור הבא שלו
            }
            else if (commandCard.Description.Contains("קלף הגנה"))
            {
                // יש לממש את הלוגיקה לקלף הגנה
            }
            else if (commandCard.Description.Contains("אם תפתרו נכון את השאלה הרוויחו"))
            {
                // יש לממש את הלוגיקה לכרטיס מחקר ופיתוח
            }
        }

        // מיישמת את ההשפעות של כרטיס הפתעה
        private void ApplySurpriseCardEffect(Card card)
        {
            SurpriseCard surpriseCard = (SurpriseCard)card;
            if (surpriseCard.Description.Contains("קבל") || surpriseCard.Description.Contains("הרוויח"))
            {
                this.currentBalance += surpriseCard.Amount;
            }
            else if (surpriseCard.Description.Contains("שלם") || surpriseCard.Description.Contains("השקיעו") || surpriseCard.Description.Contains("הפסדת"))
            {
                this.currentBalance -= surpriseCard.Amount;
            }
            if (surpriseCard.Description.Contains("ערכי הנכסים יורדים"))
            {
                int percentage = ExtractPercentage(surpriseCard.Description);
                foreach (var property in properties)
                {
                    property.PropertyPrice -= property.PropertyPrice * percentage / 100;
                }
            }
            if (surpriseCard.Description.Contains("בחר מתחרה אחד"))
            {
                PlayDiceWithRival();
            }
        }

        // מיישמת את ההשפעות של כרטיס הידעת
        private void ApplyDidYouKnowCardEffect(Card card, string selectedAnswer)
        {
            DidYouKnowCard didYouKnowCard = (DidYouKnowCard)card;
            if (selectedAnswer == didYouKnowCard.CorrectAnswer)
            {
                this.currentBalance += 300; // מוסיף כסף אם התשובה נכונה
            }
        }


        // מוצאת את המיקום הקרוב ביותר לפי סוג הכרטיס
        private int FindNearestPosition(string type)
        {
            int currentPosition = this.CurrentPosition;
            int[] positions;

            switch (type)
            {
                case "ידעת":
                    positions = new int[] { 8, 18, 23, 33, 39 };
                    break;
                case "הפתעה":
                    positions = new int[] { 6, 16, 26, 36 };
                    break;
                case "סיכוי":
                    positions = new int[] { 3, 13, 21, 28, 37 };
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
        public void UpdateStatistics()
        {
            DBservices dbs = new DBservices();
            dbs.UpdatePlayerStatistics(this.playerId, this.statistics);
        }

        public void IncrementLosses()
        {
            statistics.TotalLosses++;
            UpdateStatistics();
        }


        public void IncrementGamesPlayed()
        {
            statistics.TotalGamesPlayed++;
            UpdateStatistics();
        }

        public void UpdateMoneyAndProperties(double money, int propertiesCount)
        {
            statistics.TotalMoney = ((statistics.TotalMoney * (statistics.TotalGamesPlayed - 1)) + money) / statistics.TotalGamesPlayed;
            statistics.TotalPropertiesOwned = ((statistics.TotalPropertiesOwned * (statistics.TotalGamesPlayed - 1)) + propertiesCount) / statistics.TotalGamesPlayed;
            UpdateStatistics();
        }
    }
}









