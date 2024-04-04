using HarmonyLib;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace MoreShipUpgrades.Patches.Enemies
{
    [HarmonyPatch(typeof(ButlerBeesEnemyAI))]
    internal class ButlerBeesPatcher
    {
        private static LguLogger logger = new LguLogger(typeof(RedLocustBeesPatch).Name);
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(ButlerBeesEnemyAI.OnCollideWithPlayer))]
        public static IEnumerable<CodeInstruction> OnCollideWithPlayer_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo beeReduceDamage = typeof(Beekeeper).GetMethod(nameof(Beekeeper.CalculateBeeDamage));
            List<CodeInstruction> codes = instructions.ToList();
            int index = 0;
            index = Tools.FindInteger(index, ref codes, 10, beeReduceDamage, errorMessage: "Couldn't find the damage number applied to the player when colliding");
            return codes.AsEnumerable();
        }
    }
}
