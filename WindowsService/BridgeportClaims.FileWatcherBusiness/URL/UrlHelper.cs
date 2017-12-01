using System;
using BridgeportClaims.FileWatcherBusiness.Extensions;

namespace BridgeportClaims.FileWatcherBusiness.URL
{
    public static class UrlHelper
    {
        public static string GetUrlFromFullFileName(string fullFileName, string rootDomain, string pathToRemove)
        {
            if (fullFileName.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(fullFileName));
            if (rootDomain.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(rootDomain));
            if (pathToRemove.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(pathToRemove));
            var path = fullFileName.Replace(pathToRemove, string.Empty);
            path = path.Replace(@"\", "/");
            var retVal = $"{rootDomain}{path}";
            return retVal;
        }
    }
}