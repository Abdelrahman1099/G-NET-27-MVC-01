using AutoMapper;
using GymManagement.BLL.ViewModels.Members;
using GymManagement.BLL.ViewModels.Sessions;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(D => D.Adress, o => o.MapFrom(S => new Adress()
                {
                    BulidingNumber = S.BuildingNumber,
                    City = S.City,
                    Street = S.Street
                }))
                .ForMember(D => D.HealthRecord , o => o.MapFrom(S => new HealthRecord()
                {
                    Height = S.HealthRecordViewModel.Height,
                    BloodType = S.HealthRecordViewModel.BloodType,
                    Note = S.HealthRecordViewModel.Note,
                    Wieght = S.HealthRecordViewModel.Weight
                }));
            CreateMap<Member, MemberViewModel>()
                .ForMember(D => D.DateOfBirth, o => o.MapFrom(S => S.DateOfBirth.ToShortDateString()))
                .ForMember(D => D.Address, o => o.MapFrom(S => $"{S.Adress.BulidingNumber} - {S.Adress.Street} - {S.Adress.City}"));
            CreateMap<HealthRecord, HealthRecordViewModel>();
            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(D => D.City, o => o.MapFrom(S => S.Adress.City))
                .ForMember(D => D.BuildingNumber, o => o.MapFrom(S => S.Adress.BulidingNumber))
                .ForMember(D => D.Street, o => o.MapFrom(S => S.Adress.Street));
            MappSession();
        }

        private void MappSession()
        {
            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
        }
    }
}
