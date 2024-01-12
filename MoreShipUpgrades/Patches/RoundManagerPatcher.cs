using HarmonyLib;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatcher
    {
        private static int previousDaysDeadline = TimeOfDay.Instance.daysUntilDeadline;
        private static int DEFAULT_DAYS_DEADLINE = 3;
        private static bool savedPrevious = false;
        private static LGULogger logger = new LGULogger(nameof(RoundManagerPatcher));

        /// <summary>
        /// Shoutout to ustaalon (https://github.com/ustaalon) for pointing out the issue when increasing the amount of days before deadline affecting
        /// the enemy spawning
        /// </summary>
        [HarmonyPatch("PlotOutEnemiesForNextHour")]
        [HarmonyPatch("AdvanceHourAndSpawnNewBatchOfEnemies")]
        [HarmonyPrefix]
        public static void ChangeDaysForEnemySpawns()
        {
            logger.LogDebug("Changing deadline to allow spawning enemies.");
            previousDaysDeadline = TimeOfDay.Instance.daysUntilDeadline;
            TimeOfDay.Instance.daysUntilDeadline %= DEFAULT_DAYS_DEADLINE;
            savedPrevious = true;
        }

        [HarmonyPatch("PlotOutEnemiesForNextHour")]
        [HarmonyPatch("AdvanceHourAndSpawnNewBatchOfEnemies")]
        [HarmonyPostfix]
        public static void UndoChangeDaysForEnemySpawns()
        {
            if (!savedPrevious) return;
            logger.LogDebug("Changing back the deadline...");
            TimeOfDay.Instance.daysUntilDeadline = previousDaysDeadline;
            savedPrevious = false;
        }
    }
}
