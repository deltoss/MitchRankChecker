using System;

namespace MitchRankChecker.Model.Interfaces
{
    /// <summary>
    /// Interface implemented by
    /// models that should be
    /// trackable.
    /// </summary>
    public interface ITrackable
    {
        /// <summary>
        /// The date and time this entry was created.
        /// </summary>
        DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The date and time this entry was last updated.
        /// </summary>
        DateTime? LastUpdatedAt { get; set; }
    }
}