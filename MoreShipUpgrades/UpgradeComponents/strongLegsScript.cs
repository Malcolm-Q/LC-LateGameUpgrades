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
    internal class strongLegsScript : BaseUpgrade
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.UpgradeObjects.Add("Strong Legs", gameObject);
        }

        public override void Increment()
        {
            UpgradeBus.instance.legLevel++;
            foreach( CustomTerminalNode node in UpgradeBus.instance.terminalNodes )
            {
                if(node.Name.ToLower() == "strong legs")
                {
                    node.Description = $"Can jump an additional {UpgradeBus.instance.cfg.JUMP_FORCE - 13 + (UpgradeBus.instance.legLevel * UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT)} units high.";
                    GameNetworkManager.Instance.localPlayerController.jumpForce += UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
                }
            }
        }

        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Strong Legs")) { UpgradeBus.instance.UpgradeObjects.Add("Strong Legs", gameObject); }
        }

        public override void load()
        {
            UpgradeBus.instance.strongLegs = true;
            GameNetworkManager.Instance.localPlayerController.jumpForce = UpgradeBus.instance.cfg.JUMP_FORCE;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Strong Legs is active!</color>";
            float amountToIncrement = 0;
            for(int i = 0; i < UpgradeBus.instance.legLevel; i++)
            {
                amountToIncrement += UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
            }

            GameNetworkManager.Instance.localPlayerController.jumpForce += amountToIncrement;
        }


    }
}
