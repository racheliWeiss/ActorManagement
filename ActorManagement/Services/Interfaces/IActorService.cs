using ActorManagement.Models;

namespace ActorManagement.Services.Interfaces
{
    public interface IActorService
    {
        Task<ActorsResponses> GetAllActorsAsync(string nameDescription, int maxRank = int.MaxValue, int minRank = 0, int page = 1, int pageSize = 10);
        Task<Actor> GetActorByIdAsync(string id);
        Task<ActorResponse> AddActorAsync(Actor actor);
        Task<ActorResponse> UpdateActorAsync(Actor actor);
        Task<ActorResponse> DeleteActorAsync(string id);
    }
}
