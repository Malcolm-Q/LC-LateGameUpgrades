using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace MoreShipUpgrades.UpgradeComponents
{
    internal class nightVisionScript : BaseUpgrade
    {
        private float nightBattery;
        private Transform batteryBar;
        private PlayerControllerB client;
        private bool batteryExhaustion;
        private Key toggleKey;

        public static string UPGRADE_NAME = "NV Headset Batteries";
        public static string PRICES_DEFAULT = "300,400,500";

        private static LGULogger logger = new LGULogger(UPGRADE_NAME);
        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
            batteryBar = transform.GetChild(0).GetChild(0).transform;
            transform.GetChild(0).gameObject.SetActive(false);
            if(Enum.TryParse(UpgradeBus.instance.cfg.TOGGLE_NIGHT_VISION_KEY, out Key toggle))
            {
                toggleKey = toggle;
            }
            else
            {
                logger.LogWarning("Error parsing the key for toggle night vision, defaulted to LeftAlt");
                toggleKey = Key.LeftAlt;
            }
        }

        public override void Register()
        {
            base.Register();
        }

        void LateUpdate()
        {
            if (client == null) { return; }

            if (Keyboard.current[toggleKey].wasPressedThisFrame && !batteryExhaustion)
            {
                Toggle();
            }

            float maxBattery = UpgradeBus.instance.cfg.NIGHT_BATTERY_MAX + (UpgradeBus.instance.nightVisionLevel * UpgradeBus.instance.cfg.NIGHT_VIS_BATTERY_INCREMENT);

            if (UpgradeBus.instance.nightVisionActive)
            {
                nightBattery -= Time.deltaTime * (UpgradeBus.instance.cfg.NIGHT_VIS_DRAIN_SPEED - (UpgradeBus.instance.nightVisionLevel * UpgradeBus.instance.cfg.NIGHT_VIS_DRAIN_INCREMENT));
                nightBattery = Mathf.Clamp(nightBattery, 0f, maxBattery);
                batteryBar.parent.gameObject.SetActive(true);

                if (nightBattery <= 0f)
                {
                    TurnOff(true);
                }
            }
            else if (!batteryExhaustion)
            {
                nightBattery += Time.deltaTime * (UpgradeBus.instance.cfg.NIGHT_VIS_REGEN_SPEED + (UpgradeBus.instance.nightVisionLevel * UpgradeBus.instance.cfg.NIGHT_VIS_REGEN_INCREMENT));
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
            if(exhaust)
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
            client.nightVision.range = UpgradeBus.instance.cfg.NIGHT_VIS_RANGE + (UpgradeBus.instance.nightVisionLevel * UpgradeBus.instance.cfg.NIGHT_VIS_RANGE_INCREMENT);
            client.nightVision.intensity = UpgradeBus.instance.cfg.NIGHT_VIS_INTENSITY + (UpgradeBus.instance.nightVisionLevel * UpgradeBus.instance.cfg.NIGHT_VIS_INTENSITY_INCREMENT);
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

        public override void load()
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
        public void EnableOnClient(bool save = true)
        {
            if(client == null) { client = GameNetworkManager.Instance.localPlayerController; }
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
                        float drain = (UpgradeBus.instance.cfg.NIGHT_BATTERY_MAX - (UpgradeBus.instance.cfg.NIGHT_BATTERY_MAX * UpgradeBus.instance.cfg.NIGHT_VIS_STARTUP)) / UpgradeBus.instance.cfg.NIGHT_VIS_DRAIN_SPEED;
                        float regen = UpgradeBus.instance.cfg.NIGHT_BATTERY_MAX / UpgradeBus.instance.cfg.NIGHT_VIS_REGEN_SPEED;
                        return string.Format(AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME), level, price, drain, regen);
                    }
                default:
                    {
                        float regenAdjustment = Mathf.Clamp(UpgradeBus.instance.cfg.NIGHT_VIS_REGEN_SPEED + (UpgradeBus.instance.cfg.NIGHT_VIS_REGEN_INCREMENT * (level - 1)), 0, 1000);
                        float drainAdjustment = Mathf.Clamp(UpgradeBus.instance.cfg.NIGHT_VIS_DRAIN_SPEED - (UpgradeBus.instance.cfg.NIGHT_VIS_DRAIN_INCREMENT * (level - 1)), 0, 1000);
                        float batteryLife = UpgradeBus.instance.cfg.NIGHT_BATTERY_MAX + (UpgradeBus.instance.cfg.NIGHT_VIS_BATTERY_INCREMENT * (level - 1));

                        string drainTime = "infinite";
                        if (drainAdjustment != 0) drainTime = ((batteryLife - (batteryLife * UpgradeBus.instance.cfg.NIGHT_VIS_STARTUP)) / drainAdjustment).ToString("F2");

                        string regenTime = "infinite";
                        if (regenAdjustment != 0) regenTime = (batteryLife / regenAdjustment).ToString("F2");

                        return string.Format(AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME), level, price, drainTime, regenTime);
                    }
            }
        }
    }
}