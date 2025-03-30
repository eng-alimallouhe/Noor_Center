using LMS.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations.Orders
{
    public class SellOrderConfigurations :
        IEntityTypeConfiguration<SellOrder>
    {
        public void Configure(EntityTypeBuilder<SellOrder> builder)
        {
            builder.ToTable("SellOrders");
        }

    }
}
