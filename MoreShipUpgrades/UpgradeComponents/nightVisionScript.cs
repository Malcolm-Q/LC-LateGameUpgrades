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
        void Start()
        {
            StartCoroutine(lateApply());
        }

        private IEnumerator lateApply()
        {
            yield return new WaitForSeconds(1);
            UpgradeBus.instance.nightVision = true;
            transform.parent = GameObject.Find("HangarShip").transform;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Press Left-Alt to toggle Night Vision!!!";
        }

        void Update()
        {
            if (Keyboard.current[Key.LeftAlt].wasPressedThisFrame)
            {
                UpgradeBus.instance.nightVisionActive = !UpgradeBus.instance.nightVisionActive;
                Light nightVis = GameNetworkManager.Instance.localPlayerController.nightVision;
                if (UpgradeBus.instance.nightVisionActive)
                {
                    UpgradeBus.instance.nightVisColor = nightVis.color;
                    UpgradeBus.instance.nightVisRange = nightVis.range;
                    UpgradeBus.instance.nightVisIntensity = nightVis.intensity;
                    nightVis.color = Color.green;
                    nightVis.range = 3000f;
                    nightVis.intensity = 1500f;
                }
                else
                {
                    nightVis.color = UpgradeBus.instance.nightVisColor;
                    nightVis.range = UpgradeBus.instance.nightVisRange;
                    nightVis.intensity = UpgradeBus.instance.nightVisIntensity;
                }

            }
        }
    }
}
