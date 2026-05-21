using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL
{
    public class MocPlanRepository : IPlanRepository
    {
        public Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            var plans = new List<Plan>()
            {
            new Plan(){ Name = "TEST" }
            };
            return plans;
        }
        

        public Task<Plan?> GetById(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
