using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Patches.PlayerController;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items
{
    public class Helmet : GrabbableObject, IDisplayInfo
    {
        internal enum DamageMitigationMode
        {
            TotalPerHit,
            PartialTilLowHealth,
        }
        static readonly LguLogger logger = new LguLogger(nameof(Helmet));
        internal const string ITEM_NAME = "Helmet";
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
            return string.Format(AssetBundleHandler.GetInfoFromJSON("Helmet"), UpgradeBus.Instance.PluginConfiguration.HELMET_HITS_BLOCKED.Value);
        }

        internal static void ExecuteHelmetDamageMitigation(ref PlayerControllerB player, ref int damageNumber)
        {
            DamageMitigationMode pickedMode = DamageMitigationMode.TotalPerHit;
            Enum.TryParse(typeof(DamageMitigationMode), UpgradeBus.Instance.PluginConfiguration.HELMET_DAMAGE_MITIGATION_MODE.Value, out object parsedRestriction);
            if (parsedRestriction == null)
                logger.LogError($"An error occured parsing the damage mitigation mode ({UpgradeBus.Instance.PluginConfiguration.HELMET_DAMAGE_MITIGATION_MODE.Value}), defaulting to TotalPerHit");
            else 
                pickedMode = (DamageMitigationMode)parsedRestriction;
            switch(pickedMode)
            {
                case DamageMitigationMode.TotalPerHit: ExecuteHelmetCompleteMitigation(ref player, ref damageNumber); break;
                case DamageMitigationMode.PartialTilLowHealth: ExecuteHelmetPartialMitigation(ref player, ref damageNumber); break;
            }
        }

        internal static void ExecuteHelmetCompleteMitigation(ref PlayerControllerB player, ref int damageNumber)
        {
            logger.LogDebug($"Player {player.playerUsername} is wearing a helmet, executing helmet logic...");
            UpgradeBus.Instance.helmetHits--;
            if (UpgradeBus.Instance.helmetHits <= 0)
            {
                logger.LogDebug("Helmet has ran out of durability, breaking the helmet...");
                BreakHelmet(ref player);
            }
            else
            {
                logger.LogDebug($"Helmet still has some durability ({UpgradeBus.Instance.helmetHits}), decreasing it...");
                HitHelmet(ref player);
            }
            damageNumber = 0;
        }

        internal static void ExecuteHelmetPartialMitigation(ref PlayerControllerB player, ref int damageNumber)
        {
            int health = player.health;
            int updatedHealth = health - Mathf.CeilToInt(damageNumber * Mathf.Clamp((100f - UpgradeBus.Instance.PluginConfiguration.HELMET_DAMAGE_REDUCTION) / 100f, 0f, damageNumber));
            if (updatedHealth > 0)
            {
                HitHelmet(ref player);
                damageNumber = Mathf.CeilToInt(damageNumber * ((100f - UpgradeBus.Instance.PluginConfiguration.HELMET_DAMAGE_REDUCTION) / 100f));
            }
            else
            {
                BreakHelmet(ref player);
                damageNumber = 0;
            }
        }
        internal static void HitHelmet(ref PlayerControllerB player)
        {
            if (player.IsHost || player.IsServer) LguStore.Instance.PlayAudioOnPlayerClientRpc(new NetworkBehaviourReference(player), "helmet");
            else LguStore.Instance.ReqPlayAudioOnPlayerServerRpc(new NetworkBehaviourReference(player), "helmet");
        }
        internal static void BreakHelmet(ref PlayerControllerB player)
        {
            UpgradeBus.Instance.wearingHelmet = false;
            if (player.IsHost || player.IsServer) LguStore.Instance.DestroyHelmetClientRpc(player.playerClientId);
            else LguStore.Instance.ReqDestroyHelmetServerRpc(player.playerClientId);
            if (player.IsHost || player.IsServer) LguStore.Instance.PlayAudioOnPlayerClientRpc(new NetworkBehaviourReference(player), "breakWood");
            else LguStore.Instance.ReqPlayAudioOnPlayerServerRpc(new NetworkBehaviourReference(player), "breakWood");
        }
    }
}
