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
    /// API controller to manage
    /// search entries.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SearchEntriesController : ControllerBase
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
        public SearchEntriesController(RankCheckerDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the list of search entry objects.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/SearchEntries
        ///
        /// </remarks>
        /// <returns>The list of search entry items.</returns>
        /// <response code="200">Returns the search entry item</response>
        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SearchEntry>>> GetSearchEntries()
        {
            return await _context.SearchEntries.ToListAsync();
        }

        /// <summary>
        /// Gets a search entry object given an ID.
        /// </summary>
        /// <param name="id">The id of the search entry to get.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/SearchEntries/5
        ///
        /// </remarks>
        /// <returns>The search entry item.</returns>
        /// <response code="200">Returns the search entry item</response>
        /// <response code="404">If the item is not found</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [HttpGet("{id}")]
        public async Task<ActionResult<SearchEntry>> GetSearchEntry(int id)
        {
            var searchEntry = await _context.SearchEntries.FindAsync(id);

            if (searchEntry == null)
            {
                return NotFound();
            }

            return searchEntry;
        }

        /// <summary>
        /// Checks if a search entry exists in the database.
        /// </summary>
        /// <param name="id">The search entry to check if it exists in database.</param>
        /// <returns>True if it exists, false otherwise</returns>
        private bool SearchEntryExists(int id)
        {
            return _context.SearchEntries.Any(e => e.Id == id);
        }
    }
}
