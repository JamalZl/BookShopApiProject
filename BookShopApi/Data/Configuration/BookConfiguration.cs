using BookShopApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Data.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(b => b.Name).HasMaxLength(70).IsRequired(true);
            builder.Property(b => b.Language).HasMaxLength(50).IsRequired(true);
            builder.Property(b => b.Image).HasMaxLength(150).IsRequired(false);
            builder.Property(x => x.SalePrice).IsRequired(true).HasColumnType("decimal(18,2)");
            builder.Property(x => x.CostPrice).IsRequired(true).HasColumnType("decimal(18,2)");
            builder.Property(b => b.DisplayStatus).HasDefaultValue(true);
            builder.Property(b => b.PageCount).IsRequired(true);
            builder.Property(b => b.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(b => b.ModifiedAt).HasDefaultValueSql("GETUTCDATE()");
            builder.HasOne(x => x.Author).WithMany(x => x.Books).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(x => x.Genre).WithMany(x => x.Books).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
