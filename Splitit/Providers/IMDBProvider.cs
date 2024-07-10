using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Splitit.Models;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Xml;
using System.Xml.Linq;

namespace Splitit.Providers
{
    public class IMDBProvider : AbstractProvider
    {
        public IMDBProvider(IOptions<Dictionary<string, string>> urls, HttpClient client) : base(urls, client)
        {
        }

        public override string KeyName { get { return "IMDB"; } }


        public async override Task<List<Actor>> ReturnActorsFromContent(HttpResponseMessage res)
        {
            var content = await res.Content.ReadAsStringAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);
            var actorNodes = doc.DocumentNode.SelectNodes("//li[@class='ipc-metadata-list-summary-item']");
            List<Actor> actors = new List<Actor>();
            if (actorNodes == null) return actors;
            else
            {
                actors = actorNodes.Select(x=>NodeToActor(x)).ToList();
            }

            return actors;
        }


        private Actor NodeToActor(HtmlNode node)
        {
            Actor actor = new Actor();
            var nameNode = node.SelectSingleNode(".//h3[@class='ipc-title__text']");
            if (nameNode != null)
            {
                var arr = nameNode.InnerText.Trim().Split('.');
                string name = arr[1].Trim();
                actor.Name = name;
                string rankStr = arr[0].Trim();
                int rank;
                if (int.TryParse(rankStr, out rank)){
                    actor.Rank = rank;
                }
            }
            var detailsNode = node.SelectSingleNode(".//div[@class='ipc-html-content-inner-div']");
            if (detailsNode != null)
            {
                actor.Details = detailsNode.InnerText.Trim();
            }
            var typeNodes = node.SelectNodes(".//ul[@data-testid='nlib-professions']/li");
            if (typeNodes != null && typeNodes.Nodes().Any())
            {
                actor.Type = typeNodes.First().InnerText.Trim();
            }
            var urlNode = node.SelectSingleNode(".//a[@class='ipc-title-link-wrapper']");
            if ( urlNode != null)
            {
                actor.Source = KeyName;
            }

            return actor;
        }


    }
}
