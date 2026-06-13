using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _context;
        private readonly DbSet<TEntity> _dbset;

        public GenericRepository(GymDbContext context) 
        {
            _context = context;
            _dbset = _context.Set<TEntity>();
        }
        public void Add(TEntity entity)
        {
             _dbset.Add(entity);
           
        }

        public async Task<bool> AnyAsyc(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate, ct);
        }

        public void Delete(TEntity entity)
        {
            _dbset.Remove(entity);
            
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await _context.Set<TEntity>().FirstOrDefaultAsync(predicate, ct); 
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            return tracking ? await _dbset.ToListAsync(ct) : await _dbset.AsNoTracking().ToListAsync(ct);
        }

        public async Task<TEntity?> GetById(int id, CancellationToken ct = default)
        {
           return await _dbset.FindAsync(id, ct);
        }

        public void Update(TEntity entity)
        {
            _dbset.Update(entity);
        }
    }
}
