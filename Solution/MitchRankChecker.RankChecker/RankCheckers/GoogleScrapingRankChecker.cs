using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using MitchRankChecker.Model;

namespace MitchRankChecker.RankChecker.RankCheckers
{
    /// <summary>
    /// Rank checker for Google search Engine.
    /// </summary>
    public class GoogleScrapingRankChecker : ScrapingRankChecker
    {
        /// <summary>
        /// Instantiates a new rank checker.
        /// </summary>
        /// <param name="rankCheckRequest">The model detailing on the rank check information and parameters to configure the rank check.</param>
        /// <returns></returns>
        public GoogleScrapingRankChecker(RankCheckRequest rankCheckRequest) : base(rankCheckRequest)
        {
        }

        /// <inheritdoc/>
        protected override SearchEntry ExtractSearchEntry(HtmlNode searchEntryElement, List<HtmlNode> searchEntryElements)
        {
            HtmlNode citeElement = searchEntryElement.SelectSingleNode($"descendant::cite");
            var searchEntry = new SearchEntry();
            searchEntry.CreatedAt = DateTime.Now;
            searchEntry.LastUpdatedAt = DateTime.Now;
            searchEntry.RankCheckRequestId = RankCheckRequest.Id;
            searchEntry.RankCheckRequest = RankCheckRequest;
            searchEntry.Rank = searchEntryElements.IndexOf(searchEntryElement) + 1;
            searchEntry.Url = citeElement.InnerText;
            return searchEntry;
        }

        /// <inheritdoc/>
        public override async Task<List<HtmlNode>> ExtractSearchEntryElementsAsync()
        {
            HttpClient client = new HttpClient();
            string searchUrl = GetSearchUrl();
            using (HttpResponseMessage response = await client.GetAsync(searchUrl))
            {
                using (HttpContent content = response.Content)
                {
                    string result = await content.ReadAsStringAsync();
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(result);

                    HtmlNodeCollection htmlNodes = document.DocumentNode.SelectNodes($"//*[@id='search'][1]//*[@class='g']//descendant::cite[1]//ancestor::*[@class='g'][1]");

                    if (htmlNodes != null)
                        return htmlNodes.ToList();
                    else
                        return new List<HtmlNode>();
                }
            }
        }

        /// <inheritdoc/>
        protected override bool ShouldRecordRank(HtmlNode searchEntryElement)
        {
            HtmlNode citeElement = searchEntryElement.SelectSingleNode($"descendant::cite");
            if (citeElement == null)
                return false;
            if (!citeElement.InnerText.Contains(RankCheckRequest.WebsiteUrl))
                return false;
            return true;
        }

        /// <summary>
        /// Gets the Search URL based on
        /// the rank check request
        /// information, and the
        /// query strings the Google
        /// Search Engine utilises.
        /// </summary>
        /// <returns>The search URL</returns>
        private string GetSearchUrl()
        {
            var uriBuilder = new UriBuilder(RankCheckRequest.SearchUrl);
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["q"] = RankCheckRequest.TermToSearch;
            query["num"] = RankCheckRequest.MaximumRecords.ToString();
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }
    }
}