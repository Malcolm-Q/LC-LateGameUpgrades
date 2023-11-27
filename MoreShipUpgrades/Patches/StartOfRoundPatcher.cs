using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch]
    internal class StartOfRoundPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(StartOfRound), "Awake")]
        private static void AddToItemList(ref StartOfRound __instance)
        {
            foreach(Item item in Plugin.upgradeItems)
            {
                __instance.allItemsList.itemsList.Add(item);
            }
        }

    }
}
