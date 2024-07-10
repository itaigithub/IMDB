using Splitit.Models;

namespace Splitit.Providers
{
    public interface IProvider
    {
        public Task<List<Actor>> GetActorsFromUrl();
    }
}
