using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class BackMuscles : TierUpgrade, IUpgradeWorldBuilding
    {
        internal float alteredWeight = 1f;
        internal static BackMuscles Instance;
        public const string UPGRADE_NAME = "Back Muscles";
        public const string PRICES_DEFAULT = "600,700,800";
        internal const string WORLD_BUILDING_TEXT = "\n\nCompany-issued hydraulic girdles which are only awarded to high-performing {0} who can afford to opt in." +
            " Highly valued by all employees of The Company for their combination of miraculous health-preserving benefits and artificial, intentionally-implemented scarcity." +
            " Sardonically called the 'Back Muscles Upgrade' by some. Comes with a user manual, which mostly contains minimalistic ads for girdle maintenance contractors." +
            " Most of the phone numbers don't work anymore.\n\n";
        void Awake()
        {

            upgradeName = UPGRADE_NAME;
            Instance = this;
        }
        public override void Increment()
        {
            base.Increment();
            UpdatePlayerWeight();
        }

        public override void Load()
        {
            base.Load();
            UpdatePlayerWeight();
        }
        public override void Unwind()
        {
            base.Unwind();
            UpdatePlayerWeight();
        }

        public static float DecreasePossibleWeight(float defaultWeight)
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return defaultWeight;
            return defaultWeight * (UpgradeBus.Instance.PluginConfiguration.CARRY_WEIGHT_REDUCTION.Value - GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.CARRY_WEIGHT_INCREMENT.Value);
        }
        public static void UpdatePlayerWeight()
        {
            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            if (player.ItemSlots.Length <= 0) return;

            Instance.alteredWeight = 1f;
            for (int i = 0; i < player.ItemSlots.Length; i++)
            {
                GrabbableObject obj = player.ItemSlots[i];
                if (obj == null) continue;

                Instance.alteredWeight += Mathf.Clamp(DecreasePossibleWeight(obj.itemProperties.weight - 1f), 0f, 10f);
            }
            player.carryWeight = Instance.alteredWeight;
            if (player.carryWeight < 1f) { player.carryWeight = 1f; }
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "departments" : "employees");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            Func<int, float> infoFunction = level => (UpgradeBus.Instance.PluginConfiguration.CARRY_WEIGHT_REDUCTION.Value - (level * UpgradeBus.Instance.PluginConfiguration.CARRY_WEIGHT_INCREMENT.Value)) * 100;
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }

}
