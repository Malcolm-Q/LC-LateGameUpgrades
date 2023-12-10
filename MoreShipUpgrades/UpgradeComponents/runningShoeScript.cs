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
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.UpgradeObjects.Add("Running Shoes", gameObject);
        }

        public override void Increment()
        {
            GameNetworkManager.Instance.localPlayerController.movementSpeed += UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
            UpgradeBus.instance.runningLevel++;
            foreach( CustomTerminalNode node in UpgradeBus.instance.terminalNodes )
            {
                if(node.Name.ToLower() == "running shoes")
                {
                    node.Description = $"You can run {(UpgradeBus.instance.cfg.MOVEMENT_SPEED - 4.6f) + (UpgradeBus.instance.cfg.MOVEMENT_INCREMENT * UpgradeBus.instance.runningLevel)} units faster";
                }
            }
        }

        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Running Shoes")) { UpgradeBus.instance.UpgradeObjects.Add("Running Shoes", gameObject); }
        }

        public override void load()
        {
            UpgradeBus.instance.runningShoes = true;
            GameNetworkManager.Instance.localPlayerController.movementSpeed = UpgradeBus.instance.cfg.MOVEMENT_SPEED;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Running Shoes is active!</color>";
            float amountToIncrement = 0;
            for(int i = 0; i < UpgradeBus.instance.runningLevel; i++)
            {
                amountToIncrement += UpgradeBus.instance.cfg.MOVEMENT_INCREMENT;
            }

            GameNetworkManager.Instance.localPlayerController.movementSpeed += amountToIncrement;
        }
    }
}
