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
    public class QrOrderTableMap : IEntityTypeConfiguration<QrOrderTable>
    {
        public void Configure(EntityTypeBuilder<QrOrderTable> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(400);
            builder.Property(x => x.TableNo).IsRequired();
            builder.Property(x => x.QrCodeUrl).HasMaxLength(3000);
            builder.Property(x => x.RestaurantId).IsRequired();
        }
    }
}
