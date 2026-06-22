using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Sessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Tasks;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService) 
        {
            _sessionService = sessionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct = default) 
        {
            var result = await _sessionService.GetAllSessionAsync(ct);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct) 
        {
            ViewBag.traniers = new SelectList(await _sessionService.GetAllTrainersForDropDownAsync(ct), "Id", "Name");
            ViewBag.categories = new SelectList(await _sessionService.GetAllCategoriesForDropDownAsync(ct), "Id", "CategoryName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct = default) 
        {
            if (ModelState.IsValid)
            {
                var result = await _sessionService.CreateSessionAsync(model, ct);
                if (result.success)
                {
                    TempData["SuccesMessage"] = "Session Created Succesfully";
                }
                else
                {
                    TempData["ErrorMessage"] = result.error;
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {
            var result = await _sessionService.GetSessionDetailsByIdAsync(id, ct);
            if (result.success)
                return View(result.Value);
            TempData["ErrorMessage"] = result.error;
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct = default)
        {
            var result = await _sessionService.GetSessionToUpdateAsync(id, ct);
            if (result.success)
            {
                ViewBag.traniers = new SelectList(await _sessionService.GetAllTrainersForDropDownAsync(ct), "Id", "Name");
                return View(result);
            }
            TempData["errorMessage"] = result.error;
            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SessionToUpdateViewModel model, CancellationToken ct = default)
        {
          if(!ModelState.IsValid) return View(model);
          var result = await _sessionService.UpdateSessionAsync(id, model, ct);
            if (result.success)
            {
                TempData["SuccessMessage"] = "session Updated";
                return RedirectToAction("Index");
            }
            else 
            {
                TempData["ErrorMessage"] = result.error;
                return View(model);
            }

        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct = default) 
        {
            var result = await _sessionService.GetSessionDetailsByIdAsync(id,ct);
            if (result.success)

            return View(result.Value);
            TempData["ErorrMessage"] = result.error;
            return RedirectToAction("Index");
            
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct = default)
        {
            var result = await _sessionService.DeleteSessionAsync(id,ct);
            if (result.success)
            {
                TempData["sucessMessage"] = "session deleted";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction("Index");

            }

        }
        
    }
};
