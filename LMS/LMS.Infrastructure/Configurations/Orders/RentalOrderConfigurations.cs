using LMS.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations.Orders
{
    public class RentalOrderConfigurations :
        IEntityTypeConfiguration<RentalOrder>
    {
        public void Configure(EntityTypeBuilder<RentalOrder> builder)
        {
            builder.ToTable("RentalOrders");

            builder.Property(ro => ro.StartDate)
                    .IsRequired();

            builder.Property(ro => ro.EndDate)
                    .IsRequired();

            builder.Property(ro => ro.InitialCost)
                    .HasColumnType("decimal(7, 2)");

            builder.Property(ro => ro.LateCost)
                    .HasColumnType("decimal(7, 2)");

            builder.HasOne(ro => ro.Book)
                    .WithMany()
                    .HasForeignKey(ro => ro.BookId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
