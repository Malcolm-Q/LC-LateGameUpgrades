using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    internal class NightVision : TierUpgrade
    {
        private float nightBattery;
        private Transform batteryBar;
        private PlayerControllerB client;
        private bool batteryExhaustion;
        private Key toggleKey;
        internal static NightVision instance;

        public static string UPGRADE_NAME = "NV Headset Batteries";
        public static string PRICES_DEFAULT = "300,400,500";

        private static LGULogger logger;
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(upgradeName);
            instance = this;
            base.Start();
            batteryBar = transform.GetChild(0).GetChild(0).transform;
            transform.GetChild(0).gameObject.SetActive(false);
            if (Enum.TryParse(UpgradeBus.instance.cfg.TOGGLE_NIGHT_VISION_KEY, out Key toggle))
            {
                toggleKey = toggle;
            }
            else
            {
                logger.LogWarning("Error parsing the key for toggle night vision, defaulted to LeftAlt");
                toggleKey = Key.LeftAlt;
            }
        }

        void LateUpdate()
        {
            if (client == null) { return; }

            if (Keyboard.current[toggleKey].wasPressedThisFrame && !batteryExhaustion)
            {
                Toggle();
            }

            float maxBattery = UpgradeBus.instance.cfg.NIGHT_BATTERY_MAX + UpgradeBus.instance.nightVisionLevel * UpgradeBus.instance.cfg.NIGHT_VIS_BATTERY_INCREMENT;

            if (UpgradeBus.instance.nightVisionActive)
            {
                nightBattery -= Time.deltaTime * (UpgradeBus.instance.cfg.NIGHT_VIS_DRAIN_SPEED - UpgradeBus.instance.nightVisionLevel * UpgradeBus.instance.cfg.NIGHT_VIS_DRAIN_INCREMENT);
                nightBattery = Mathf.Clamp(nightBattery, 0f, maxBattery);
                batteryBar.parent.gameObject.SetActive(true);

                if (nightBattery <= 0f)
                {
                    TurnOff(true);
                }
            }
            else if (!batteryExhaustion)
            {
                nightBattery += Time.deltaTime * (UpgradeBus.instance.cfg.NIGHT_VIS_REGEN_SPEED + UpgradeBus.instance.nightVisionLevel * UpgradeBus.instance.cfg.NIGHT_VIS_REGEN_INCREMENT);
                nightBattery = Mathf.Clamp(nightBattery, 0f, maxBattery);

                if (nightBattery >= maxBattery)
                {
                    batteryBar.parent.gameObject.SetActive(false);
                }
                else
                {
                    batteryBar.parent.gameObject.SetActive(true);
                }
            }
            // this ensures the vanilla behaviour for the night vision light remains
            if (client.isInsideFactory || UpgradeBus.instance.nightVisionActive) client.nightVision.enabled = true;
            else client.nightVision.enabled = false;

            float scale = nightBattery / maxBattery;
            batteryBar.localScale = new Vector3(scale, 1, 1);
        }

        private void Toggle()
        {
            UpgradeBus.instance.nightVisionActive = !UpgradeBus.instance.nightVisionActive;

            if (UpgradeBus.instance.nightVisionActive)
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
            UpgradeBus.instance.nightVisionActive = false;
            client.nightVision.color = UpgradeBus.instance.nightVisColor;
            client.nightVision.range = UpgradeBus.instance.nightVisRange;
            client.nightVision.intensity = UpgradeBus.instance.nightVisIntensity;
            if (exhaust)
            {
                batteryExhaustion = true;
                StartCoroutine(BatteryRecovery());
            }
        }

        private void TurnOn()
        {
            UpgradeBus.instance.nightVisColor = client.nightVision.color;
            UpgradeBus.instance.nightVisRange = client.nightVision.range;
            UpgradeBus.instance.nightVisIntensity = client.nightVision.intensity;

            client.nightVision.color = UpgradeBus.instance.cfg.NIGHT_VIS_COLOR;
            client.nightVision.range = UpgradeBus.instance.cfg.NIGHT_VIS_RANGE + UpgradeBus.instance.nightVisionLevel * UpgradeBus.instance.cfg.NIGHT_VIS_RANGE_INCREMENT;
            client.nightVision.intensity = UpgradeBus.instance.cfg.NIGHT_VIS_INTENSITY + UpgradeBus.instance.nightVisionLevel * UpgradeBus.instance.cfg.NIGHT_VIS_INTENSITY_INCREMENT;
            nightBattery -= UpgradeBus.instance.cfg.NIGHT_VIS_STARTUP; // 0.1f
        }

        private IEnumerator BatteryRecovery()
        {
            yield return new WaitForSeconds(UpgradeBus.instance.cfg.NIGHT_VIS_EXHAUST);
            batteryExhaustion = false;
        }

        public override void Increment()
        {
            UpgradeBus.instance.nightVisionLevel++;
            LGUStore.instance.UpdateLGUSaveServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, JsonConvert.SerializeObject(new SaveInfo()));
        }

        public override void Load()
        {
            EnableOnClient(false);
        }
        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.nightVision = false;
            UpgradeBus.instance.nightVisionLevel = 0;
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
            GameObject go = Instantiate(UpgradeBus.instance.nightVisionPrefab, position + Vector3.up, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
            logger.LogInfo("Request to spawn night vision goggles received.");
        }
        public void EnableOnClient(bool save = true)
        {
            if (client == null) { client = GameNetworkManager.Instance.localPlayerController; }
            transform.GetChild(0).gameObject.SetActive(true);
            UpgradeBus.instance.nightVision = true;
            if (save) { LGUStore.instance.UpdateLGUSaveServerRpc(client.playerSteamId, JsonConvert.SerializeObject(new SaveInfo())); }
            HUDManager.Instance.chatText.text += $"\n<color=#FF0000>Press {UpgradeBus.instance.cfg.TOGGLE_NIGHT_VISION_KEY} to toggle Night Vision!!!</color>";
        }

        public void DisableOnClient()
        {
            UpgradeBus.instance.nightVisionActive = false;
            client.nightVision.color = UpgradeBus.instance.nightVisColor;
            client.nightVision.range = UpgradeBus.instance.nightVisRange;
            client.nightVision.intensity = UpgradeBus.instance.nightVisIntensity;

            transform.GetChild(0).gameObject.SetActive(false);
            UpgradeBus.instance.nightVision = false;
            LGUStore.instance.UpdateLGUSaveServerRpc(client.playerSteamId, JsonConvert.SerializeObject(new SaveInfo()));
            client = null;
        }

        public static string GetNightVisionInfo(int level, int price)
        {
            switch (level)
            {
                case 1:
                    {
                        float drain = (UpgradeBus.instance.cfg.NIGHT_BATTERY_MAX - UpgradeBus.instance.cfg.NIGHT_BATTERY_MAX * UpgradeBus.instance.cfg.NIGHT_VIS_STARTUP) / UpgradeBus.instance.cfg.NIGHT_VIS_DRAIN_SPEED;
                        float regen = UpgradeBus.instance.cfg.NIGHT_BATTERY_MAX / UpgradeBus.instance.cfg.NIGHT_VIS_REGEN_SPEED;
                        return string.Format(AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME), level, price, drain, regen);
                    }
                default:
                    {
                        float regenAdjustment = Mathf.Clamp(UpgradeBus.instance.cfg.NIGHT_VIS_REGEN_SPEED + UpgradeBus.instance.cfg.NIGHT_VIS_REGEN_INCREMENT * (level - 1), 0, 1000);
                        float drainAdjustment = Mathf.Clamp(UpgradeBus.instance.cfg.NIGHT_VIS_DRAIN_SPEED - UpgradeBus.instance.cfg.NIGHT_VIS_DRAIN_INCREMENT * (level - 1), 0, 1000);
                        float batteryLife = UpgradeBus.instance.cfg.NIGHT_BATTERY_MAX + UpgradeBus.instance.cfg.NIGHT_VIS_BATTERY_INCREMENT * (level - 1);

                        string drainTime = "infinite";
                        if (drainAdjustment != 0) drainTime = ((batteryLife - batteryLife * UpgradeBus.instance.cfg.NIGHT_VIS_STARTUP) / drainAdjustment).ToString("F2");

                        string regenTime = "infinite";
                        if (regenAdjustment != 0) regenTime = (batteryLife / regenAdjustment).ToString("F2");

                        return string.Format(AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME), level, price, drainTime, regenTime);
                    }
            }
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            string info = GetNightVisionInfo(1, initialPrice);
            for (int i = 0; i < maxLevels; i++)
                info += GetNightVisionInfo(i + 2, incrementalPrices[i]);
            return info;
        }
    }
}