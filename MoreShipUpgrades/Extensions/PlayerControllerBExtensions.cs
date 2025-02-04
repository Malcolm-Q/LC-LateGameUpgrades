using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using System.Net;
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
                weight = Mathf.Clamp(weight + BackMuscles.DecreasePossibleWeight(grabbableObject.itemProperties.weight - 1f), 0f, 10f);
            }
            return weight;
        }

        public static bool ContainsItemOfType<T>(this PlayerControllerB player, T excludingItem = null) where T : GrabbableObject
        {
            foreach(GrabbableObject grabbableObject in player.ItemSlots)
            {
                if (grabbableObject == null) continue;
                if (excludingItem != null && grabbableObject == excludingItem) continue;
                if (grabbableObject is T) return true;
            }
            return false;
        }

        public static bool CheckIfCurrentlyHeldObjectIsInInventory(this PlayerControllerB player)
        {
            foreach (GrabbableObject grabbableObject in player.ItemSlots)
            {
                if (grabbableObject == null) continue;
                if (grabbableObject == player.currentlyHeldObjectServer) return true;
            }
            return false;
        }
        public static bool CheckIfCurrentlyTwoHandedInInventory(this PlayerControllerB player)
        {
            int twoHandedCount = 0;
            int maxTwoHandedCount = 1 + UpgradeBus.Instance.PluginConfiguration.DeeperPocketsConfiguration.InitialEffect + (BaseUpgrade.GetUpgradeLevel(DeepPockets.UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.DeeperPocketsConfiguration.IncrementalEffect);

            for (int i = 0; i < player.ItemSlots.Length && twoHandedCount < maxTwoHandedCount; i++)
            {
                GrabbableObject item = player.ItemSlots[i];
                if (item == null) continue;
                if (!item.itemProperties.twoHanded) continue;
                twoHandedCount++;
            }
            if (twoHandedCount < maxTwoHandedCount)
            {
                HUDManager.Instance.holdingTwoHandedItem.enabled = false;
                return false;
            }
            return true;
        }

        public static bool IsTeleporting(this PlayerControllerB player)
        {
            return player.shipTeleporterId != -1 && player.shipTeleporterId != 0;
        }
        public static bool RemainIsHoldingObjectIfTeleporting(this PlayerControllerB player)
        {
            return player.IsTeleporting() && player.CheckIfCurrentlyHeldObjectIsInInventory();
        }

        public static bool RemainActivatingItemIfTeleporting(this PlayerControllerB player)
        {
            return player.IsTeleporting() && player.activatingItem;
        }

        public static bool RemainTwoHandedIfTeleporting(this PlayerControllerB player)
        {
            return player.IsTeleporting() && player.CheckIfCurrentlyTwoHandedInInventory();
        }

        public static GrabbableObject RemainCurrentlyHeldObjectServerIfTeleporting(this PlayerControllerB player, GrabbableObject defaultValue)
        {
            if (player.IsTeleporting()) return player.CheckIfCurrentlyHeldObjectIsInInventory() ? player.currentlyHeldObjectServer : defaultValue;
            return defaultValue;
        }

        public static float RemainCarryWeightIfTeleporting(this PlayerControllerB player, float defaultValue)
        {
            if (player.IsTeleporting()) return Mathf.Clamp(defaultValue + player.GetCurrentWeightFromInventory(),1f,10f);
            return defaultValue;
        }
    }
}
