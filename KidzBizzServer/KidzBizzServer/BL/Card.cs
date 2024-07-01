namespace KidzBizzServer.BL
{
    // Enum לסוגי הפעולות של הכרטיס
    public enum CardAction
    {
        Command = 1,  // פקודה
        Surprise = 2, // הפתעה
        DidYouKnow = 3 // הידעת
    }

    // מחלקת כרטיס בסיסית
    public abstract class Card
    {
        public int CardId { get; set; }
        public string Description { get; set; }
        public CardAction Action { get; set; }

        protected Card(int cardId, string description, CardAction action)
        {
            CardId = cardId;
            Description = description;
            Action = action;
        }

        protected Card() { }

        public abstract bool UpdateCard();
    }

    // מחלקת כרטיס הידעת
    public class DidYouKnowCard : Card
    {
        public string Question1 { get; set; }
        public string Question2 { get; set; }
        public string Answer { get; set; }

        public DidYouKnowCard(int cardId, string description, string question1, string question2, string answer)
            : base(cardId, description, CardAction.DidYouKnow)
        {
            Question1 = question1;
            Question2 = question2;
            Answer = answer;
        }

        public DidYouKnowCard() { }

        public override bool UpdateCard()
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateDidYouKnowCard(this);
        }
    }

    // מחלקת כרטיס הפתעה
    public class SurpriseCard : Card
    {
        public double Amount { get; set; }
        public int MoveTo { get; set; }
        public bool IsGameState { get; set; } // מצב משחק, לדוגמה כרטיס יציאה מהכלא

        public SurpriseCard(int cardId, string description, double amount, int moveTo, bool isGameState)
            : base(cardId, description, CardAction.Surprise)
        {
            Amount = amount;
            MoveTo = moveTo;
            IsGameState = isGameState;
        }

        public SurpriseCard() { }

        public override bool UpdateCard()
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateSurpriseCard(this);
        }
    }

    // מחלקת כרטיס פקודה
    public class CommandCard : Card
    {
        public double Amount { get; set; }
        public int MoveTo { get; set; }

        public CommandCard(int cardId, string description, double amount, int moveTo)
            : base(cardId, description, CardAction.Command)
        {
            Amount = amount;
            MoveTo = moveTo;
        }

        public CommandCard() { }

        public override bool UpdateCard()
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateCommandCard(this);
        }
    }
}
