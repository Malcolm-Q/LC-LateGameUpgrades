using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship;

namespace MoreShipUpgrades.API
{
    public static class QuantumDisruptorApi
    {

        public static bool IsQuantumDisruptorEnabled()
        {
            return UpgradeBus.Instance.PluginConfiguration.QuantumDisruptorConfiguration.Enabled;
        }

        public static bool IsQuantumDisruptorCommandEnabled()
        {
            return IsQuantumDisruptorEnabled() && QuantumDisruptor.CurrentMode == QuantumDisruptor.UpgradeModes.RevertTime;
        }

        public static QuantumDisruptor.ResetModes GetQuantumDisruptorResetMode()
        {
            return QuantumDisruptor.CurrentResetMode;
        }

        public static int GetCurrentRevertUsages()
        {
            if (!IsQuantumDisruptorCommandEnabled()) return -1;
            return QuantumDisruptor.Instance.currentUsages;
        }

        public static int GetMaximumRevertUsages()
        {
            if (!IsQuantumDisruptorCommandEnabled()) return -1;
            return QuantumDisruptor.Instance.availableUsages;
        }

        public static int GetHoursToReducePerUsage()
        {
            if (!IsQuantumDisruptorCommandEnabled()) return -1;
            return QuantumDisruptor.Instance.hoursToReduce;
        }

        public static (bool, string) CanFireQuantumDisruptorRevertTime()
        {
            if (!IsQuantumDisruptorCommandEnabled()) return (false, "Quantum Disruptor Revert Command is not available.");

            return QuantumDisruptor.Instance.CanRevertTime();
        }

        public static void TriggerQuantumDisruptorRevertTime()
        {
            (bool, string) output = CanFireQuantumDisruptorRevertTime();
            if (!output.Item1) return;
            if (QuantumDisruptor.Instance.IsServer) QuantumDisruptor.Instance.RevertTimeClientRpc();
            else QuantumDisruptor.Instance.RevertTimeServerRpc();
        }
    }
}
