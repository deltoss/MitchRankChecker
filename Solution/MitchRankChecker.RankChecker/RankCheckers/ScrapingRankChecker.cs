using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MitchRankChecker.RankChecker.Interfaces;
using MitchRankChecker.Model;
using HtmlAgilityPack;

namespace MitchRankChecker.RankChecker.RankCheckers
{
    /// <summary>
    /// <para>
    /// Abstract rank checker class, defining
    /// common functionality and codebase
    /// between the various rank checkers.
    /// </para>
    /// <para>
    /// Checks the ranks through the method of
    /// scraping from the particular search
    /// engine pages.
    /// </para>
    /// </summary>
    public abstract class ScrapingRankChecker : IRankChecker
    {
        /// <summary>
        /// Details the rank check request, configuring
        /// the rank checker.
        /// </summary>
        public RankCheckRequest RankCheckRequest { get; set; }

        /// <summary>
        /// The scraped search entry elements.
        /// </summary>
        public List<HtmlNode> SearchEntryElements { get; set; }

        /// <summary>
        /// The scraped search entry elements
        /// that are relevant.
        /// </summary>
        public List<HtmlNode> RelevantSearchEntryElements { get; set; }

        /// <summary>
        /// Instantiates a new rank checker.
        /// </summary>
        /// <param name="rankCheckRequest">The model detailing on the rank check information and parameters to configure the rank check.</param>
        public ScrapingRankChecker(RankCheckRequest rankCheckRequest)
        {
            RankCheckRequest = rankCheckRequest;
        }

        #region IRankChecker Implementation
        /// <inheritdoc/>
        public List<SearchEntry> ExtractRankEntries()
        {
            Task<List<SearchEntry>> task = ExtractRankEntriesAsync();
            task.Wait();
            return task.Result;
        }

        /// <inheritdoc/>
        public async Task<List<SearchEntry>> ExtractRankEntriesAsync()
        {
            var searchEntries = new List<SearchEntry>();

            List<HtmlNode> searchEntryElements = await ExtractSearchEntryElementsAsync();
            // If no search results found
            if (searchEntryElements == null || searchEntryElements.Count < 1)
                return searchEntries;
            SearchEntryElements = searchEntryElements.ToList();
            RelevantSearchEntryElements = searchEntryElements.Where(x => ShouldRecordRank(x)).ToList();
            foreach (HtmlNode searchEntryElement in RelevantSearchEntryElements)
            {
                try
                {
                    SearchEntry searchEntry = ExtractSearchEntry(searchEntryElement, searchEntryElements);
                    searchEntries.Add(searchEntry);
                }
                catch (Exception ex)
                {
                    throw new KeyNotFoundException($"Unable to extract the SearchEntry model from the HTMLNode with XPath of {searchEntryElement.XPath}", ex);
                }
            }
            return searchEntries;
        }
        #endregion

        /// <summary>
        /// Checks for the passed search entry node
        /// for whether it should have its search
        /// rank recorded or not.
        /// </summary>
        /// <returns>Boolean whether the particular search entry node should have its rank recorded.</returns>
        protected abstract bool ShouldRecordRank(HtmlNode searchEntryElement);

        /// <summary>
        /// Extracts the search entry elements
        /// of interest from the HTML document
        /// to be parsed and processed.
        /// </summary>
        /// <returns>Collection of HTML Nodes representing the search entries.</returns>
        public List<HtmlNode> ExtractSearchEntryElements()
        {
            Task<List<HtmlNode>> task = ExtractSearchEntryElementsAsync();
            task.Wait();
            return task.Result;
        }

        /// <summary>
        /// Extracts the search entry elements
        /// of interest from the HTML document
        /// to be parsed and processed.
        /// </summary>
        /// <returns>Collection of HTML Nodes representing the search entries.</returns>
        public abstract Task<List<HtmlNode>> ExtractSearchEntryElementsAsync();

        /// <summary>
        /// Extracts a search entry model
        /// From a given HTML Node element
        /// representing a search entry.
        /// </summary>
        /// <returns>The populated search entry model.</returns>
        protected abstract SearchEntry ExtractSearchEntry(HtmlNode searchEntryElement, List<HtmlNode> searchEntryElements);
    }
}