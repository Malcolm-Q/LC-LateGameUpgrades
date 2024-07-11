using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Enemies;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace MoreShipUpgrades.Patches.Enemies
{
    [HarmonyPatch(typeof(ButlerEnemyAI))]
    internal static class ButlerEnemyAIPatcher
    {
        [HarmonyTranspiler]
        [HarmonyPatch(nameof(ButlerEnemyAI.LookForChanceToMurder))]
        static IEnumerable<CodeInstruction> CheckLOSTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo targetPlayer = typeof(ButlerEnemyAI).GetField(nameof(ButlerEnemyAI.targetPlayer));
            MethodInfo isWearingFedora = typeof(FedoraSuit).GetMethod(nameof(FedoraSuit.IsWearingFedoraSuit));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindLocalField(ref index, ref codes, localIndex: 3, skip: true, errorMessage: "Couldn't find the 'isPlayerControlled' field used on Line of Sight check");
            index++;
            codes.Insert(index+1, new CodeInstruction(opcode: OpCodes.And));
            codes.Insert(index+1, new CodeInstruction(opcode: OpCodes.Not));
            codes.Insert(index+1, new CodeInstruction(opcode: OpCodes.Call, operand: isWearingFedora));
            codes.Insert(index+1, new CodeInstruction(opcode: OpCodes.Ldfld, targetPlayer));
            codes.Insert(index + 1, new CodeInstruction(opcode: OpCodes.Ldarg_0));
            return codes;
        }

    }
}
