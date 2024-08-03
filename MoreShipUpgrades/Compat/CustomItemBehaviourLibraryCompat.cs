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
            if (player.currentlyHeldObjectServer.GetComponent<ContainerBehaviour>() != null) return true;
            if (player.currentlyHeldObject.GetComponent<ContainerBehaviour>() != null) return true;
            return false;
        }
    }
}
