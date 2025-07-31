using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using Newtonsoft.Json;
using System.Collections;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreShipUpgrades.Input;
using UnityEngine.UI;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Upgrades.Custom;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player
{
    internal class NightVision : TierUpgrade, IPlayerSync, IUpgradeWorldBuilding
    {
        float nightBattery;
        PlayerControllerB client;
        public bool batteryExhaustion;
        internal GameObject nightVisionPrefab;
        internal bool nightVisionActive = false;
        internal float nightVisRange;
        internal float nightVisIntensity;
        internal Color nightVisColor;
        public static NightVision Instance { get; internal set; }

        public const string SIMPLE_UPGRADE_NAME = "Night Vision";
        public const string UPGRADE_NAME = "NV Headset Batteries";
        public const string PRICES_DEFAULT = "300,400,500";
        internal const string WORLD_BUILDING_TEXT = "\n\nService package for your crew's Night Vision Headset that optimizes the function of its capacitor," +
            " leading to improved uptime and shorter recharge period.\n\n";

        private static readonly LguLogger logger = new(UPGRADE_NAME);

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return WORLD_BUILDING_TEXT;
        }
        void Awake()
        {
            Instance = this;
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().NightVisionUpgradeConfiguration.OverrideName;
            nightVisionPrefab = AssetBundleHandler.GetItemObject(SIMPLE_UPGRADE_NAME).spawnPrefab;
        }
        internal override void Start()
        {
            base.Start();
            NightVisionUpgradeConfiguration config = GetConfiguration().NightVisionUpgradeConfiguration;
            transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Tools.ConvertValueToColor(config.BarColour.LocalValue, Color.green);
            transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Tools.ConvertValueToColor(config.TextColour.LocalValue, Color.white);
            transform.GetChild(0).GetChild(2).GetComponent<Image>().color = Tools.ConvertValueToColor(config.BarColour.LocalValue, Color.green);
            transform.GetChild(0).gameObject.SetActive(false);
        }

        void LateUpdate()
        {
            if (client == null) { return; }

            NightVisionUpgradeConfiguration config = GetConfiguration().NightVisionUpgradeConfiguration;
            float maxBattery = config.InitialEffects[0].Value + ((GetUpgradeLevel(UPGRADE_NAME) + 1) * config.IncrementalEffects[0].Value);

            if (nightVisionActive)
            {
                nightBattery -= Time.deltaTime * (config.InitialEffects[1].Value - ((GetUpgradeLevel(UPGRADE_NAME) + 1) * config.IncrementalEffects[1].Value));
                nightBattery = Mathf.Clamp(nightBattery, 0f, maxBattery);
                transform.GetChild(0).gameObject.SetActive(true);

                if (nightBattery <= 0f)
                {
                    TurnOff(true);
                }
            }
            else if (!batteryExhaustion)
            {
                nightBattery += Time.deltaTime * (config.InitialEffects[2].Value + ((GetUpgradeLevel(UPGRADE_NAME) + 1) * config.IncrementalEffects[2].Value));
                nightBattery = Mathf.Clamp(nightBattery, 0f, maxBattery);

                if (nightBattery >= maxBattery)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            // this ensures the vanilla behaviour for the night vision light remains
            client.nightVision.enabled = client.isInsideFactory || nightVisionActive;

            float scale = nightBattery / maxBattery;
            transform.GetChild(0).GetChild(0).localScale = new Vector3(scale, 1, 1);
        }

        public void Toggle()
        {
            if (!GetActiveUpgrade(SIMPLE_UPGRADE_NAME)) return;
            if (UpgradeBus.Instance.GetLocalPlayer().inTerminalMenu) return;
            nightVisionActive = !nightVisionActive;
            if (client == null) { client = GameNetworkManager.Instance.localPlayerController; }

            if (nightVisionActive)
            {
                TurnOn();
            }
            else
            {
                TurnOff();
            }
        }

        private void TurnOff(bool exhaust = false)
        {
            nightVisionActive = false;
            client.nightVision.color = nightVisColor;
            client.nightVision.range = nightVisRange;
            client.nightVision.intensity = nightVisIntensity;
            if (exhaust)
            {
                batteryExhaustion = true;
                StartCoroutine(BatteryRecovery());
            }
        }

        private void TurnOn()
        {
            nightVisColor = client.nightVision.color;
            nightVisRange = client.nightVision.range;
            nightVisIntensity = client.nightVision.intensity;

            NightVisionUpgradeConfiguration config = GetConfiguration().NightVisionUpgradeConfiguration;
            client.nightVision.color = Tools.ConvertValueToColor(config.LightColour.LocalValue, Color.green);
            client.nightVision.range = config.InitialEffects[3].Value + (GetUpgradeLevel(UPGRADE_NAME) * config.IncrementalEffects[3].Value);
            client.nightVision.intensity = config.InitialEffects[4].Value + (GetUpgradeLevel(UPGRADE_NAME) * config.IncrementalEffects[4].Value);
            nightBattery -= config.StartupPercentage.Value; // 0.1f
        }

        private IEnumerator BatteryRecovery()
        {
            yield return new WaitForSeconds(GetConfiguration().NightVisionUpgradeConfiguration.ExhaustTime.Value);
            batteryExhaustion = false;
        }

        public override void Load()
        {
            base.Load();
            if (!GetActiveUpgrade(SIMPLE_UPGRADE_NAME)) return;
            EnableOnClient();
        }
        public override void Unwind()
        {
            base.Unwind();
            if (!GetActiveUpgrade(SIMPLE_UPGRADE_NAME)) return;
            DisableOnClient();
        }

        [ServerRpc(RequireOwnership = false)]
        public void EnableNightVisionServerRpc()
        {
            logger.LogDebug("Enabling night vision for all clients...");
            EnableNightVisionClientRpc();
        }

        [ClientRpc]
        private void EnableNightVisionClientRpc()
        {
            logger.LogDebug("Request to enable night vision on this client received.");
            EnableOnClient();
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnNightVisionItemOnDeathServerRpc(Vector3 position)
        {
            GameObject go = Instantiate(nightVisionPrefab, position + Vector3.up, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
            logger.LogInfo("Request to spawn night vision goggles received.");
        }
        public void EnableOnClient()
        {
            if (client == null) { client = GameNetworkManager.Instance.localPlayerController; }
            transform.GetChild(0).gameObject.SetActive(true);
            UpgradeBus.Instance.activeUpgrades[SIMPLE_UPGRADE_NAME] = true;
            HUDManager.Instance.chatText.text += $"\n<color=#FF0000>Press {Keybinds.NvgAction.GetBindingDisplayString()} to toggle Night Vision!!!</color>";
        }

        public void DisableOnClient()
        {
            nightVisionActive = false;
            if (client != null && client.nightVision != null)
            {
                client.nightVision.color = nightVisColor;
                client.nightVision.range = nightVisRange;
                client.nightVision.intensity = nightVisIntensity;
            }

            transform.GetChild(0).gameObject.SetActive(false);
            UpgradeBus.Instance.activeUpgrades[SIMPLE_UPGRADE_NAME] = false;
            client = null;
        }

        public static string GetNightVisionInfo(int level, int price)
        {
            NightVisionUpgradeConfiguration config = GetConfiguration().NightVisionUpgradeConfiguration;
            float regenAdjustment = Mathf.Clamp(config.InitialEffects[1].Value + (config.IncrementalEffects[1].Value * level), 0, 1000);
            float drainAdjustment = Mathf.Clamp(config.InitialEffects[2].Value - (config.IncrementalEffects[2].Value * level), 0, 1000);
            float batteryLife = config.InitialEffects[0].Value + (config.IncrementalEffects[0].Value * level);

            string drainTime = "infinite";
            if (drainAdjustment != 0) drainTime = ((batteryLife - (batteryLife * config.StartupPercentage.Value)) / drainAdjustment).ToString("F2");

            string regenTime = "infinite";
            if (regenAdjustment != 0) regenTime = (batteryLife / regenAdjustment).ToString("F2");

            return string.Format(AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME), level, GetUpgradePrice(price, config.PurchaseMode), drainTime, regenTime);
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            StringBuilder stringBuilder = new();
            NightVisionUpgradeConfiguration config = GetConfiguration().NightVisionUpgradeConfiguration;
            float drain = (config.InitialEffects[0].Value - (config.IncrementalEffects[0].Value * config.StartupPercentage.Value)) / config.InitialEffects[2].Value;
            float regen = config.InitialEffects[0].Value / config.InitialEffects[1].Value;
            stringBuilder.Append($"The affected item (Night vision Googles) has a base drain time to empty of {drain} seconds and regeneration time to full of {regen} seconds.\n\n");
            stringBuilder.Append(GetNightVisionInfo(1, initialPrice));
            for (int i = 0; i < maxLevels; i++)
                stringBuilder.Append(GetNightVisionInfo(i + 2, incrementalPrices[i]));
            return stringBuilder.ToString();
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                NightVisionUpgradeConfiguration config = GetConfiguration().NightVisionUpgradeConfiguration;
                string[] prices = config.Prices.Value.Split(',');
                return config.ItemPrice.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }

        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().NightVisionUpgradeConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<NightVision>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            NightVisionUpgradeConfiguration configuration = GetConfiguration().NightVisionUpgradeConfiguration;

            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, configuration);
        }

        public void ResetPlayerAttribute()
        {
            if (client == null) return;
            DisableOnClient();
        }
    }
}