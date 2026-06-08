using GymManagement.BLL.ViewModels.Members;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembesAsync(CancellationToken ct);
        Task<MemberViewModel?> GetAllMembeDetailsAsync(int memberId, CancellationToken ct);
        Task<HealthRecordViewModel?> GetAllMembeHealthRecordAsync(int memberId, CancellationToken ct);
        Task<bool>CreateMemberViewModel(CreateMemberViewModel model, CancellationToken ct);    
        Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int  memberId, CancellationToken ct);    
        Task<bool>UpdateMemberViewModel(int memberId, MemberToUpdateViewModel model, CancellationToken ct);   
        Task<bool>DeleteMemberViewModel(int memberId, CancellationToken ct);   
    }
}
