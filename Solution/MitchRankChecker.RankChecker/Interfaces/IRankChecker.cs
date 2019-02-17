using System.Collections.Generic;
using System.Threading.Tasks;
using MitchRankChecker.Model;

namespace MitchRankChecker.RankChecker.Interfaces
{
    /// <summary>
    /// Interface to check the website ranks
    /// against a particular search term
    /// for a search engine.
    /// </summary>
    public interface IRankChecker
    {
        /// <summary>
        /// Extracts the rank entries.
        /// </summary>
        /// <returns>List of Search rank entries.</returns>
        Task<List<SearchEntry>> ExtractRankEntriesAsync();

        /// <summary>
        /// Extracts the rank entries.
        /// </summary>
        /// <returns>List of Search rank entries.</returns>
        List<SearchEntry> ExtractRankEntries();
    }
}