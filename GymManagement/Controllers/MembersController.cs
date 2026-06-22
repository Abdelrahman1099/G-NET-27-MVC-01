using GymManagement.BLL.Services.Attachment;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Members;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IAttachmentService _attachmentService;

        public MembersController(IMemberService member, IAttachmentService attachmentService) 
        {

            _memberService = member;
            _attachmentService = attachmentService;
        }

        public async Task<IActionResult> Index(CancellationToken ct) 
         {
            var members = await _memberService.GetAllMembesAsync(ct);

            //ViewData["Data01"] = "Hello From viewData";
            //ViewBag.Data2 = "Hello From ViewBag";
            //TempData["Data3"] = "Hello From Data3";

             return View(members);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel model, CancellationToken ct) 
        {
            if (ModelState.IsValid) 
            {
                var result = await _memberService.CreateMemberAsync(model, ct);

                if(result)
                {
                    TempData["SuccesMessage"] = "Member Created Succesfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to Create Member";
                }

                    return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct) 
        {
            var result = await _memberService.GetAllMembeDetailsAsync(id, ct);
            if(result is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction("Index");
            }
            return View(result);
        }

        [HttpGet]
        public async Task<ActionResult> HealthRecordDetails(int id, CancellationToken ct) 
        {
            var result = await _memberService.GetAllMembeHealthRecordAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "HealthRecord Not Found";
                return RedirectToAction("Index");
            }
            return View(result);
        }


        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct) 
        {
            var result = await _memberService.GetMemberToUpdateAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction("Index");
            }
            return View(result);
        }


        [HttpPost]
        public async Task<IActionResult> EditMember(int id, MemberToUpdateViewModel model, CancellationToken ct) 
        {

            if (ModelState.IsValid)
            {
                var result = await _memberService.UpdateMemberViewModel(id, model, ct);

                if (result)
                {
                    TempData["SuccesMessage"] = "Member Update Succesfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to Update Member";
                }

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct) 
        {
            var result = await _memberService.GetAllMembeDetailsAsync(id, ct);

            if (result is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction("Index");
            }

            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct) 
        {
            var result = await _memberService.DeleteMemberViewModel(id, ct);

            if (result)
            {
                TempData["SuccesMessage"] = "Member Delete Succesfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Delete Member";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Picture(int id, CancellationToken ct = default) 
        {
            var member = await _memberService.GetAllMembeDetailsAsync(id, ct);
            if (member is null || string.IsNullOrWhiteSpace(member.Photo)) return NotFound();
            var result = _attachmentService.GetFile("MembersPicture", member.Photo);
            if (result is null) return NotFound();
            return File(result.Value.stream, result.Value.contentType);
        }

    }
}
