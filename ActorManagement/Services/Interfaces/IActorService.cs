using ActorManagement.Models;

namespace ActorManagement.Services.Interfaces
{
    public interface IActorService
    {
        Task PreloadActorsFromIMDb();
        Task<List<Actor>> GetAllActorsAsync(string nameDescription, int minRank = 0, int maxRank = int.MaxValue, int page = 1, int pageSize = 10);
        Task<Actor> GetActorByIdAsync(string id);
        Task<Actor> AddActorAsync(Actor actor);
        Task<Actor> UpdateActorAsync(Actor actor);
        ActorResponse DeleteActorAsync(string id);
    }
}
