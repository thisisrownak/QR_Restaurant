using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QR_Restaurant.Data.Entities;

namespace QR_Restaurant.DAL.Mapping
{
    public class DeliveryAreaMap : IEntityTypeConfiguration<DeliveryArea>
    {
        public void Configure(EntityTypeBuilder<DeliveryArea> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(400);
        }
    }
}
