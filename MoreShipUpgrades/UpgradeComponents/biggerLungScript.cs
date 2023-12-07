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
            StartCoroutine(LateApply());
        }

        private IEnumerator LateApply()
        {
            yield return new WaitForSeconds(1f);
            DontDestroyOnLoad(gameObject);
            load();
        }

        public override void Increment()
        {
            UpgradeBus.instance.lungLevel++;
            players = GameObject.FindObjectsOfType<PlayerControllerB>();
            foreach (PlayerControllerB player in players)
            {
                player.sprintTime += UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT; //17
            }
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
            foreach (PlayerControllerB player in players)
            {
                player.sprintTime = UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE; //17
            }

            foreach(CustomTerminalNode node in UpgradeBus.instance.terminalNodes)
            {
                if(node.Name.ToLower() == "Bigger Lungs" && node.Price > 0)
                {
                    node.Price /= 2;
                }
            }
            UpgradeBus.instance.biggerLungs = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Bigger Lungs is active!</color>";
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Bigger Lungs"))
            {
                UpgradeBus.instance.UpgradeObjects.Add("Bigger Lungs", gameObject);
            }
            else
            {
                Plugin.mls.LogWarning("Bigger Lungs is already in upgrade dict.");
            }

            float amountToIncrement = 0;
            for(int i = 0; i < UpgradeBus.instance.lungLevel; i++)
            {
                amountToIncrement += UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT;
            }

            foreach(PlayerControllerB player in players)
            {
                player.sprintTime += amountToIncrement;
            }
        }
    }
}
