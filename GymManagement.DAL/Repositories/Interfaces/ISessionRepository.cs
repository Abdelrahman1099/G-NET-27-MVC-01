using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionWithTrainerAndCategoryAsync(CancellationToken cancellationToken = default);
        Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct);
        Task<Session?> GetSessionByIdAsync(int sessionId, CancellationToken ct = default);
    }

}
