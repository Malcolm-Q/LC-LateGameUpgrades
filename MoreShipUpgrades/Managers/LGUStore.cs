using MoreShipUpgrades.Misc;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    public class LGUStore : NetworkBehaviour
    {
        public static LGUStore instance;

        private void Start()
        {
            instance = this;
        }

        [ServerRpc(RequireOwnership = false)]
        public void ReqSpawnServerRpc(string goName, bool increment = false)
        {
            if(!increment)
            {
                foreach(CustomTerminalNode customNode in UpgradeBus.instance.terminalNodes)
                {
                    if(customNode.Name == goName)
                    {
                        GameObject go = Instantiate(customNode.Prefab, Vector3.zero, Quaternion.identity);
                        go.GetComponent<NetworkObject>().Spawn();
                        UpdateNodesClientRpc(goName);
                        UpdateUpgradeBusClientRpc(goName, new NetworkObjectReference(go));
                    }
                }
            }
            else
            {
                UpdateNodesClientRpc(goName, increment);
            }
        }

        [ClientRpc]
        private void UpdateNodesClientRpc(string goName, bool increment = false)
        {
            foreach(CustomTerminalNode customNode in UpgradeBus.instance.terminalNodes)
            {
                if(customNode.Name == goName)
                {
                    customNode.Unlocked = true;
                    if(increment)
                    {
                        customNode.CurrentUpgrade++;
                        UpgradeBus.instance.UpgradeObjects[goName].GetComponent<BaseUpgrade>().Increment();
                    }
                }
            }
        }

        [ClientRpc]
        private void UpdateUpgradeBusClientRpc(string goName, NetworkObjectReference go)
        {
            go.TryGet(out NetworkObject networkObject);
            if(networkObject != null)
            {
                UpgradeBus.instance.UpgradeObjects.Add(goName, networkObject.gameObject);
            }
        }
    }
}
