using LMS.Domain.Entities.Users;

namespace LMS.Domain.Entities.Orders
{
    public class DeliveryOrder : Order
    {
        //Foreign Key: AddressId ==> one(Address)-to-one(PrintOrder) relationship
        public Guid AddressId { get; set; }

        public string CustomerName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public Address Address { get; set; }

        public DeliveryOrder()
        {
            Address = null!;
        }
    }
}
