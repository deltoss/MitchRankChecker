using System;
using MitchRankChecker.RankChecker.RankCheckers;
using MitchRankChecker.Model;
using System.Net.Http;

namespace MitchRankChecker.RankChecker.Factories
{
    /// <summary>
    /// Factory to create a given scraping
    /// rank checker based on a set of parameters.
    /// </summary>
    public class ScrapingRankCheckerFactory : IRankCheckerFactory
    {
        /// <summary>
        /// Instantiate the factory.
        /// </summary>
        /// <param name="client">The HTTP client, used to scrape the data from search engines with.</param>
        public ScrapingRankCheckerFactory(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// The HTTP client, used to scrape the data from search engines with.
        /// </summary>
        private HttpClient _client { get; set; }
        
        /// <summary>
        /// The HTTP client, used to scrape the data from search engines with.
        /// </summary>
        public HttpClient Client {
            get
            {
                return _client;
            }
        }

        /// <summary>
        /// Creates a scraping rank checker
        /// based on a set of parameters.
        /// </summary>
        /// <param name="rankCheckRequest">The model detailing on the rank check information and parameters to configure the rank check.</param>
        /// <returns></returns>
        public IRankChecker CreateRankChecker(RankCheckRequest rankCheckRequest)
        {
            if (string.IsNullOrWhiteSpace(rankCheckRequest.SearchUrl))
                throw new InvalidOperationException($"To create a rank checker, the SearchUrl must be provided with the rank check request.");
            if (rankCheckRequest.SearchUrl.Contains("www.google.com/search"))
                return new GoogleScrapingRankChecker(rankCheckRequest, Client);
            else if (rankCheckRequest.SearchUrl.Contains("www.bing.com/search"))
                return new BingScrapingRankChecker(rankCheckRequest, Client);
            else if (rankCheckRequest.SearchUrl.Contains("au.search.yahoo.com/search"))
                return new YahooScrapingRankChecker(rankCheckRequest, Client);
            throw new NotSupportedException($"No rank checker could be created from the given URL of '{rankCheckRequest.SearchUrl}'.");
        }
    }
}