using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HostedServiceBackgroundTasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MitchRankChecker.EntityFramework;
using MitchRankChecker.Model;
using MitchRankChecker.Model.Enumerations;
using MitchRankChecker.RankChecker.Factories;
using MitchRankChecker.RankChecker.RankCheckers;

namespace MitchRankChecker.WebApi.Services
{
    /// <summary>
    /// The rank checking service.
    /// </summary>
    public interface IRankCheckService
    {
        /// <summary>
        /// Gets all rank checker requests from persistence.
        /// </summary>
        /// <returns>The collection of rank check requests.</returns>
        Task<IEnumerable<RankCheckRequest>> GetRankCheckRequestsAsync();

        /// <summary>
        /// Gets a single rank check request from persistence.
        /// </summary>
        /// <returns>The single rank check request.</returns>
        Task<RankCheckRequest> GetRankCheckRequestAsync(int id);

        /// <summary>
        /// Deletes a single rank check request from persistence.
        /// </summary>
        /// <returns>True if deleted the rank check request. False otherwise.`</returns>
        Task<bool> DeleteRankCheckRequestAsync(int id);

        /// <summary>
        /// Creates and queues a rank check request.
        /// </summary>
        /// <param name="rankCheckRequest">The parameters of the rank check request to create with.</param>
        /// <returns>The persisted rank check request.</returns>
        Task<RankCheckRequest> QueueRankCheckRequestAsync(RankCheckRequest rankCheckRequest);

        /// <summary>
        /// Gets search entries for a given rank check request.
        /// </summary>
        /// <param name="rankCheckRequestId">The id of the rank check request to get search entries for.</param>
        /// <returns>The search entries for a given rank check request.</returns>
        Task<IEnumerable<SearchEntry>> GetSearchEntriesAsync(int rankCheckRequestId);
    }
}
