using System.Diagnostics;
using BarbeariaSilva.Models;
using Microsoft.AspNetCore.Mvc;

namespace BarbeariaSilva.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CadastroCliente()
        {
            return View("~/Views/Cliente/Cadastro.cshtml");
        }

        [HttpGet]
        public IActionResult CadastroBarbeiro()
        {
            return View("~/Views/Barbeiro/Cadastro.cshtml");
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
