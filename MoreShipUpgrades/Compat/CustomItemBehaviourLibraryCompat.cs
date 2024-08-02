namespace MoreShipUpgrades.Compat
{
    internal static class CustomItemBehaviourLibraryCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.github.WhiteSpike.CustomItemBehaviourLibrary");
    }
}
