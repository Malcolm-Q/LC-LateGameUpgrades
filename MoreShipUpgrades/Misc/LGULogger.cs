using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
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
        public void LogCodeInstruction(CodeInstruction code)
        {
            LogDebug(code.opcode + ", " + code.operand);
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
