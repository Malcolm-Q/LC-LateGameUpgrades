using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Util;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(DepositItemsDesk))]
    internal static class DepositItemsDeskPatcher
    {
        [HarmonyPatch(nameof(DepositItemsDesk.SellItemsOnServer))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> SellItemsOnServerTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo scrapValue = typeof(GrabbableObject).GetField(nameof(GrabbableObject.scrapValue));
            FieldInfo itemsOnCounter = typeof(DepositItemsDesk).GetField(nameof(DepositItemsDesk.itemsOnCounter));

            MethodInfo checkCollectionScrap = typeof(CollectionUpgradeManager).GetMethod(nameof(CollectionUpgradeManager.CheckCollectionScrap));

            int index = 0;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            Tools.FindField(ref index, ref codes, findField: scrapValue, skip: true);
            codes.Insert(index, new CodeInstruction(OpCodes.Call, checkCollectionScrap));
            codes.Insert(index, new CodeInstruction(OpCodes.Callvirt, codes[index - 2].operand));
            codes.Insert(index, new CodeInstruction(OpCodes.Ldloc_2));
            codes.Insert(index, new CodeInstruction(OpCodes.Ldfld, itemsOnCounter));
            codes.Insert(index, new CodeInstruction(OpCodes.Ldarg_0));
            return codes;
        }
    }
}
