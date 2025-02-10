using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using System.Collections.Generic;
using System.Reflection;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(QuicksandTrigger))]
    internal static class QuicksandTriggerPatcher
    {
        [HarmonyPatch(nameof(QuicksandTrigger.StopSinkingLocalPlayer))]
        [HarmonyPatch(nameof(QuicksandTrigger.OnTriggerStay))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> OnTriggerStayTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo movementHinderance = typeof(QuicksandTrigger).GetField(nameof(QuicksandTrigger.movementHinderance));
            MethodInfo reduceMovementHinderance = typeof(RubberBoots).GetMethod(nameof(RubberBoots.ReduceMovementHinderance));
            MethodInfo ClearMovementHinderance = typeof(RubberBoots).GetMethod(nameof(RubberBoots.ClearMovementHinderance));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindField(ref index, ref codes, findField: movementHinderance, addCode: reduceMovementHinderance, errorMessage: "Couldn't find the movement hinderance debuff when walking on water surface");
            Tools.FindIntegerReverse(ref index, ref codes, findValue: 1, addCode: ClearMovementHinderance, errorMessage: "Couldn't find the int used to tell the player that their movement is hindered");
            return codes;
        }
    }
}
