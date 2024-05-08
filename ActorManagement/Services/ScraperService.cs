using ActorManagement.Data;
using ActorManagement.Models;
using ActorManagement.Services.Interfaces;
using HtmlAgilityPack;

namespace ActorManagement.Services
{
    public class ScraperService: IScraperService
    {
        private readonly ActorDbContext _context;
        private readonly IConfiguration _config;

        public ScraperService(ActorDbContext context, IConfiguration configt)
        {
            _context = context;
            _config = configt;
        }

        public async Task PreloadActors()
        {
            var actors = await Scraper.ScrapeActorsFromIMDb(_config);

            // Check if any actors exist in the database before preloading
            if (!_context.Actors.Any())
            {
                _context.Actors.AddRange(actors);
                await _context.SaveChangesAsync();
            }
        }

       
    }

}

