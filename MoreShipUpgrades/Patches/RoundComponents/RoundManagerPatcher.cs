using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Commands;

namespace MoreShipUpgrades.Patches.RoundComponents
{
    [HarmonyPatch(typeof(RoundManager))]
    internal static class RoundManagerPatcher
    {
        static int previousDaysDeadline = TimeOfDay.Instance.daysUntilDeadline;
        static int DEFAULT_DAYS_DEADLINE = 4;
        static bool savedPrevious = false;
        static LguLogger logger = new LguLogger(nameof(RoundManagerPatcher));

        /// <summary>
        /// Shoutout to ustaalon (https://github.com/ustaalon) for pointing out the issue when increasing the amount of days before deadline affecting
        /// the enemy spawning
        /// </summary>
        [HarmonyPatch(nameof(RoundManager.PlotOutEnemiesForNextHour))]
        [HarmonyPatch(nameof(RoundManager.AdvanceHourAndSpawnNewBatchOfEnemies))]
        [HarmonyPrefix]
        static void ChangeDaysForEnemySpawns()
        {
            if (!UpgradeBus.Instance.PluginConfiguration.EXTEND_DEADLINE_ENABLED.Value) return; //  Don't bother changing something if we never touch it
            if (TimeOfDay.Instance.daysUntilDeadline < DEFAULT_DAYS_DEADLINE) return; // Either it's already fine or some other mod already changed the value to be acceptable
            logger.LogDebug("Changing deadline to allow spawning enemies.");
            previousDaysDeadline = TimeOfDay.Instance.daysUntilDeadline;
            TimeOfDay.Instance.daysUntilDeadline %= DEFAULT_DAYS_DEADLINE;
            savedPrevious = true;
        }

        [HarmonyPatch(nameof(RoundManager.PlotOutEnemiesForNextHour))]
        [HarmonyPatch(nameof(RoundManager.AdvanceHourAndSpawnNewBatchOfEnemies))]
        [HarmonyPostfix]
        static void UndoChangeDaysForEnemySpawns()
        {
            if (!UpgradeBus.Instance.PluginConfiguration.EXTEND_DEADLINE_ENABLED.Value) return; //  Don't bother changing something if we never touch it
            if (!savedPrevious) return;
            logger.LogDebug("Changing back the deadline...");
            TimeOfDay.Instance.daysUntilDeadline = previousDaysDeadline;
            savedPrevious = false;
        }

        [HarmonyPatch(nameof(RoundManager.DespawnPropsAtEndOfRound))]
        [HarmonyPostfix]
        static void DespawnPropsAtEndOfRoundPostfix()
        {
            if (!UpgradeBus.Instance.PluginConfiguration.SCRAP_INSURANCE_ENABLED.Value) return;
            ScrapInsurance.TurnOffScrapInsurance();
        }
    }
}
