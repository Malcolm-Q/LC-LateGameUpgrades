using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;
using UnityEngine;


namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(StormyWeather))]
    internal class StormyWeatherPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("LightningStrike")]
        static void CheckIfLightningRodPresent(StormyWeather __instance,ref Vector3 strikePosition, bool useTargetedObject)
        {
            if (UpgradeBus.instance.LightningIntercepted && useTargetedObject)
            {
                // we need to check useTargetedObject so we're not rerouting random strikes to the ship.
                Debug.Log("$$$\nINTERCEPTED STRIKE\n$$$");
                if(UpgradeBus.instance.terminal == null) UpgradeBus.instance.terminal = GameObject.Find("TerminalScript").GetComponent<Terminal>();
                strikePosition = UpgradeBus.instance.terminal.transform.position;
                UpgradeBus.instance.LightningIntercepted = false;
                __instance.staticElectricityParticle.gameObject.SetActive(true);
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        static void InterceptSelectedObject(StormyWeather __instance, GrabbableObject ___targetingMetalObject)
        {
            if(!UpgradeBus.instance.lightningRod || !LGUStore.instance.IsHost || !LGUStore.instance.IsServer) { return; }
            if(___targetingMetalObject == null)
            {
                UpgradeBus.instance.CanTryInterceptLightning = true;
            }
            else
            {
                if (!UpgradeBus.instance.CanTryInterceptLightning) return;
                UpgradeBus.instance.CanTryInterceptLightning = false;

                if(UpgradeBus.instance.terminal == null) UpgradeBus.instance.terminal = GameObject.Find("TerminalScript").GetComponent<Terminal>();
                float dist = Vector3.Distance(___targetingMetalObject.transform.position, UpgradeBus.instance.terminal.transform.position);
                Debug.Log($"DISTANCE FROM SHIP: {dist}\nEFFECTIVE DISTANCE: {UpgradeBus.instance.cfg.LIGHTNING_ROD_DIST}");
                if(dist > UpgradeBus.instance.cfg.LIGHTNING_ROD_DIST) { return; }

                dist /= UpgradeBus.instance.cfg.LIGHTNING_ROD_DIST;
                float prob = 1 - dist;
                float rand = Random.value;
                Debug.Log($"NUMBER TO BEAT: {prob}\nNUMBER: {rand}");
                if(rand < prob)
                {
                    Debug.Log("PLANNING INTERCEPTTION");
                    __instance.staticElectricityParticle.Stop();
                    UpgradeBus.instance.LightningIntercepted = true;
                    LGUStore.instance.CoordinateInterceptionClientRpc();
                }
            }
        }
    }
}
