using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades
{
    internal class StrongLegs : GameAttributeTierUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Strong Legs";
        public static string PRICES_DEFAULT = "150,190,250";
        internal const string WORLD_BUILDING_TEXT = "\n\nOne-time issuance of {0}." +
            " Comes with a vague list of opt-in maintenance procedures offered by The Company, which includes such gems as 'actuation optimization'," +
            " 'weight & balance personalization', and similar nigh-meaningless corpo-tech jargon. All of it is expensive.\n\n";

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
        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "proprietary pressure-assisted kneebraces to your crew" : "a proprietary pressure-assisted kneebrace");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.instance.cfg.JUMP_FORCE_UNLOCK + level * UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
