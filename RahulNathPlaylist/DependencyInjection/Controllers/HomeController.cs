using System.Diagnostics;
using DependencyInjection.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DependencyInjection.Models;

namespace DependencyInjection.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITeamsRepository _teamsRepository;


        public HomeController(ILogger<HomeController> logger, ITeamsRepository teamsRepository)
        {
            _logger = logger;
            _teamsRepository = teamsRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public string GetTeamName()
        {
            return _teamsRepository.GetTeamName();
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