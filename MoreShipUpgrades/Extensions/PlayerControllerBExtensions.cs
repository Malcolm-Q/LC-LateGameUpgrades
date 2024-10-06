using GameNetcodeStuff;
using UnityEngine;

namespace MoreShipUpgrades.Extensions
{
    internal static class PlayerControllerBExtensions
    {
        public static float GetCurrentWeightFromInventory(this PlayerControllerB player)
        {
            float weight = 0f;
            foreach (GrabbableObject grabbableObject in player.ItemSlots)
            {
                if (grabbableObject == null) continue;
                weight = Mathf.Clamp(weight + (grabbableObject.itemProperties.weight - 1f), 0f, 10f);
            }
            Plugin.mls.LogDebug(weight);
            return weight;
        }

        public static bool IsTeleporting(this PlayerControllerB player)
        {
            return player.shipTeleporterId != -1;
        }

        public static bool RemainActivatingItemIfTeleporting(this PlayerControllerB player)
        {
            return player.IsTeleporting() && player.activatingItem;
        }

        public static bool RemainTwoHandedIfTeleporting(this PlayerControllerB player)
        {
            return player.IsTeleporting() && player.twoHanded;
        }

        public static GrabbableObject RemainCurrentlyHeldObjectServerIfTeleporting(this PlayerControllerB player, GrabbableObject defaultValue)
        {
            if (player.IsTeleporting()) return player.currentlyHeldObjectServer;
            return defaultValue;
        }

        public static float RemainCarryWeightIfTeleporting(this PlayerControllerB player, float defaultValue)
        {
            if (player.IsTeleporting()) return Mathf.Clamp(defaultValue + player.GetCurrentWeightFromInventory(),1f,10f);
            return defaultValue;
        }
    }
}
