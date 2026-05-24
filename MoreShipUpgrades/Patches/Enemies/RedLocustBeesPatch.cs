using HarmonyLib;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace MoreShipUpgrades.Patches.Enemies
{
    [HarmonyPatch(typeof(RedLocustBees))]
    internal static class RedLocustBeesPatch
    {
        private static LguLogger logger = new LguLogger(typeof(RedLocustBeesPatch).Name);
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(RedLocustBees.OnCollideWithPlayer))]
        public static IEnumerable<CodeInstruction> OnCollideWithPlayer_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo beeReduceDamage = typeof(Beekeeper).GetMethod(nameof(Beekeeper.CalculateBeeDamage));
            List<CodeInstruction> codes = instructions.ToList();
            int index = 0;
            Tools.FindInteger(ref index, ref codes, 10, skip: true, errorMessage: "Couldn't skip the number used to check the player's health");
            Tools.FindInteger(ref index, ref codes, 10, beeReduceDamage, errorMessage: "Couldn't find the damage number applied to the player when colliding");
            return codes.AsEnumerable();
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(RedLocustBees.SpawnHiveClientRpc))]
        public static IEnumerable<CodeInstruction> SpawnHiveClientRpcTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo beeIncreaseHiveValue = typeof(Beekeeper).GetMethod(nameof(Beekeeper.GetHiveScrapValue));

            List<CodeInstruction> codes = instructions.ToList();
            int index = 0;
			Tools.FindArgumentField(ref index, ref codes, argumentIndex: 2, addCode: beeIncreaseHiveValue, errorMessage: "Could not find the argument with hive's scrap value");
			Tools.FindArgumentField(ref index, ref codes, argumentIndex: 2, addCode: beeIncreaseHiveValue, errorMessage: "Could not find the argument with hive's scrap value");
			Tools.FindArgumentField(ref index, ref codes, argumentIndex: 2, addCode: beeIncreaseHiveValue, errorMessage: "Could not find the argument with hive's scrap value");
			return codes.AsEnumerable();
        }
    }
}
