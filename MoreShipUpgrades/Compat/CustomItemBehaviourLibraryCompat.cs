using CustomItemBehaviourLibrary.AbstractItems;
using GameNetcodeStuff;
using LethalLib.Modules;

namespace MoreShipUpgrades.Compat
{
    internal static class CustomItemBehaviourLibraryCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.github.WhiteSpike.CustomItemBehaviourLibrary");

        public static bool CheckForContainers(ref PlayerControllerB player)
        {
            // No putting wheelbarrows in your deeper pockets
            if (player.currentlyHeldObjectServer is ContainerBehaviour) return true;
            if (player.currentlyHeldObject is ContainerBehaviour) return true;
            return false;
        }
    }
}
