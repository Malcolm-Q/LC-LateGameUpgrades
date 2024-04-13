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
using CSync.Lib;
using MoreShipUpgrades.Misc.Util;


namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class NightVision : TierUpgrade
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

        public const string UPGRADE_NAME = "NV Headset Batteries";
        public const string PRICES_DEFAULT = "300,400,500";

        private static LguLogger logger = new LguLogger(UPGRADE_NAME);
        void Awake()
        {
            Instance = this;
            upgradeName = UpgradeBus.Instance.PluginConfiguration.OVERRIDE_UPGRADE_NAMES ? UpgradeBus.Instance.PluginConfiguration.NIGHT_VISION_OVERRIDE_NAME : UPGRADE_NAME;
            nightVisionPrefab = AssetBundleHandler.GetItemObject("Night Vision").spawnPrefab;
        }
        internal override void Start()
        {
            base.Start();
            transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Tools.ConvertValueToColor(UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_UI_BAR_COLOR.LocalValue, Color.green);
            transform.GetChild(0).GetChild(1).GetComponent<Text>().color = Tools.ConvertValueToColor(UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_UI_TEXT_COLOR.LocalValue, Color.white);
            transform.GetChild(0).GetChild(2).GetComponent<Image>().color = Tools.ConvertValueToColor(UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_UI_BAR_COLOR.LocalValue, Color.green);
            transform.GetChild(0).gameObject.SetActive(false);
        }

        void LateUpdate()
        {
            if (client == null) { return; }

            float maxBattery = UpgradeBus.Instance.PluginConfiguration.NIGHT_BATTERY_MAX.Value + GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_BATTERY_INCREMENT.Value;

            if (nightVisionActive)
            {
                nightBattery -= Time.deltaTime * (UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_DRAIN_SPEED.Value - GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_DRAIN_INCREMENT.Value);
                nightBattery = Mathf.Clamp(nightBattery, 0f, maxBattery);
                transform.GetChild(0).gameObject.SetActive(true);

                if (nightBattery <= 0f)
                {
                    TurnOff(true);
                }
            }
            else if (!batteryExhaustion)
            {
                nightBattery += Time.deltaTime * (UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_REGEN_SPEED.Value + GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_REGEN_INCREMENT.Value);
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
            if (client.isInsideFactory || nightVisionActive) client.nightVision.enabled = true;
            else client.nightVision.enabled = false;

            float scale = nightBattery / maxBattery;
            transform.GetChild(0).GetChild(0).localScale = new Vector3(scale, 1, 1);
        }

        public void Toggle()
        {
            if (!GetActiveUpgrade(UPGRADE_NAME)) return;
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

            client.nightVision.color = Tools.ConvertValueToColor(UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_COLOR.LocalValue, Color.green);
            client.nightVision.range = UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_RANGE.Value + GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_RANGE_INCREMENT.Value;
            client.nightVision.intensity = UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_INTENSITY.Value + GetUpgradeLevel(UPGRADE_NAME) * UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_INTENSITY_INCREMENT.Value;
            nightBattery -= UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_STARTUP.Value; // 0.1f
        }

        private IEnumerator BatteryRecovery()
        {
            yield return new WaitForSeconds(UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_EXHAUST.Value);
            batteryExhaustion = false;
        }

        public override void Increment()
        {
            base.Increment();
            LguStore.Instance.UpdateLGUSaveServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, JsonConvert.SerializeObject(new SaveInfo()));
        }

        public override void Load()
        {
            base.Load();
            EnableOnClient(false);
        }
        public override void Unwind()
        {
            base.Unwind();
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
        public void EnableOnClient(bool save = true)
        {
            if (client == null) { client = GameNetworkManager.Instance.localPlayerController; }
            transform.GetChild(0).gameObject.SetActive(true);
            UpgradeBus.Instance.activeUpgrades[UPGRADE_NAME] = true;
            if (save) { LguStore.Instance.UpdateLGUSaveServerRpc(client.playerSteamId, JsonConvert.SerializeObject(new SaveInfo())); }
            HUDManager.Instance.chatText.text += $"\n<color=#FF0000>Press {Keybinds.NvgAction.GetBindingDisplayString()} to toggle Night Vision!!!</color>";
        }

        public void DisableOnClient()
        {
            nightVisionActive = false;
            client.nightVision.color = nightVisColor;
            client.nightVision.range = nightVisRange;
            client.nightVision.intensity = nightVisIntensity;

            transform.GetChild(0).gameObject.SetActive(false);
            UpgradeBus.Instance.activeUpgrades[UPGRADE_NAME] = false;
            LguStore.Instance.UpdateLGUSaveServerRpc(client.playerSteamId, JsonConvert.SerializeObject(new SaveInfo()));
            client = null;
        }

        public static string GetNightVisionInfo(int level, int price)
        {
            switch (level)
            {
                case 1:
                    {
                        float drain = (UpgradeBus.Instance.PluginConfiguration.NIGHT_BATTERY_MAX.Value - UpgradeBus.Instance.PluginConfiguration.NIGHT_BATTERY_MAX.Value * UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_STARTUP.Value) / UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_DRAIN_SPEED.Value;
                        float regen = UpgradeBus.Instance.PluginConfiguration.NIGHT_BATTERY_MAX.Value / UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_REGEN_SPEED.Value;
                        return string.Format(AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME), level, price, drain, regen);
                    }
                default:
                    {
                        float regenAdjustment = Mathf.Clamp(UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_REGEN_SPEED.Value + UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_REGEN_INCREMENT.Value * (level - 1), 0, 1000);
                        float drainAdjustment = Mathf.Clamp(UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_DRAIN_SPEED.Value - UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_DRAIN_INCREMENT.Value * (level - 1), 0, 1000);
                        float batteryLife = UpgradeBus.Instance.PluginConfiguration.NIGHT_BATTERY_MAX.Value + UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_BATTERY_INCREMENT.Value * (level - 1);

                        string drainTime = "infinite";
                        if (drainAdjustment != 0) drainTime = ((batteryLife - batteryLife * UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_STARTUP.Value) / drainAdjustment).ToString("F2");

                        string regenTime = "infinite";
                        if (regenAdjustment != 0) regenTime = (batteryLife / regenAdjustment).ToString("F2");

                        return string.Format(AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME), level, price, drainTime, regenTime);
                    }
            }
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append(GetNightVisionInfo(1, initialPrice));
            for (int i = 0; i < maxLevels; i++)
                stringBuilder.Append(GetNightVisionInfo(i + 2, incrementalPrices[i]));
            return stringBuilder.ToString();
        }
        internal override bool CanInitializeOnStart()
        {
            string[] prices = UpgradeBus.Instance.PluginConfiguration.NIGHT_VISION_UPGRADE_PRICES.Value.Split(',');
            bool free = UpgradeBus.Instance.PluginConfiguration.NIGHT_VISION_PRICE.Value <= 0 && prices.Length == 1 && (prices[0] == "" || prices[0] == "0");
            return free;
        }
    }
}