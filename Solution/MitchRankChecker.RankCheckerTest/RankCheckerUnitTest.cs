using System;
using Xunit;
using MitchRankChecker.Model;
using MitchRankChecker.Model.Enumerations;
using MitchRankChecker.RankChecker.RankCheckers;
using MitchRankChecker.RankChecker.Interfaces;
using System.Collections.Generic;

namespace MitchRankChecker.RankCheckerTest
{
    public class RankCheckerUnitTest
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

            IRankChecker rankChecker = new GoogleScrapingRankChecker(rankCheckRequest);
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

            IRankChecker rankChecker = new BingScrapingRankChecker(rankCheckRequest);
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

            IRankChecker rankChecker = new YahooScrapingRankChecker(rankCheckRequest);
            List<SearchEntry> entries = rankChecker.ExtractRankEntries();
            Assert.True(entries.Count > 0);
        }
    }
}
