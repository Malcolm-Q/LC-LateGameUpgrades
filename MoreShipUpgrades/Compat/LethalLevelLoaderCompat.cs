using LethalLevelLoader;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MoreShipUpgrades.Compat
{
    internal static class LethalLevelLoaderCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(LethalLevelLoader.Plugin.ModGUID);

		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
		internal static void GrabAllAvailableLevels(ref SelectableLevel[] levels)
        {
            levels = levels.Where(x => PatchedContent.TryGetExtendedContent(x, out ExtendedLevel extendedLevel) && !extendedLevel.IsRouteLocked).ToArray();
        }

		[MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
		internal static bool IsLocked(ref SelectableLevel level)
        {
            PatchedContent.TryGetExtendedContent(level, out ExtendedLevel extendedLevel);
            if (extendedLevel == null || extendedLevel.IsRouteLocked) return true;
            return false;
        }
    }
}
