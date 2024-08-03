using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Shotgun;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(ShotgunItem))]
    internal static class ShotgunPatcher
    {
        [HarmonyPatch(nameof(ShotgunItem.ReloadGunEffectsClientRpc))]
        [HarmonyPrefix]
        static void ReloadGunEffectsClientRpcPrefix(ShotgunItem __instance, bool start)
        {
            if (start)
            {
                __instance.gunAnimator.speed *= 1f + SleightOfHand.ComputeSleightOfHandSpeedBoost();
            }
            else
            {
                __instance.gunAnimator.speed /= 1f + SleightOfHand.ComputeSleightOfHandSpeedBoost();
            }
        }

        [HarmonyPatch(nameof(ShotgunItem.reloadGunAnimation), MethodType.Enumerator)]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> reloadGunAnimationTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo getSleightOfHandSpeedBoost = typeof(SleightOfHand).GetMethod(nameof(SleightOfHand.GetSleightOfHandSpeedBoost));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindFloat(ref index, ref codes, findValue: 0.3f, addCode: getSleightOfHandSpeedBoost, errorMessage: "Couldn't find the first value for WaitForSeconds");
            Tools.FindFloat(ref index, ref codes, findValue: 0.95f, addCode: getSleightOfHandSpeedBoost, errorMessage: "Couldn't find the second value for WaitForSeconds");
            Tools.FindFloat(ref index, ref codes, findValue: 0.95f, addCode: getSleightOfHandSpeedBoost, errorMessage: "Couldn't find the third value for WaitForSeconds");
            Tools.FindFloat(ref index, ref codes, findValue: 0.45f, addCode: getSleightOfHandSpeedBoost, errorMessage: "Couldn't find the third value for WaitForSeconds");
            return codes;
        }
    }
}
