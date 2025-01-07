using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship;

namespace MoreShipUpgrades.API
{
    public static class DiscombobulatorApi
    {
        public static bool IsDiscombobulatorEnabled()
        {
            return UpgradeBus.Instance.PluginConfiguration.DiscombobulatorUpgradeConfiguration.Enabled;
        }

        public static bool CanFireDiscombobulator()
        {
            if (!IsDiscombobulatorEnabled()) return false;
            return Discombobulator.instance.flashCooldown <= 0f;
        }

        public static float GetDiscombobulatorCooldown()
        {
            if (!IsDiscombobulatorEnabled()) return -1;
            return Discombobulator.instance.flashCooldown;
        }

        public static void SetDiscombobulatorCooldown(float value)
        {
            if (!IsDiscombobulatorEnabled()) return;
            Discombobulator.instance.SetCooldownServerRpc(value);
        }

        public static void AddDiscombobulatorCooldown(float value)
        {
            if (!IsDiscombobulatorEnabled()) return;
            Discombobulator.instance.SetCooldownServerRpc(Discombobulator.instance.flashCooldown + value);
        }

        public static void RemoveDiscombobulatorCooldown(float value)
        {
            if (!IsDiscombobulatorEnabled()) return;
            Discombobulator.instance.SetCooldownServerRpc(Discombobulator.instance.flashCooldown - value);
        }

        const int ERROR = -1;
        const int SUCCESS_FIRED = 0;
        public static int FireDiscombobulator()
        {
            if (!CanFireDiscombobulator()) return ERROR;

            if (Discombobulator.instance.IsServer) Discombobulator.instance.UseDiscombobulatorClientRpc();
            else Discombobulator.instance.UseDiscombobulatorServerRpc();

            return SUCCESS_FIRED;
        }
    }
}
