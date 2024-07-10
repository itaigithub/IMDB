using Splitit.EntityFM;
using Splitit.Models;
using Splitit.Providers;
using Splitit.Services.interfaces;
using System.Reflection.Metadata;

namespace Splitit.Services
{

    public class DBInitializerService : IDBInitializerService
    {
        readonly IActorService _actorService;
        private readonly IProvider _provider;
        public DBInitializerService(IActorService actorService, IProvider provider)
        {
            _actorService = actorService;
            _provider = provider;
        }

        public async Task InitializeDataBase()
        {
            List<Actor> actors =  await _provider.GetActorsFromUrl();
            await _actorService.RemoveAll(false);
            await _actorService.Add(actors);
        }

    }
}
