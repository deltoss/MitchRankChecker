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
        #region Constructors
        /// <summary>
        /// Instantiates a new rank checker.
        /// </summary>
        /// <param name="rankCheckRequest">The model detailing on the rank check information and parameters to configure the rank check.</param>
        /// <param name="client">The HTTP client, used to scrape the data from search engines with.</param>
        public GoogleScrapingRankChecker(RankCheckRequest rankCheckRequest, HttpClient client) : base(rankCheckRequest, client)
        {
        }
        #endregion

        #region ScrapingRankChecker Implementations
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
            string searchUrl = GetSearchUrl();
            using (HttpResponseMessage response = await Client.GetAsync(searchUrl).ConfigureAwait(false))
            {
                using (HttpContent content = response.Content)
                {
                    string result = await content.ReadAsStringAsync().ConfigureAwait(false);
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
            string citeText = citeElement.InnerText.ToLower();
            string websiteUrl = RankCheckRequest.WebsiteUrl.ToLower();
            if (!citeText.Contains(websiteUrl))
                return false;
            return true;
        }
        #endregion

        #region Methods
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
        #endregion
    }
}