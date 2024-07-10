using Microsoft.Extensions.Options;
using Splitit.Models;

namespace Splitit.Providers
{
    //not implemented
    public class RottenTomatoesProvider : AbstractProvider
    {
        public RottenTomatoesProvider(IOptions<Dictionary<string, string>> urls, HttpClient client) : base(urls, client)
        {
        }

        public override string KeyName { get { return "RottenTomatoes"; } }

        public async Task<List<Actor>> GetActors()
        {
            throw new NotImplementedException();
        }

        public Task<List<Actor>> GetActorsFromUrl()
        {
            throw new NotImplementedException();
        }

        public override Task<List<Actor>> ReturnActorsFromContent(HttpResponseMessage res)
        {
            throw new NotImplementedException();
        }


    }
}
