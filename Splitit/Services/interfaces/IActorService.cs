using Splitit.Models;

namespace Splitit.Services
{
    public interface IActorService
    {
        Task<List<Actor>> Get(string name = null, int? minRank = null, int? maxRank = null);
        Task<Actor> Get(int id);
        Task Add(Actor actor);
        Task Add(List<Actor> actors);
        Task RemoveAll(bool save);
        Task<Boolean> Remove(int id);
        Task<Boolean> Update(Actor actor);
        Task<Boolean> IsRankUnique(Actor actor);

    }
}
