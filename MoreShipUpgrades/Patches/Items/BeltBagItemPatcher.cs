using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(BeltBagItem))]
    internal static class BeltBagItemPatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(BeltBagItem.ItemInteractLeftRight))]
        public static IEnumerable<CodeInstruction> BeltBagItemInteractTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo bonusDistance = typeof(MechanicalArms).GetMethod(nameof(MechanicalArms.GetIncreasedGrabDistance));

            var codes = new List<CodeInstruction>(instructions);
            var index = 0;
            Tools.FindFloat(ref index, ref codes, 4f, bonusDistance, errorMessage: "Couldn't find belt bag item's debug pickup distance");
            Tools.FindFloat(ref index, ref codes, 4f, bonusDistance, errorMessage: "Couldn't find belt bag item's pickup distance");
            return codes.AsEnumerable();
        }
    }
}
