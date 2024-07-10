using Microsoft.Extensions.Options;
using Splitit.Models;

namespace Splitit.Providers
{
    public abstract class AbstractProvider : IProvider
    {
        private readonly Dictionary<string, string> _urls;
        private readonly HttpClient _client;
        public AbstractProvider(IOptions<Dictionary<string, string>> urls, HttpClient client)
        {
            _urls = urls.Value;
            _client = client;
        }
        public abstract string KeyName { get; }

        public abstract Task<List<Actor>> ReturnActorsFromContent(HttpResponseMessage res);

        public async Task<List<Actor>> GetActorsFromUrl()
        {
            List<Actor> res = new List<Actor>();
            string url = _urls[KeyName];
            HttpRequestMessage message = new();
            {
                message.Headers.Add("Accept", "application/json");
            }
            message.Method = HttpMethod.Get;
            message.RequestUri = new Uri(url);
            HttpResponseMessage httpResponseMessage = await _client.SendAsync(message);            
            if (httpResponseMessage.IsSuccessStatusCode)
            {
               res = await ReturnActorsFromContent(httpResponseMessage);
            }
            else
            {
                throw new GetActorsHtmlException(url);
            }
            return res;
        }

        public class GetActorsHtmlException : Exception
        {
            public GetActorsHtmlException(string url) : base(String.Format("Could not load actor data from {0} ", url))
            {
            }
        }

    }
}
