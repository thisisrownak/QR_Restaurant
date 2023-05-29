namespace QR_Restaurant.Data.Entities
{
    public class DeliveryArea:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Range { get; set; }
        public double DeliveryCharge { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }   
}
