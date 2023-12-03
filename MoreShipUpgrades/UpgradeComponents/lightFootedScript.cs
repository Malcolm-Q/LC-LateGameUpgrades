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
    internal class lightFootedScript : BaseUpgrade
    {
        void Start()
        {
            StartCoroutine(lateApply());
        }

        private IEnumerator lateApply()
        {
            yield return new WaitForSeconds(1);
            UpgradeBus.instance.softSteps = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Light Footed is active!</color>";
            foreach(CustomTerminalNode node in UpgradeBus.instance.terminalNodes)
            {
                if(node.Name.ToLower() == "light footed" && node.Price > 0)
                {
                    node.Price /= 2;
                }
            }
            UpgradeBus.instance.UpgradeObjects.Add("Light Footed", gameObject);
            DontDestroyOnLoad(gameObject);
            load();
        }

        public override void Increment()
        {
            UpgradeBus.instance.lightLevel++;
            foreach( CustomTerminalNode node in UpgradeBus.instance.terminalNodes )
            {
                if(node.Name.ToLower() == "light footed")
                {
                    node.Description = $"Audible Noise Distance is reduced by {UpgradeBus.instance.cfg.NOISE_REDUCTION + (UpgradeBus.instance.cfg.NOISE_REDUCTION_INCREMENT * UpgradeBus.instance.lightLevel)} units. \nApplies to both sprinting and walking.";
                }
            }

        }
    }
}
