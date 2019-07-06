using System.Data;
using NLog;
using NLog.Targets;
using NLog.Config;
using cs = LakerFileImporter.ConfigService.ConfigService;

namespace LakerFileImporter.Logging
{
    public static class LoggingConfigurator
    {
        public static void ConfigureNLog()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            var dbTarget = new DatabaseTarget();
            config.AddTarget("Database", dbTarget);

            dbTarget.ConnectionString = cs.GetDbConnStr();
            dbTarget.CommandType = CommandType.StoredProcedure;
            dbTarget.CommandText = "[dbo].[uspNLogInsert]";
            dbTarget.Name = "db";
            var machineName = new DatabaseParameterInfo
            {
                Name = "@machineName",
                Layout = "${machinename}"
            };
            var siteName = new DatabaseParameterInfo
            {
                Name = "@siteName",
                Layout = "${iis-site-name}"
            };
            var logged = new DatabaseParameterInfo
            {
                Name = "@logged",
                Layout = "${date}"
            };
            var level = new DatabaseParameterInfo
            {
                Name = "@level",
                Layout = "${level}"
            };
            var userName = new DatabaseParameterInfo
            {
                Name = "@username",
                Layout = "${aspnet-user-identity}"
            };
            var message = new DatabaseParameterInfo
            {
                Name = "@message",
                Layout = "${message}"
            };
            var logger = new DatabaseParameterInfo
            {
                Name = "@logger",
                Layout = "${logger}"
            };
            var properties = new DatabaseParameterInfo
            {
                Name = "@properties",
                Layout = "${all-event-properties:separator=|}"
            };
            var serverName = new DatabaseParameterInfo
            {
                Name = "@serverName",
                Layout = "${aspnet-request:serverVariable=SERVER_NAME}"
            };
            var port = new DatabaseParameterInfo
            {
                Name = "@port",
                Layout = "${aspnet-request:serverVariable=SERVER_PORT}"
            };
            var url = new DatabaseParameterInfo
            {
                Name = "@url",
                Layout = "${aspnet-request:serverVariable=HTTP_URL}"
            };
            var https = new DatabaseParameterInfo
            {
                Name = "@https",
                Layout = "${when:inner=1:when='${aspnet-request:serverVariable=HTTPS}' == 'on'}${when:inner=0:when='${aspnet-request:serverVariable=HTTPS}' != 'on'}"
            };
            var serverAddress = new DatabaseParameterInfo
            {
                Name = "@serverAddress",
                Layout = ""
            };
            var remoteAddress = new DatabaseParameterInfo
            {
                Name = "@remoteAddress",
                Layout = "${aspnet-request:serverVariable=REMOTE_ADDR}:${aspnet-request:serverVariable=REMOTE_PORT}"
            };
            var callSite = new DatabaseParameterInfo
            {
                Name = "@callSite",
                Layout = "${callsite}"
            };
            var exception = new DatabaseParameterInfo
            {
                Name = "@exception",
                Layout = "${exception:tostring}"
            };
            dbTarget.Parameters.Add(machineName);
            dbTarget.Parameters.Add(siteName);
            dbTarget.Parameters.Add(logged);
            dbTarget.Parameters.Add(level);
            dbTarget.Parameters.Add(userName);
            dbTarget.Parameters.Add(message);
            dbTarget.Parameters.Add(logger);
            dbTarget.Parameters.Add(properties);
            dbTarget.Parameters.Add(serverName);
            dbTarget.Parameters.Add(port);
            dbTarget.Parameters.Add(url);
            dbTarget.Parameters.Add(https);
            dbTarget.Parameters.Add(serverAddress);
            dbTarget.Parameters.Add(remoteAddress);
            dbTarget.Parameters.Add(callSite);
            dbTarget.Parameters.Add(exception);

            // Step 4. Define rules
            var nLogRule = new LoggingRule("*", LogLevel.Debug, LogLevel.Fatal, dbTarget);
            config.LoggingRules.Add(nLogRule);

            // Step 5. Activate the configuration
            LogManager.Configuration = config;
        }
    }
}