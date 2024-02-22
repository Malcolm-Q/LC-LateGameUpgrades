using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(ItemDropship))]
    internal static class DropPodPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(ItemDropship.Update))]
        static void UpdateTimer(ref float ___shipTimer, bool ___deliveringOrder, Terminal ___terminalScript, StartOfRound ___playersManager, ItemDropship __instance)
        {
            if (!BaseUpgrade.GetActiveUpgrade(FasterDropPod.UPGRADE_NAME)) { return; }

            float upgradedTimer = UpgradeBus.Instance.PluginConfiguration.FASTER_DROP_POD_TIMER.Value;

            if (__instance.IsServer)
            {
                if (!___deliveringOrder)
                {
                    if (___terminalScript.orderedItemsFromTerminal.Count > 0)
                    {
                        if (___playersManager.shipHasLanded)
                        {
                            if(___shipTimer >= upgradedTimer && ___shipTimer < 40f)
                            {
                                ___shipTimer = 41f;
                            }
                        }
                    }
                }
            }
        }
    }
}
