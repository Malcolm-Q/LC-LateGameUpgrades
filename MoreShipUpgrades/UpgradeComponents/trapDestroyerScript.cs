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
    public class trapDestroyerScript : NetworkBehaviour
    {
        void Start()
        {
            StartCoroutine(lateApply());
        }

        private IEnumerator lateApply()
        {
            yield return new WaitForSeconds(1);
            UpgradeBus.instance.DestroyTraps = true;
            UpgradeBus.instance.trapHandler = this;
            transform.parent = GameObject.Find("HangarShip").transform;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Malware Broadcaster is active!";
        }

        [ServerRpc(RequireOwnership = false)]
        public void ReqDestroyObjectServerRpc(NetworkObjectReference go)
        {
            go.TryGet(out NetworkObject netObj);
            HUDManager.Instance.AddTextToChatOnServer("Enter", 0);
            if (netObj == null)
            {
                HUDManager.Instance.AddTextToChatOnServer("Can't retrieve obj", 0);
                return;
            }
            if (netObj.gameObject.name == "Landmine(Clone)" || netObj.gameObject.name == "TurretContainer(Clone)")
            {
                GameNetworkManager.Destroy(netObj.gameObject);
                HUDManager.Instance.AddTextToChatOnServer("Destroyed", 0);
            }
            else
            {
                HUDManager.Instance.AddTextToChatOnServer(netObj.gameObject.name, 0);
            }
        }
    }
}
