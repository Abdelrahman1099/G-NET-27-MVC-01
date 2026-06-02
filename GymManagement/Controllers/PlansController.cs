using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using GymManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        //private readonly GymDbContext _dbContext = new GymDbContext();

        private readonly IGenericRepository<Plan> _planRepositories;

        public PlansController(IGenericRepository<Plan> planRepository)
        {
            _planRepositories = planRepository;
        }
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            //var plans = await _dbContext.Plans.ToListAsync();

            var plans = await _planRepositories.GetAllAsync(ct : ct);

            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {
            //var plan = await _dbContext.Plans.FirstOrDefaultAsync(p => p.Id == id);

            var plan = await _planRepositories.GetById(id, ct);

            if (plan is null) return RedirectToAction(nameof(Index));
            return View(plan);
        
        }
    }
}
