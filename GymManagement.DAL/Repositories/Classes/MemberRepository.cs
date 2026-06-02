using GymManagement.DAL.Models;
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
    public class MemberRepository : IMemberRepository
    {
        private readonly GymDbContext _dbContext;

        public MemberRepository(GymDbContext context)
        {
            _dbContext = context;
        }

        public async Task<int> AddAsync(Member member, CancellationToken ct = default)
        {
            await _dbContext.Members.AddAsync(member, ct);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Member member, CancellationToken ct = default)
        {
            _dbContext.Members.Remove(member);
            return await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<Member>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            return tracking ? await _dbContext.Members.ToListAsync(ct) : await _dbContext.Members.AsNoTracking().ToListAsync(ct);
        }

        public Task<Member?> GetById(int id, CancellationToken ct = default)
        {
            return _dbContext.Members.FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<int> UpdateAsync(Member member, CancellationToken ct = default)
        {
            _dbContext.Members.Update(member);
            return await _dbContext.SaveChangesAsync(ct);
        }
    }
}
