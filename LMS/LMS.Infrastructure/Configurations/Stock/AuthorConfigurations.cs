using LMS.Domain.Entities.Stock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations.Stock
{
    public class AuthorConfigurations :
        IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("Authors");

            builder.HasKey(a => a.AuthorId);

            builder.Property(a => a.AuthorName)
                    .IsRequired()
                    .HasMaxLength(60);
            
            builder.Property(a => a.AuthorDescription)
                    .IsRequired()
                    .HasMaxLength(512);
            
            builder.Property(a => a.IsActive)
                    .IsRequired();
            
            builder.Property(a => a.CreatedAt)
                    .IsRequired();
            
            builder.Property(a => a.UpdatedAt)
                    .IsRequired();
        }
    }
}
