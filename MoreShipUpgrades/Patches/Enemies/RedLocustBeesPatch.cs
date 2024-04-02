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
            index = Tools.FindInteger(index, ref codes, 10, skip: true, errorMessage: "Couldn't skip the number used to check the player's health");
            index = Tools.FindInteger(index, ref codes, 10, beeReduceDamage, errorMessage: "Couldn't find the damage number applied to the player when colliding");
            return codes.AsEnumerable();
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(RedLocustBees.SpawnHiveClientRpc))]
        public static IEnumerable<CodeInstruction> SpawnHiveClientRpcTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo beeIncreaseHiveValue = typeof(Beekeeper).GetMethod(nameof(Beekeeper.GetHiveScrapValue));

            List<CodeInstruction> codes = instructions.ToList();
            bool found = false;
            for (int i = 0; i < codes.Count; i++)
            {
                if (found) break;
                if (codes[i].opcode != OpCodes.Ldarg_2) continue;
                if (!(codes[i + 1].opcode == OpCodes.Stfld && codes[i + 1].operand.ToString() == "System.Int32 scrapValue")) continue;

                codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldarg_2));
                codes.Insert(i + 1, new CodeInstruction(OpCodes.Starg, 2));
                codes.Insert(i + 1, new CodeInstruction(OpCodes.Call, beeIncreaseHiveValue));
                found = true;
            }
            if (!found) { logger.LogError("Did not find the hive scrap value..."); }
            return codes.AsEnumerable();
        }
    }
}
