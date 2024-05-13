
namespace KidzBizzServer.BL
{

    // יצירת enum לסוגי הפעולות של הכרטיס
    public enum CardAction
    {
        Command = 1,  // פקודה
        Surprise = 2, // הפתעה
        DidYouKnow = 3 // הידעת
    }
    public class Card
    {
        int cardId;
        string description;
        CardAction action;  // שימוש ב־enum עבור סוג הפעולה
        double amount;
        int moveTo;  // New property to specify board move location



        /// 1 - פקודה: השחקן חייב לבצע הוראה מסוימת.
        /// 2 - הפתעה: מפעיל אירוע משחק בלתי צפוי.
        /// 3 - הידעת: מספק עובדה או מידע טריוויה.
        public Card(int cardId, string description, CardAction action, double amount, int moveTo)
        {
            this.cardId = cardId;
            this.description = description;
            this.action = action;
            this.amount = amount;
            this.moveTo = moveTo;

        }

        public Card()
        {

        }

        public int CardId { get => cardId; set => cardId = value; }
        public string Description { get => description; set => description = value; }
        public CardAction Action { get => action; set => action = value; }
        public double Amount { get => amount; set => amount = value; }
        public int MoveTo { get => moveTo; set => moveTo = value; }


        // Method to retrieve all cards from the database
        public List<Card> ReadAllCards()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadCards();
        }
        public bool UpdateCard(int cardId)
        {
            DBservices dbs = new DBservices();
            if (CheckCard(cardId))
            {
                Card updatedsuccess = dbs.UpdateCard(this);
                return updatedsuccess != null;
            }
            else
            {
                
                return false;
            }
        }

        private bool CheckCard(int cardId)
        {
            DBservices dbs = new DBservices();
            List<Card> cards = dbs.ReadCards();
            foreach (Card card in cards)
            {
                if (card.CardId == cardId)
                {
                    return true;  
                }
            }
            return false;  
        }

    }
}


