using System.Diagnostics;
using BarbeariaSilva.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarbeariaSilva.Controllers
{
    public class SobreController : Controller
    {
        private readonly ILogger<SobreController> _logger;

        public SobreController(ILogger<SobreController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
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
