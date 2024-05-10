
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


        /// 1 - פקודה: השחקן חייב לבצע הוראה מסוימת.
        /// 2 - הפתעה: מפעיל אירוע משחק בלתי צפוי.
        /// 3 - הידעת: מספק עובדה או מידע טריוויה.
        public Card(int cardId, string description, CardAction action, double amount)
        {
            this.cardId = cardId;
            this.description = description;
            this.action = action;
            this.amount = amount;
        }

        public Card()
        {

        }

        public int CardId { get => cardId; set => cardId = value; }
        public string Description { get => description; set => description = value; }
        public CardAction Action { get => action; set => action = value; }
        public double Amount { get => amount; set => amount = value; }


        // Method to retrieve all cards from the database
        public static List<Card> ReadAllCards()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadCards();
        }
    }
}


