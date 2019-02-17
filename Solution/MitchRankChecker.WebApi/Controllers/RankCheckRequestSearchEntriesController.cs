using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MitchRankChecker.EntityFramework;
using MitchRankChecker.Model;

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
        /// <summary>
        /// The Entity Framework database context
        /// to manipulate the database with.
        /// </summary>
        private readonly RankCheckerDbContext _context;

        /// <summary>
        /// Constructor that instantiates a new controller.
        /// </summary>
        /// <param name="context">The Entity framework database context.</param>
        public RankCheckRequestSearchEntriesController(RankCheckerDbContext context)
        {
            _context = context;
        }

        // GET: api/RankCheckRequests/{rankCheckRequestId:int}/SearchEntries
        /// <summary>
        /// Gets a rank check request object given an ID.
        /// </summary>
        /// <param name="rankCheckRequestId">The id of the rank check request to get search entries for.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/RankCheckRequests/1/SearchEntries
        ///
        /// </remarks>
        /// <returns>The rank check request item.</returns>
        /// <response code="200">Returns the rank check request item</response>
        /// <response code="404">If the item is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SearchEntry>>> GetRankCheckRequestSearchEntries(int rankCheckRequestId)
        {
            var rankCheckRequest = await _context.RankCheckRequests.FindAsync(rankCheckRequestId);
            if (rankCheckRequest == null)
            {
                return NotFound();
            }
            
            return await _context.SearchEntries.Where(x => x.RankCheckRequestId == rankCheckRequestId).ToListAsync();
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
