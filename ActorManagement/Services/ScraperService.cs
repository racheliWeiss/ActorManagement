using ActorManagement.Data;
using ActorManagement.Models;
using ActorManagement.Services.Interfaces;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ActorManagement.Services
{
    public class ScraperService: IScraperService
    {
        private readonly ActorDbContext _context;
        private readonly SettingsUrl _settingsUrl;

        public ScraperService(ActorDbContext context, IOptions<SettingsUrl> options)
        {
            _context = context;
            _settingsUrl = options.Value;
        }

        public async Task PreloadActors()
        {
          

            var actors = await Scraper.ScrapeActorsFromIMDb(_settingsUrl.IMDbSettings);

            if (!_context.Actors.Any())
            {
                _context.Actors.AddRange(actors);
                await _context.SaveChangesAsync();
            }
        }

       
    }

}

