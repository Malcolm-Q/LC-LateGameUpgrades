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
    public class trapDestroyerScript : BaseUpgrade
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.UpgradeObjects.Add("Malware Broadcaster", gameObject);
        }

        public override void load()
        {
            UpgradeBus.instance.DestroyTraps = true;
            UpgradeBus.instance.trapHandler = this;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Malware Broadcaster is active!</color>";
        }

        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Malware Broadcaster")) { UpgradeBus.instance.UpgradeObjects.Add("Malware Broadcaster", gameObject); }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ReqDestroyObjectServerRpc(NetworkObjectReference go)
        {
            go.TryGet(out NetworkObject netObj);
            if (netObj == null)
            {
                HUDManager.Instance.AddTextToChatOnServer("Can't retrieve obj", 0);
                return;
            }
            if (netObj.gameObject.name == "Landmine(Clone)" || netObj.gameObject.name == "TurretContainer(Clone)")
            {
                if (UpgradeBus.instance.cfg.EXPLODE_TRAP) { SpawnExplosionClientRpc(netObj.gameObject.transform.position); }
                GameNetworkManager.Destroy(netObj.gameObject);
            }
        }

        [ClientRpc]
        private void SpawnExplosionClientRpc(Vector3 position)
        {
            if (UpgradeBus.instance.cfg.EXPLODE_TRAP) { Landmine.SpawnExplosion(position + Vector3.up, true, 5.7f, 6.4f); }
        }
    }
}
