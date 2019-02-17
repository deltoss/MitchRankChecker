using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HostedServiceBackgroundTasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MitchRankChecker.EntityFramework;
using MitchRankChecker.Model;
using MitchRankChecker.Model.Enumerations;
using MitchRankChecker.RankChecker.Factories;
using MitchRankChecker.RankChecker.Interfaces;

namespace MitchRankChecker.WebApi.Controllers
{
    /// <summary>
    /// API controller to manage
    /// rank check requests.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RankCheckRequestsController : ControllerBase
    {
        /// <summary>
        /// The Entity Framework database context
        /// to manipulate the database with.
        /// </summary>
        private readonly RankCheckerDbContext _context;

        /// <summary>
        /// The queue to assign background services into.
        /// </summary>
        private IBackgroundTaskQueue _queue;

        /// <summary>
        /// Constructor that instantiates a new controller.
        /// </summary>
        /// <param name="context">The Entity framework database context.</param>
        public RankCheckRequestsController(RankCheckerDbContext context, IBackgroundTaskQueue queue)
        {
            _context = context;
            _queue = queue;
        }

        /// <summary>
        /// Gets the list of rank check request objects.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/RankCheckRequests
        ///
        /// </remarks>
        /// <returns>The list of rank check request items.</returns>
        /// <response code="200">Returns the rank check request item</response>
        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RankCheckRequest>>> GetSearchRankChecks()
        {
            return await _context.RankCheckRequests.ToListAsync();
        }

        /// <summary>
        /// Gets a rank check request object given an ID.
        /// </summary>
        /// <param name="id">The id of the rank check request to get.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/RankCheckRequests/5
        ///
        /// </remarks>
        /// <returns>The rank check request item.</returns>
        /// <response code="200">Returns the rank check request item</response>
        /// <response code="404">If the item is not found</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public async Task<ActionResult<RankCheckRequest>> GetRankCheckRequest(int id)
        {
            var rankCheckRequest = await _context.RankCheckRequests.FindAsync(id);

            if (rankCheckRequest == null)
            {
                return NotFound();
            }

            return rankCheckRequest;
        }

        /// <summary>
        /// Creates a rank check request given an ID, which will be processed in the background.
        /// </summary>
        /// <param name="rankCheckRequest">The parameters of the rank check request to create with.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/RankCheckRequests
        ///     {
        ///        "SearchUrl": "www.google.com/search",
        ///        "MaximumRecords": 100,
        ///        "TermToSearch": "Online Title Search",
        ///        "WebsiteUrl": "www.infotrack.com.au",
        ///     }
        /// <br />
        /// </remarks>
        /// <returns>A newly created RankCheckRequest object</returns>
        /// <response code="201">The item created</response>
        [HttpPost]
        public async Task<ActionResult<RankCheckRequest>> PostRankCheckRequest(RankCheckRequest rankCheckRequest)
        {
            rankCheckRequest.Status = RankCheckRequestStatus.InQueue;
            _context.RankCheckRequests.Add(rankCheckRequest);
            await _context.SaveChangesAsync();

            _queue.QueueBackgroundWorkItem(async token =>
            {
                RankCheckRequest updatedRankCheckRequest;
                updatedRankCheckRequest = await _context.RankCheckRequests.Where(x => x.Id == rankCheckRequest.Id).FirstAsync();
                updatedRankCheckRequest.Status = RankCheckRequestStatus.InProgress;
                _context.Attach(updatedRankCheckRequest);
                _context.Entry(updatedRankCheckRequest).Property(x => x.StatusId).IsModified = true; // Mark the specific field as modified to only update that field
                await _context.SaveChangesAsync();
                try
                {
                    var scrapingRankCheckerFactory = new ScrapingRankCheckerFactory();
                    IRankChecker rankChecker = scrapingRankCheckerFactory.CreateRankChecker(updatedRankCheckRequest);
                    List<SearchEntry> relevantSearchEntries = await rankChecker.ExtractRankEntriesAsync();
                    if (relevantSearchEntries != null && relevantSearchEntries.Count > 0)
                        await _context.SearchEntries.AddRangeAsync(relevantSearchEntries);

                    updatedRankCheckRequest.Status = RankCheckRequestStatus.Completed;
                    _context.Attach(updatedRankCheckRequest);
                    _context.Entry(updatedRankCheckRequest).Property(x => x.StatusId).IsModified = true;
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    updatedRankCheckRequest.Status = RankCheckRequestStatus.Error;
                    updatedRankCheckRequest.ErrorMessage = ex.Message;
                    _context.Attach(updatedRankCheckRequest);
                    _context.Entry(updatedRankCheckRequest).Property(x => x.StatusId).IsModified = true;
                    _context.Entry(updatedRankCheckRequest).Property(x => x.ErrorMessage).IsModified = true;
                    await _context.SaveChangesAsync();
                }
                
            });

            return CreatedAtAction("GetRankCheckRequest", new { id = rankCheckRequest.Id }, rankCheckRequest);
        }

        /// <summary>
        /// Deletes a rank check request given an ID.
        /// </summary>
        /// <param name="id">The id of the rank check request to delete.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/RankCheckRequests/5
        ///
        ///
        /// </remarks>
        /// <response code="200">The deleted item if deleted successfully</response>
        /// <response code="404">If the item is not found</response>
        /// <response code="406">Not acceptable. Cannot delete an item that's in the InProgress state and currently being processed</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(406)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<RankCheckRequest>> DeleteRankCheckRequest(int id)
        {
            var rankCheckRequest = await _context.RankCheckRequests.FindAsync(id);
            if (rankCheckRequest == null)
            {
                return NotFound();
            }

            if (rankCheckRequest.Status == RankCheckRequestStatus.InProgress)
                return StatusCode(406);

            _context.RankCheckRequests.Remove(rankCheckRequest);
            await _context.SaveChangesAsync();

            return rankCheckRequest;
        }

        /// <summary>
        /// Checks if a rank check request exists in the database.
        /// </summary>
        /// <param name="id">The rank check request to check if it exists in database.</param>
        /// <returns>True if it exists, false otherwise</returns>
        private bool RankCheckRequestExists(int id)
        {
            return _context.RankCheckRequests.Any(e => e.Id == id);
        }
    }
}
