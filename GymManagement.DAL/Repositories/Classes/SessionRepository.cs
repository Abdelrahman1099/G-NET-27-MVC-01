using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _context;

        public SessionRepository(GymDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<Session>> GetAllSessionWithTrainerAndCategoryAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Sessions.AsNoTracking().Include(S => S.Trainer).Include(S => S.Category).ToListAsync(cancellationToken = default);
        }

        public async Task<int> GetCountOfBookedSlotsAsync(int sessionId, CancellationToken ct)
        {
            return await _context.Bookings.AsNoTracking().CountAsync(B => B.SessionId == sessionId);
        }
    }
}
