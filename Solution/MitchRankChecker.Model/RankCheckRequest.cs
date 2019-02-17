using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using MitchRankChecker.Model.Interfaces;
using System.Collections.Generic;
using MitchRankChecker.Model.Enumerations;

namespace MitchRankChecker.Model
{
    /// <summary>
    /// The model class that captures information 
    /// pertaining to a user request for a rank check.
    /// </summary>
    public class RankCheckRequest: ITrackable
    {
        /// <summary>
        /// The unique identifier acting as the primary key.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The Search Url to begin the rank check with.
        /// </summary>
        /// <value></value>
        [Required]
        [Url]
        public string SearchUrl { get; set; }

        /// <summary>
        /// Maximum number of search records
        /// to go through. If you only specify
        /// 100, it means the rank check only
        /// uses the top 100 results.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int MaximumRecords { get; set; }

        /// <summary>
        /// Represents the status in the job queue.
        /// </summary>
        [Required]
        public int StatusId { get; set; }

        /// <summary>
        /// Represents the status in the job queue.
        /// </summary>
        public RankCheckRequestStatus Status
        { 
            get {
                return RankCheckRequestStatus.FromId<RankCheckRequestStatus>(StatusId);
            }
            set {
                if (value == null)
                    StatusId = 0;
                else
                    StatusId = value.Id;
            }
        }

        /// <summary>
        /// <para>The error message.</para>
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// <para>The term to search</para>
        /// </summary>
        [Required]
        public string TermToSearch { get; set; }

        /// <summary>
        /// <para>The Website URL to check rankings for.</para>
        /// </summary>
        [Required]
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// The associated search rank entries.
        /// </summary>
        public List<SearchEntry> SearchEntries { get; set; }
        
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