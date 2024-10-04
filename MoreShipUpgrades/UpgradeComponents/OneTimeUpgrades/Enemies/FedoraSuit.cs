using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using System.Collections.Generic;
using Unity.Netcode;
using GameNetcodeStuff;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Enemies
{
    internal class FedoraSuit : OneTimeUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Fedora Suit";
        internal const string WORLD_BUILDING_TEXT = "\n\nValid and more-recent Staff lanyards for {0}," +
            " indicating to any current and former Company Staff that you are permitted to be within the facility. Doesn't work on turrets, as those are... newer.\n\n";
        internal static FedoraSuit instance;

        internal Dictionary<ulong, bool> wearingFedora;
        public override bool CanInitializeOnStart => GetConfiguration().FEDORA_SUIT_PRICE.Value <= 0;

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "your crew" : "you");
        }
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().FEDORA_SUIT_OVERRIDE_NAME;
            wearingFedora = [];
            instance = this;
        }

        public override void Load()
        {
            base.Load();
            ulong clientId = GameNetworkManager.Instance.localPlayerController.actualClientId;
            if (IsServer)
                ToggleFedoraToPlayerClientRpc(clientId, fedora: true);
            else
                ToggleFedoraToPlayerServerRpc(clientId, fedora: true);
        }

        public override void Unwind()
        {
            base.Unwind();
            ulong clientId = GameNetworkManager.Instance.localPlayerController.actualClientId;
            if (IsServer)
                ToggleFedoraToPlayerClientRpc(clientId, fedora: false);
            else
                ToggleFedoraToPlayerServerRpc(clientId, fedora: false);
        }

        [ServerRpc(RequireOwnership = false)]
        internal void ToggleFedoraToPlayerServerRpc(ulong clientId, bool fedora)
        {
            ToggleFedoraToPlayerClientRpc(clientId, fedora);
        }
        [ClientRpc]
        internal void ToggleFedoraToPlayerClientRpc(ulong clientId, bool fedora)
        {
            wearingFedora[clientId] = fedora;
        }
        public static int IsWearingFedoraSuitInt(PlayerControllerB spottedPlayer)
        {
            return IsWearingFedoraSuit(spottedPlayer) ? 1 : 0;
        }

        public static bool IsWearingFedoraSuit(PlayerControllerB spottedPlayer)
        {
            if (!GetConfiguration().FEDORA_SUIT_ENABLED) return false;
            ulong clientId = spottedPlayer.actualClientId;
            return instance.wearingFedora.GetValueOrDefault(clientId, false);
        }
        public override string GetDisplayInfo(int price = -1)
        {
            return $"${price} - Butlers will not feel the urge of stabbing you due to your sophisticated taste in clothes.";
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().FEDORA_SUIT_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<FedoraSuit>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = GetConfiguration();

            return UpgradeBus.Instance.SetupOneTimeTerminalNode(UPGRADE_NAME,
                                    shareStatus: configuration.SHARED_UPGRADES.Value || !configuration.FEDORA_SUIT_INDIVIDUAL.Value,
                                    configuration.FEDORA_SUIT_ENABLED.Value,
                                    configuration.FEDORA_SUIT_PRICE.Value,
                                    configuration.OVERRIDE_UPGRADE_NAMES ? configuration.FEDORA_SUIT_OVERRIDE_NAME : "");
        }
    }
}
