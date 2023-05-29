using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QR_Restaurant.DAL.Mapping;
using QR_Restaurant.Data.Entities;

namespace QR_Restaurant.DAL.Context
{
    public class QR_Context : IdentityDbContext<AppUser, AppRole, string>
    {
        public QR_Context(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<MenuProduct> MenuProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_MenuProduct_RT> Order_MenuProduct_RT { get; set; }
        public DbSet<QrOrderTable> QrOrderTables { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<MenuProductFeature> ProductFeatures { get; set; }
        public DbSet<MenuProductFeatureItem> ProductFeatureItems { get; set; }
        public DbSet<DeliveryArea> DeliveryAreas { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new Order_MenuProduct_RT_Map());
            builder.ApplyConfiguration(new MenuProductMap());
            builder.ApplyConfiguration(new MenuCategoryMap());
            builder.ApplyConfiguration(new OrderMap());
            builder.ApplyConfiguration(new RestaurantMap());
            builder.ApplyConfiguration(new QrOrderTableMap());
            builder.ApplyConfiguration(new PaymentMap());
            builder.ApplyConfiguration(new MenuProductFeatureMap());
            builder.ApplyConfiguration(new MenuProductFeatureItemMap());
            builder.ApplyConfiguration(new DeliveryAreaMap());

            base.OnModelCreating(builder);
        }
    }
}
