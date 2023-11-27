using HarmonyLib;
using MoreShipUpgrades.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("Awake")]
        private static void AddToItemList(ref StartOfRound __instance)
        {
            foreach(Item item in Plugin.upgradeItems)
            {
                __instance.allItemsList.itemsList.Add(item);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch("playersFiredGameOver")]
        private static void GameOverResetUpgradeManager()
        {
            UpgradeBus.instance.ResetAllValues();
        }

    }
}
