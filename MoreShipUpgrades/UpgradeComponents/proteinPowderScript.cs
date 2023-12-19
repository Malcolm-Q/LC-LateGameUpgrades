using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class proteinPowderScript : BaseUpgrade
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.proteinLevel++;
            LGUStore.instance.UpdateForceMultsServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, (UpgradeBus.instance.cfg.PROTEIN_INCREMENT * UpgradeBus.instance.proteinLevel)+UpgradeBus.instance.cfg.PROTEIN_UNLOCK_FORCE);
        }

        public override void load()
        {
            UpgradeBus.instance.proteinPowder = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Protein Powder is active!</color>";
            LGUStore.instance.UpdateForceMultsServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, UpgradeBus.instance.cfg.PROTEIN_UNLOCK_FORCE);
        }

        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Protein Powder")) { UpgradeBus.instance.UpgradeObjects.Add("Protein Powder", gameObject); }
        }

        public override void Unwind()
        {
            UpgradeBus.instance.proteinLevel = 0;
            UpgradeBus.instance.proteinPowder = false;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Protein Powder has been disabled</color>";
            LGUStore.instance.UpdateForceMultsServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, 0);
        }
    }
}
