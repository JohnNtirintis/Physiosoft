using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Physiosoft.Configuration;
using Physiosoft.DAO;
using Physiosoft.Data;
using Physiosoft.Logger;
using Physiosoft.Repisotories;
using Physiosoft.Service;

namespace Physiosoft
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string connectionName = "DefaultConnection";

            NLogger.LogInfo($"Creating Builder with args");
            var builder = WebApplication.CreateBuilder(args);

            NLogger.LogInfo($"Creating connection");
            var connString = builder.Configuration.GetConnectionString(connectionName);
            builder.Services.AddDbContext<PhysiosoftDbContext>(options => options.UseSqlServer(connString));
            builder.Services.AddAutoMapper(typeof(MapperConfig));

            // Authentication services 
            NLogger.LogInfo($"Adding Authentication Services");
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.Cookie.HttpOnly = true;
                    options.SlidingExpiration = true;
                });

            NLogger.LogInfo($"Adding Ddatabase Context via");
            builder.Services.AddDbContext<PhysiosoftDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString(connectionName))
               .LogTo(Console.WriteLine, LogLevel.Information));

            NLogger.LogInfo($"Adding Services to the container/builder");
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IUserDAO, UserDaoImpl>();
            builder.Services.AddScoped<UserAuthenticationService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            NLogger.LogInfo($"Building app");
            var app = builder.Build();

            // Cnfigure the HTTP request pipeline
            if(!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Authentication}/{action=Login}/{id?}");

            NLogger.LogInfo("Starting Application");
            app.Run();
        }
    }
}
