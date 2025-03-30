using LMS.Domain.Entities.Orders;
using LMS.Domain.Enums.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations.Orders
{
    public class OrderConfigurations :
        IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.OrderId);

            builder.Property(o => o.CustomerId)
                    .IsRequired();

            builder.Property(o => o.EmployeeId)
                    .IsRequired();

            builder.Property(o => o.DepartmentId)
                    .IsRequired();

            builder.Property(o => o.Status)
                    .HasDefaultValue(OrderStatus.Pending);

            builder.Property(o => o.DeliveryMethod)
                    .IsRequired(false);

            builder.Property(o => o.PaymentMethod)
                    .IsRequired();


            builder.Property(o => o.PaymentStatus)
                    .IsRequired();

            builder.Property(o => o.Cost)
                    .HasColumnType("Decimal(19,2)")
                    .IsRequired();

            builder.Property(o => o.IsActive)
                    .IsRequired();

            builder.Property(o => o.CreatedAt)
                    .IsRequired();

            builder.Property(o => o.UpdatedAt)
                    .IsRequired();


            builder.HasOne(o => o.Department)
                    .WithMany(d => d.Orders)
                    .HasForeignKey(o => o.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Employee)
                    .WithMany(e => e.Orders)
                    .HasForeignKey(o => o.EmployeeId)
                    .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Customer)
                    .WithMany(e => e.Orders)
                    .HasForeignKey(o => o.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
