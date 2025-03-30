using LMS.Domain.Entities.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations.HR
{
    public class PenaltyConfigurations :
        IEntityTypeConfiguration<Penalty>
    {
      public void Configure(EntityTypeBuilder<Penalty> builder)
        {
            builder.ToTable("Penalties");

            builder.HasKey(p => p.PenaltyId);
            
            builder.Property(p => p.Amount)
                    .HasColumnType("decimal(5,2)")
                    .IsRequired();
            
            builder.Property(p => p.Reason)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(p => p.Date)
                    .IsRequired();
            
            builder.Property(p => p.IsActive)
                    .IsRequired();


            builder.HasOne(p => p.Employee)
                .WithMany(e => e.Penalties)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
