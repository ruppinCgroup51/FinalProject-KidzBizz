﻿namespace KidzBizzServer.BL
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

        public Property GetPropertyDetails(int propertyId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetPropertyDetails(propertyId);
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
        public bool BuyProperty(int playerId, int propertyId)
        {
            DBservices dbs = new DBservices();

            //// בדיקת בעלות הנכס
            //var ownership = CheckPropertyOwnership(propertyId, playerId, playerId);
            //if (ownership != null)
            //{
            //    return false; // הנכס כבר תפוס
            //}

            // קבלת כל הנכסים
            List<Property> Properties = dbs.ReadProperties();

            // חיפוש הנכס המבוקש ברשימת הנכסים
            Property propertyToBuy = null;
            foreach (var property in Properties)
            {
                if (property.PropertyId == propertyId)
                {
                    propertyToBuy = property;
                    break;
                }
            }

            if (propertyToBuy == null)
            {
                return false; // הנכס לא נמצא
            }

            // קבלת פרטי השחקן
            Player player = dbs.GetPlayerById(playerId);
            if (player == null)
            {
                return false; // השחקן לא נמצא
            }

            // בדיקת אם לשחקן יש מספיק כסף לרכישת הנכס
            if (player.CurrentBalance >= propertyToBuy.PropertyPrice)
            {
                // עדכון הכסף של השחקן
                player.CurrentBalance -= propertyToBuy.PropertyPrice;
                dbs.UpdatePlayerBalance(playerId, (decimal)player.CurrentBalance);

                // הוספת הנכס לרשימת הנכסים של השחקן
                dbs.AddPropertyToPlayer(playerId, propertyId);

                // עדכון פרטי השחקן במסד הנתונים
                dbs.UpdatePlayer(player);

                return true; // הרכישה הצליחה
            }

            return false; // לשחקן אין מספיק כסף
        }
    }
}






