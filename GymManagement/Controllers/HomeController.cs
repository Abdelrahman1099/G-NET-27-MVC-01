using System.Diagnostics;
using System.Threading.Tasks;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAnalaticsService _analaticsService;

        public HomeController(ILogger<HomeController> logger, IAnalaticsService analaticsService)
        {
            _logger = logger;
            _analaticsService = analaticsService;
        }

        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var result = await _analaticsService.GetDataAsync(ct);
            return View(result);
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
