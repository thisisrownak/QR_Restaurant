using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QR_Restaurant.UI.Helper;
using QR_Restaurant.DAL.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QR_Restaurant.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using QR_Restaurant.DAL.Seed;
using QR_Restaurant.Business.Abstract;
using QR_Restaurant.Core.Repository;
using QR_Restaurant.Business.Concrete;
using FluentValidation.AspNetCore;
using QR_Restaurant.UI.Areas.Admin.Validations;
using Microsoft.AspNetCore.Localization.Routing;
using Rotativa.AspNetCore;
using QR_Restaurant.UI.UIService;
using QR_Restaurant.UI.SingalR;

namespace QR_Restaurant.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IMenuCategoryService, MenuCategoryService>();
            services.AddScoped<IQrOrderTableService, QrOrderTableService>();
            services.AddScoped<IMenuProductService, MenuProductService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrder_MenuProduct_RT_Service, Order_MenuProduct_RT_Service>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IMenuProductFeatureService, MenuProductFeatureService>();
            services.AddScoped<IMenuProductFeatureItemService, MenuProductFeatureItemService>();
            services.AddScoped<IDeliveryAreaService, DeliveryAreaService>();
            
            services.AddSingleton<ICartSessionService, CartSessionService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<LocService>();
            services.AddSingleton<ValidationLocalizer>();
            services.AddSingleton<OrderHub>();

            services.AddSignalR();                   

            services.AddSession();
            services.AddDistributedMemoryCache();
            services.AddDbContext<QR_Context>(options => { options.UseSqlServer(Configuration.GetConnectionString("MssqlConnection")); });

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                //options.Password.RequiredUniqueChars = 2;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

            })
             .AddEntityFrameworkStores<QR_Context>()
             .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/Denied";
                options.Cookie.Name = "AppCookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(60);
                options.LoginPath = "/Account/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            services.AddRazorPages()
            .AddRazorRuntimeCompilation();

            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });            

            services.AddControllersWithViews()
                 .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<RegisterRestaurantValidator>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // ********************* Note : If open DB Initializer and run application, database is created automatically with default datas  *******************
            DbInitializer.Initialize(app.ApplicationServices);
            DbInitializer.SeedFull(app.ApplicationServices).Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            var supportedCultures = new List<CultureInfo>
            {
                 new CultureInfo("en"),
                 new CultureInfo("es"),
                 new CultureInfo("de"),
                 new CultureInfo("ru"),
                 new CultureInfo("zh"),
                 new CultureInfo("ar"),
                 new CultureInfo("hi"),
                 new CultureInfo("pt"),
                 new CultureInfo("ja"),
                 new CultureInfo("tr"),
                 new CultureInfo("fr"),
                 new CultureInfo("pl"),
                 new CultureInfo("it")
            };   

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
                DefaultRequestCulture = new RequestCulture("en"),
                RequestCultureProviders = new[] { new CookieRequestCultureProvider() }
            });

            app.UseCookiePolicy();
            app.UseSession();
            app.UseFileServer();
            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                     name: "areas",
                     pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHub<OrderHub>("/orderHub");

            });
            RotativaConfiguration.Setup(env.WebRootPath, "Rotativa");

        }
    }
}
