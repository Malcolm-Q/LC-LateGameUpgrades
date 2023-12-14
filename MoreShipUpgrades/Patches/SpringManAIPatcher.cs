using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(SpringManAI))]
    internal class SpringManAIPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("Update")]
        private static bool DetectCoilItem(ref SpringManAI __instance, bool ___stoppingMovement, float ___currentAnimSpeed)
        {
            if(UpgradeBus.instance.coilHeadItems.Count > 0)
            {
                foreach(coilHeadItem item in UpgradeBus.instance.coilHeadItems)
                {
                    if(item == null) 
                    {
                        UpgradeBus.instance.coilHeadItems.Remove(item);
                        continue;
                    }
                    if (item.HasLineOfSightToPosition(__instance.transform.position))
                    {
                        if (!___stoppingMovement)
                        {
                            RoundManager.PlayRandomClip(__instance.creatureVoice, __instance.springNoises, false, 1f, 0);
                            if(__instance.IsOwner)
                            {
                                __instance.SetAnimationStopServerRpc();
                            }
                            __instance.creatureAnimator.SetTrigger("springBoing");
                            ___stoppingMovement = true;
                        }
                        __instance.mainCollider.isTrigger = false;
                        __instance.creatureAnimator.SetFloat("WalkSpeed", 0f);
                        ___currentAnimSpeed = 0f;
                        __instance.agent.speed = 0f;
                        __instance.creatureAnimator.speed = 0f; // idk why this isn't working so we're just doing this for now
                        return false; // Transpilers limit your potential to fix future incompatibilities.
                    }
                }
            }
            return true;
        }
    }

}
