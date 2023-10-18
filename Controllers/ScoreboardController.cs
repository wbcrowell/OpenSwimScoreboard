using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenSwimScoreboard.Models;

namespace OpenSwimScoreboard.Controllers
{
    public class ScoreboardController : Controller
    {
        private readonly ILogger<ScoreboardController> _logger;

        public ScoreboardController(ILogger<ScoreboardController> logger)
        {
            _logger = logger;
        }

        public IActionResult Html()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Svg()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
