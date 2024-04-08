namespace KidzBizzServer.BL
{
    public class Property
    {
        int propertyId;
        int typeId;
        string propertyName;
        double propertyPrice;

        public Property(int propertyId, int typeId, string propertyName, double propertyPrice)
        {
            this.propertyId = propertyId;
            this.typeId = typeId;
            this.propertyName = propertyName;
            this.propertyPrice = propertyPrice;
        }

        public Property()
        {

        }

        public int PropertyId { get => propertyId; set => propertyId = value; }
        public int TypeId { get => typeId; set => typeId = value; }
        public string PropertyName { get => propertyName; set => propertyName = value; }
        public double PropertyPrice { get => propertyPrice; set => propertyPrice = value; }
    }
}
