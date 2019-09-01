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
    public class RankCheckService : IRankCheckService
    {
        /// <summary>
        /// The Entity Framework database context
        /// to manipulate the database with.
        /// </summary>
        private readonly RankCheckerDbContext _context;

        /// <summary>
        /// The queue to assign background services into.
        /// </summary>
        private readonly IBackgroundTaskQueue _queue;

        /// <summary>
        /// Constructor that instantiates a new service.
        /// </summary>
        /// <param name="context">The Entity framework database context.</param>
        /// <param name="queue">The background job processing queue</param>
        public RankCheckService(RankCheckerDbContext context, IBackgroundTaskQueue queue)
        {
            _context = context;
            _queue = queue;
        }

        /// <summary>
        /// Gets all rank check requests from persistence.
        /// </summary>
        /// <returns>The collection of rank check requests.</returns>
        public async Task<IEnumerable<RankCheckRequest>> GetRankCheckRequestsAsync()
        {
            return await _context.RankCheckRequests.ToListAsync();
        }

        /// <summary>
        /// Deletes a single rank check request from persistence.
        /// </summary>
        /// <returns>True if deleted the rank check request. False otherwise.`</returns>
        public async Task<bool> DeleteRankCheckRequestAsync(int id)
        {
            var rankCheckRequest = await GetRankCheckRequestAsync(id);
            if (rankCheckRequest == null)
            {
                throw new KeyNotFoundException($"Could not find the rank check request of ${id}");
            }
            if (rankCheckRequest.Status == RankCheckRequestStatus.InProgress)
            {
                throw new NotSupportedException($"Deleting a rank check request ({rankCheckRequest.Id}) whilst in progress is not supported.");
            }

            _context.RankCheckRequests.Remove(rankCheckRequest);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Gets a single rank check request from persistence.
        /// </summary>
        /// <returns>The single rank check request.</returns>
        public async Task<RankCheckRequest> GetRankCheckRequestAsync(int id)
        {
            return await _context.RankCheckRequests.FindAsync(id);
        }

        /// <summary>
        /// Creates and queues a rank check request.
        /// </summary>
        /// <param name="rankCheckRequest">The parameters of the rank check request to create with.</param>
        /// <returns>The persisted rank check request.</returns>
        public async Task<RankCheckRequest> QueueRankCheckRequestAsync(RankCheckRequest rankCheckRequest)
        {
            rankCheckRequest.Status = RankCheckRequestStatus.InQueue;
            _context.RankCheckRequests.Add(rankCheckRequest);
            await _context.SaveChangesAsync();

            _queue.QueueBackgroundWorkItem(async (token, provider) =>
            {
                RankCheckRequest updatedRankCheckRequest;
                var context = provider.GetService<RankCheckerDbContext>();

                updatedRankCheckRequest = await context.RankCheckRequests.Where(x => x.Id == rankCheckRequest.Id).FirstAsync();
                updatedRankCheckRequest.Status = RankCheckRequestStatus.InProgress;
                context.Attach(updatedRankCheckRequest);
                context.Entry(updatedRankCheckRequest).Property(x => x.StatusId).IsModified = true; // Mark the specific field as modified to only update that field
                await context.SaveChangesAsync();
                try
                {
                    var scrapingRankCheckerFactory = provider.GetService<IRankCheckerFactory>();
                    IRankChecker rankChecker = scrapingRankCheckerFactory.CreateRankChecker(updatedRankCheckRequest);
                    List<SearchEntry> relevantSearchEntries = await rankChecker.ExtractRankEntriesAsync();
                    if (relevantSearchEntries != null && relevantSearchEntries.Count > 0)
                        await context.SearchEntries.AddRangeAsync(relevantSearchEntries);

                    updatedRankCheckRequest.Status = RankCheckRequestStatus.Completed;
                    context.Attach(updatedRankCheckRequest);
                    context.Entry(updatedRankCheckRequest).Property(x => x.StatusId).IsModified = true;
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    updatedRankCheckRequest.Status = RankCheckRequestStatus.Error;
                    updatedRankCheckRequest.ErrorMessage = ex.Message;
                    context.Attach(updatedRankCheckRequest);
                    context.Entry(updatedRankCheckRequest).Property(x => x.StatusId).IsModified = true;
                    context.Entry(updatedRankCheckRequest).Property(x => x.ErrorMessage).IsModified = true;
                    await context.SaveChangesAsync();
                }
            });

            return rankCheckRequest;
        }

        /// <summary>
        /// Gets search entries for a given rank check request.
        /// </summary>
        /// <param name="rankCheckRequestId">The id of the rank check request to get search entries for.</param>
        /// <returns>The search entries for a given rank check request.</returns>
        public async Task<IEnumerable<SearchEntry>> GetSearchEntriesAsync(int rankCheckRequestId)
        {
            return await _context.SearchEntries.Where(x => x.RankCheckRequestId == rankCheckRequestId).ToListAsync();
        }
    }
}
