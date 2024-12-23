using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrutalCompanyMinus;
using BrutalCompanyMinus.Minus;
using BrutalCompanyMinus.Minus.Handlers;
using BrutalCompanyMinus.Minus.MonoBehaviours;
using HarmonyLib;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship;

namespace MoreShipUpgrades.Compat
{
    public static class BrutalCompanyMinusExtraCompat // pull request 22.12.2024 from tixomirof
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(BrutalCompanyMinus.Plugin.GUID);
        public static string BeforePatchMessage =>
            "Brutal Company Minus Extra has been detected. Proceeding to patch...";
        public static string SuccessfulPatchMessage =>
            "Patched Brutal Company Minus Extra mod related components for correct behaviour " +
            "on Midas Touch upgrade for scrap that is spawned by Brutal's events/ScrapAmount multiplier stat. " + 
            "If any issues arise related to the scrap value calculation when both LGU and BCME mods are present, " +
            "report to LGU first.";

        #region Compat Tool Classes
        class GrabbableHazardState
        {
            public int minValue;
            public int maxValue;
        }
        #endregion
        #region Compat Tool Methods
        /// <summary>
        /// Multiplies <see cref="RoundManager.scrapValueMultiplier"/> by <see cref="MidasTouch.IncreaseScrapValue(float)"/>
        /// for scrap value calculation in given code <paramref name="instructions"/>
        /// </summary>
        /// <param name="instructions">Method's CIL instructions</param>
        /// <param name="scrapErrorMessagePrefix">String prefix for errorMessage param in
        /// <see cref="Tools.FindField(ref int, ref List{CodeInstruction}, System.Reflection.FieldInfo, System.Reflection.MethodInfo, bool, bool, bool, bool, bool, bool, string)"/></param>
        /// <returns>New method's CIL instructions</returns>
        private static IEnumerable<CodeInstruction> MultiplyScrapValueOnSpawn(
            IEnumerable<CodeInstruction> instructions, string scrapErrorMessagePrefix)
        {
            var midasMultiplier = typeof(MidasTouch).GetMethod(nameof(MidasTouch.IncreaseScrapValue));
            var managerMultiplier = typeof(RoundManager).GetField(nameof(RoundManager.scrapValueMultiplier));

            var codes = new List<CodeInstruction>(instructions);
            var index = 0;
            Tools.FindField(ref index, ref codes, findField: managerMultiplier, addCode: midasMultiplier,
                errorMessage: $"Scrap value multiplier for brutal company minus extra's {scrapErrorMessagePrefix} scrap generation was not found.");
            return codes;
        }

        /// <summary>
        /// Multiplies <see cref="Item.minValue"/> and <see cref="Item.maxValue"/> for <paramref name="grabbableHazard"/> 
        /// by <see cref="MidasTouch.IncreaseScrapValueInteger(int)"/>.<br></br>Must be called before dungeon generation starts.
        /// </summary>
        /// <param name="__state">Original state of <paramref name="grabbableHazard"/></param>
        /// <param name="isProcessing">Check for multiply method calls to prevent unwanted 1 billion scrap value items</param>
        /// <param name="grabbableHazard">Expected <see cref="GrabbableTurret"/>'s or <see cref="GrabbableLandmine"/>'s in-game asset</param>
        private static void GrabbableHazardPrefix(out GrabbableHazardState __state, ref bool isProcessing, Item grabbableHazard)
        {
            __state = new GrabbableHazardState
            {
                minValue = 0,
                maxValue = 0
            };

            if (!isProcessing)
            {
                isProcessing = true;
                __state.minValue = grabbableHazard.minValue;
                __state.maxValue = grabbableHazard.maxValue;
                grabbableHazard.minValue = MidasTouch.IncreaseScrapValueInteger(grabbableHazard.minValue);
                grabbableHazard.maxValue = MidasTouch.IncreaseScrapValueInteger(grabbableHazard.maxValue);
            }
        }

