namespace MitchRankChecker.RankChecker.Interfaces
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
         /// <returns>The rank checker object.</returns>
         IRankChecker CreateRankChecker();
    }
}