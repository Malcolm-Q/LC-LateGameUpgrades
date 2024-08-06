using BepInEx.Logging;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using UnityEngine;

namespace MoreShipUpgrades.Patches.NetworkManager
{
    [HarmonyPatch(typeof(GameNetworkManager))]
    internal static class GameNetworkManagerPatcher
    {
        static LguLogger logger = new LguLogger(nameof(GameNetworkManagerPatcher));
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameNetworkManager.Disconnect))]
        static void ResetUpgradeBus()
        {
            logger.LogDebug("Resetting the Upgrade Bus due to disconnecting...");
            UpgradeBus.Instance.ResetAllValues();
            BaseUpgrade[] upgradeObjects = Object.FindObjectsOfType<BaseUpgrade>();
            foreach (BaseUpgrade upgrade in upgradeObjects)
            {
                Object.Destroy(upgrade.gameObject);
            }
        }
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GameNetworkManager.SaveGame))]
        static void SaveGamePrefix(GameNetworkManager __instance)
        {
            if (!__instance.isHostingGame || StartOfRound.Instance.inShipPhase || PlayerManager.instance.GetUpgradeSpentCredits() <= 0) return;
            int previousCredits = ES3.Load<int>("GroupCredits", __instance.currentSaveFileName);
            ES3.Save("GroupCredits", previousCredits - PlayerManager.instance.GetUpgradeSpentCredits(), __instance.currentSaveFileName);
            PlayerManager.instance.ResetUpgradeSpentCredits();
        }

        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameNetworkManager.SaveGame))]
        static void SaveGamePostfix(GameNetworkManager __instance)
        {
            if (!__instance.isHostingGame) return;
            logger.LogDebug("Saving the LGU upgrades unto a json file...");
            LguStore.Instance.ServerSaveFile();

        }
    }
}
