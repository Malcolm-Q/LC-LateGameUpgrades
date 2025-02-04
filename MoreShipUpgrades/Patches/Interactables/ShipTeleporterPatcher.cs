using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Compat;
using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace MoreShipUpgrades.Patches.Interactables
{
    [HarmonyPatch(typeof(ShipTeleporter))]
    internal static class ShipTeleporterPatcher
    {
        [HarmonyPatch(nameof(ShipTeleporter.PressButtonEffects))]
        [HarmonyPostfix]
        static void PressButtonEffectsPostfix(ShipTeleporter __instance)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.ParticleInfuserConfiguration.Enabled) return;
            __instance.teleporterAnimator.speed *= ParticleInfuser.IncreaseTeleportSpeed();
            __instance.shipTeleporterAudio.pitch *= ParticleInfuser.IncreaseTeleportSpeed();
            ParticleInfuser.instance.StartCoroutine(ParticleInfuser.instance.ResetTeleporterSpeed(__instance));
        }
        [HarmonyPatch(nameof(ShipTeleporter.beamUpPlayer), MethodType.Enumerator)]
        [HarmonyPrefix]
        static bool beamUpPlayerPrefix()
        {
            LategameConfiguration config = UpgradeBus.Instance.PluginConfiguration;
            if (!config.INTERN_ENABLED || config.INTERNS_TELEPORT_RESTRICTION == Interns.TeleportRestriction.None) return true;
            PlayerControllerB playerToBeamUp = StartOfRound.Instance.mapScreen.targetedPlayer;
            return !Interns.instance.ContainsRecentlyInterned(playerToBeamUp);
        }

        [HarmonyPatch(nameof(ShipTeleporter.beamOutPlayer), MethodType.Enumerator)]
        [HarmonyTranspiler]
        [HarmonyDebug]
        static IEnumerable<CodeInstruction> beamOutPlayerTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo SetPlayerTeleporterId = typeof(ShipTeleporter).GetMethod(nameof(ShipTeleporter.SetPlayerTeleporterId), BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo DecreaseTeleportTime = typeof(ParticleInfuser).GetMethod(nameof(ParticleInfuser.DecreaseTeleportTime));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;
            Tools.FindFloat(ref index, ref codes, findValue: 1f, addCode: DecreaseTeleportTime);

            return codes;
        }

        [HarmonyPatch(nameof(ShipTeleporter.beamUpPlayer), MethodType.Enumerator)]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> beamUpPlayerTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo DecreaseTeleportTime = typeof(ParticleInfuser).GetMethod(nameof(ParticleInfuser.DecreaseTeleportTime));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindFloat(ref index, ref codes, findValue: 3f, addCode: DecreaseTeleportTime);
            Tools.FindFloat(ref index, ref codes, findValue: 3f, addCode: DecreaseTeleportTime);
            return codes;
        }

        [HarmonyPatch(nameof(ShipTeleporter.TeleportPlayerOutWithInverseTeleporter))]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> TeleportPlayerOutWithInverseTeleporterTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo SetPlayerTeleporterId = typeof(ShipTeleporter).GetMethod(nameof(ShipTeleporter.SetPlayerTeleporterId), BindingFlags.Instance | BindingFlags.NonPublic);
            MethodInfo DropAllHeldItems = typeof(PlayerControllerB).GetMethod(nameof(PlayerControllerB.DropAllHeldItems));

            List<CodeInstruction> codes = new(instructions);
            int index = 0;

            Tools.FindLocalField(ref index, ref codes, localIndex: 0, skip: true, store: true);
            List<CodeInstruction> setTeleporterIdCodes = new();
            while(index < codes.Count)
            {
                setTeleporterIdCodes.Add(codes[index]);
                codes.RemoveAt(index);
                if (setTeleporterIdCodes[setTeleporterIdCodes.Count-1].opcode == OpCodes.Call && (MethodInfo)setTeleporterIdCodes[setTeleporterIdCodes.Count-1].operand == SetPlayerTeleporterId) break;
            }
            Tools.FindMethod(ref index, ref codes, findMethod: DropAllHeldItems, skip: true, errorMessage: "DropAllHeldItems method went missing.");
            if (index >= codes.Count)
            {
                return instructions;
            }
            codes.InsertRange(index + 1, setTeleporterIdCodes);

            return codes;
        }

        [HarmonyPatch(nameof(ShipTeleporter.TeleportPlayerOutWithInverseTeleporter))]
        [HarmonyPrefix]
        static void TeleportPlayerOutWithInverseTeleporterPrefix(ShipTeleporter __instance, int playerObj)
        {
            if (StartOfRound.Instance.allPlayerScripts[playerObj].isPlayerDead) return;
            __instance.SetPlayerTeleporterId(StartOfRound.Instance.allPlayerScripts[playerObj], 2);

        }

        [HarmonyPatch(nameof(ShipTeleporter.Update))]
        [HarmonyPostfix]
        static void UpdatePostfix(ShipTeleporter __instance)
        {
            LategameConfiguration config = UpgradeBus.Instance.PluginConfiguration;
            if (!config.INTERN_ENABLED || config.INTERNS_TELEPORT_RESTRICTION == Interns.TeleportRestriction.None || !__instance.buttonTrigger.interactable) return;
            PlayerControllerB playerToBeamUp = StartOfRound.Instance.mapScreen.targetedPlayer;
            __instance.buttonTrigger.interactable &= !Interns.instance.ContainsRecentlyInterned(playerToBeamUp);
            if (!__instance.buttonTrigger.interactable)
            {
                __instance.buttonTrigger.disabledHoverTip = $"[Recent intern must {(config.INTERNS_TELEPORT_RESTRICTION == Interns.TeleportRestriction.ExitBuilding ? "exit the building" : "enter the ship")} first]";
            }
        }
    }
}
