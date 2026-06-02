using GymManagement.DAL.Models;
using GymManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> GetAllAsync(bool tracking = false, CancellationToken ct = default);
        Task<Member?> GetById(int id, CancellationToken ct = default);
        Task<int> AddAsync(Member plan, CancellationToken ct = default);
        Task<int> UpdateAsync(Member plan, CancellationToken ct = default);
        Task<int> DeleteAsync(Member plan, CancellationToken ct = default);

    }
}
