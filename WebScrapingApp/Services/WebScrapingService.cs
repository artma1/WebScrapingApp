using HtmlAgilityPack;
using System;
using System.Linq;

namespace WebScrappingApp.Services
{
  public class WebScrapingService
  {
    public string ExtractDeepestText(string url)
    {
      try
      {
        // Carrega o conteúdo da URL
        var web = new HtmlWeb();
        var document = web.Load(url);

        // Obtém o nó raiz do HTML
        var rootNode = document.DocumentNode;

        // Encontra o texto mais profundo
        var deepestNode = GetDeepestNode(rootNode);
        var deepestNodeLine = deepestNode?.OuterHtml;
        return deepestNode?.InnerText.Trim() ?? "No text found";
      }
      catch (Exception ex)
      {
        // Log para debug (opcional) e retorno de erro
        Console.WriteLine($"Erro ao processar a URL {url}: {ex.Message}");
        return $"Error: {ex.Message}";
      }
    }

    private HtmlNode GetDeepestNode(HtmlNode node)
    {
      // Se o nó não tem filhos, é o mais profundo
      if (!node.HasChildNodes)
        return node;

      // Itera pelos nós filhos para encontrar o mais profundo
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
