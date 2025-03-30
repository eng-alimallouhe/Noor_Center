using LMS.Domain.Entities.Stock;

namespace LMS.Domain.Entities.Orders
{
    public class RentalOrder : Order
    {
        //Foreign Key: BookId ==> one(Book)-to-many(RentalOrder) relationship
        public Guid BookId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal InitialCost { get; set; }
        public decimal LateCost { get; set; }

        //navigation property: 
        public Book Book { get; set; }

        public RentalOrder()
        {
            InitialCost = 0.0m;
            Book = null!;
        }
    }
}
