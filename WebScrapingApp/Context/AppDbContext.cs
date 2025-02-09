using Microsoft.EntityFrameworkCore;

namespace WebScrappingApp.Context
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<WebScrappingApp.Models.WebScrapingResult> WebScrappingResults { get; set; }
  }
}
