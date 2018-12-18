using BridgeportClaims.Data.Dtos;

namespace BridgeportClaims.Data.DataProviders.Dashboards
{
    public interface IDashboardProvider
    {
        DashboardDto GetDashboardKpis();
    }
}