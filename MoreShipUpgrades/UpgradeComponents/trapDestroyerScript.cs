using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    public class trapDestroyerScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Malware Broadcaster";
        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void load()
        {
            base.load();

            UpgradeBus.instance.DestroyTraps = true;
            UpgradeBus.instance.trapHandler = this;
        }

        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.DestroyTraps = false;
        }
        public override void Register()
        {
            base.Register();
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
