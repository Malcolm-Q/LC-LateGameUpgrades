using LethalLib.Modules;
using MoreShipUpgrades.Input;
using MoreShipUpgrades.Misc;
using Newtonsoft.Json;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

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
                InputAction WheelbarrowKey = Keybinds.WheelbarrowAction;
                InputAction NvgKey = Keybinds.NvgAction;
                string nvgconfig = UpgradeBus.Instance.PluginConfiguration.TOGGLE_NIGHT_VISION_KEY.Value;
                string wbconfig = UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_DROP_ALL_CONTROL_BIND.Value;
                cfg.SynchronizeConfiguration();
                CheckMapObjectsConfigurations();
                UpgradeBus.Instance.PluginConfiguration.NIGHT_VIS_COLOR.Value = col;
                UpgradeBus.Instance.PluginConfiguration.TOGGLE_NIGHT_VISION_KEY.Value = nvgconfig;
                UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_DROP_ALL_CONTROL_BIND.Value = wbconfig;
                Keybinds.WheelbarrowAction = WheelbarrowKey;
                Keybinds.NvgAction = NvgKey;
                UpgradeBus.Instance.Reconstruct();
                retrievedPluginConfiguration = true;
            }
            if (IsHost || IsServer)
            {
                StartCoroutine(WaitALittleToShareTheFile());
            }
        }
        void CheckMapObjectsConfigurations()
        {
            CheckMedkit();
        }
        void CheckMedkit()
        {
            int amount = UpgradeBus.Instance.spawnableMapObjectsAmount["MedkitMapItem"];
            if (amount == UpgradeBus.Instance.PluginConfiguration.EXTRACTION_CONTRACT_AMOUNT_MEDKITS.Value) return;
            MapObjects.RemoveMapObject(UpgradeBus.Instance.spawnableMapObjects["MedkitMapItem"], Levels.LevelTypes.All);
            AnimationCurve curve = new AnimationCurve(new Keyframe(0f, UpgradeBus.Instance.PluginConfiguration.EXTRACTION_CONTRACT_AMOUNT_MEDKITS.Value), new Keyframe(1f, UpgradeBus.Instance.PluginConfiguration.EXTRACTION_CONTRACT_AMOUNT_MEDKITS.Value));
            MapObjects.RegisterMapObject(mapObject: UpgradeBus.Instance.spawnableMapObjects["MedkitMapItem"], levels: Levels.LevelTypes.All, spawnRateFunction: (level) => curve);
        }
        private IEnumerator WaitALittleToShareTheFile()
        {
            yield return new WaitForSeconds(0.5f);
            logger.LogInfo("Now sharing save file with clients...");
            LguStore.Instance.ShareSaveServerRpc();
        }
    }
}
