using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.Misc
{
    internal class LGULogger
    {
        private string moduleName;
        private ManualLogSource logSource;
        public LGULogger(string moduleName)
        {
            this.moduleName = moduleName;
            logSource = Plugin.mls;
        }
        public void LogMessage(string message)
        {
            logSource.LogMessage(string.Format("[{0}] " + message, moduleName));
        }
        public void LogInfo(string message)
        {
            logSource.LogInfo(string.Format("[{0}] " + message, moduleName));
        }
        public void LogWarning(string message) 
        { 
            logSource.LogWarning(string.Format("[{0}] " + message, moduleName));
        }
        public void LogError(string message)
        {
            logSource.LogError(string.Format("[{0}] " + message, moduleName));
        }
    }
}
