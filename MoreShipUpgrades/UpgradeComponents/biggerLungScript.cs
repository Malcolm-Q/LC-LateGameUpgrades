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
    internal class biggerLungScript : BaseUpgrade
    {
        private PlayerControllerB[] players;

        void Start()
        {
            players = GameObject.FindObjectsOfType<PlayerControllerB>();
            foreach (PlayerControllerB player in players)
            {
                player.sprintTime = UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE; //17
            }
            foreach(CustomTerminalNode node in UpgradeBus.instance.terminalNodes)
            {
                if(node.Name.ToLower() == "bigger lungs" && node.Price > 0)
                {
                    node.Price /= 2;
                }
            }
            UpgradeBus.instance.biggerLungs = true;
            transform.parent = GameObject.Find("HangarShip").transform;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Bigger Lungs is active!</color>";
        }

        public override void Increment()
        {
            UpgradeBus.instance.lungLevel++;
            foreach( CustomTerminalNode node in UpgradeBus.instance.terminalNodes )
            {
                if(node.Name.ToLower() == "bigger lungs")
                {
                    node.Description = $"Stamina Time is {UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE - 11 + UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT} units longer";
                }
            }

        }
    }
}
