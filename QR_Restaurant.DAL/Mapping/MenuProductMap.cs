using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.DAL.Mapping
{
    public class MenuProductMap : IEntityTypeConfiguration<MenuProduct>
    {
        public void Configure(EntityTypeBuilder<MenuProduct> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(400);
            builder.Property(x => x.Description).HasMaxLength(800);
            builder.Property(x => x.PhotoUrl).HasMaxLength(3000);
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.MenuCategoryId).IsRequired();
        }
    }
}