        /// <summary>
        /// Returns <see cref="Item.minValue"/> and <see cref="Item.maxValue"/> for <paramref name="grabbableHazard"/>
        /// back to original values using <paramref name="__state"/>.<br></br>Must be called after dungeon generation finishes.
        /// </summary>
        /// <param name="__state">Original state of <paramref name="grabbableHazard"/>, must be prefilled by
        /// <see cref="GrabbableHazardPrefix(out GrabbableHazardState, ref bool, Item)"/></param>
        /// <param name="isProcessing">Link for internal class <see cref="Boolean"/> value, which will set it to <see langword="false"/>, to prevent multiple method calls</param>
        /// <param name="grabbableHazard">Expected <see cref="GrabbableTurret"/>'s or <see cref="GrabbableLandmine"/>'s in-game asset</param>
        private static void GrabbableHazardPostfix(GrabbableHazardState __state, Action isProcessing, Item grabbableHazard)
        {
            if (__state.minValue == __state.maxValue && __state.minValue == 0) return;

            Task.Run(async () =>
            {
                const int generationWait = 10000;

                await Task.Delay(generationWait);
                grabbableHazard.minValue = __state.minValue;
                grabbableHazard.maxValue = __state.maxValue;
                isProcessing();
            });
        }
        #endregion
        #region Patchers
        [HarmonyPatch(typeof(Manager.Spawn))]
        internal static class ManagerSpawnPatcher // patch for brutal's outside/inside scrap spawns
        {
            [HarmonyPatch(nameof(Manager.Spawn.DoSpawnScrapInside))]
            [HarmonyTranspiler]
            private static IEnumerable<CodeInstruction> DoSpawnScrapInsideTranspiler(IEnumerable<CodeInstruction> instructions)
                => MultiplyScrapValueOnSpawn(instructions, "inside");

            [HarmonyPatch(nameof(Manager.Spawn.DoSpawnScrapOutside))]
            [HarmonyTranspiler]
            private static IEnumerable<CodeInstruction> DoSpawnScrapOutsideTranspiler(IEnumerable<CodeInstruction> instructions)
                => MultiplyScrapValueOnSpawn(instructions, "outside");
        }

        [HarmonyPatch(typeof(GrabbableTurret), nameof(GrabbableTurret.onTurretStart))]
        internal static class GrabbableTurretPatcher // patch for grabbable turret event item to use midas touch multiplier
        {
            [HarmonyPrefix]
            private static void OnTurretStartPrefix(out GrabbableHazardState __state)
                => GrabbableHazardPrefix(out __state, ref isProcessing, Assets.grabbableTurret);

            [HarmonyPostfix]
            private static void OnTurretStartPostfix(GrabbableHazardState __state)
                => GrabbableHazardPostfix(__state, () => isProcessing = false, Assets.grabbableTurret);

            private static bool isProcessing = false;
        }

        [HarmonyPatch(typeof(GrabbableLandmine), nameof(GrabbableLandmine.onLandmineStart))]
        internal static class GrabbableLandminePatcher // patch for grabbable landmine event item to use midas touch multiplier
        {
            [HarmonyPrefix]
            private static void OnLandmineStartPrefix(out GrabbableHazardState __state)
                => GrabbableHazardPrefix(out __state, ref isProcessing, Assets.grabbableLandmine);

            [HarmonyPostfix]
            private static void OnLandmineStartPostfix(GrabbableHazardState __state)
                => GrabbableHazardPostfix(__state, () => isProcessing = false, Assets.grabbableLandmine);

            private static bool isProcessing = false;
        }

        [HarmonyPatch(typeof(LevelModifications))]
        internal static class LevelModificationsPatcher // patch for transmuted scrap (aka pickles, teeth, gold bar events) to use midas touch multiplier
        {
            [HarmonyPatch(nameof(LevelModifications.OnwaitForScrapToSpawnToSync))]
            [HarmonyTranspiler]
            private static IEnumerable<CodeInstruction> OnwaitForScrapToSpawnToSyncTranspiler(IEnumerable<CodeInstruction> instructions)
                => MultiplyScrapValueOnSpawn(instructions, "transmuted");
        }
        #endregion
    }
}
