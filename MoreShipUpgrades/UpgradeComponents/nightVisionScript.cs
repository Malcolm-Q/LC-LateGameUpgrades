using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class nightVisionScript : NetworkBehaviour
    {
        private float nightBattery;
        private Transform batteryBar;
        private Light nightVis;
        void Start()
        {
            StartCoroutine(lateApply());
            batteryBar = transform.GetChild(0).GetChild(0).transform;
            batteryBar.gameObject.SetActive(true);
            nightVis = GameNetworkManager.Instance.localPlayerController.nightVision;
        }

        private IEnumerator lateApply()
        {
            yield return new WaitForSeconds(1);
            UpgradeBus.instance.nightVision = true;
            transform.parent = GameObject.Find("HangarShip").transform;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Press Left-Alt to toggle Night Vision!!!</color>";
        }

        void Update()
        {
            if (Keyboard.current[Key.LeftAlt].wasPressedThisFrame)
            {
                UpgradeBus.instance.nightVisionActive = !UpgradeBus.instance.nightVisionActive;
                if (UpgradeBus.instance.nightVisionActive)
                {
                    UpgradeBus.instance.nightVisColor = nightVis.color;
                    UpgradeBus.instance.nightVisRange = nightVis.range;
                    UpgradeBus.instance.nightVisIntensity = nightVis.intensity;

                    //nightVis.color = Color.green;
                    //nightVis.range = 3000f;
                    //nightVis.intensity = 1500f;
                    nightVis.color = Plugin.cfg.NIGHT_VIS_COLOR;
                    nightVis.range = Plugin.cfg.NIGHT_VIS_RANGE;
                    nightVis.intensity = Plugin.cfg.NIGHT_VIS_INTENSITY;
                }
                else
                {
                    nightVis.color = UpgradeBus.instance.nightVisColor;
                    nightVis.range = UpgradeBus.instance.nightVisRange;
                    nightVis.intensity = UpgradeBus.instance.nightVisIntensity;
                }

            }
            if(UpgradeBus.instance.nightVisionActive)
            {
                nightBattery -= Time.deltaTime * Plugin.cfg.NIGHT_VIS_DRAIN_SPEED; //0.1f
                nightBattery = Mathf.Clamp(nightBattery, 0f, 1f);
                batteryBar.parent.gameObject.SetActive(true);
                if(nightBattery <= 0f)
                {
                    UpgradeBus.instance.nightVisionActive = false;
                    nightVis.color = UpgradeBus.instance.nightVisColor;
                    nightVis.range = UpgradeBus.instance.nightVisRange;
                    nightVis.intensity = UpgradeBus.instance.nightVisIntensity;
                }
            }
            else
            {
                nightBattery += Time.deltaTime * Plugin.cfg.NIGHT_VIS_REGEN_SPEED; //0.05f
                nightBattery = Mathf.Clamp(nightBattery, 0f, 1f);
                if(nightBattery >= 1f)
                {
                    batteryBar.parent.gameObject.SetActive(false);
                }
                else
                {
                    batteryBar.parent.gameObject.SetActive(true);
                }
            }
            batteryBar.localScale = new Vector3(nightBattery, 1, 1);
        }
    }
}
