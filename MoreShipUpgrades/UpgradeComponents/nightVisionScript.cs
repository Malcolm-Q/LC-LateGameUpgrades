using BepInEx.Configuration;
using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
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

        void Start()
        {
            StartCoroutine(lateApply());
            batteryBar = transform.GetChild(0).GetChild(0).transform;
        }

        private IEnumerator lateApply()
        {
            yield return new WaitForSeconds(1);
            UpgradeBus.instance.nightVision = true;
            HUDManager.Instance.chatText.text += $"\n<color=#FF0000>Press {UpgradeBus.instance.cfg.TOGGLE_NIGHT_VISION_KEY} to toggle Night Vision!!!</color>";
            client = GameNetworkManager.Instance.localPlayerController;
            UpgradeBus.instance.UpgradeObjects.Add("Night Vision", gameObject);
            DontDestroyOnLoad(gameObject);
            load();
        }

        void LateUpdate()
        {
            if (client == null) { return; }

            // Check if the configured key is pressed
            if (Enum.TryParse(UpgradeBus.instance.cfg.TOGGLE_NIGHT_VISION_KEY, out Key toggleKey) && Keyboard.current[toggleKey].wasPressedThisFrame && !batteryExhaustion)
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
                    nightBattery -= UpgradeBus.instance.cfg.NIGHT_VIS_STARTUP; //0.1f
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
                nightBattery -= Time.deltaTime * UpgradeBus.instance.cfg.NIGHT_VIS_DRAIN_SPEED; //0.1f
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
                nightBattery += Time.deltaTime * UpgradeBus.instance.cfg.NIGHT_VIS_REGEN_SPEED; //0.05f
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
    }
}