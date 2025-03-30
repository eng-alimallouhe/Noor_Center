using LMS.Domain.Entities.Stock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations.Stock
{
    public class GenreConfigurations :
        IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("Genres");

            builder.HasKey(g => g.GenreId);

            builder.Property(g => g.GenreName)
                    .IsRequired()
                    .HasMaxLength(60);

            builder.Property(g => g.GenreDescription)
                    .IsRequired()
                    .HasMaxLength(512);

            builder.Property(g => g.IsActive)
                    .IsRequired();
            
            builder.Property(g => g.CreatedAt)
                    .IsRequired();
            
            builder.Property(g => g.UpdatedAt)
                    .IsRequired();
        }
    }
}
