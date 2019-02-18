using System;
using Xunit;
using MitchRankChecker.Model;
using MitchRankChecker.Model.Enumerations;
using MitchRankChecker.RankChecker.RankCheckers;
using MitchRankChecker.RankChecker.Factories;
using System.Collections.Generic;
using System.Net.Http;

namespace MitchRankChecker.RankCheckerTest
{
    public class ScrapingRankCheckerUnitTest
    {
        [Fact]
        public void TestGoogleRankChecker()
        {
            var rankCheckRequest = new RankCheckRequest()
            {
                SearchUrl = "www.google.com/search",
                MaximumRecords = 100,
                StatusId = RankCheckRequestStatus.InQueue.Id,
                TermToSearch = "Online Title Search",
                WebsiteUrl = "www.infotrack.com.au",
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now
            };

            IRankCheckerFactory factory = new ScrapingRankCheckerFactory(new HttpClient());
            IRankChecker rankChecker = factory.CreateRankChecker(rankCheckRequest);
            Assert.IsAssignableFrom<GoogleScrapingRankChecker>(rankChecker);

            List<SearchEntry> entries = rankChecker.ExtractRankEntries();
            Assert.True(entries.Count > 0);
        }

        [Fact]
        public void TestBingRankChecker()
        {
            var rankCheckRequest = new RankCheckRequest()
            {
                SearchUrl = "www.bing.com/search",
                MaximumRecords = 100,
                StatusId = RankCheckRequestStatus.InQueue.Id,
                TermToSearch = "Online Title Search",
                WebsiteUrl = "www.infotrack.com.au",
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now
            };

            IRankCheckerFactory factory = new ScrapingRankCheckerFactory(new HttpClient());
            IRankChecker rankChecker = factory.CreateRankChecker(rankCheckRequest);
            Assert.IsAssignableFrom<BingScrapingRankChecker>(rankChecker);

            List<SearchEntry> entries = rankChecker.ExtractRankEntries();
            Assert.True(entries.Count > 0);
        }

        [Fact]
        public void TestYahooRankChecker()
        {
            var rankCheckRequest = new RankCheckRequest()
            {
                SearchUrl = "au.search.yahoo.com/search",
                MaximumRecords = 100,
                StatusId = RankCheckRequestStatus.InQueue.Id,
                TermToSearch = "Online Title Search",
                WebsiteUrl = "www.infotrack.com.au",
                CreatedAt = DateTime.Now,
                LastUpdatedAt = DateTime.Now
            };

            IRankCheckerFactory factory = new ScrapingRankCheckerFactory(new HttpClient());
            IRankChecker rankChecker = factory.CreateRankChecker(rankCheckRequest);
            Assert.IsAssignableFrom<YahooScrapingRankChecker>(rankChecker);

            List<SearchEntry> entries = rankChecker.ExtractRankEntries();
            Assert.True(entries.Count > 0);
        }
    }
}
