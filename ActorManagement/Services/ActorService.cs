using ActorManagement.Data;
using ActorManagement.Models;
using ActorManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ActorManagement.Services
{
  

    public class ActorService :IActorService
    {
        private readonly ActorDbContext _context;

        public ActorService(ActorDbContext context)
        {
            _context = context;
        }
        public async Task PreloadActorsFromIMDb()
        {
            var actors = await Scraper.ScrapeActorsFromIMDb();

            // Check if any actors exist in the database before preloading
            if (!_context.Actors.Any())
            {
                _context.Actors.AddRange(actors);
                await _context.SaveChangesAsync();
            }
       
        }

        public async Task<Actor> AddActorAsync(Actor actor)
        {
            const string ACTOR_ID = "1";
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }
            actor.Id = _context.Actors.Count() > 0 ? _context.Actors.Max(a => a.Id) + 1 : ACTOR_ID;
            _context.Add(actor);
            _context.SaveChanges();
            return actor;
        }

        public async Task<ActorResponse> DeleteActorAsync(string id)
        {
            ActorResponse response = new ActorResponse();
            var existingActor = GetActorByIdAsync(id);
            if (existingActor != null)
            {
                _context.Remove(existingActor);
                response.IsSuccess = true;
            }
            return response;
        }

    
        public async Task<List<Actor>> GetAllActorsAsync(string nameDescription = null, int minRank = 0, int maxRank = int.MaxValue, int page = 1, int pageSize = 5)
        {
            var query = _context.Actors.AsQueryable();

            // Apply filters based on parameters
            if (!string.IsNullOrEmpty(nameDescription))
            {
                query = query.Where(a => a.Name.Contains(nameDescription));
            }

            query = query.Where(a => a.Rank >= minRank && a.Rank <= maxRank);

            // Implement paging logic (assuming page starts from 1)
            int skip = (page - 1) * pageSize;
            query = query.Skip(skip).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<Actor> GetActorByIdAsync(string id)
        {
            return await _context.Actors.FindAsync(id);
        }

        public async Task<Actor> UpdateActorAsync(Actor actor)
        {
            var existingActor = await GetActorByIdAsync(actor.Id);
            if (existingActor == null)
            {
                throw new ArgumentException("Actor not found", nameof(actor.Id));
            }

            // Update actor details
            existingActor.Name = actor.Name;
            existingActor.Details = actor.Details;
            existingActor.Type = actor.Type;
            existingActor.Rank = actor.Rank;
            existingActor.Source = actor.Source;

            _context.Actors.Update(existingActor);
            await _context.SaveChangesAsync();

            return existingActor;
        }
    }
}
