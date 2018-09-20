using System;
using BridgeportClaims.Business.Enums;
using c = BridgeportClaims.Business.StringConstants.Constants;
using cm = System.Configuration.ConfigurationManager;

namespace BridgeportClaims.Business.ConfigService
{
    public static class ConfigService
    {
        public static string GetFileLocationByFileType(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Images:
                    return GetAppSetting(c.ImagesFileLocationKey);
                case FileType.Invoices:
                    return GetAppSetting(c.InvoicesFileLocationKey);
                case FileType.Checks:
                    return GetAppSetting(c.ChecksFileLocationKey);
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
            }
        }

        public static string GetAppSetting(string key) => cm.AppSettings[key];

        internal static string GetDbConnStr() 
            => cm.ConnectionStrings[c.DbConnStrName].ConnectionString;

        public static bool AppIsInDebugMode
            => Convert.ToBoolean(GetAppSetting(c.AppIsInDebugMode)?.ToLower());

        public static string GetRootDomainByFileType(FileType fileType)
        {
            switch (fileType)
            {
                case FileType.Images:
                    return GetAppSetting(c.ImagesRootDomainNameKey);
                case FileType.Invoices:
                    return GetAppSetting(c.InvoicesRootDomainNameKey);
                case FileType.Checks:
                    return GetAppSetting(c.ChecksRootDomainNameKey);
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
            }
        }
    }
}