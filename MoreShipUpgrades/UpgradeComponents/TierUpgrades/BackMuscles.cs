using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class BackMuscles : TierUpgrade
    {
        public static string UPGRADE_NAME = "Back Muscles";
        public static string PRICES_DEFAULT = "600,700,800";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            base.Start();
        }

        public override void Increment()
        {
            UpgradeBus.instance.backLevel++;
            UpdatePlayerWeight();
        }

        public override void Load()
        {
            base.Load();
            UpgradeBus.instance.exoskeleton = true;
            UpdatePlayerWeight();
        }
        public override void Unwind()
        {
            base.Unwind();
            UpgradeBus.instance.exoskeleton = false;
            UpgradeBus.instance.backLevel = 0;
            UpdatePlayerWeight();
        }

        public static float DecreasePossibleWeight(float defaultWeight)
        {
            if (!UpgradeBus.instance.exoskeleton) return defaultWeight;
            return defaultWeight * (UpgradeBus.instance.cfg.CARRY_WEIGHT_REDUCTION - UpgradeBus.instance.backLevel * UpgradeBus.instance.cfg.CARRY_WEIGHT_INCREMENT);
        }
        public static void UpdatePlayerWeight()
        {
            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            if (player.ItemSlots.Length <= 0) return;

            UpgradeBus.instance.alteredWeight = 1f;
            for (int i = 0; i < player.ItemSlots.Length; i++)
            {
                GrabbableObject obj = player.ItemSlots[i];
                if (obj == null) continue;

                UpgradeBus.instance.alteredWeight += Mathf.Clamp(DecreasePossibleWeight(obj.itemProperties.weight - 1f), 0f, 10f);
            }
            player.carryWeight = UpgradeBus.instance.alteredWeight;
            if (player.carryWeight < 1f) { player.carryWeight = 1f; }
        }
    }

}
