using HarmonyLib;
using MoreShipUpgrades.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(TerminalAccessibleObject))]
    internal class TerminalAccessibleObjectPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("CallFunctionFromTerminal")]
        private static bool DestroyObject(ref TerminalAccessibleObject __instance)
        {
            if(!UpgradeBus.instance.DestroyTraps || __instance.gameObject.layer != LayerMask.NameToLayer("MapHazards")) { return true; }
            if(__instance.gameObject.GetComponent<Landmine>() != null)
            {
                __instance.gameObject.GetComponent<Landmine>().ExplodeMineServerRpc();
            }
            else
            {
                __instance.NetworkObject.Despawn();
            }
            return false;
        }
    }
}
