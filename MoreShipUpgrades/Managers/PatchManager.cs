using HarmonyLib;
using MoreShipUpgrades.Input;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Patches.Enemies;
using MoreShipUpgrades.Patches.HUD;
using MoreShipUpgrades.Patches.Interactables;
using MoreShipUpgrades.Patches.Items;
using MoreShipUpgrades.Patches.NetworkManager;
using MoreShipUpgrades.Patches.PlayerController;
using MoreShipUpgrades.Patches.RoundComponents;
using MoreShipUpgrades.Patches.TerminalComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.Managers
{
    internal static class PatchManager
    {
        internal static readonly Harmony harmony = new(Metadata.GUID);

        internal static void TryPatchBetaVersion()
        {
            UpgradeBus.Instance.IsBeta = false;
        }
        internal static void PatchMainVersion()
        {
            TryPatchBetaVersion();
            PatchEnemies();
            PatchHUD();
            PatchInteractables();
            PatchItems();
            PatchVitalComponents();
            PatchWeather();
        }
        static void PatchEnemies()
        {
            try
            {
                harmony.PatchAll(typeof(BaboonBirdAIPatcher));
                harmony.PatchAll(typeof(ButlerBeesPatcher));
                harmony.PatchAll(typeof(EnemyAIPatcher));
                harmony.PatchAll(typeof(EnemyAICollisionDetectPatcher));
                harmony.PatchAll(typeof(HoarderBugAIPatcher));
                harmony.PatchAll(typeof(RedLocustBeesPatch));
                harmony.PatchAll(typeof(SpringManAIPatcher));
                Plugin.mls.LogInfo("Enemies have been patched");
            }
            catch (Exception exception)
            {
                Plugin.mls.LogError("An error has occurred patching enemies...");
                Plugin.mls.LogError(exception);
            }
        }

        static void PatchHUD()
        {
            try
            {
                harmony.PatchAll(typeof(HudManagerPatcher));
                Plugin.mls.LogInfo("HUD has been patched");
            }
            catch (Exception exception)
            {
                Plugin.mls.LogError("An error has occurred patching HUD...");
                Plugin.mls.LogError(exception);
            }
        }

        static void PatchInteractables()
        {
            try
            {
                harmony.PatchAll(typeof(DoorLockPatcher));
                harmony.PatchAll(typeof(InteractTriggerPatcher));
                harmony.PatchAll(typeof(StartMatchLevelPatcher));
                harmony.PatchAll(typeof(SteamValveHazardPatch));
                Plugin.mls.LogInfo("Interactables have been patched");
            }
            catch (Exception exception)
            {
                Plugin.mls.LogError("An error has occurred patching interactables...");
                Plugin.mls.LogError(exception);
            }
        }

        static void PatchItems()
        {
            try
            {
                harmony.PatchAll(typeof(BoomBoxPatcher));
                harmony.PatchAll(typeof(DropPodPatcher));
                harmony.PatchAll(typeof(GrabbableObjectPatcher));
                harmony.PatchAll(typeof(KnifePatcher));
                harmony.PatchAll(typeof(PatchToolPatcher));
                harmony.PatchAll(typeof(RadarBoosterPatcher));
                harmony.PatchAll(typeof(ShovelPatcher));
                harmony.PatchAll(typeof(WalkiePatcher));
                Plugin.mls.LogInfo("Items have been patched");
            }
            catch (Exception exception)
            {
                Plugin.mls.LogError("An error has occurred patching items...");
                Plugin.mls.LogError(exception);
            }
        }
        static void PatchVitalComponents()
        {
            try
            {
                harmony.PatchAll(typeof(GameNetworkManagerPatcher));
                harmony.PatchAll(typeof(PlayerControllerBPatcher));
                harmony.PatchAll(typeof(RoundManagerPatcher));
                harmony.PatchAll(typeof(RoundManagerTranspilerPatcher));
                harmony.PatchAll(typeof(StartOfRoundPatcher));
                harmony.PatchAll(typeof(TimeOfDayPatcher));
                harmony.PatchAll(typeof(TimeOfDayTranspilerPatcher));
                harmony.PatchAll(typeof(TerminalAccessibleObjectPatcher));
                harmony.PatchAll(typeof(TerminalPatcher));
                harmony.PatchAll(typeof(Keybinds));
                Plugin.mls.LogInfo("Game managers have been patched");
            }
            catch (Exception exception)
            {
                Plugin.mls.LogError("An error has occurred patching the game managers...");
                Plugin.mls.LogError(exception);
            }
        }
        static void PatchWeather()
        {
            try
            {
                harmony.PatchAll(typeof(StormyWeather));
                Plugin.mls.LogInfo("Weathers have been patched");
            }
            catch (Exception exception)
            {
                Plugin.mls.LogError("An error has occurred patching weathers...");
                Plugin.mls.LogError(exception);
            }
        }
    }
}
