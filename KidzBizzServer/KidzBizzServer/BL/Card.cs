namespace KidzBizzServer.BL
{
    public enum CardAction
    {
        Command = 1,  // פקודה
        Surprise = 2, // הפתעה
        DidYouKnow = 3 // הידעת
    }

    public class Card
    {
        public int CardId { get; set; }
        public string Description { get; set; }
        public CardAction Action { get; set; }

        public static Card GetCardById(int cardId)
        {
            DBservices dbs = new DBservices();
            Card card = dbs.GetCardById(cardId);

            switch (card.Action)
            {
                case CardAction.Command:
                    return dbs.GetCommandCardDetails(cardId);
                case CardAction.Surprise:
                    return dbs.GetSurpriseCardDetails(cardId);
                case CardAction.DidYouKnow:
                    return dbs.GetDidYouKnowCardDetails(cardId);
                default:
                    throw new InvalidOperationException("Unknown card action type");
            }
        }

        public static List<Card> GetAllCards()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadCards();
        }

        public static List<CommandCard> GetAllCommandCards()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadCommandCards();
        }

        public static List<SurpriseCard> GetAllSurpriseCards()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadSurpriseCards();
        }

        public static List<DidYouKnowCard> GetAllDidYouKnowCards()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadDidYouKnowCards();
        }

        public static Card GetRandomCard()
        {
            var cards = GetAllCards();
            if (cards == null || cards.Count == 0)
            {
                return null;
            }

            var random = new Random();
            var randomCard = cards[random.Next(cards.Count)];

            DBservices dbs = new DBservices();

            switch (randomCard.Action)
            {
                case CardAction.Command:
                    return dbs.GetCommandCardDetails(randomCard.CardId);
                case CardAction.Surprise:
                    return dbs.GetSurpriseCardDetails(randomCard.CardId);
                case CardAction.DidYouKnow:
                    return dbs.GetDidYouKnowCardDetails(randomCard.CardId);
                default:
                    throw new InvalidOperationException("Unknown card action type");
            }
        }
    }

    public class CommandCard : Card
    {
        public double Amount { get; set; }
        public string MoveTo { get; set; }
    }

    public class SurpriseCard : Card
    {
        public double Amount { get; set; }
    }

    public class DidYouKnowCard : Card
    {
        public string Question { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
