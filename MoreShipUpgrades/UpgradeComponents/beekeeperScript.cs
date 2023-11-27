using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
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
    internal class beekeeperScript : NetworkBehaviour
    {
        void Start()
        {
            StartCoroutine(lateApply());
        }

        private IEnumerator lateApply()
        {
            yield return new WaitForSeconds(1);
            UpgradeBus.instance.beekeeper = true;
            transform.parent = GameObject.Find("HangarShip").transform;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Beekeeper is active!";
        }
    }
}
