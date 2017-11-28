using System.Data.SqlClient;
using BridgeportClaims.FileWatcherBusiness.Disposable;
using cm = BridgeportClaims.FileWatcherBusiness.ConfigService.ConfigService;

namespace BridgeportClaims.FileWatcherBusiness.DAL
{
    internal class ImageDataProvider
    {
        private readonly string _dbConnStr = cm.GetDbConnStr();

        internal void DeleteImageFile(string fileName) =>
            DisposableService.Using(() => new SqlConnection(_dbConnStr), conn =>
            {
                DisposableService.Using(() => new SqlCommand("", conn), cmd =>
                {

                });
            });
    }
}