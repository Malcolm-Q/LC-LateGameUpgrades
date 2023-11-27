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
    internal class trapDestroyerScript : NetworkBehaviour
    {
        void Start()
        {
            StartCoroutine(lateApply());
        }

        private IEnumerator lateApply()
        {
            yield return new WaitForSeconds(1);
            UpgradeBus.instance.DestroyTraps = true;
            transform.parent = GameObject.Find("HangarShip").transform;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Malware Broadcaster is active!";
        }
    }
}
