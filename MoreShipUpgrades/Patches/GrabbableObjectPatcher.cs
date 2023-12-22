using HarmonyLib;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(GrabbableObject))]
    internal class GrabbableObjectPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("PocketItem")]
        private static void DisableParticles(GrabbableObject __instance)
        {
            ParticleSystem particles = __instance.GetComponentInChildren<ParticleSystem>();
            if(particles != null )
            {
                particles.Stop();
                particles.Clear();
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("EquipItem")]
        private static void EnableParticles(GrabbableObject __instance)
        {
            ParticleSystem particles = __instance.GetComponentInChildren<ParticleSystem>();
            if(particles != null )
            {
                particles.Play();
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("DiscardItem")]
        private static void EnableParticlesDrop(GrabbableObject __instance)
        {
            ParticleSystem particles = __instance.GetComponentInChildren<ParticleSystem>();
            if(particles != null )
            {
                particles.Play();
            }
        }
    }

}
