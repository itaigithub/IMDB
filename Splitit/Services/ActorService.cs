using Microsoft.EntityFrameworkCore;
using Splitit.EntityFM;
using Splitit.Models;

namespace Splitit.Services
{
    public class ActorService : IActorService
    {
        private readonly ActorsDbContext _context;

        public ActorService(ActorsDbContext context)
        {
            _context = context;
        }

        public async Task<List<Actor>> Get(string? name, int? minRank, int? maxRank)
        {
            return await _context.Actors.Where(x=>

            ( (String.IsNullOrEmpty(name) || x.Name.ToLower().Equals(name.ToLower())) 
             && (!minRank.HasValue || minRank.Value <= x.Rank)
             && (!maxRank.HasValue || maxRank.Value >= x.Rank)
            )
            
            ).ToListAsync();

        }

        public async Task<Actor> Get(int id)
        {
            return await _context.Actors.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Add(Actor actor)
        {
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
        }

        public async Task Add(List<Actor> actors)
        {
            await _context.Actors.AddRangeAsync(actors);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAll(bool save)
        {
            _context.Actors.ExecuteDeleteAsync();
            if (save)
            {
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Boolean> Remove(int id)
        {
            var todelete = await Get(id);
            if (todelete == null)
                return false;
            _context.Actors.Remove(todelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Boolean> Update(Actor actor)
        {
            var dbActor = await Get(actor.Id);
            if (dbActor == null)
            {
                return false;
            }
            dbActor.Source = actor.Source;
            dbActor.Rank = actor.Rank;
            dbActor.Name = actor.Name;
            dbActor.Details = actor.Details;
            await _context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> IsRankUnique(Actor actor)
        {
            if (actor == null)
            {
                return false;
            }
            return !Get(null, minRank: actor.Rank, maxRank: actor.Rank).Result.Any(x => x.Id != actor.Id && actor.Rank == x.Rank);
        }
    }
}
