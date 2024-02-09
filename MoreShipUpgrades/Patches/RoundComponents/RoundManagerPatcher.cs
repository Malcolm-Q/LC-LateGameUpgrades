using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Commands;

namespace MoreShipUpgrades.Patches.RoundComponents
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatcher
    {
        private static int previousDaysDeadline = TimeOfDay.Instance.daysUntilDeadline;
        private static int DEFAULT_DAYS_DEADLINE = 4;
        private static bool savedPrevious = false;
        private static LGULogger logger = new LGULogger(nameof(RoundManagerPatcher));

        /// <summary>
        /// Shoutout to ustaalon (https://github.com/ustaalon) for pointing out the issue when increasing the amount of days before deadline affecting
        /// the enemy spawning
        /// </summary>
        [HarmonyPatch(nameof(RoundManager.PlotOutEnemiesForNextHour))]
        [HarmonyPatch(nameof(RoundManager.AdvanceHourAndSpawnNewBatchOfEnemies))]
        [HarmonyPrefix]
        public static void ChangeDaysForEnemySpawns()
        {
            if (!UpgradeBus.instance.cfg.EXTEND_DEADLINE_ENABLED.Value) return; //  Don't bother changing something if we never touch it
            if (TimeOfDay.Instance.daysUntilDeadline < DEFAULT_DAYS_DEADLINE) return; // Either it's already fine or some other mod already changed the value to be acceptable
            logger.LogDebug("Changing deadline to allow spawning enemies.");
            previousDaysDeadline = TimeOfDay.Instance.daysUntilDeadline;
            TimeOfDay.Instance.daysUntilDeadline %= DEFAULT_DAYS_DEADLINE;
            savedPrevious = true;
        }

        [HarmonyPatch(nameof(RoundManager.PlotOutEnemiesForNextHour))]
        [HarmonyPatch(nameof(RoundManager.AdvanceHourAndSpawnNewBatchOfEnemies))]
        [HarmonyPostfix]
        public static void UndoChangeDaysForEnemySpawns()
        {
            if (!UpgradeBus.instance.cfg.EXTEND_DEADLINE_ENABLED.Value) return; //  Don't bother changing something if we never touch it
            if (!savedPrevious) return;
            logger.LogDebug("Changing back the deadline...");
            TimeOfDay.Instance.daysUntilDeadline = previousDaysDeadline;
            savedPrevious = false;
        }

        [HarmonyPatch(nameof(RoundManager.DespawnPropsAtEndOfRound))]
        [HarmonyPostfix]
        public static void DespawnPropsAtEndOfRoundPostfix()
        {
            if (!UpgradeBus.instance.cfg.SCRAP_INSURANCE_ENABLED.Value) return;
            ScrapInsurance.TurnOffScrapInsurance();
        }
    }
}
