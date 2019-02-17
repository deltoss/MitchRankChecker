using MitchRankChecker.RankChecker.Interfaces;
using MitchRankChecker.RankChecker.RankCheckers;

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
        public IRankChecker CreateRankChecker()
        {
            throw new System.NotImplementedException();
        }
    }
}