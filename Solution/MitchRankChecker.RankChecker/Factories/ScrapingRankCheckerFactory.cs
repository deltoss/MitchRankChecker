using System;
using MitchRankChecker.RankChecker.Interfaces;
using MitchRankChecker.RankChecker.RankCheckers;
using MitchRankChecker.Model;

namespace MitchRankChecker.RankChecker.Factories
{
    /// <summary>
    /// Factory to create a given scraping
    /// rank checker based on a set of parameters.
    /// </summary>
    public class ScrapingRankCheckerFactory : IRankCheckerFactory
    {
        /// <summary>
        /// Creates a scraping rank checker
        /// based on a set of parameters.
        /// </summary>
        /// <returns></returns>
        public IRankChecker CreateRankChecker(RankCheckRequest rankCheckRequest)
        {
            if (string.IsNullOrWhiteSpace(rankCheckRequest.SearchUrl))
                throw new InvalidOperationException($"To create a rank checker, the SearchUrl must be provided with the rank check request.");
            if (rankCheckRequest.SearchUrl.Contains("www.google.com/search"))
                return new GoogleScrapingRankChecker(rankCheckRequest);
            else if (rankCheckRequest.SearchUrl.Contains("www.bing.com/search"))
                return new BingScrapingRankChecker(rankCheckRequest);
            else if (rankCheckRequest.SearchUrl.Contains("au.search.yahoo.com/search"))
                return new YahooScrapingRankChecker(rankCheckRequest);
            throw new NotSupportedException($"No rank checker could be created from the given URL of '{rankCheckRequest.SearchUrl}'.");
        }
    }
}