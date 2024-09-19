using LCVR;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Items.RadarBooster;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.RadarBooster
{
    internal class ChargingBooster : TierUpgrade
    {
        internal const string UPGRADE_NAME = "Charging Booster";
        internal static ChargingBooster Instance { get; private set; }
        internal float chargeCooldown;
        void Awake()
        {
            Instance = this;
        }
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            LategameConfiguration config = GetConfiguration();
            overridenUpgradeName = config.CHARGING_BOOSTER_OVERRIDE_NAME;
            chargeCooldown = config.CHARGING_BOOSTER_COOLDOWN.Value;
            base.Start();
        }
        public override void Load()
        {
            base.Load();
            RadarBoosterItem[] radarBoosters = FindObjectsOfType<RadarBoosterItem>();
            for (int i = 0; i < radarBoosters.Length; i++)
            {
                RadarBoosterItem radarBooster = radarBoosters[i];
                if (radarBooster.GetComponent<ChargingStationManager>() != null) continue;
                radarBooster.gameObject.AddComponent<ChargingStationManager>();
            }
        }
        public override void Increment()
        {
            base.Increment();
            LategameConfiguration config = GetConfiguration();
            chargeCooldown = Mathf.Clamp(chargeCooldown - config.CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE.Value, 0f, config.CHARGING_BOOSTER_COOLDOWN.Value);
        }
        [ServerRpc(RequireOwnership = false)]
        internal void UpdateCooldownServerRpc(NetworkBehaviourReference radarBooster)
        {
            UpdateCooldownClientRpc(radarBooster);
        }
        [ClientRpc]
        void UpdateCooldownClientRpc(NetworkBehaviourReference radarBooster)
        {
            radarBooster.TryGet(out RadarBoosterItem radar);
            if (radar == null) return;
            ChargingStationManager chargingStation = radar.GetComponent<ChargingStationManager>();
            if (chargingStation == null) return;
            chargingStation.cooldown = chargeCooldown;
        }
        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            static float infoFunction(int level)
            {
                LategameConfiguration config = GetConfiguration();
                return config.CHARGING_BOOSTER_COOLDOWN.Value - (level * config.CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Radar boosters will have a recharge cooldown of {2} seconds.\n";

            return $"LVL 1 - ${initialPrice} -  Provides charging stations to the radar boosters. After used, goes on cooldown for {GetConfiguration().CHARGING_BOOSTER_COOLDOWN.Value} seconds\n" + Tools.GenerateInfoForUpgrade(infoFormat, 0, incrementalPrices, infoFunction, skipFirst: true);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                LategameConfiguration config = GetConfiguration();
                string[] prices = config.CHARGING_BOOSTER_PRICES.Value.Split(',');
                return config.CHARGING_BOOSTER_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().CHARGING_BOOSTER_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<ChargingBooster>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                shareStatus: true,
                                                configuration.CHARGING_BOOSTER_ENABLED.Value,
                                                configuration.CHARGING_BOOSTER_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.CHARGING_BOOSTER_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.CHARGING_BOOSTER_OVERRIDE_NAME : "");
        }
    }
}
