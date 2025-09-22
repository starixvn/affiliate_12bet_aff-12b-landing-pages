using Fanex.Affiliate.LandingPages.Core.Models;

namespace Fanex.Affiliate.LandingPages.Core.Services.Interfaces;

public interface IUnderMaintenanceService
{
    Task<ScheduleMaintenance> GetUnderMaintenanceInfoAsync();

    Task<bool> GetIsUnderMaintenanceByDomainAsync(string domain);
}
