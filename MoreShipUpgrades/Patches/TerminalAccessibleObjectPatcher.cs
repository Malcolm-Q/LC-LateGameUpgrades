using HarmonyLib;
using MoreShipUpgrades.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(TerminalAccessibleObject))]
    internal class TerminalAccessibleObjectPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("CallFunctionFromTerminal")]
        private static bool DestroyObject(ref TerminalAccessibleObject __instance)
        {
            if(!UpgradeBus.instance.DestroyTraps) { return true; }
            __instance.NetworkObject.Despawn();
            return false;
        }
    }
}
