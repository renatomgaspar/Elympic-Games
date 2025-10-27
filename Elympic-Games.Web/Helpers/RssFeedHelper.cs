using Elympic_Games.Web.Models.News;
using HtmlAgilityPack;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Xml.Linq;

namespace Elympic_Games.Web.Helpers
{
    public class RssFeedHelper : IRssFeedHelper
    {
        public async Task<List<New>> GetEsportsNewsAsync(string rssUrl, int maxItems = 9)
        {
            var news = new List<New>();

            using var reader = XmlReader.Create(rssUrl);
            var feed = SyndicationFeed.Load(reader);

            foreach (var item in feed.Items.Take(maxItems))
            {
                string imageUrl = "";

                var contentEncoded = item.ElementExtensions
                    .FirstOrDefault(e => e.OuterName == "encoded" && e.OuterNamespace.Contains("content"))?
                    .GetObject<string>();

                if (!string.IsNullOrEmpty(contentEncoded))
                {
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(contentEncoded);

                    var imgNode = htmlDoc.DocumentNode.SelectSingleNode("//figure//img");
                    if (imgNode != null)
                    {
                        imageUrl = imgNode.GetAttributeValue("src", "");
                    }
                }

                news.Add(new New
                {
                    Title = item.Title.Text,
                    Link = item.Links.FirstOrDefault()?.Uri.ToString(),
                    PublishDate = item.PublishDate.DateTime,
                    ImageUrl = imageUrl
                });
            }

            return news;
        }
    }

    public class RssItem
    {
        
    }
}

