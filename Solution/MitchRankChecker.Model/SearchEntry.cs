using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using MitchRankChecker.Model.Interfaces;

namespace MitchRankChecker.Model
{
    /// <summary>
    /// Model class that contains information
    /// pertaining to a particular entry of
    /// interest in a search engine for
    /// a specific search rank check.
    /// </summary>
    public class SearchEntry : ITrackable
    {
        /// <summary>
        /// The unique identifier acting as the primary key.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// A number representing the rank of the entry
        /// for a particular search rank check request.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Rank { get; set; }

        /// <summary>
        /// The Url which the entry links to.
        /// </summary>
        [Required]
        [Url]
        public string Url { get; set; }

        /// <summary>
        /// The foreign key property relating to the
        /// associated search rank check request.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int RankCheckRequestId { get; set; }
        /// <summary>
        /// The associated search rank check request.
        /// </summary>
        public RankCheckRequest RankCheckRequest { get; set; }
        
        #region ITrackable Implementation
        /// <summary>
        /// The date and time this entry was created.
        /// </summary>
        [DataType(DataType.Date)]
        [ConcurrencyCheck]
        public DateTime? CreatedAt { get; set; }
        
        /// <summary>
        /// The date and time this entry was last updated.
        /// </summary>
        [DataType(DataType.Date)]
        [ConcurrencyCheck]
        public DateTime? LastUpdatedAt { get; set; }
        #endregion
    }
}