using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MitchRankChecker.Model;
using MitchRankChecker.WebApi.Services;

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
        private readonly IRankCheckService _rankCheckService;

        /// <summary>
        /// Constructor that instantiates a new controller.
        /// </summary>
        /// <param name="rankCheckService">Service that performs the rank checking logic.</param>
        public RankCheckRequestsController(IRankCheckService rankCheckService)
        {
            _rankCheckService = rankCheckService;
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
            return (await _rankCheckService.GetRankCheckRequestsAsync()).ToList();
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
            var rankCheckRequest = await _rankCheckService.GetRankCheckRequestAsync(id);

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
            RankCheckRequest persistedRankCheckRequest = await _rankCheckService.QueueRankCheckRequestAsync(rankCheckRequest);
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
        public async Task<ActionResult<bool>> DeleteRankCheckRequest(int id)
        {
            try {
                return await _rankCheckService.DeleteRankCheckRequestAsync(id);
            }
            catch (KeyNotFoundException) {
                return NotFound();
            }
            catch (NotSupportedException) {
                return StatusCode(406);
            }
        }
    }
}
