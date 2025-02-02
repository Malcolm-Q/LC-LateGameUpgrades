using HarmonyLib;
using MoreShipUpgrades.Compat;
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
using MoreShipUpgrades.Patches.Weather;

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
            PatchWeather();
            PatchCompatibility();
        }
        static void PatchCompatibility()
        {
            if (OxygenCompat.Enabled)
            {
                Plugin.mls.LogInfo("Oxygen mod has been detected. Proceeding to patch...");
                harmony.PatchAll(typeof(OxygenLogicPatcher));
                Plugin.mls.LogInfo("Patched Oxygen mod related components for correct behaviour on Oxygen Canisters upgrade in relation to oxygen consumption. If any issues arise related to the oxygen mechanic when both LGU and Oxygen mods are present, report to LGU first.");
            }
            if (LethalCompanyVRCompat.Enabled)
            {
                Plugin.mls.LogInfo("Lethal Company Virtual Reality mod has been detected. Proceeding to patch...");
                harmony.PatchAll(typeof(VRControllerPatcher));
                Plugin.mls.LogInfo("Patched Lethal Company Virtual Reality mod related components for correct behaviour on Back Muscles upgrade in relation to player's weight. If any issues arise related to the weight mechanic when both LGU and Lethal Company Virtual Reality mods are present, report to LGU first.");
            }
            if (ShipInventoryCompat.Enabled)
            {
                Plugin.mls.LogInfo("Ship Inventory mod has been detected. Proceeding to patch...");
                harmony.PatchAll(typeof(RoundManagerPatchesPatcher));
                Plugin.mls.LogInfo("Patched ShipInventory mod related components for correct behaviour on Scrap Keeper in relation of keeping items in the chute based on chance. If any issues arise related to the item chute being cleared on team wipe when both LGU and ShipInventory mods are present, report to LGU first.");
            }
            if (LethalConstellationsCompat.Enabled)
            {
                Plugin.mls.LogInfo("Lethal Constellations has been detected. Proceeding to patch...");
                harmony.PatchAll(typeof(LevelStuffPatcher));
                Plugin.mls.LogInfo("Patched Lethal Constellations mod related components for correct behaviour on Efficient Engines to affect constellation prices. If any issues arise related to this mechanic with both mods installed, report to LGU first.");
            }
            
            if (BrutalCompanyMinusExtraCompat.Enabled)
            {
                Plugin.mls.LogInfo(BrutalCompanyMinusExtraCompat.BeforePatchMessage);
                harmony.PatchAll(typeof(BrutalCompanyMinusExtraCompat.ManagerSpawnPatcher));
                harmony.PatchAll(typeof(BrutalCompanyMinusExtraCompat.GrabbableTurretPatcher));
                harmony.PatchAll(typeof(BrutalCompanyMinusExtraCompat.GrabbableLandminePatcher));
                harmony.PatchAll(typeof(BrutalCompanyMinusExtraCompat.LevelModificationsPatcher));
                Plugin.mls.LogInfo(BrutalCompanyMinusExtraCompat.SuccessfulPatchMessage);
            }

            if (ToilheadCompat.Enabled)
            {
                Plugin.mls.LogInfo("Toilhead has been detected. Proceeding to patch...");
                harmony.PatchAll(typeof(ToilheadCompat.FollowTerminalAccessibleObjectBehaviourPatcher));
                Plugin.mls.LogInfo("Patched Toilhead mod related components for correct behaviour on Malware Broadcaster to allow destroying and increased down time. If any issues arise related to turret enemies, report to LGU first.");
            }
        }
        static void PatchEnemies()
        {
            harmony.PatchAll(typeof(ButlerEnemyAIPatcher));
            harmony.PatchAll(typeof(ButlerBeesPatcher));
            harmony.PatchAll(typeof(ClaySurgeonAIPatcher));
            harmony.PatchAll(typeof(EnemyAIPatcher));
            harmony.PatchAll(typeof(EnemyAICollisionDetectPatcher));
            harmony.PatchAll(typeof(HoarderBugAIPatcher));
            harmony.PatchAll(typeof(RedLocustBeesPatch));
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
            harmony.PatchAll(typeof(QuicksandTriggerPatcher));
            harmony.PatchAll(typeof(SteamValveHazardPatch));
            harmony.PatchAll(typeof(EntranceTeleportPatcher));
            harmony.PatchAll(typeof(ShipTeleporterPatcher));
            harmony.PatchAll(typeof(VehicleControllerPatcher));
            Plugin.mls.LogInfo("Interactables have been patched");
        }

        static void PatchItems()
        {
            harmony.PatchAll(typeof(BeltBagItemPatcher));
            harmony.PatchAll(typeof(BoomBoxPatcher));
            harmony.PatchAll(typeof(DropPodPatcher));
            harmony.PatchAll(typeof(GrabbableObjectPatcher));
            harmony.PatchAll(typeof(KnifePatcher));
            harmony.PatchAll(typeof(PatchToolPatcher));
            harmony.PatchAll(typeof(RadarBoosterPatcher));
            harmony.PatchAll(typeof(ShovelPatcher));
            harmony.PatchAll(typeof(ShotgunPatcher));
            harmony.PatchAll(typeof(WalkiePatcher));
            harmony.PatchAll(typeof(SprayPaintItemPatcher));
            harmony.PatchAll(typeof(JetpackItemPatcher));
            Plugin.mls.LogInfo("Items have been patched");
        }
        static void PatchVitalComponents()
        {
            harmony.PatchAll(typeof(GameNetworkManagerPatcher));
            harmony.PatchAll(typeof(PlayerControllerBPatcher));
            harmony.PatchAll(typeof(RoundManagerPatcher));
            harmony.PatchAll(typeof(StartOfRoundPatcher));
            harmony.PatchAll(typeof(TimeOfDayPatcher));
            harmony.PatchAll(typeof(TimeOfDayTranspilerPatcher));
            harmony.PatchAll(typeof(TerminalAccessibleObjectPatcher));
            harmony.PatchAll(typeof(TerminalPatcher));
            harmony.PatchAll(typeof(Keybinds));
            Plugin.mls.LogInfo("Game managers have been patched");
        }
        static void PatchWeather()
        {
            harmony.PatchAll(typeof(StormyWeatherPatcher));
            Plugin.mls.LogInfo("Weathers have been patched");
        }
    }
}
