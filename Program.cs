using IMS_WebApplication.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace IMS_WebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.IsEssential = true;
                options.Cookie.HttpOnly = true;
            });
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            // Application service.
            builder.Services.AddScoped<IClientRepository, ClientRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default", 
                pattern: "{controller=Client}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
