﻿using LMS.Domain.Entities.Stock;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations.Stock
{
    public class BookConfigurations :
        IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");

            builder.Property(b => b.ISBN)
                    .IsRequired()
                    .HasMaxLength(13);

            builder.Property(b => b.Pages)
                    .IsRequired();

            builder.Property(b => b.RentalCost)
                    .HasColumnType("Decimal(7,2)")
                    .IsRequired();

            builder.Property(b => b.PublishedYear)
                    .IsRequired();

            builder.HasOne(b => b.Genre)
                    .WithMany(g => g.Books)
                    .HasForeignKey(b => b.GenreId)
                    .IsRequired();

            builder.HasOne(b => b.Author)
                    .WithMany(a => a.Books)
                    .HasForeignKey(b => b.AuthorId)
                    .IsRequired();
        }
    }
}
