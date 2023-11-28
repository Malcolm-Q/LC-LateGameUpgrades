using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(RedLocustBees))]
    internal class RedLocustPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("OnCollideWithPlayer")]
        private static bool ReduceBeeDamage(ref Collider other, RedLocustBees __instance, ref float ___timeSinceHittingPlayer, ref int ___beesZappingMode)
        {
            if(!UpgradeBus.instance.softSteps) { return true; }
            __instance.OnCollideWithPlayer(other);
            if (___timeSinceHittingPlayer < 0.4f)
            {
                return false;
            }
            PlayerControllerB playerControllerB = __instance.MeetsStandardPlayerCollisionConditions(other, false, false);
            if (playerControllerB != null)
            {
                ___timeSinceHittingPlayer = 0f;
                if (playerControllerB.health <= 10 || playerControllerB.criticallyInjured)
                {
                    PlayerControllerB player = StartOfRound.Instance.allPlayerScripts[(int)GameNetworkManager.Instance.localPlayerController.playerClientId];
                    playerControllerB.KillPlayer(Vector3.zero, true, CauseOfDeath.Electrocution, 3);
                    __instance.BeeKillPlayerServerRpc((int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                }
                else
                {
                    playerControllerB.DamagePlayer((int)(10 * Plugin.cfg.BEEKEEPER_DAMAGE_MULTIPLIER), true, true, CauseOfDeath.Electrocution, 3, false, default(Vector3));
                }
                if (___beesZappingMode != 3)
                {
                    ___beesZappingMode = 3;
                    __instance.EnterAttackZapModeServerRpc((int)GameNetworkManager.Instance.localPlayerController.playerClientId);
                }
            }
            return false;
        }
    }
}
