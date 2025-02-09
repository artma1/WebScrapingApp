using WebScrappingApp.Context;
using Microsoft.EntityFrameworkCore;
using WebScrappingApp.Services;
namespace WebScrappingApp
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddControllersWithViews();

      builder.Services.AddDbContext<AppDbContext>(options =>
          options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

      builder.Services.AddScoped<WebScrapingService>();

      builder.Services.AddCors(options =>
      {
        options.AddPolicy("AllowAll", policy =>
        {
          policy.AllowAnyOrigin()  // Permitir todas as origens
                .AllowAnyMethod()  // Permitir qualquer método (GET, POST, etc.)
                .AllowAnyHeader(); // Permitir qualquer cabeçalho
        });
      });

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
      app.UseCors("AllowAll");
      app.UseAuthorization();

      app.MapControllerRoute(
          name: "default",
          pattern: "{controller=WebScrapingResults}/{action=Index}/{id?}");

      app.Run();
    }
  }
}
