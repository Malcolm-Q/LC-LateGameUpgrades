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
            StartCoroutine(lateApply());
        }

        private IEnumerator lateApply()
        {
            yield return new WaitForSeconds(1);
            DontDestroyOnLoad(gameObject);
            load();
        }

        public override void Increment()
        {
            UpgradeBus.instance.legLevel++;
            foreach( CustomTerminalNode node in UpgradeBus.instance.terminalNodes )
            {
                if(node.Name.ToLower() == "strong legs")
                {
                    node.Description = $"Can jump an additional {UpgradeBus.instance.cfg.JUMP_FORCE - 13 + (UpgradeBus.instance.legLevel * UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT)} units high.";
                    PlayerControllerB[] players = GameObject.FindObjectsOfType<PlayerControllerB>();
                    foreach (PlayerControllerB player in players)
                    {
                        player.jumpForce += UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
                    }
                }
            }
        }

        public override void load()
        {
            UpgradeBus.instance.strongLegs = true;
            PlayerControllerB[] players = GameObject.FindObjectsOfType<PlayerControllerB>();
            foreach (PlayerControllerB player in players)
            {
                player.jumpForce = UpgradeBus.instance.cfg.JUMP_FORCE;
            }
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Strong Legs is active!</color>";
            foreach(CustomTerminalNode node in UpgradeBus.instance.terminalNodes)
            {
                if(node.Name.ToLower() == "strong legs" && node.Price > 0)
                {
                    node.Price /= 2;
                }
            }
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Strong Legs"))
            {
                UpgradeBus.instance.UpgradeObjects.Add("Strong Legs", gameObject);
            }
            else
            {
                Plugin.mls.LogWarning("Strong Legs is already in upgrade dict.");
            }
            float amountToIncrement = 0;
            for(int i = 0; i < UpgradeBus.instance.legLevel; i++)
            {
                amountToIncrement += UpgradeBus.instance.cfg.JUMP_FORCE_INCREMENT;
            }

            foreach(PlayerControllerB player in players)
            {
                player.jumpForce += amountToIncrement;
            }
        }


    }
}
