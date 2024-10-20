using System.Reflection;

namespace MoreShipUpgrades.Compat
{
    internal static class GeneralImprovementsCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(GeneralImprovements.Metadata.GUID);

        internal static bool PatchedTeleporter()
        {
            return GeneralImprovements.Plugin.KeepItemsDuringInverse.Value != GeneralImprovements.Enums.eItemsToKeep.None || GeneralImprovements.Plugin.KeepItemsDuringTeleport.Value != GeneralImprovements.Enums.eItemsToKeep.None;
        }
    }

}
