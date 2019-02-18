using MitchRankChecker.Model;
using MitchRankChecker.RankChecker.RankCheckers;

namespace MitchRankChecker.RankChecker.Factories
{
    /// <summary>
    /// Interface to create a Rank
    /// Checker, using the abstract
    /// factory design pattern.
    /// </summary>
    public interface IRankCheckerFactory
    {
        /// <summary>
        /// Creates the appropriate rank checker
        /// given a set of parameters.
        /// </summary>
        /// <param name="rankCheckRequest">The rank check request object.</param>
        /// <returns>The rank checker object.</returns>
        IRankChecker CreateRankChecker(RankCheckRequest rankCheckRequest);
    }
}