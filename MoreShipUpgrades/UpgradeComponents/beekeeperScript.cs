using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class beekeeperScript : BaseUpgrade
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.UpgradeObjects.Add("Beekeeper", gameObject);
        }

        public override void Increment()
        {
            UpgradeBus.instance.beeLevel++;
            LGUStore.instance.UpdateBeePercsServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, UpgradeBus.instance.beeLevel);
        }

        public override void load()
        {
            UpgradeBus.instance.beekeeper = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Beekeeper is active!</color>";
            LGUStore.instance.UpdateBeePercsServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, 0);
        }

        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Beekeeper")) { UpgradeBus.instance.UpgradeObjects.Add("Beekeeper", gameObject); }
        }

        public override void Unwind()
        {
            UpgradeBus.instance.beeLevel = 0;
            UpgradeBus.instance.beekeeper = false;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Beekeeper has been disabled</color>";
            LGUStore.instance.UpdateBeePercsServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, 0);
        }
    }
}
