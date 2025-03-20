using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImaginationWorkgroup.Web
{
    public class LoggingConfig
    {
        public static void SetupLogging()
        {
            LoggingExtensions.Logging.Log.InitializeWith<LoggingExtensions.NLog.NLogLog>();
            "LoggingConfig".Log().Info("Logging initialized");
        }
    }
}