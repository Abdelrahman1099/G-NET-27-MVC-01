using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Members;
using GymManagement.DAL.Models;
using GymManagement.DAL.Models.Enums;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;

        public MemberService(IGenericRepository<Member> memberRepository) 
        {
            _memberRepository = memberRepository;
        }
        public Task<bool> CreateMemberViewModel(CreateMemberViewModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMemberViewModel(int memberId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<MemberViewModel?> GetAllMembeDetailsAsync(int memberId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<HealthRecordViewModel?> GetAllMembeHealthRecordAsync(int memberId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembesAsync(CancellationToken ct)
        {
            var members = await _memberRepository.GetAllAsync(ct: ct);
            //var membersViewModels = new List<MemberViewModel>();
            //foreach (var member in members) 
            //{
            //    var memberViewModel = new MemberViewModel()
            //    {
            //        Id = member.Id,
            //        Name = member.Name,
            //        Photo = member.Photo,
            //        Phone = member.Phone,
            //        Email = member.Email,
            //        Gender = member.Gender.ToString()
            //    };
            //    membersViewModels.Add(memberViewModel);
            //}

            var membersViewModels = members.Select(member => new MemberViewModel()
            {
                Id = member.Id,
                Name = member.Name,
                Photo = member.Photo,
                Phone = member.Phone,
                Email = member.Email,
                Gender = member.Gender.ToString()
            });

            return membersViewModels;   
        }

        public Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateMemberViewModel(int memberId, MemberToUpdateViewModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
