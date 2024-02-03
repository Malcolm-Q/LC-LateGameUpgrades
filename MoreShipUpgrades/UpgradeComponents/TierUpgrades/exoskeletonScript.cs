using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class exoskeletonScript : BaseUpgrade
    {
        public const string UPGRADE_NAME = "Back Muscles";
        public static string PRICES_DEFAULT = "600,700,800";
        internal const string WORLD_BUILDING_TEXT = "\n\nCompany-issued hydraulic girdles which are only awarded to high-performing {0} who can afford to opt in." +
            " Highly valued by all employees of The Company for their combination of miraculous health-preserving benefits and artificial, intentionally-implemented scarcity." +
            " Sardonically called the 'Back Muscles Upgrade' by some. Comes with a user manual, which mostly contains minimalistic ads for girdle maintenance contractors." +
            " Most of the phone numbers don't work anymore.\n\n";

        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.backLevel++;
            UpdatePlayerWeight();
        }

        public override void load()
        {
            base.load();
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
        public override void Register()
        {
            base.Register();
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
