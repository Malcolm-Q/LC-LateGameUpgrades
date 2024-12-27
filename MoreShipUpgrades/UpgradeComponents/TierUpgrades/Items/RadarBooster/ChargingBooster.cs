using CSync.Lib;
using MoreShipUpgrades.Configuration.Interfaces.TierUpgrades;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.UpgradeComponents.Items.RadarBooster;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.RadarBooster
{
    internal class ChargingBooster : TierUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Charging Booster";
        internal const string WORLD_BUILDING_TEXT = "Terminal hyperlink that leads to an old, decaying section of the Company Portal." +
            " The design of the page is bare and archaic, and all text is written in caps." +
            " There is a barely-labeled transaction window with your payment credentials autofilled. Clicking 'ACCEPT' causes a hissing noise to emanate from your nearby Radar Booster.";
        internal static ChargingBooster Instance { get; private set; }
        internal float chargeCooldown;

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            Instance = this;
        }
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            ITierMultipleEffectUpgradeConfiguration<float,int> config = GetConfiguration().ChargingBoosterConfiguration;
            overridenUpgradeName = config.OverrideName;
            chargeCooldown = config.GetEffectPair(0).Item1;
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
            ITierMultipleEffectUpgradeConfiguration<float, int> config = GetConfiguration().ChargingBoosterConfiguration;
            (SyncedEntry<float>, SyncedEntry<float>) cooldownPair = config.GetEffectPair(0);
            chargeCooldown = Mathf.Clamp(chargeCooldown - cooldownPair.Item2.Value, 0f, cooldownPair.Item1.Value);
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
                ITierMultipleEffectUpgradeConfiguration<float, int> config = GetConfiguration().ChargingBoosterConfiguration;
                (SyncedEntry<float>, SyncedEntry<float>) cooldownPair = config.GetEffectPair(0);
                return cooldownPair.Item1.Value - (level * config.GetEffectPair(0).Item2.Value);
            }
            const string infoFormat = "LVL {0} - ${1} - Radar boosters will have a recharge cooldown of {2} seconds.\n";

            ITierMultipleEffectUpgradeConfiguration<float, int> config = GetConfiguration().ChargingBoosterConfiguration;
            (SyncedEntry<float>, SyncedEntry<float>) cooldownPair = config.GetEffectPair(0);
            return $"LVL 1 - ${initialPrice} -  Provides charging stations to the radar boosters. After used, goes on cooldown for {cooldownPair.Item1.Value} seconds\n" + Tools.GenerateInfoForUpgrade(infoFormat, 0, incrementalPrices, infoFunction, skipFirst: true);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                ITierMultipleEffectUpgradeConfiguration<float, int> upgradeConfig = GetConfiguration().ChargingBoosterConfiguration;
                string[] prices = upgradeConfig.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().ChargingBoosterConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<ChargingBooster>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().ChargingBoosterConfiguration);
        }
    }
}
