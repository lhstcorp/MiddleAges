using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiddleAges.Data;
using MiddleAges.Entities;
using MiddleAges.Hubs;
using MiddleAges.Timed_Hosted_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddleAges
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    providerOptions => providerOptions.EnableRetryOnFailure()));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<Player>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;
                    options.User.RequireUniqueEmail = true;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager<SignInManager<Player>>();
            services.AddControllersWithViews();

            services.AddAuthentication();
            services.AddAuthorization();
            services.AddRazorPages();
            services.AddSignalR();

            services.AddHostedService<DailyUpdateBackgroundService>();
            services.AddHostedService<HourlyBackgroundService>();
            services.AddHostedService<PlayerLvlBackgroundService>();
            /*
            services.AddHostedService<ProductionBackgroundService>();                       
            services.AddHostedService<RecruitBackgroundService>();
            services.AddHostedService<WarBackgroundService>();
            
            */
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Main/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Main}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}
