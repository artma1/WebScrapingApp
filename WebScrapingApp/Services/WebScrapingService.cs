using HtmlAgilityPack;


namespace WebScrappingApp.Services
{
  public class WebScrapingService
  {
    public string ExtractDeepestText(string url)
    {
      try
      {
        var web = new HtmlWeb();
        var document = web.Load(url);

        var rootNode = document.DocumentNode;

        var deepestNode = GetDeepestNode(rootNode);
        var deepestNodeLine = deepestNode?.OuterHtml;
        return deepestNode?.InnerText.Trim() ?? "No text found";
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error in processing URL {url}: {ex.Message}");
        return $"Error: {ex.Message}";
      }
    }

    private HtmlNode GetDeepestNode(HtmlNode node)
    {
      if (!node.HasChildNodes)
        return node;

      HtmlNode deepestNode = null;
      int maxDepth = -1;

      foreach (var child in node.ChildNodes)
      {
        var currentNode = GetDeepestNode(child);
        var currentDepth = GetDepth(currentNode);

        if (currentDepth > maxDepth)
        {
          maxDepth = currentDepth;
          deepestNode = currentNode;
        }
      }

      return deepestNode;
    }

    private int GetDepth(HtmlNode node)
    {
      int depth = 0;
      while (node?.ParentNode != null)
      {
        depth++;
        node = node.ParentNode;
      }
      return depth;
    }
  }
}
