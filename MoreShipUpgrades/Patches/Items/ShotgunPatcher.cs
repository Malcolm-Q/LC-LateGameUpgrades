using HarmonyLib;
using MoreShipUpgrades.Misc.Upgrades;
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
            if (!BaseUpgrade.GetActiveUpgrade(SleightOfHand.UPGRADE_NAME)) return;
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
        [HarmonyDebug]
        [HarmonyPatch(nameof(ShotgunItem.ShootGun))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> ShootGunTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo GetHollowPointDamageBoost = typeof(HollowPoint).GetMethod(nameof(HollowPoint.GetHollowPointDamageBoost));
            MethodInfo GetLongBarrelRangeBoost = typeof(LongBarrel).GetMethod(nameof(LongBarrel.GetLongBarrelRangeBoost));
            FieldInfo enemyColliders = typeof(ShotgunItem).GetField(nameof(ShotgunItem.enemyColliders), BindingFlags.Instance | BindingFlags.NonPublic);

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindField(ref index, ref codes, findField: enemyColliders, skip: true);
            Tools.FindFloat(ref index, ref codes, findValue: 15f, addCode: GetLongBarrelRangeBoost);
            Tools.FindFloat(ref index, ref codes, findValue: 3.7f, addCode: GetLongBarrelRangeBoost);
            Tools.FindInteger(ref index, ref codes, findValue: 5, addCode: GetHollowPointDamageBoost);
            Tools.FindFloat(ref index, ref codes, findValue: 6f, addCode: GetLongBarrelRangeBoost);
            Tools.FindInteger(ref index, ref codes, findValue: 3, addCode: GetHollowPointDamageBoost);
            Tools.FindInteger(ref index, ref codes, findValue: 2, addCode: GetHollowPointDamageBoost);

            return codes;
        }
    }
}
