namespace KidzBizzServer.BL
{
    public class Card
    {
        int cardId;
        string description;
        string action;
        double amount;

        public Card(int cardId, string description, string action, double amount)
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
        public string Action { get => action; set => action = value; }
        public double Amount { get => amount; set => amount = value; }
    }
}
