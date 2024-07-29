using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.Compat
{
    internal static class LethalLevelLoaderCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(LethalLevelLoader.Plugin.ModGUID);
    }
}
