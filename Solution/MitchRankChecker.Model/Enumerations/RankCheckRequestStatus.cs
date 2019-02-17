namespace MitchRankChecker.Model.Enumerations
{
    /// <summary>
    /// Enumeration which determines
    /// the status of the rank check
    /// process.
    /// </summary>
    public class RankCheckRequestStatus : Enumeration
    {
        #region Constructors
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public RankCheckRequestStatus()
            : base()
        { 
        }
        /// <summary>
        /// Constructor to creates an enumeration.
        /// </summary>
        /// <param name="id">The Id of the enumeration value</param>
        /// <param name="name">The name of the enumeration value</param>
        protected RankCheckRequestStatus(int id, string name)
            : base(id, name)
        { 
        }
        #endregion

        #region Static Members
        /// <summary>
        /// Enumeration value representing an
        /// undefined status for a search rank
        /// check process.
        /// </summary>
        public static readonly RankCheckRequestStatus Unspecified = new UnspecifiedSearchRankStatus();
        /// <summary>
        /// Enumeration value representing an
        /// in-queue status, where the search
        /// rank process has yet to be started,
        /// and is still pending.
        /// </summary>
        public static readonly RankCheckRequestStatus InQueue = new InQueueSearchRankStatus();
        /// <summary>
        /// Enumeration value representing an
        /// in-queue status, where the search
        /// rank process has started, and is
        /// currently being processed.
        /// </summary>
        public static readonly RankCheckRequestStatus InProgress = new InProgressSearchRankStatus();
        /// <summary>
        /// Enumeration value representing an
        /// in-queue status, where the search
        /// rank process been processed
        /// and completed with results.
        /// </summary>
        public static readonly RankCheckRequestStatus Completed = new CompletedSearchRankStatus();
        /// <summary>
        /// Enumeration value representing an
        /// in-queue status, where the search
        /// rank process has encountered an
        /// error.
        /// </summary>
        public static readonly RankCheckRequestStatus Error = new ErrorSearchRankStatus();
        #endregion

        #region Private Enumeration Classes
        private class UnspecifiedSearchRankStatus : RankCheckRequestStatus
        {
            public UnspecifiedSearchRankStatus(): base(0, "Unspecified")
            { }
        }

        private class InQueueSearchRankStatus : RankCheckRequestStatus
        {
            public InQueueSearchRankStatus(): base(1, "InQueue")
            { }
        }
        
        private class InProgressSearchRankStatus : RankCheckRequestStatus
        {
            public InProgressSearchRankStatus(): base(2, "InProgress")
            { }
        }
        
        private class CompletedSearchRankStatus : RankCheckRequestStatus
        {
            public CompletedSearchRankStatus(): base(3, "Completed")
            { }
        }

        private class ErrorSearchRankStatus : RankCheckRequestStatus
        {
            public ErrorSearchRankStatus(): base(4, "Error")
            { }
        }
        #endregion
    }
}