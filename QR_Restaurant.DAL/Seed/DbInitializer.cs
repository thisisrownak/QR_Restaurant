using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using QR_Restaurant.DAL.Context;
using QR_Restaurant.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Restaurant.DAL.Seed
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<QR_Context>();
                if (dbContext != null)
                    dbContext.Database.EnsureCreated();
            }
        }

        public static async Task SeedFull(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<QR_Context>();

                if (!dbContext.UserRoles.Any())
                {
                    //await dbContext.Database.MigrateAsync();

                    var roleManager =
                        serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

                    var roles = new AppRole[]
                    {
                        new AppRole {Name = "Admin"},
                        new AppRole {Name = "RestaurantAdmin"},
                        new AppRole {Name = "Waiter"}
                    };
                    foreach (var role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role.Name))
                        {
                            await roleManager.CreateAsync(role);
                        }
                    }
                }

                if (!dbContext.Restaurants.Any())
                {
                    Restaurant[] restaurants = new Restaurant[]
                    {
                    new Restaurant {CreatedDate= DateTime.Now, IsActive = true, Name="Test Restaurant", Description="Special burgers on charcoal fire", LogoUrl="default.png", IsShowTableName = true, IsShowTableNo = true, ManagerUser="Admin", RestaurantUrl ="null", Address = "Test Address", Phone = "(234) 26 2343"}
                    };

                    foreach (var restaurant in restaurants)
                    {
                        dbContext.Restaurants.Add(restaurant);
                    }
                    dbContext.SaveChanges();
                }

                if (!dbContext.MenuCategories.Any())
                {
                    MenuCategory[] menuCategories = new MenuCategory[]
                    {
                    new MenuCategory {Name="Food Menus", CreatedDate= DateTime.Now, IsActive = true, RowNumber=1, RestaurantId = 1}
                    };

                    foreach (var menuCategory in menuCategories)
                    {
                        dbContext.MenuCategories.Add(menuCategory);
                    }
                    dbContext.SaveChanges();
                }

                if (!dbContext.MenuProducts.Any())
                {
                    MenuProduct[] menuProducts = new MenuProduct[]
                    {
                    new MenuProduct {Description= "With onion, pickle, chedar cheese on charcoal fire", Name="Hamma Burger",PhotoUrl="default.png", Price=12.4m, RowNumber=1, MenuCategoryId=1, IsActive = true, CreatedDate=DateTime.Now }
                    };

                    foreach (var menuProduct in menuProducts)
                    {
                        dbContext.MenuProducts.Add(menuProduct);
                    }
                    dbContext.SaveChanges();
                }

                if (!dbContext.ProductFeatures.Any())
                {
                    MenuProductFeature[] feautres = new MenuProductFeature[]
                    {
                    new MenuProductFeature {IsMultiSelect=true, Name="Extras", MenuProductId=1, IsActive = true, CreatedDate=DateTime.Now }
                    };

                    foreach (var feature in feautres)
                    {
                        dbContext.ProductFeatures.Add(feature);
                    }
                    dbContext.SaveChanges();
                }

                if (!dbContext.ProductFeatureItems.Any())
                {
                    MenuProductFeatureItem[] feautreItems = new MenuProductFeatureItem[]
                    {
                    new MenuProductFeatureItem {Name="Cheddar", Price=4m, ProductFeatureId=1, IsActive = true, CreatedDate=DateTime.Now },
                    new MenuProductFeatureItem {Name="Tomato", Price=2m, ProductFeatureId=1, IsActive = true, CreatedDate=DateTime.Now },
                    new MenuProductFeatureItem {Name="Onion", Price=3m, ProductFeatureId=1, IsActive = true, CreatedDate=DateTime.Now }
                    };

                    foreach (var featureItem in feautreItems)
                    {
                        dbContext.ProductFeatureItems.Add(featureItem);
                    }
                    dbContext.SaveChanges();
                }

                if (!dbContext.Users.Any())
                {
                    var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                    var UsersOfAdmin = new AppUser[]
                    {
                        new AppUser {UserName = "Admin", Name="Test", Surname="Admin" , RegistrationDate=DateTime.Now, Email="test@hotmail.com" , PhoneNumber = "(345) 45 5645", EmailConfirmed = true, PhoneNumberConfirmed = true, Avatar="default.png" }
                    };

                    foreach (var user in UsersOfAdmin)
                    {
                        IdentityResult result = userManager.CreateAsync(user, "123456").Result;
                        userManager.AddToRoleAsync(user, "Admin").Wait();
                    }

                    var UsersOfRestaurant = new AppUser[]
                  {
                        new AppUser {UserName = "Test", Name="Henry", Surname="Frod" , RegistrationDate=DateTime.Now, Email="henryFrod@hotmail.com" , RestaurantId=1, PhoneNumber = "(345) 45 5675", EmailConfirmed = true, PhoneNumberConfirmed = true, Avatar = "default.png" }
                  };

                    foreach (var user in UsersOfRestaurant)
                    {
                        IdentityResult result = userManager.CreateAsync(user, "123456").Result;
                        userManager.AddToRoleAsync(user, "RestaurantAdmin").Wait();
                    }

                }

            }
        }
    }
}
