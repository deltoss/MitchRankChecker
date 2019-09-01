using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MitchRankChecker.Model;
using MitchRankChecker.WebApi.Services;

namespace MitchRankChecker.WebApi.Controllers
{
    /// <summary>
    /// API controller to manage relevant
    /// search entries for rank check requests.
    /// </summary>
    [Route("api/RankCheckRequests/{rankCheckRequestId:int}/SearchEntries")]
    [ApiController]
    public class RankCheckRequestSearchEntriesController : ControllerBase
    {
        private readonly IRankCheckService _rankCheckService;

        /// <summary>
        /// Constructor that instantiates a new controller.
        /// </summary>
        /// <param name="rankCheckService">Service that performs the rank checking logic.</param>
        public RankCheckRequestSearchEntriesController(IRankCheckService rankCheckService)
        {
            _rankCheckService = rankCheckService;
        }

        // GET: api/RankCheckRequests/{rankCheckRequestId:int}/SearchEntries
        /// <summary>
        /// Gets search entries for a given rank check request.
        /// </summary>
        /// <param name="rankCheckRequestId">The id of the rank check request to get search entries for.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/RankCheckRequests/1/SearchEntries
        ///
        /// </remarks>
        /// <returns>The search entries for a given rank check request.</returns>
        /// <response code="200">Returns the search entries for a given rank check request.</response>
        /// <response code="404">If the rank check request is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SearchEntry>>> GetRankCheckRequestSearchEntries(int rankCheckRequestId)
        {
            RankCheckRequest rankCheckRequest = await _rankCheckService.GetRankCheckRequestAsync(rankCheckRequestId);
            if (rankCheckRequest == null)
            {
                return NotFound();
            }
            
            return (await _rankCheckService.GetSearchEntriesAsync(rankCheckRequestId)).ToList();
        }
    }
}
