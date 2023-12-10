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
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.UpgradeObjects.Add("Bigger Lungs", gameObject);
        }

        public override void Increment()
        {
            UpgradeBus.instance.lungLevel++;
            players = GameObject.FindObjectsOfType<PlayerControllerB>();
            GameNetworkManager.Instance.localPlayerController.sprintTime += UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT;  //17
            foreach( CustomTerminalNode node in UpgradeBus.instance.terminalNodes )
            {
                if(node.Name.ToLower() == "bigger lungs")
                {
                    node.Description = $"Stamina Time is {(UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE - 11) + (UpgradeBus.instance.lungLevel * UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT)} units longer";
                }
            }

        }

        public override void load()
        {
            players = GameObject.FindObjectsOfType<PlayerControllerB>();
            GameNetworkManager.Instance.localPlayerController.sprintTime = UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE; //17
            UpgradeBus.instance.biggerLungs = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Bigger Lungs is active!</color>";

            float amountToIncrement = 0;
            for(int i = 0; i < UpgradeBus.instance.lungLevel; i++)
            {
                amountToIncrement += UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT;
            }

            GameNetworkManager.Instance.localPlayerController.sprintTime += amountToIncrement;
        }

        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Bigger Lungs")) { UpgradeBus.instance.UpgradeObjects.Add("Bigger Lungs", gameObject); }
        }
    }
}
