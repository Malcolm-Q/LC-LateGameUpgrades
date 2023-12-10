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
            foreach( CustomTerminalNode node in UpgradeBus.instance.terminalNodes )
            {
                if(node.Name.ToLower() == "beekeeper")
                {
                    node.Description = $"Circuit bees do %{Mathf.Round(100 * (UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (UpgradeBus.instance.beeLevel * UpgradeBus.instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT)))} of their base damage.";
                }
            }
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
    }
}
