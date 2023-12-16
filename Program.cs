using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Physiosoft.Configuration;
using Physiosoft.DAO;
using Physiosoft.Data;
using Physiosoft.Repisotories;
using Physiosoft.Service;
using Serilog;

namespace Physiosoft
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<PhysiosoftDbContext>(options => options.UseSqlServer(connString));
            builder.Services.AddAutoMapper(typeof(MapperConfig));

            /*builder.Host.UseSerilog((context, config) =>
            {
                config.ReadFrom.Configuration(context.Configuration);
            });*/

            // Authentication services 
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.Cookie.HttpOnly = true;
                    options.SlidingExpiration = true;
                });

            builder.Services.AddDbContext<PhysiosoftDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
               .LogTo(Console.WriteLine, LogLevel.Information));


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IUserDAO, UserDaoImpl>();
            builder.Services.AddScoped<UserAuthenticationService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Authentication}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
