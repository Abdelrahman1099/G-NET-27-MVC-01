using GymManagement.BLL.ViewModels.Analatics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IAnalaticsService
    {
        Task<AnalaticsViewModel> GetDataAsync(CancellationToken ct = default);

    }
}
