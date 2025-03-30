namespace LMS.Domain.Entities.Users
{
    public class Address
    {
        //Primary Key:
        public Guid AddressId { get; set; }

        //Foreign Key:
        public Guid? CustomerId { get; set; }

        public string Latitude { get; set; } = string.Empty;
        public string Longitude { get; set; } = string.Empty;
        public string GZipCode { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public Address()
        {
            AddressId = Guid.NewGuid();
        }

    }
}
