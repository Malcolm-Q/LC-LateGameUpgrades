using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    class BiggerLungs : GameAttributeTierUpgrade, IUpgradeWorldBuilding, IPlayerSync
    {
        public const string UPGRADE_NAME = "Bigger Lungs";
        public static string PRICES_DEFAULT = "350,450,550";
        internal const string WORLD_BUILDING_TEXT = "\n\nService package for {0}." +
            " Opting into every maintenance procedure will arrange for your suit's pipes to be cleaned and repaired, filters re-issued," +
            " and DRM removed from the integrated air conditioning system.\n\n";

        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LguLogger(UPGRADE_NAME);
            changingAttribute = GameAttribute.PLAYER_SPRINT_TIME;
            initialValue = UpgradeBus.Instance.PluginConfiguration.SPRINT_TIME_INCREASE_UNLOCK.Value;
            incrementalValue = UpgradeBus.Instance.PluginConfiguration.SPRINT_TIME_INCREMENT.Value;
        }
        public static float ApplyPossibleIncreasedStaminaRegen(float regenValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_ENABLED.Value) return regenValue;
            if (!GetActiveUpgrade(UPGRADE_NAME) || GetUpgradeLevel(UPGRADE_NAME) < (UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_STAMINA_REGEN_APPLY_LEVEL.Value-1)) return regenValue;
            return regenValue * Mathf.Clamp(UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_STAMINA_REGEN_INCREASE.Value + (UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_STAMINA_REGEN_INCREMENTAL_INCREASE * Mathf.Abs(GetUpgradeLevel(UPGRADE_NAME) - UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_STAMINA_REGEN_APPLY_LEVEL.Value - 1)), 0f, 10f);

        }

        public static float ApplyPossibleReducedJumpStaminaCost(float jumpCost)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_ENABLED.Value) return jumpCost;
            if (!GetActiveUpgrade(UPGRADE_NAME) || GetUpgradeLevel(UPGRADE_NAME) < (UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_JUMP_STAMINA_APPLY_LEVEL.Value-1)) return jumpCost;
            return jumpCost * Mathf.Clamp(UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_JUMP_STAMINA_COST_DECREASE.Value - (UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_JUMP_STAMINA_COST_INCREMENTAL_DECREASE * Mathf.Abs(GetUpgradeLevel(UPGRADE_NAME) - UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_JUMP_STAMINA_APPLY_LEVEL.Value - 1)), 0f, 10f);
        }
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "your crew's suit oxigen delivery systems" : "your suit's oxygen delivery system");
        }

        string GetBiggerlungsInfo(int level, int price)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.SPRINT_TIME_INCREASE_UNLOCK.Value + level * UpgradeBus.Instance.PluginConfiguration.SPRINT_TIME_INCREMENT.Value;
            System.Func<int, float> costReductionInfo = level => 1f-(UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_JUMP_STAMINA_COST_DECREASE.Value - level * UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_JUMP_STAMINA_COST_INCREMENTAL_DECREASE.Value);
            System.Func<int, float> staminaRegenerationInfo = level => (UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_STAMINA_REGEN_INCREASE.Value + level * UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_STAMINA_REGEN_INCREMENTAL_INCREASE.Value)-1f;
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format(AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME), level, price, infoFunction(level-1)));
            if (level >= UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_STAMINA_REGEN_APPLY_LEVEL.Value) sb.Append($"Stamina regeneration is increased by {Mathf.FloorToInt(staminaRegenerationInfo(level)*100f)}%\n");
            if (level >= UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_JUMP_STAMINA_APPLY_LEVEL.Value) sb.Append($"Stamina used when jumping is reduced by {Mathf.FloorToInt(costReductionInfo(level)*100f)}%\n");
            return sb.ToString();
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetBiggerlungsInfo(1, initialPrice));
            for (int i = 0; i < maxLevels; i++)
                sb.Append(GetBiggerlungsInfo(i + 2, incrementalPrices[i]));
            return sb.ToString();
        }
        internal override bool CanInitializeOnStart()
        {
            string[] prices = UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_UPGRADE_PRICES.Value.Split(',');
            bool free = UpgradeBus.Instance.PluginConfiguration.BIGGER_LUNGS_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            return free;
        }
    }
}
