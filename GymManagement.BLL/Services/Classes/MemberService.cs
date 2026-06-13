using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Members;
using GymManagement.DAL;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            ) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {

            var emailExsist = await _unitOfWork.GetRepository<Member>().AnyAsyc(M => M.Email == model.Email, ct);
            var PhoneExsist = await _unitOfWork.GetRepository<Member>().AnyAsyc(M => M.Phone == model.Phone, ct);

            if(emailExsist || PhoneExsist) { return false; }

            //var member = new Member()
            //{
            //    Name = model.Name,
            //    Email = model.Email,
            //    Phone = model.Phone,
            //    Gender = model.Gender,
            //    Adress = new Adress ()
            //    {
            //        BulidingNumber = model.BuildingNumber,
            //        City = model.City,
            //        Street = model.Street,
            //    },
            //    HealthRecord = new HealthRecord ()
            //    {
            //        Height = model.HealthRecordViewModel.Height,
            //        Wieght = model.HealthRecordViewModel.Weight,
            //        BloodType = model.HealthRecordViewModel.BloodType,
            //        Note = model.HealthRecordViewModel.Note,
            //    }
            //};

            var member =  _mapper.Map<Member>(model);

            _unitOfWork.GetRepository<Member>().Add(member);
            var count = await _unitOfWork.SaveChangesAsync(ct);

            return count > 0;
        }

        public Task<bool> CreateMemberViewModel(CreateMemberViewModel model, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteMemberViewModel(int memberId, CancellationToken ct)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetById(memberId, ct);
            if (member is null) return false;
            var hasFutureSessions = await _unitOfWork.GetRepository<Booking>().AnyAsyc(B => B.Id == B.MemberId && B.Session.StartDate > DateTime.Now, ct);
            if (!hasFutureSessions) return false;
            _unitOfWork.GetRepository<Member>().Delete(member);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0;
        }

        public async Task<MemberViewModel?> GetAllMembeDetailsAsync(int memberId, CancellationToken ct)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetById(memberId, ct);
            if(member == null) return null;
            //var model = new MemberViewModel()
            //{
            //    Photo = member.Photo,
            //    Name = member.Name,
            //    Email = member.Email,
            //    Phone = member.Phone,
            //    Gender = member.Gender.ToString(),
            //    DateOfBirth = member.DateOfBirth.ToString(),
            //    Address = $"{member.Adress.BulidingNumber} - {member.Adress.City} - {member.Adress.Street}"
            //};

            var model = _mapper.Map<MemberViewModel>(member);

            var activeMemberShip = await _unitOfWork.GetRepository<MemperShip>().FirstOrDefaultAsync(M => M.MemberId == memberId && M.EndDate > DateTime.UtcNow, ct);
            if(activeMemberShip is not null)
            {
                model.PlanName = activeMemberShip.Plan.Name;
                model.MembershipStartDate = activeMemberShip.CreatedAt.ToString(); 
                model.MembershipEndDate = activeMemberShip.EndDate.ToString(); 
            }
            return model;
        }

        public async Task<HealthRecordViewModel?> GetAllMembeHealthRecordAsync(int memberId, CancellationToken ct)
        {
            var healthRecord = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(H => H.MemberId == memberId, ct);
            if(healthRecord is null) return null;
            //var model = new HealthRecordViewModel()
            //{
            //    Height = healthRecord.Height,
            //    Weight = healthRecord.Wieght,
            //    BloodType = healthRecord.BloodType,
            //    Note = healthRecord.Note
            //};

            var model = _mapper.Map<HealthRecordViewModel>(healthRecord);

            return model;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembesAsync(CancellationToken ct)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
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

            //var membersViewModels = members.Select(member => new MemberViewModel()
            //{
            //    Id = member.Id,
            //    Name = member.Name,
            //    Photo = member.Photo,
            //    Phone = member.Phone,
            //    Email = member.Email,
            //    Gender = member.Gender.ToString()
            //});

            var result = _mapper.Map<IEnumerable<MemberViewModel>>(members);

            return result;   
        }

        public async Task<MemberToUpdateViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetById(memberId, ct);
            if (member is null) return null;
            //var model = new MemberToUpdateViewModel()
            //{
            //    Name = member.Name,
            //    Photo = member.Photo,
            //    Phone = member.Phone,
            //    Email = member.Email,
            //    BuildingNumber = member.Adress.BulidingNumber,
            //    City = member.Adress.City,
            //    Street = member.Adress.Street

            //};

            var model = _mapper.Map<MemberToUpdateViewModel>(member);   

            return model;
        }

        public async Task<bool> UpdateMemberViewModel(int memberId, MemberToUpdateViewModel model, CancellationToken ct)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetById(memberId, ct);
            if (member is null) return false;

            var emailExsist = await _unitOfWork.GetRepository<Member>().AnyAsyc(M => M.Email == model.Email && M.Id != memberId, ct);
            var PhoneExsist = await _unitOfWork.GetRepository<Member>().AnyAsyc(M => M.Phone == model.Phone && M.Id != memberId, ct);

            if (emailExsist || PhoneExsist) { return false; }

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Adress.BulidingNumber = model.BuildingNumber;
            member.Adress.City = model.City;
            member.Adress.Street = model.Street;

            _unitOfWork.GetRepository<Member>().Update(member);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0;

        }
    }
}
