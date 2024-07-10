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
using MoreShipUpgrades.Patches.Vehicle;
using MoreShipUpgrades.Patches.Weather;
using System;

namespace MoreShipUpgrades.Managers
{
    internal static class PatchManager
    {
        internal static readonly Harmony harmony = new(Metadata.GUID);
        internal static void PatchMainVersion()
        {
            PatchEnemies();
            PatchHUD();
            PatchInteractables();
            PatchItems();
            PatchVitalComponents();
            PatchVehicle();
            PatchWeather();
        }
        static void PatchEnemies()
        {
            harmony.PatchAll(typeof(BaboonBirdAIPatcher));
            harmony.PatchAll(typeof(ButlerBeesPatcher));
            harmony.PatchAll(typeof(ClaySurgeonAIPatcher));
            harmony.PatchAll(typeof(EnemyAIPatcher));
            harmony.PatchAll(typeof(EnemyAICollisionDetectPatcher));
            harmony.PatchAll(typeof(HoarderBugAIPatcher));
            harmony.PatchAll(typeof(RedLocustBeesPatch));
            harmony.PatchAll(typeof(SpringManAIPatcher));
            Plugin.mls.LogInfo("Enemies have been patched");
        }

        static void PatchHUD()
        {
            harmony.PatchAll(typeof(HudManagerPatcher));
            Plugin.mls.LogInfo("HUD has been patched");
        }

        static void PatchInteractables()
        {
            harmony.PatchAll(typeof(DepositItemsDeskPatcher));
            harmony.PatchAll(typeof(DoorLockPatcher));
            harmony.PatchAll(typeof(HangarShipDoorPatcher));
            harmony.PatchAll(typeof(InteractTriggerPatcher));
            harmony.PatchAll(typeof(StartMatchLevelPatcher));
            harmony.PatchAll(typeof(SteamValveHazardPatch));
            Plugin.mls.LogInfo("Interactables have been patched");
        }

        static void PatchItems()
        {
            harmony.PatchAll(typeof(BoomBoxPatcher));
            harmony.PatchAll(typeof(DropPodPatcher));
            harmony.PatchAll(typeof(GrabbableObjectPatcher));
            harmony.PatchAll(typeof(KnifePatcher));
            harmony.PatchAll(typeof(PatchToolPatcher));
            harmony.PatchAll(typeof(RadarBoosterPatcher));
            harmony.PatchAll(typeof(ShovelPatcher));
            harmony.PatchAll(typeof(WalkiePatcher));
            harmony.PatchAll(typeof(SprayPaintItemPatcher));
            Plugin.mls.LogInfo("Items have been patched");
        }
        static void PatchVitalComponents()
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

        static void PatchVehicle()
        {
            harmony.PatchAll(typeof(VehicleControllerPatcher));
            Plugin.mls.LogInfo("Vehicles have been patched");
        }
        static void PatchWeather()
        {
            harmony.PatchAll(typeof(StormyWeatherPatcher));
            Plugin.mls.LogInfo("Weathers have been patched");
        }
    }
}
