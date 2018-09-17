using BridgeportClaims.Maintenance.Business.Tasks;

namespace BridgeportClaims.Maintenance.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            TaskService.ProcessMaintenanceTask();
        }
    }
}
