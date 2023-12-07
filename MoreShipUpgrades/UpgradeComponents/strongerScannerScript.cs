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
            DontDestroyOnLoad(gameObject);
            load();

        }

        public override void load()
        {
            UpgradeBus.instance.scannerUpgrade = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Better Scanner is active!</color>";
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Better Scanner"))
            {
                UpgradeBus.instance.UpgradeObjects.Add("Better Scanner", gameObject);
            }
            else
            {
                Plugin.mls.LogWarning("Better Scanner is already in upgrade dict.");
            }
        }
    }
}
