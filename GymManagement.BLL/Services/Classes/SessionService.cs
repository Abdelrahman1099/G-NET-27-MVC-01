using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Sessions;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using GymManagement.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct)
        {
            if (model.EndDate <= model.StartDate) return false;
            if (model.StartDate <= DateTime.Now) return false;
            if(model.Capacity < 1 || model.Capacity > 25) return false;

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetById(model.TrainerId, ct);
            if (trainer is null) return false;
            var category = await _unitOfWork.GetRepository<Category>().GetById(model.CategoryId, ct);
            if(category is null) return false;
            var isValid = Enum.TryParse<Specialty>(category.CategoryName, out var CategorySpecialty);
            if (!isValid || trainer.specialty != CategorySpecialty) return false;

            var session = _mapper.Map<Session>(model);
            _unitOfWork.GetRepository<Session>().Add(session);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetAllCategoriesForDropDownAsync(CancellationToken ct = default)
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(false, ct);
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionAsync(CancellationToken ct)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionWithTrainerAndCategoryAsync(ct);

            if (sessions == null || !sessions.Any()) return null;
            {

            }

            var mappedSession = sessions.Select(S => new SessionViewModel
            {
                Id = S.Id,
                Capacity = S.Capacity,
                CategoryName = S.Category.CategoryName,
                TrainerName = S.Trainer.Name,
                Description = S.Description,
                EndDate = S.EndDate,
                StartDate = S.StartDate,
                
            });

            foreach (var session in mappedSession) 
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);    
            }

            return mappedSession;
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(false, ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }
    }
}
