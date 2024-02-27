using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Items.RadarBooster;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
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
            chargeCooldown = UpgradeBus.Instance.PluginConfiguration.CHARGING_BOOSTER_COOLDOWN.Value;
            base.Start();
        }
        public override void Load()
        {
            base.Load();
            RadarBoosterItem[] radarBoosters = FindObjectsOfType<RadarBoosterItem>();
            for(int i = 0; i < radarBoosters.Length; i++)
            {
                RadarBoosterItem radarBooster = radarBoosters[i];
                if (radarBooster.GetComponent<ChargingStationManager>() != null) continue;
                radarBooster.gameObject.AddComponent<ChargingStationManager>();
            }
        }
        public override void Increment()
        {
            base.Increment();
            chargeCooldown = Mathf.Clamp(chargeCooldown - UpgradeBus.Instance.PluginConfiguration.CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE.Value, 0f, UpgradeBus.Instance.PluginConfiguration.CHARGING_BOOSTER_COOLDOWN.Value);
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
            return "Provides charging stations to the radar boosters.";
        }
    }
}
