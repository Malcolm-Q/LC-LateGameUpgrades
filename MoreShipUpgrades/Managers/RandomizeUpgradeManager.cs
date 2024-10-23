using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using System;

namespace MoreShipUpgrades.Managers
{
    public static class RandomizeUpgradeManager
    {
        static int visibleNodes = 0;
        public enum RandomizeUpgradeEvents
        {
            PerQuota,
            PerMoonRouting,
            PerMoonLanding,
        }

        static RandomizeUpgradeEvents ConfiguredChangeEvent
        {
            get
            {
                return UpgradeBus.Instance.PluginConfiguration.RANDOMIZE_UPGRADES_CHANGE_UPGRADES_EVENT;
            }
        }

        static int ConfiguredUpgradeAmount
        {
            get
            {
                return UpgradeBus.Instance.PluginConfiguration.RANDOMIZE_UPGRADES_AMOUNT;
            }
        }

        static bool IsRandomizedEnabled
        {
            get
            {
                return UpgradeBus.Instance.PluginConfiguration.RANDOMIZE_UPGRADES_ENABLED;
            }
        }

        static bool AlwaysShowPurchased
        {
            get
            {
                return UpgradeBus.Instance.PluginConfiguration.RANDOMIZE_UPGRADES_ALWAYS_SHOW_PURCHASED;
            }
        }

        const double UPGRADE_PICK_PROBABILITY = 0.5f;
        internal static void RandomizeUpgrades()
        {
            if (!IsRandomizedEnabled) return;
            RandomizeUpgrades(UpgradeBus.Instance.randomUpgradeSeed);
        }

        internal static void RandomizeUpgrades(RandomizeUpgradeEvents triggeredEvent)
        {
            if (!LguStore.Instance.IsHost) return;
            if (!IsRandomizedEnabled) return;
            if (ConfiguredChangeEvent != triggeredEvent) return;
            LguStore.Instance.RandomizeUpgradesClientRpc(new Random().Next());
        }
        internal static void RandomizeUpgrades(int seed)
        {
            if (!IsRandomizedEnabled) return;
            SetRandomUpgradeSeed(seed);
            Random rand = new(seed);
            ResetAllUpgrades();
            int maximumUpgrade = ConfiguredUpgradeAmount;
            if (UpgradeBus.Instance.terminalNodes.Count - visibleNodes < maximumUpgrade)
            {
                foreach (CustomTerminalNode node in UpgradeBus.Instance.terminalNodes)
                    node.Visible = true;
            }
            else
            {
                int upgradeCounter = 0;
                while (upgradeCounter < maximumUpgrade)
                {
                    CustomTerminalNode selectedNode = UpgradeBus.Instance.terminalNodes[rand.Next(0, UpgradeBus.Instance.terminalNodes.Count)];
                    if (selectedNode.Visible) continue;
                    if (upgradeCounter < maximumUpgrade && rand.NextDouble() > UPGRADE_PICK_PROBABILITY)
                    {
                        selectedNode.Visible = true;
                        upgradeCounter++;
                    }
                }
            }
        }

        static void ResetAllUpgrades()
        {
            visibleNodes = 0;
            foreach (CustomTerminalNode node in UpgradeBus.Instance.terminalNodes)
            {
                node.Visible = !IsRandomizedEnabled || (node.Unlocked && (node.OriginalName != NightVision.UPGRADE_NAME || node.CurrentUpgrade > 0) && AlwaysShowPurchased);
                if (node.Visible) visibleNodes++;
            }
        }

        internal static void SetRandomUpgradeSeed(int seed)
        {
            if (seed == 0)
            {
                Plugin.mls.LogInfo("Random upgrades seed wasn't set, calculating a new seed..");
                UpgradeBus.Instance.randomUpgradeSeed = new Random().Next();
                LguStore.Instance.ServerSaveFile();
            }
            else
            {
                UpgradeBus.Instance.randomUpgradeSeed = seed;
            }
        }

        internal static int GetRandomUpgradeSeed()
        {
            return UpgradeBus.Instance.randomUpgradeSeed;
        }
    }
}
