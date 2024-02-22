using MoreShipUpgrades.Misc;
using Newtonsoft.Json;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    internal class ConfigSynchronizationManager : NetworkBehaviour
    {
        bool retrievedPluginConfiguration;
        internal static ConfigSynchronizationManager Instance {  get; private set; }
        static LguLogger logger = new LguLogger(nameof(ConfigSynchronizationManager));
        void Awake()
        {
            Instance = this;
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendConfigServerRpc()
        {
            logger.LogInfo("Client has requested hosts client");
            ConfigSynchronization cfg = new ConfigSynchronization();
            cfg.SetupSynchronization();
            string json = JsonConvert.SerializeObject(cfg);
            SendConfigClientRpc(json);
        }

        [ClientRpc]
        private void SendConfigClientRpc(string json)
        {
            if (retrievedPluginConfiguration)
            {
                logger.LogInfo("Config has already been received from host on this client, disregarding.");
                return;
            }
            if (!IsHost && !IsServer)
            {
                ConfigSynchronization cfg = JsonConvert.DeserializeObject<ConfigSynchronization>(json);
                logger.LogInfo("Config received, deserializing and constructing...");
                Color col = UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_COLOR.Value;
                cfg.SynchronizeConfiguration();
                UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_COLOR.Value = col;
                UpgradeBus.Instance.Reconstruct();
                retrievedPluginConfiguration = true;
            }
            if (IsHost || IsServer)
            {
                StartCoroutine(WaitALittleToShareTheFile());
            }
        }
        private IEnumerator WaitALittleToShareTheFile()
        {
            yield return new WaitForSeconds(0.5f);
            logger.LogInfo("Now sharing save file with clients...");
            LguStore.Instance.ShareSaveServerRpc();
        }
    }
}
