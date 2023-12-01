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
    internal class strongerScannerScript : BaseUpgrade
    {

        void Start()
        {
            StartCoroutine(lateApply());
        }

        private IEnumerator lateApply()
        {
            yield return new WaitForSeconds(1);
            UpgradeBus.instance.scannerUpgrade = true;
            transform.parent = GameObject.Find("HangarShip").transform;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Better Scanner is active!</color>";
        }

        public override void Increment()
        {
            //UpgradeBus.instance.scanLevel++;
            foreach( CustomTerminalNode node in UpgradeBus.instance.terminalNodes )
            {
                if(node.Name.ToLower() == "better scanner")
                {
                    node.Description = $"Can scan the ship from an additional {UpgradeBus.instance.cfg.SHIP_AND_ENTRANCE_DISTANCE_INCREASE} units away.  \nCan scan all other nodes from an additional {UpgradeBus.instance.cfg.NODE_DISTANCE_INCREASE} units away.";
                    if (!UpgradeBus.instance.cfg.REQUIRE_LINE_OF_SIGHT) { node.Description += "  \nDoes not require Line of Sight!"; }
                }
            }

        }

    }
}
