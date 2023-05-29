using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.DAL.Mapping
{
    public class Order_MenuProduct_RT_Map : IEntityTypeConfiguration<Order_MenuProduct_RT>
    {
        public void Configure(EntityTypeBuilder<Order_MenuProduct_RT> builder)
        {
            builder.HasKey(rc => new { rc.OrderId, rc.MenuProductId, rc.FeatureItemsList });
            builder.HasOne(bc => bc.Order).WithMany(b => b.Order_MenuProducts).HasForeignKey(bc => bc.OrderId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(bc => bc.MenuProduct).WithMany(b => b.Orders_MenuProduct).HasForeignKey(bc => bc.MenuProductId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.OrderId).IsRequired();
            builder.Property(x => x.MenuProductId).IsRequired();
            builder.Property(x => x.FeatureItemsList).HasMaxLength(3000);
        }
    }
}
