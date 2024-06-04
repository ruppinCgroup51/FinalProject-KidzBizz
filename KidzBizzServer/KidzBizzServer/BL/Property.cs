namespace KidzBizzServer.BL
{
    public class Property
    {
        int propertyId;
        string propertyName;
        double propertyPrice;

        public Property(int propertyId, string propertyName, double propertyPrice)
        {
            this.propertyId = propertyId;
            this.propertyName = propertyName;
            this.propertyPrice = propertyPrice;
        }

        public Property()
        {

        }

        public int PropertyId { get => propertyId; set => propertyId = value; }
        public string PropertyName { get => propertyName; set => propertyName = value; }
        public double PropertyPrice { get => propertyPrice; set => propertyPrice = value; }

        public List<Property> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadProperties();
        }

        public List<Property> ReadPropertiesByPlayerId(int id)
        {
            DBservices dbs = new DBservices();
            return dbs.ReadPropertiesByPlayerId(id);
        }
        public object CheckPropertyOwnership(int propertyId, int playerId, int playerAiId)
        {
            // בדיקת בעלות הנכס ע"י השחקן
            List<Property> playerProperties = ReadPropertiesByPlayerId(playerId);
            foreach (var property in playerProperties)
            {
                if (property.PropertyId == propertyId)
                {
                    return new { OwnerType = "Player", OwnerId = playerId };
                }
            }

            // בדיקת בעלות הנכס ע"י שחקן ה-AI
            List<Property> aiPlayerProperties = ReadPropertiesByPlayerId(playerAiId);
            foreach (var property in aiPlayerProperties)
            {
                if (property.PropertyId == propertyId)
                {
                    return new { OwnerType = "AI Player", OwnerId = playerAiId };
                }
            }

            // הנכס לא בבעלות של אף שחקן
            return null;
        }

    }
}



