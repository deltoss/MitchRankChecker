using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using MitchRankChecker.Model;

namespace MitchRankChecker.RankChecker.RankCheckers
{
    /// <summary>
    /// Rank checker for Yahoo search Engine.
    /// </summary>
    public class YahooScrapingRankChecker : ScrapingRankChecker
    {
        /// <summary>
        /// Affects how many search results are
        /// requested per page at a time to scrape.
        /// </summary>
        protected int _pageSize = 10;

        /// <summary>
        /// Instantiates a new rank checker.
        /// </summary>
        /// <param name="rankCheckRequest">The model detailing on the rank check information and parameters to configure the rank check.</param>
        /// <returns></returns>
        public YahooScrapingRankChecker(RankCheckRequest rankCheckRequest) : base(rankCheckRequest)
        {
        }

        /// <inheritdoc/>
        protected override SearchEntry ExtractSearchEntry(HtmlNode searchEntryElement, List<HtmlNode> searchEntryElements)
        {
            HtmlNode citeElement = searchEntryElement.SelectSingleNode($"descendant::*[contains(@class,'compTitle')][1]//span[1]");
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
            var searchEntryElements = new List<HtmlNode>();
            int currentPage = 1;
            HtmlNodeCollection htmlNodes = await GetResultsFromPageAsync(currentPage);
            while (htmlNodes != null && htmlNodes.Count > 0 && searchEntryElements.Count < RankCheckRequest.MaximumRecords)
            {
                searchEntryElements.AddRange(htmlNodes);
                htmlNodes = await GetResultsFromPageAsync(++currentPage);
            }

            if (searchEntryElements.Count > RankCheckRequest.MaximumRecords)
                searchEntryElements.RemoveRange(RankCheckRequest.MaximumRecords - 1, searchEntryElements.Count - RankCheckRequest.MaximumRecords);
            return searchEntryElements;
        }

        private async Task<HtmlNodeCollection> GetResultsFromPageAsync(int currentPage)
        {
            string searchUrl = GetSearchUrl(currentPage);
            HttpClient client = new HttpClient();
            using (HttpResponseMessage response = await client.GetAsync(searchUrl))
            {
                using (HttpContent content = response.Content)
                {
                    string result = await content.ReadAsStringAsync();
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(result);

                    return document.DocumentNode.SelectNodes($"//*[contains(@class,'searchCenterMiddle')][1]//*[contains(@class,'dd')][contains(@class,'algo')]//descendant::*[contains(@class,'compTitle')][1]//span[1]//ancestor::*[contains(@class,'dd')][contains(@class,'algo')][1]");
                }
            }
        }

        /// <inheritdoc/>
        protected override bool ShouldRecordRank(HtmlNode searchEntryElement)
        {
            HtmlNode citeElement = searchEntryElement.SelectSingleNode($"descendant::*[contains(@class,'compTitle')][1]//span[1]");
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
        private string GetSearchUrl(int pageNumber)
        {
            var uriBuilder = new UriBuilder(RankCheckRequest.SearchUrl);
            NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["p"] = RankCheckRequest.TermToSearch;
            if (pageNumber > 1)
            {
                query["b"] = $"{(pageNumber - 1) * _pageSize + 1}";
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }
    }
}