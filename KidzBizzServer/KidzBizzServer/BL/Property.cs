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
     

    }
}
