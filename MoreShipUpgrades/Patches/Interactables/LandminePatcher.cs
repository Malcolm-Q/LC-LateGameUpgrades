using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(Landmine))]
    internal static class LandminePatcher
    {
        [HarmonyPatch(nameof(Landmine.SpawnExplosion))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> SpawnExplosionTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo explosionResistance = typeof(ExplosionResistance).GetMethod(nameof(ExplosionResistance.GetExplosionDamageResistance), BindingFlags.Public | BindingFlags.Static);
            MethodInfo killPlayer = typeof(PlayerControllerB).GetMethod(nameof(PlayerControllerB.KillPlayer), BindingFlags.Public | BindingFlags.Instance);
            MethodInfo damagePlayer = typeof(PlayerControllerB).GetMethod(nameof(PlayerControllerB.DamagePlayer), BindingFlags.Public | BindingFlags.Instance);

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindArgumentField(ref index, ref codes, argumentIndex: 4, addCode: explosionResistance, errorMessage: "Could not find the argument related to damage number when near explosion");

            return codes;
        }
    }
}
