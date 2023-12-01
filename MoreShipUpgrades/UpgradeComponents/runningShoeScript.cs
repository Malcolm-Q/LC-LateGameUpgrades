using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
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
    internal class runningShoeScript : BaseUpgrade
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
                player.movementSpeed = UpgradeBus.instance.cfg.MOVEMENT_SPEED;
            }
            transform.parent = GameObject.Find("HangarShip").transform;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Running Shoes is active!</color>";
            foreach(CustomTerminalNode node in UpgradeBus.instance.terminalNodes)
            {
                if(node.Name.ToLower() == "running shoes" && node.Price > 0)
                {
                    node.Price /= 2;
                }
            }
        }

        public override void Increment()
        {
            UpgradeBus.instance.runningLevel++;
            foreach( CustomTerminalNode node in UpgradeBus.instance.terminalNodes )
            {
                if(node.Name.ToLower() == "running shoes")
                {
                    node.Description = $"You can run {UpgradeBus.instance.cfg.MOVEMENT_SPEED - 4.6f + (UpgradeBus.instance.cfg.MOVEMENT_INCREMENT * UpgradeBus.instance.runningLevel)} units faster";
                }
            }

        }

    }
}
