using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class StrongLegs : GameAttributeTierUpgrade
    {
        public static string UPGRADE_NAME = "Strong Legs";
        public static string PRICES_DEFAULT = "150,190,250";

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(UPGRADE_NAME);
            base.Start();
            changingAttribute = GameAttribute.PLAYER_JUMP_FORCE;
            initialValue = UpgradeBus.instance.cfg.JUMP_FORCE_UNLOCK;
            incrementalValue = UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
        }

        public override void Increment()
        {
            base.Increment();
            UpgradeBus.instance.legLevel++;
        }

        public override void Unwind()
        {
            UnloadUpgradeAttribute(ref UpgradeBus.instance.strongLegs, ref UpgradeBus.instance.legLevel);
            base.Unwind();
        }
        public override void Load()
        {
            LoadUpgradeAttribute(ref UpgradeBus.instance.strongLegs, UpgradeBus.instance.legLevel);
            base.Load();
        }
        public static int ReduceFallDamage(int defaultValue)
        {
            if (!(UpgradeBus.instance.strongLegs && UpgradeBus.instance.legLevel == UpgradeBus.instance.cfg.STRONG_LEGS_UPGRADE_PRICES.Split(',').Length)) return defaultValue;
            return (int)(defaultValue * (1.0f - UpgradeBus.instance.cfg.STRONG_LEGS_REDUCE_FALL_DAMAGE_MULTIPLIER));
        }
    }
}
