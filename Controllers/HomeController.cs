using Microsoft.AspNetCore.Mvc;
using NoFlowEngine.Models;
using System.Diagnostics;

namespace NoFlowEngine.Controllers
{
    /// <summary>
    /// The <c>HomeController</c> class is a standard controller providing actions for the home page, AboutUs page, and error display.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes a new instance of the <c>HomeController</c> class.
        /// </summary>
        /// <param name="logger">The logger instance for logging information.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns the <c>Index</c> view.  This is typically the main landing page of the application.
        /// </summary>
        /// <returns>An <c>IActionResult</c> representing the <c>Index</c> view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Returns the <c>AboutUs</c> view, typically displaying the application's team and goal.
        /// </summary>
        /// <returns>An <c>IActionResult</c> representing the <c>AboutUs</c> view.</returns>
        public IActionResult AboutUs()
        {
            return View();
        }

        /// <summary>
        /// Returns the <c>Error</c> view, displaying error information.  It includes a <c>RequestId</c> which can be useful for debugging.
        /// </summary>
        /// <returns>An <c>IActionResult</c> representing the <c>Error</c> view.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}