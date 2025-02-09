using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebScrappingApp.Context;
using WebScrappingApp.Models;
using WebScrappingApp.Services;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WebScrapingApp.Controllers
{
  public class WebScrapingResultsController : Controller
  {
    private readonly IWebHostEnvironment _env;
    private readonly AppDbContext _context;
    private readonly WebScrapingService _scraper;


    public WebScrapingResultsController(AppDbContext context, WebScrapingService scraper, IWebHostEnvironment env)
    {
      _env = env;
      _context = context;
      _scraper = scraper;
    }


    public async Task<IActionResult> Index()
    {
      return View(await _context.WebScrappingResults.ToListAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Index(int linesToCreate)
    {
      ////////////
      /// Defining number of lines to generate
      ////////////

      if (linesToCreate != 0)
      {
        int linesNumber = linesToCreate;
        string testString = "find me if you can";

        ////////////
        /// Generating the text to test
        ////////////
        string htmlCreate = GenerateHtml(testString, linesNumber);

        ////////////
        /// Updating the Edit view on line 18 using the created text
        ////////////
        ModifyView(htmlCreate, 18);

      }

      return View("Edit");
    }


    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var webScrapingResult = await _context.WebScrappingResults
          .FirstOrDefaultAsync(m => m.Id == id);
      if (webScrapingResult == null)
      {
        return NotFound();
      }

      return View(webScrapingResult);
    }

    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Text,Url,Date")] WebScrapingResult webScrapingResult)
    {

      string scrapedText = _scraper.ExtractDeepestText(webScrapingResult.Url);
      webScrapingResult.Text = scrapedText;
      webScrapingResult.Date = DateTime.Now;


      if (ModelState.IsValid)
      {
        try
        {
          _context.Add(webScrapingResult);
          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error processing URL {webScrapingResult.Url}: {ex.Message}");
        }
      }
      return View();
    }

    public string GenerateHtml(string findMe, int lines)
    {
      // StringBuilder para construir o HTML de forma eficiente
      var htmlBuilder = new StringBuilder();

      for (int i = 0; i < lines; i++)
      {
        htmlBuilder.AppendLine("<div> not me");
      }

      htmlBuilder.AppendLine($"  <div>{findMe}</div>");

      for (int i = 0; i < lines; i++)
      {
        htmlBuilder.AppendLine("also not me</div>");
      }

      return htmlBuilder.ToString();
    }

    public void ModifyView(string htmlContent, int lineToStart)
    {

      string basePath = _env.ContentRootPath;
      string viewFilePath = Path.Combine(basePath, "Views", "WebScrapingResults", "Edit.cshtml");

      var lines = System.IO.File.ReadAllLines(viewFilePath).ToList();

      var newLines = htmlContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

      for (int i = 0; i < newLines.Length; i++)
      {
        if (lineToStart + i < lines.Count)
        {
          lines[lineToStart + i] = newLines[i];
        }
        else
        {
          lines.Add(newLines[i]);
        }
      }
      if (!lines.Any(line => line.Trim().Equals("</div>", StringComparison.OrdinalIgnoreCase)))
      {
        lines.Add("</div>");
      }
      if (!lines.Any(line => line.Trim().Equals("</body>", StringComparison.OrdinalIgnoreCase)))
      {
        lines.Add("</body>");
      }
      if (!lines.Any(line => line.Trim().Equals("</html>", StringComparison.OrdinalIgnoreCase)))
      {
        lines.Add("</html>");
      }

      System.IO.File.WriteAllLines(viewFilePath, lines);
    }


    public async Task<IActionResult> Edit(WebScrapingResult webScrapingResult)
    {

      return View(webScrapingResult);
    }

    public async Task<IActionResult> Delete(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var webScrapingResult = await _context.WebScrappingResults
          .FirstOrDefaultAsync(m => m.Id == id);
      if (webScrapingResult == null)
      {
        return NotFound();
      }

      return View(webScrapingResult);
    }

    // POST: WebScrapingResults/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var webScrapingResult = await _context.WebScrappingResults.FindAsync(id);
      if (webScrapingResult != null)
      {
        _context.WebScrappingResults.Remove(webScrapingResult);
      }

      await _context.SaveChangesAsync();
      return RedirectToAction(nameof(Index));
    }

    private bool WebScrapingResultExists(int id)
    {
      return _context.WebScrappingResults.Any(e => e.Id == id);
    }
  }
}
