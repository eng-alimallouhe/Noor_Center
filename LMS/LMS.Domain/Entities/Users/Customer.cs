using LMS.Domain.Entities.Financial;
using LMS.Domain.Entities.Orders;

namespace LMS.Domain.Entities.Users
{
    public class Customer : User
    {
        //Foreign Key: LevelId ==> one(customer)-to-one(level) relationship
        public Guid LevelId { get; set; }

        public decimal points { get; set; }

        //navigation property:
        public LoyaltyLevel Level { get; set; } = null!;
        public ICollection<Address> Addresses { get; set; } = [];
        public Cart Cart { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<FinancialRevenue> FinancialRevenues { get; set; } = [];

        public Customer()
        {
            LevelId = Guid.NewGuid();
        }

    }
}
