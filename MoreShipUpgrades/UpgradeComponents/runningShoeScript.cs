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

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class runningShoeScript : NetworkBehaviour
    {
        void Start()
        {
            StartCoroutine(LateApply());
        }

        private IEnumerator LateApply()
        {
            yield return new WaitForSeconds(1);
            UpgradeBus.instance.runningShoes = true;
            PlayerControllerB[] players = GameObject.FindObjectsOfType<PlayerControllerB>();
            foreach (PlayerControllerB player in players)
            {
                player.movementSpeed = Plugin.cfg.MOVEMENT_SPEED;
            }
            transform.parent = GameObject.Find("HangarShip").transform;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Running Shoes is active!</color>";
        }
    }
}
