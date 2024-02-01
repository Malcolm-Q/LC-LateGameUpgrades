using HarmonyLib;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace MoreShipUpgrades.Patches.Enemies
{
    [HarmonyPatch(typeof(RedLocustBees))]
    internal class RedLocustBeesPatch
    {
        private static LGULogger logger = new LGULogger(typeof(RedLocustBeesPatch).Name);
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(RedLocustBees.OnCollideWithPlayer))]
        public static IEnumerable<CodeInstruction> OnCollideWithPlayer_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo beeReduceDamage = typeof(beekeeperScript).GetMethod(nameof(beekeeperScript.CalculateBeeDamage));
            List<CodeInstruction> codes = instructions.ToList();
            bool found = false;
            for (int i = 0; i < codes.Count; i++)
            {
                if (found) break;
                if (!(codes[i].opcode == OpCodes.Callvirt && codes[i].operand.ToString() == "Void DamagePlayer(Int32, Boolean, Boolean, CauseOfDeath, Int32, Boolean, UnityEngine.Vector3)")) continue;

                /* 
                * ldc.i4.s  10  -> damageNumber
                * ldc.i4.1      -> damageSFX
                * ldc.i4.1      -> callRPC
                * ldc.i4.s  11  -> CauseDeath
                * ldc.i4.3      -> DeathAnimationTime
                * ldc.i4.0      -> fallDamage
                * ldloca.s V_1  
                * initobj[UnityEngine.CoreModule]UnityEngine.Vector3
                * ldloc.1       -> force
                * callvirt instance void GameNetcodeStuff.PlayerControllerB::DamagePlayer(int32, bool, bool, valuetype CauseOfDeath, int32, bool, valuetype[UnityEngine.CoreModule]UnityEngine.Vector3)
                */
                codes.Insert(i - 8, new CodeInstruction(OpCodes.Call, beeReduceDamage));
                found = true;
            }
            if (!found) { logger.LogError("Did not find DamagePlayer function"); }
            return codes.AsEnumerable();
        }

        [HarmonyTranspiler]
        [HarmonyPatch(nameof(RedLocustBees.SpawnHiveClientRpc))]
        public static IEnumerable<CodeInstruction> SpawnHiveClientRpcTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo beeIncreaseHiveValue = typeof(beekeeperScript).GetMethod(nameof(beekeeperScript.GetHiveScrapValue));

            List<CodeInstruction> codes = instructions.ToList();
            bool found = false;
            for (int i = 0; i < codes.Count; i++)
            {
                if (found) break;
                if (!(codes[i].opcode == OpCodes.Ldarg_2)) continue;
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
