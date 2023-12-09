using BepInEx.Configuration;
using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
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

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.UpgradeObjects.Add("Night Vision", gameObject);
            batteryBar = transform.GetChild(0).GetChild(0).transform;
            transform.GetChild(0).gameObject.SetActive(false);
            if(Enum.TryParse(UpgradeBus.instance.cfg.TOGGLE_NIGHT_VISION_KEY, out Key toggle))
            {
                toggleKey = toggle;
            }
            else
            {
                toggleKey = Key.LeftAlt;
            }
        }

        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Night Vision")) { UpgradeBus.instance.UpgradeObjects.Add("Night Vision", gameObject); }
        }

        void LateUpdate()
        {
            if (client == null) { return; }

            // Check if the configured key is pressed
            if (Keyboard.current[toggleKey].wasPressedThisFrame && !batteryExhaustion)
            {
                UpgradeBus.instance.nightVisionActive = !UpgradeBus.instance.nightVisionActive;

                if (UpgradeBus.instance.nightVisionActive)
                {
                    UpgradeBus.instance.nightVisColor = client.nightVision.color;
                    UpgradeBus.instance.nightVisRange = client.nightVision.range;
                    UpgradeBus.instance.nightVisIntensity = client.nightVision.intensity;

                    client.nightVision.color = UpgradeBus.instance.cfg.NIGHT_VIS_COLOR;
                    client.nightVision.range = UpgradeBus.instance.cfg.NIGHT_VIS_RANGE;
                    client.nightVision.intensity = UpgradeBus.instance.cfg.NIGHT_VIS_INTENSITY;
                    nightBattery -= UpgradeBus.instance.cfg.NIGHT_VIS_STARTUP; // 0.1f
                }
                else
                {
                    client.nightVision.color = UpgradeBus.instance.nightVisColor;
                    client.nightVision.range = UpgradeBus.instance.nightVisRange;
                    client.nightVision.intensity = UpgradeBus.instance.nightVisIntensity;
                }
            }

            if (UpgradeBus.instance.nightVisionActive)
            {
                nightBattery -= Time.deltaTime * UpgradeBus.instance.cfg.NIGHT_VIS_DRAIN_SPEED; // 0.1f
                nightBattery = Mathf.Clamp(nightBattery, 0f, 1f);
                batteryBar.parent.gameObject.SetActive(true);

                if (nightBattery <= 0f)
                {
                    UpgradeBus.instance.nightVisionActive = false;
                    client.nightVision.color = UpgradeBus.instance.nightVisColor;
                    client.nightVision.range = UpgradeBus.instance.nightVisRange;
                    client.nightVision.intensity = UpgradeBus.instance.nightVisIntensity;
                    batteryExhaustion = true;
                    StartCoroutine(BatteryRecovery());
                }
            }
            else if (!batteryExhaustion)
            {
                nightBattery += Time.deltaTime * UpgradeBus.instance.cfg.NIGHT_VIS_REGEN_SPEED; // 0.05f
                nightBattery = Mathf.Clamp(nightBattery, 0f, 1f);

                if (nightBattery >= 1f)
                {
                    batteryBar.parent.gameObject.SetActive(false);
                }
                else
                {
                    batteryBar.parent.gameObject.SetActive(true);
                }
            }

            if (client.isInsideFactory || UpgradeBus.instance.nightVisionActive)
            {
                client.nightVision.enabled = true;
            }
            else
            {
                client.nightVision.enabled = false;
            }

            batteryBar.localScale = new Vector3(nightBattery, 1, 1);
        }

        private IEnumerator BatteryRecovery()
        {
            yield return new WaitForSeconds(UpgradeBus.instance.cfg.NIGHT_VIS_EXHAUST);
            batteryExhaustion = false;
        }

        public override void Increment()
        {
            // Increment the night vision level
            UpgradeBus.instance.nightVisionLevel++;

            // Apply adjustments based on the new level
            AdjustNightVisionProperties();
        }

        public override void load()
        {
            // Load adjustments based on the current level
            AdjustNightVisionProperties();
            EnableOnClient(false);
        }

        public void EnableOnClient(bool save = true)
        {
            if(client == null) { client = GameNetworkManager.Instance.localPlayerController; }
            if (save) { LGUStore.instance.UpdateLGUSaveServerRpc(client.playerSteamId, JsonConvert.SerializeObject(new SaveInfo())); }
            transform.GetChild(0).gameObject.SetActive(true);
            UpgradeBus.instance.nightVision = true;
            HUDManager.Instance.chatText.text += $"\n<color=#FF0000>Press {UpgradeBus.instance.cfg.TOGGLE_NIGHT_VISION_KEY} to toggle Night Vision!!!</color>";
        }

        public void DisableOnClient()
        {
            transform.GetChild(0).gameObject.SetActive(false);
            UpgradeBus.instance.nightVision = false;
            LGUStore.instance.UpdateLGUSaveServerRpc(client.playerSteamId, JsonConvert.SerializeObject(new SaveInfo()));
            client = null;
        }

        private void AdjustNightVisionProperties()
        {
            float regenAdjustment = Mathf.Pow(1 + UpgradeBus.instance.cfg.NIGHT_VIS_REGEN_INCREASE_PERCENT / 100f, UpgradeBus.instance.nightVisionLevel) * 0.01f;
            float drainAdjustment = Mathf.Pow(1 - UpgradeBus.instance.cfg.NIGHT_VIS_DRAIN_DECREASE_PERCENT / 100f, UpgradeBus.instance.nightVisionLevel) * 0.01f;

            UpgradeBus.instance.cfg.NIGHT_VIS_REGEN_SPEED += regenAdjustment;
            UpgradeBus.instance.cfg.NIGHT_VIS_DRAIN_SPEED -= drainAdjustment;
        }
    }
}