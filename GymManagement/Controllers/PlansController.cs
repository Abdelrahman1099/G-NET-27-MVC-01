using GymManagement.DbContexts;
using GymManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        private readonly GymDbContext _dbContext = new GymDbContext();

        public async Task<IActionResult> Index()
        {
            var plans = await _dbContext.Plans.ToListAsync();

            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await _dbContext.Plans.FirstOrDefaultAsync(p => p.Id == id);
            if (plan is null) return RedirectToAction(nameof(Index));
            return View(plan);
        
        }
    }
}
