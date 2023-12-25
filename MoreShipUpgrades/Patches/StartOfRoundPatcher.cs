using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("Start")]
        private static void InitLGUStore(PlayerControllerB __instance)
        {
            if(__instance.NetworkManager.IsHost || __instance.NetworkManager.IsServer)
            {
                GameObject refStore = GameObject.Instantiate(UpgradeBus.instance.modStorePrefab);
                refStore.GetComponent<NetworkObject>().Spawn();
            }
            foreach(GameObject sample in UpgradeBus.instance.samplePrefabs.Values)
            {
                Item item = sample.GetComponent<PhysicsProp>().itemProperties;
                if(!StartOfRound.Instance.allItemsList.itemsList.Contains(item))
                {
                    StartOfRound.Instance.allItemsList.itemsList.Add(item);
                }
            }
        }
        [HarmonyPrefix]
        [HarmonyPatch("playersFiredGameOver")]
        private static void GameOverResetUpgradeManager(StartOfRound __instance)
        {
            if(__instance.NetworkManager.IsHost ||  __instance.NetworkManager.IsServer)
            {
                LGUStore.instance.PlayersFiredServerRpc();
            }
        }

        [HarmonyTranspiler]
        [HarmonyPatch("ReviveDeadPlayers")]
        public static IEnumerable<CodeInstruction> ReviveDeadPlayers_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var maximumHealthMethod = typeof(playerHealthScript).GetMethod("CheckForAdditionalHealth", BindingFlags.Public | BindingFlags.Static);
            List<CodeInstruction> codes = instructions.ToList();
            bool first = false;
            bool second = false;
            bool third = false;
            bool updateHealth = false;
            for(int i = 0; i < codes.Count; i++)
            {
                if (first && second && third && updateHealth) break;
                if (!updateHealth)
                {
                    if (codes[i].opcode == OpCodes.Ldc_I4_S && codes[i].operand.ToString() == "100" &&
                        codes[i + 1].opcode == OpCodes.Ldc_I4_0 &&
                        codes[i + 2].opcode == OpCodes.Callvirt && codes[i + 2].operand.ToString() == "Void UpdateHealthUI(Int32, Boolean)")
                    {
                        codes[i] = new CodeInstruction(OpCodes.Call, maximumHealthMethod);
                        updateHealth = true;
                    }
                }
                if (!(codes[i].opcode == OpCodes.Ldc_I4_S && codes[i].operand.ToString() == "100")) continue;
                if (!(codes[i + 1].opcode == OpCodes.Stfld && codes[i + 1].operand.ToString() == "System.Int32 health")) continue;

                codes[i] = new CodeInstruction(OpCodes.Call, maximumHealthMethod);
                if (!first) first = true;
                else if (!second) second = true;
                else third = true;
            }
            return codes.AsEnumerable();
        }
    }
}
