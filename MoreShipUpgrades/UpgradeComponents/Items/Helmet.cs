using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items
{
    public class Helmet : GrabbableObject, IDisplayInfo
    {
        public AudioClip hit;
        private AudioSource audio;

        public override void Start()
        {
            audio = GetComponent<AudioSource>();
            base.Start();
        }
        public override void DiscardItem()
        {
            base.DiscardItem();
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (buttonDown)
            {
                if (UpgradeBus.instance.wearingHelmet) return;
                UpgradeBus.instance.helmetScript = this;
                UpgradeBus.instance.wearingHelmet = true;
                UpgradeBus.instance.helmetHits = UpgradeBus.instance.cfg.HELMET_HITS_BLOCKED;
                if (IsHost) LGUStore.instance.SpawnAndMoveHelmetClientRpc(new NetworkObjectReference(playerHeldBy.GetComponent<NetworkObject>()), playerHeldBy.playerSteamId);
                else LGUStore.instance.ReqSpawnAndMoveHelmetServerRpc(new NetworkObjectReference(playerHeldBy.GetComponent<NetworkObject>()), playerHeldBy.playerSteamId);
                playerHeldBy.DespawnHeldObject();
            }
        }

        public string GetDisplayInfo()
        {
            return string.Format(AssetBundleHandler.GetInfoFromJSON("Helmet"), UpgradeBus.instance.cfg.HELMET_HITS_BLOCKED);
        }
    }
}
