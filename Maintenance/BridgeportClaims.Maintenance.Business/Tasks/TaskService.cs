using BridgeportClaims.Maintenance.Business.DAL;

namespace BridgeportClaims.Maintenance.Business.Tasks
{
    public static class TaskService
    {
        public static void ProcessMaintenanceTask()
        {
            MaintenanceService.ExecuteMaintenance();
        }
    }
}