using LethalLevelLoader;
using System;
using System.Linq;

namespace MoreShipUpgrades.Compat
{
    internal static class LethalLevelLoaderCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(LethalLevelLoader.Plugin.ModGUID);

        internal static void GrabAllAvailableLevels(ref SelectableLevel[] levels)
        {
            levels = levels.Where(x => PatchedContent.TryGetExtendedContent(x, out ExtendedLevel extendedLevel) && !extendedLevel.IsRouteLocked).ToArray();
        }

        internal static bool IsLocked(ref SelectableLevel level)
        {
            ExtendedLevel extendedLevel;
            PatchedContent.TryGetExtendedContent(level, out extendedLevel);
            if (extendedLevel == null || extendedLevel.IsRouteLocked) return true;
            return false;
        }
    }
}
