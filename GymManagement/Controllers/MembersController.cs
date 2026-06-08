using GymManagement.BLL.Services.Interfaces;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _member;

        public MembersController(IMemberService member) 
        {

            _member = member;
        }

        public async Task<IActionResult> Index(CancellationToken ct) 
         {
            var members = await _member.GetAllMembesAsync(ct);
             return View(members);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

    }
}
