using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.Analatics;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class AnalaticsService : IAnalaticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalaticsService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AnalaticsViewModel> GetDataAsync(CancellationToken ct = default)
        {
 
            var upcomingSession = await _unitOfWork.GetRepository<Session>().CountAsyc(S => S.StartDate >  DateTime.Now);
            var ongoingSession = await _unitOfWork.GetRepository<Session>().CountAsyc(S => S.StartDate <= DateTime.Now && S.EndDate > DateTime.Now);
            var completedSession = await _unitOfWork.GetRepository<Session>().CountAsyc(S => S.StartDate <= DateTime.Now);
            var totalMember = await _unitOfWork.GetRepository<Member>().CountAsyc(ct : ct);
            var totalTrainer = await _unitOfWork.GetRepository<Trainer>().CountAsyc(ct : ct);
            var totalActiveMember = await _unitOfWork.GetRepository<MemperShip>().CountAsyc(ct : ct);

            return new AnalaticsViewModel()
            {
                TotalMember = totalMember,
                TotalTrainer = totalTrainer,
                UpcomingSession = upcomingSession,
                OngoingSession = ongoingSession,
                CompletedSession = completedSession,
                ActiveMember = totalActiveMember
            };


        }
    }
}
