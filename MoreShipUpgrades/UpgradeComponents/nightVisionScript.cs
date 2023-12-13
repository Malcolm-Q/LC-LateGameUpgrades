using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using Newtonsoft.Json;
using System;
using System.Collections;
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
            UpgradeBus.instance.UpgradeObjects.Add("NV Headset Batteries", gameObject);
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
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("NV Headset Batteries")) { UpgradeBus.instance.UpgradeObjects.Add("NV Headset Batteries", gameObject); }
        }

        void LateUpdate()
        {
            if (client == null) { return; }

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
            UpgradeBus.instance.nightVisionLevel++;
            LGUStore.instance.UpdateLGUSaveServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, JsonConvert.SerializeObject(new SaveInfo()));

            AdjustNightVisionProperties();
        }

        public override void load()
        {
            AdjustNightVisionProperties();
            EnableOnClient(false);
        }
        public override void Unwind()
        {
            UpgradeBus.instance.nightVision = false;
            UpgradeBus.instance.nightVisionLevel = 0;
            AdjustNightVisionProperties();
            DisableOnClient();
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>NV Headset Batteries has been disabled.</color>";
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