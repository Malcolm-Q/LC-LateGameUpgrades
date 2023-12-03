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
    internal class beekeeperScript : BaseUpgrade
    {
        void Start()
        {
            StartCoroutine(lateApply());
        }

        private IEnumerator lateApply()
        {
            yield return new WaitForSeconds(1);
            UpgradeBus.instance.beekeeper = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Beekeeper is active!</color>";
            foreach(CustomTerminalNode node in UpgradeBus.instance.terminalNodes)
            {
                if(node.Name.ToLower() == "beekeeper" && node.Price > 0)
                {
                    node.Price /= 2;
                }
            }
            UpgradeBus.instance.UpgradeObjects.Add("Beekeeper", gameObject);
            DontDestroyOnLoad(gameObject);
            load();
        }

        public override void Increment()
        {
            UpgradeBus.instance.beeLevel++;
            foreach( CustomTerminalNode node in UpgradeBus.instance.terminalNodes )
            {
                if(node.Name.ToLower() == "beekeeper")
                {
                    node.Description = $"Circuit bees do %{Mathf.Round(100 * (UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (UpgradeBus.instance.beeLevel * UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT)))} of their base damage.";
                }
            }
        }
    }
}
