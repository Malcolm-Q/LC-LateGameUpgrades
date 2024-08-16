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
        [HarmonyPatch(nameof(ButlerEnemyAI.CheckLOS))]
        static IEnumerable<CodeInstruction> CheckLOSTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            FieldInfo isPlayerControlled = typeof(PlayerControllerB).GetField(nameof(PlayerControllerB.isPlayerControlled));

            object Instance;
            FieldInfo allPlayerScripts = typeof(StartOfRound).GetField(nameof(StartOfRound.allPlayerScripts));

            MethodInfo IsWearingFedoraSuitInt = typeof(FedoraSuit).GetMethod(nameof(FedoraSuit.IsWearingFedoraSuitInt));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindField(ref index, ref codes, findField: isPlayerControlled, skip: true, errorMessage: "Couldn't find the 'isPlayerControlled' field used on Line of Sight check");
            Instance = codes[index - 5].operand;
            Tools.FindAdd(ref index, ref codes, skip: true);
            codes.Insert(index, new CodeInstruction(opcode: OpCodes.Add));
            codes.Insert(index, new CodeInstruction(opcode: OpCodes.Call, operand: IsWearingFedoraSuitInt));
            codes.Insert(index, new CodeInstruction(opcode: OpCodes.Ldelem_Ref));
            codes.Insert(index, new CodeInstruction(opcode: OpCodes.Ldloc_S, operand: 4));
            codes.Insert(index, new CodeInstruction(opcode: OpCodes.Ldfld, operand: allPlayerScripts));
            codes.Insert(index, new CodeInstruction(opcode: OpCodes.Call, operand: Instance));
            return codes;
        }

    }
}
