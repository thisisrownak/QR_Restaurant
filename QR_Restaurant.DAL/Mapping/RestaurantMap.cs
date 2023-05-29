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
    public class RestaurantMap : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.Property(x => x.Description).HasMaxLength(3000);
            builder.Property(x => x.LogoUrl).HasMaxLength(3000);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(400);
            builder.Property(x => x.ManagerUser).IsRequired().HasMaxLength(400);
            builder.Property(x => x.RestaurantUrl).IsRequired().HasMaxLength(400);
            builder.Property(x => x.Address).HasMaxLength(1000);
            builder.Property(x => x.Phone).HasMaxLength(25);
            builder.Property(x => x.CurrencySymbol).HasMaxLength(25);
        }
    }
}
