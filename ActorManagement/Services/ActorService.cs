using ActorManagement.Data;
using ActorManagement.Models;
using ActorManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ActorManagement.Services
{


    public class ActorService : IActorService
    {
        private readonly ActorDbContext _context;

        public ActorService(ActorDbContext context)
        {
            _context = context;
        }

        public async Task<ActorResponse> AddActorAsync(Actor actor)
        {
            ActorResponse response = new ActorResponse();
            try
            {
                const string ACTOR_ID = "1";
                if (actor == null)
                {
                    throw new ArgumentNullException(nameof(actor));
                }
                actor.Id = _context.Actors.Count() > 0 ? _context.Actors.Max(a => a.Id) + 1 : ACTOR_ID;
                _context.Add(actor);
                _context.SaveChanges();

            }
            catch (Exception ex) { }
            {
                response.Errors.Add(new Error { Code = ErrorCodes.NotFound, Message = "Actor not found" });
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }

            return response;
        }

        public async Task<ActorResponse> DeleteActorAsync(string id)
        {
            ActorResponse response = new ActorResponse();
            try
            {
                var existingActor = await GetActorByIdAsync(id);
                if (existingActor != null)
                {
                    _context.Remove(existingActor);
                    await _context.SaveChangesAsync(); 
                    response.IsSuccess = true;
                }
                else
                {
                    response.Errors.Add(new Error { Code = ErrorCodes.NotFound, Message = "Actor not found" });
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error { Code = ErrorCodes.BadRequest, Message = "Failed to delete actor", AdditionalInfo = ex.Message });
                response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return response;
        }

        public async Task<ActorsResponses> GetAllActorsAsync(string nameDescription, int maxRank = int.MaxValue, int minRank = 0, int page = 1, int pageSize = 5)
        {
            ActorsResponses response = new ActorsResponses();
            try
            {
                var query = _context.Actors.AsQueryable();
                if (!string.IsNullOrEmpty(nameDescription))
                {
                    query = query.Where(a => a.Name.Contains(nameDescription));
                }
                query = query.Where(a => a.Rank >= minRank && a.Rank <= maxRank);
                int skip = (page - 1) * pageSize;
                query = query.Skip(skip).Take(pageSize);
                response.Actors = await query.ToListAsync();
            }
            catch(Exception ex)
            {
                response.Errors.Add(new Error { Code = ErrorCodes.BadRequest, Message = "Failed to delete actor", AdditionalInfo = ex.Message });
                response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return response;
        }

        public async Task<Actor> GetActorByIdAsync(string id)
        {
            return await _context.Actors.FindAsync(id);
        }

        public async Task<ActorResponse> UpdateActorAsync(Actor actor)
        {
            ActorResponse response = new ActorResponse();
            try
            {
                Actor existingActor = await GetActorByIdAsync(actor.Id);
                if (existingActor == null)
                {
                    throw new ArgumentException("Actor not found", nameof(actor.Id));
                }
                existingActor.Name = actor.Name;
                existingActor.Details = actor.Details;
                existingActor.Type = actor.Type;
                existingActor.Rank = actor.Rank;
                existingActor.Source = actor.Source;
                response.Actor = existingActor;
                _context.Actors.Update(existingActor);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Errors.Add(new Error { Code = ErrorCodes.BadRequest, Message = "Failed to delete actor", AdditionalInfo = ex.Message });
                response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            return response;
        }
    }
}
