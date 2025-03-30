using LMS.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations.Orders
{
    public class DeliveryOrderConfigurations :
        IEntityTypeConfiguration<DeliveryOrder>
    {
        public void Configure(EntityTypeBuilder<DeliveryOrder> builder)
        {
            builder.ToTable("DeliverOrders");

            builder.Property(dor => dor.CustomerName)
                    .IsRequired();
            
            builder.Property(dor => dor.DepartmentName)
                    .IsRequired();
       
            builder.HasOne(dor => dor.Address)
                    .WithOne()
                    .HasForeignKey<DeliveryOrder>(dor => dor.AddressId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
