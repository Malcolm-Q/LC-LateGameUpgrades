using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using System.Collections;
using System.Collections.Generic;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(Turret))]
    internal class TurretPatcher
    {
        [HarmonyPatch(nameof(Turret.Update))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> UpdateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindInteger(ref index, ref codes, findValue: 50, addCode: typeof(BulletResistance).GetMethod(nameof(BulletResistance.GetBulletDamageResistance)), errorMessage: "Couldn't find the integer used to determine player damage");
            Tools.FindInteger(ref index, ref codes, findValue: 50, addCode: typeof(BulletResistance).GetMethod(nameof(BulletResistance.GetBulletDamageResistance)), errorMessage: "Couldn't find the integer used to determine player damage");
            return codes;
        }
    }
}
