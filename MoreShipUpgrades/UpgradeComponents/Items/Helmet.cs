using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items
{
    public class Helmet : GrabbableObject, IDisplayInfo
    {
        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (buttonDown)
            {
                if (UpgradeBus.Instance.wearingHelmet) return;
                UpgradeBus.Instance.helmetScript = this;
                UpgradeBus.Instance.wearingHelmet = true;
                UpgradeBus.Instance.helmetHits = UpgradeBus.Instance.PluginConfiguration.HELMET_HITS_BLOCKED.Value;
                if (IsHost) LguStore.Instance.SpawnAndMoveHelmetClientRpc(new NetworkObjectReference(playerHeldBy.GetComponent<NetworkObject>()), playerHeldBy.playerSteamId);
                else LguStore.Instance.ReqSpawnAndMoveHelmetServerRpc(new NetworkObjectReference(playerHeldBy.GetComponent<NetworkObject>()), playerHeldBy.playerSteamId);
                playerHeldBy.DespawnHeldObject();
            }
        }

        public string GetDisplayInfo()
        {
            return string.Format(AssetBundleHandler.GetInfoFromJSON("Helmet"), UpgradeBus.Instance.PluginConfiguration.HELMET_HITS_BLOCKED);
        }
    }
}
