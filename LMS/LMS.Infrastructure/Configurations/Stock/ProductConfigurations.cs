using LMS.Domain.Entities.Stock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations.Stock
{
    public class ProductConfigurations :
        IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.ProductName)
                    .IsRequired()
                    .HasMaxLength(60);

            builder.Property(p => p.ProductDescription)
                    .IsRequired()
                    .HasMaxLength(255);

            builder.Property(p => p.ProductPrice)
                    .HasColumnType("Decimal(18,2)")
                    .IsRequired();

            builder.Property(p => p.ProductStock)
                    .IsRequired();

            builder.Property(p => p.IsActive)
                    .IsRequired();

            builder.Property(p => p.CreatedAt)
                    .IsRequired();

            builder.Property(p => p.UpdatedAt)
                    .IsRequired();
        }
    }
}
