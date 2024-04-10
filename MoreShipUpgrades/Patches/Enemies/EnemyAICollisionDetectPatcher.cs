using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MoreShipUpgrades.Patches.Enemies
{
    [HarmonyPatch(typeof(EnemyAICollisionDetect))]
    internal static class EnemyAICollisionDetectPatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch("IShockableWithGun.ShockWithGun")]
        static IEnumerable<CodeInstruction> ShockWithGunTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo ApplyIncreasedStunTimer = typeof(AluminiumCoils).GetMethod(nameof(AluminiumCoils.ApplyIncreasedStunTimer));
            int index = 0;
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            Tools.FindFloat(ref index, ref codes, findValue: 0.25f, addCode: ApplyIncreasedStunTimer, errorMessage: "Couldn't find the minimum current end strength cap");
            return codes;
        }
    }
}
