using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace MoreShipUpgrades.Patches.Items
{
    [HarmonyPatch(typeof(Shovel))]
    internal class ShovelPatcher
    {
        private static LGULogger logger = new LGULogger(nameof(ShovelPatcher));
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(Shovel.HitShovel))]
        public static IEnumerable<CodeInstruction> HitShovelTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo proteinHitFoce = typeof(proteinPowderScript).GetMethod(nameof(proteinPowderScript.GetShovelHitForce));
            FieldInfo shovelHitForce = typeof(Shovel).GetField(nameof(Shovel.shovelHitForce));

            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            int index = 0;
            index = Tools.FindField(index, ref codes, findField: shovelHitForce, addCode: proteinHitFoce, errorMessage: "Couldn't find shovel hit force field");
            return codes.AsEnumerable();
        }
    }
}
