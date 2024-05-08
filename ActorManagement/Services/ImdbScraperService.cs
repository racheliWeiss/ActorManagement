using ActorManagement.Data;
using ActorManagement.Models;
using HtmlAgilityPack;

namespace ActorManagement.Services
{
    public interface IScraperService 
    {
        Task PreloadActorsFromIMDb();
    }
    
    public class ImdbScraperService: IScraperService
    {
        private readonly ActorDbContext _context;

        public ImdbScraperService(ActorDbContext context)
        {
            _context = context;
        }

        public async Task PreloadActorsFromIMDb()
        {
            var actors = await ScrapeActorsFromIMDb();

            // Check if any actors exist in the database before preloading
            if (!_context.Actors.Any())
            {
                _context.Actors.AddRange(actors);
                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Actors already exist in the database. Skipping preload.");
            }
        }

        private async Task<List<Actor>> ScrapeActorsFromIMDb()
        {
            var actors = new List<Actor>();
            var url = "https://www.imdb.com/list/ls054840033/";

            try
            {
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var actorNodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='lister-item-content']");

                if (actorNodes != null)
                {
                    foreach (var actorNode in actorNodes)
                    {
                        var actorLink = actorNode.SelectSingleNode(".//h3/a");
                        if (actorLink != null)
                        {
                            var actorId = GetNumberIdFromHref(actorLink.GetAttributeValue("href", "").Split('/')[2]); // Extracting IMDb ID from the URL
                            var nameNode = actorNode.SelectSingleNode(".//p[@class='text-muted text-small']/text()[normalize-space()]");
                            var typeNode = actorNode.SelectSingleNode(".//p/span[@class='text-muted text-small']/text()[normalize-space()]");
                            var rankNode = actorNode.SelectSingleNode(".//span[@class='lister-item-index unbold text-primary']");
                            int.TryParse(rankNode?.InnerText, out int rank);
                            var actor = new Actor
                            {
                                Id = actorId,
                                Name = actorLink.InnerText.Trim(),
                                Details = nameNode != null ? nameNode.InnerText.Trim() : string.Empty,
                                Type = typeNode != null ? typeNode.InnerText.Trim() : string.Empty,
                                Rank = rank,
                                Source = "https://www.imdb.com"
                            };
                            actors.Add(actor);
                        }
                    }
                }

                else
                {
                    Console.WriteLine("No actors found on the IMDb page.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while scraping IMDb: {ex.Message}");
            }

            return actors;
        }

        string GetNumberIdFromHref(string href)
        {
            // Extracting the part after the last '?' character
            int lastSlashIndex = href.LastIndexOf('?');
            if (lastSlashIndex >= 0 && lastSlashIndex < href.Length - 1)
            {
                return href.Substring(0, lastSlashIndex);
            }
            return null;
        }
    }

}

