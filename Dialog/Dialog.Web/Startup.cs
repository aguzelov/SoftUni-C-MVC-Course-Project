using CloudinaryDotNet;
using Dialog.Common.Mapping;
using Dialog.Data;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Data.Repositories;
using Dialog.Services;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Base;
using Dialog.Web.Infrastructure.Extensions;
using Dialog.Web.Infrastructure.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Dialog.Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Dialog.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(
                    this._configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddDefaultTokenProviders()
            .AddDefaultUI()
           .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(RecentBlogsActionFilter));
            }
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSignalR();

            var account = new Account(
                this._configuration.GetSection("Cloudinary").GetSection("CloudName").Value,
                this._configuration.GetSection("Cloudinary").GetSection("APIKey").Value,
                this._configuration.GetSection("Cloudinary").GetSection("APISecret").Value);

            var cloudinary = new Cloudinary(account);

            services.AddSingleton(new Cloudinary(account));

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();
            services.AddScoped<RecentBlogsActionFilter>();

            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGalleryService, GalleryService>();
            services.AddScoped<IChatService, ChatService>();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(this._configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            app.UseDatabaseMigration(env);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "areas",
                template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });
        }
    }
}