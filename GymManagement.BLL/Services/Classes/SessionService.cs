using AutoMapper;
using GymManagement.BLL.Common;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("EndDate must be after StartDate");
            if (model.StartDate <= DateTime.Now) return Result.Validation("EndDate must be in the future");
            if(model.Capacity < 1 || model.Capacity > 25) return Result.Validation("Capacity must be between 1 and 25");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetById(model.TrainerId, ct);
            if (trainer is null) return Result.NotFound($"Trainer with Id {model.TrainerId} not found");
            var category = await _unitOfWork.GetRepository<Category>().GetById(model.CategoryId, ct);
            if(category is null) return Result.NotFound($"Category with Id {model.CategoryId} not found");
            var isValid = Enum.TryParse<Specialty>(category.CategoryName, out var CategorySpecialty);
            if (!isValid || trainer.specialty != CategorySpecialty) return Result.Validation("can not create session with this trainer");

            var session = _mapper.Map<Session>(model);
            _unitOfWork.GetRepository<Session>().Add(session);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0 ? Result.Ok() : Result.fail("failed to creat session");
        }

        public async Task<Result> DeleteSessionAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetById(sessionId, ct);
            if (session is null) return Result.NotFound($"session with id {sessionId} not found");
            if (session.EndDate > DateTime.Now) return Result.fail("cannot delete session not ended yet");
            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);
            if (bookingCount > 0) return Result.fail("cannot delete session");

            _unitOfWork.SessionRepository.Delete(session);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0 ? Result.Ok() : Result.fail("failed to update session");
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
  
            
            var models = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in models) 
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id, ct);    
            }

            return models;
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainersForDropDownAsync(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(false, ct);
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public async Task<Result<SessionViewModel>> GetSessionDetailsByIdAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionByIdAsync(sessionId, ct);
            if (session is null) return Result<SessionViewModel>.NotFound($"session with Id : {sessionId} not found");

            var mappedSession =  _mapper.Map<SessionViewModel>(session);
            mappedSession.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);

            return Result<SessionViewModel>.Ok(mappedSession);
        }

        public async Task<Result<SessionToUpdateViewModel>> GetSessionToUpdateAsync(int sessionId, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetById(sessionId, ct);
            if (session is null) return Result<SessionToUpdateViewModel>.NotFound($"session with id {sessionId} not found");
            if (session.StartDate <= DateTime.Now) return Result<SessionToUpdateViewModel>.fail("cannot update session");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);
            if(bookingCount > 0) return Result<SessionToUpdateViewModel>.fail("cannot update session");

            var mappedSession = _mapper.Map<SessionToUpdateViewModel>(session);
            return Result<SessionToUpdateViewModel>.Ok(mappedSession);
        }

        public async Task<Result> UpdateSessionAsync(int sessionId, SessionToUpdateViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetById(sessionId, ct);
            if (session is null) return Result.NotFound($"session with id {sessionId} not found");
            if (session.StartDate <= DateTime.Now) return Result.fail("cannot update session");

            var bookingCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(sessionId, ct);
            if (bookingCount > 0) return Result.fail("cannot update session");

            var mappedSession = _mapper.Map<SessionToUpdateViewModel>(session);

            if (model.StartDate >= model.EndDate) return Result.Validation("EndDate must be after StartDate");
            if (model.StartDate <= DateTime.Now) return Result.Validation("StartDate must be in the future");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetById(model.TrainerId, ct);
            if (trainer is null) return Result.NotFound($"Trainer with Id {model.TrainerId} not found");
            var category = await _unitOfWork.GetRepository<Category>().GetById(session.CategoryId, ct);
            if (category is null) return Result.NotFound($"Category with Id {session.CategoryId} not found");
            var isValid = Enum.TryParse<Specialty>(category.CategoryName, out var CategorySpecialty);
            if (!isValid || trainer.specialty != CategorySpecialty) return Result.Validation("can not create session with this trainer");

            session.StartDate = model.StartDate;
            session.EndDate = model.EndDate;
            session.Description = model.Description;
            session.UpdatedAt = DateTime.Now;

            _unitOfWork.SessionRepository.Update(session);
            var count = await _unitOfWork.SaveChangesAsync(ct);
            return count > 0 ? Result.Ok() : Result.fail("failed to update session");
        }


    }
}
