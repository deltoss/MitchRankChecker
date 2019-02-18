using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MitchRankChecker.Mvc.Controllers
{
    [Route("RankCheckRequests/{rankCheckRequestId}/SearchEntries")]
    public class RankCheckRequestSearchEntriesController : Controller
    {
        [HttpGet]
        public IActionResult Index(int rankCheckRequestId)
        {
            return View("Index", rankCheckRequestId);
        }
    }
}