 using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using GymManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext _dbContext;

       public PlanRepository(GymDbContext context)
        {
            _dbContext = context;
        }

        public async Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
            await _dbContext.Plans.AddAsync(plan, ct);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
            _dbContext.Plans.Remove(plan);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            return tracking ? await _dbContext.Plans.ToListAsync(ct) : await _dbContext.Plans.AsNoTracking().ToListAsync(ct);
        }

        public Task<Plan?> GetById(int id, CancellationToken ct = default)
        {
            return _dbContext.Plans.FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            _dbContext.Plans.Update(plan); 
            return await _dbContext.SaveChangesAsync(ct);
        }
    }
}
