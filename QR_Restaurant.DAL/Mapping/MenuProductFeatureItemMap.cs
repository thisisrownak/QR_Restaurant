using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR_Restaurant.Data.Entities;

namespace QR_Restaurant.DAL.Mapping
{
    internal class MenuProductFeatureItemMap : IEntityTypeConfiguration<MenuProductFeatureItem>
    {
        public void Configure(EntityTypeBuilder<MenuProductFeatureItem> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(400);
            builder.Property(x => x.Price).IsRequired();
        }
    }
}
