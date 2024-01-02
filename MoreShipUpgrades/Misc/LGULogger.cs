using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.Misc
{
    public class LGULogger
    {
        private string moduleName;
        private ManualLogSource logSource;
        public LGULogger(string moduleName)
        {
            this.moduleName = moduleName;
            logSource = Plugin.mls;
        }
        public void LogDebug(object message)
        {
            logSource.LogDebug($"[{moduleName}] {message}");
        }
        public void LogMessage(object message)
        {
            logSource.LogMessage($"[{moduleName}] {message}");
        }
        public void LogInfo(object message)
        {
            logSource.LogInfo($"[{moduleName}] {message}");
        }
        public void LogWarning(string message) 
        { 
            logSource.LogWarning($"[{moduleName}] {message}");
        }
        public void LogError(string message)
        {
            logSource.LogError($"[{moduleName}] {message}");
        }
    }
}
