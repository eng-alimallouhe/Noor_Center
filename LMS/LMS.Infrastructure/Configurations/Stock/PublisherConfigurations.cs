using LMS.Domain.Entities.Stock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations.Stock
{
    public class PublisherConfigurations :
        IEntityTypeConfiguration<Publisher>
    {
  
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.ToTable("Publishers");

            builder.HasKey(p => p.PublisherId);
            
            builder.Property(p => p.PublisherName)
                    .IsRequired()
                    .HasMaxLength(60);

            builder.Property(p => p.PublisherDescription)
                    .IsRequired()
                    .HasMaxLength(255);

            builder.Property(p => p.IsActive)
                    .IsRequired();

            builder.Property(p => p.CreatedAt)
                    .IsRequired();

            builder.Property(p => p.UpdatedAt)
                    .IsRequired();

            builder.HasMany(p => p.Books)
                    .WithMany(b => b.Publishers)
                    .UsingEntity(j => j.ToTable("BooksPublishers"));
        }
    }
}
