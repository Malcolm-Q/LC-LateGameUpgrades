using MoreShipUpgrades.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using System.Linq;
using GameNetcodeStuff;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using MoreShipUpgrades.Misc.TerminalNodes;

namespace MoreShipUpgrades.Managers
{
    public class LguStore : NetworkBehaviour
    {
        public static LguStore Instance { get; internal set; }
        public SaveInfo SaveInfo { get; internal set; }
        public LguSave LguSave { get; internal set; }
        private ulong playerID = 0;
        static LguLogger logger = new LguLogger(nameof(LguStore));
        const string saveDataKey = "LGU_SAVE_DATA";
        private bool receivedSave;

        private void Start()
        {
            Instance = this;
            if (NetworkManager.IsHost)
            {
                string saveFile = GameNetworkManager.Instance.currentSaveFileName;
                int saveNum = GameNetworkManager.Instance.saveFileNum;
                string filePath = Path.Combine(Application.persistentDataPath, $"LGU_{saveNum}.json");
                if (File.Exists(filePath))
                {
                    string tempJson = File.ReadAllText(filePath);
                    ES3.Save(key: saveDataKey, value: tempJson, filePath: saveFile);
                    File.Delete(filePath);
                }
                    string json = (string)ES3.Load(key: saveDataKey, defaultValue: null, filePath: saveFile);
                if (json != null)
                {
                    logger.LogInfo($"Loading save file for slot {saveNum}.");
                    LguSave = JsonConvert.DeserializeObject<LguSave>(json);
                    UpdateUpgradeBus();
                }
                else
                {
                    logger.LogInfo($"No save file found for slot {saveNum}. Creating new.");
                    LguSave = new LguSave();
                    UpdateUpgradeBus();
                }
                UpgradeBus.Instance.Reconstruct();
                HandleSpawns();
            }
            else
            {
                logger.LogInfo("Requesting hosts config...");
                ConfigSynchronizationManager.Instance.SendConfigServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ServerSaveFileServerRpc()
        {
            string saveFile = GameNetworkManager.Instance.currentSaveFileName;
            string json = JsonConvert.SerializeObject(LguSave);
            ES3.Save(key: saveDataKey, value: json, filePath: saveFile);
        }

        [ServerRpc(RequireOwnership = false)]
        public void UpdateLGUSaveServerRpc(ulong id, string json)
        {
            LguSave.playerSaves[id] = JsonConvert.DeserializeObject<SaveInfo>(json);
            logger.LogInfo($"Received and updated save info for client: {id}");
        }

        public void HandleSpawns()
        {
            int i = 0;
            foreach (CustomTerminalNode node in UpgradeBus.Instance.terminalNodes)
            {
                i++;
                GameObject go = Instantiate(node.Prefab, Vector3.zero, Quaternion.identity);
                go.GetComponent<NetworkObject>().Spawn();
            }
            logger.LogInfo($"Successfully spawned {i} upgrade objects.");

            GameObject intern = Instantiate(AssetBundleHandler.GetPerkGameObject(Interns.UPGRADE_NAME));
            intern.GetComponent<NetworkObject>().Spawn();

            GameObject extendDeadline = Instantiate(AssetBundleHandler.GetPerkGameObject(ExtendDeadlineScript.NAME));
            extendDeadline.GetComponent<NetworkObject>().Spawn();

            GameObject insurance = Instantiate(AssetBundleHandler.GetPerkGameObject(ScrapInsurance.COMMAND_NAME));
            insurance.GetComponent<NetworkObject>().Spawn();
        }

        [ServerRpc(RequireOwnership = false)]
        public void PlayersFiredServerRpc()
        {
            ResetUpgradeBusClientRpc();
            UpgradeBus.Instance.ResetAllValues(false);
            string saveFile = GameNetworkManager.Instance.currentSaveFileName;
            LguSave = new LguSave();
            string json = JsonConvert.SerializeObject(LguSave);
            ES3.Save(key: saveDataKey, value: json, filePath: saveFile);
        }

        [ClientRpc]
        private void ResetUpgradeBusClientRpc()
        {
            logger.LogInfo("Players fired, reseting upgrade bus.");
            UpgradeBus.Instance.ResetAllValues(false);
            if(!IsHost || !IsServer) LguSave = new LguSave();
            SaveInfo = new SaveInfo();
        }

        [ServerRpc(RequireOwnership = false)]
        public void ShareSaveServerRpc()
        {
            string json = JsonConvert.SerializeObject(LguSave);
            ShareSaveClientRpc(json);
        }

        [ClientRpc]
        public void ShareSaveClientRpc(string json)
        {
            if (receivedSave)
            {
                logger.LogInfo("Save file already received from host, disregarding.");
                return;
            }
            receivedSave = true;
            logger.LogInfo("Save file received, registering upgrade objects and deserializing.");
            foreach(BaseUpgrade upgrade in FindObjectsOfType<BaseUpgrade>())
            {
                upgrade.Register();
            }
            LguSave = JsonConvert.DeserializeObject<LguSave>(json);
            List<ulong> saves = LguSave.playerSaves.Keys.ToList();
            if(UpgradeBus.Instance.PluginConfiguration.SHARED_UPGRADES.Value && saves.Count > 0)
            {
                ulong steamID = LguSave.playerSaves.Keys.ToList<ulong>()[0];
                logger.LogInfo($"SHARED SAVE FILE: Loading index 0 save under steam ID: {steamID}");
                SaveInfo = LguSave.playerSaves[steamID];
                UpdateUpgradeBus(false);
            }
            else
            {
                UpdateUpgradeBus();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SyncCreditsServerRpc(int credits)
        {
            logger.LogInfo($"Request to sync credits to ${credits} received, calling ClientRpc...");
            SyncCreditsClientRpc(credits);
        }


        [ClientRpc]
        public void SyncCreditsClientRpc(int newCredits)
        {
            logger.LogInfo($"Credits have been synced to ${newCredits}");
            Terminal terminal = UpgradeBus.Instance.GetTerminal();
            terminal.groupCredits = newCredits;
        }

        [ServerRpc(RequireOwnership =false)]
        private void RegisterNewPlayerServerRpc(ulong id, string json)
        {
            LguSave.playerSaves.Add(id,JsonConvert.DeserializeObject<SaveInfo>(json));
        }
        /// <summary>
        /// Method to get old data from json and store in the new dictionaries
        /// </summary>
        /// <param name="saveInfo">Save that might contain old data</param>
        void CheckForOldData(SaveInfo saveInfo)
        {
            if (saveInfo.exoskeleton)
            {
                UpgradeBus.Instance.activeUpgrades[BackMuscles.UPGRADE_NAME] = saveInfo.exoskeleton;
                UpgradeBus.Instance.upgradeLevels[BackMuscles.UPGRADE_NAME] = saveInfo.backLevel;
            }
            if (saveInfo.beekeeper)
            {
                UpgradeBus.Instance.activeUpgrades[Beekeeper.UPGRADE_NAME] = saveInfo.beekeeper;
                UpgradeBus.Instance.upgradeLevels[Beekeeper.UPGRADE_NAME] = saveInfo.beeLevel;
            }
            if (saveInfo.biggerLungs)
            {
                UpgradeBus.Instance.activeUpgrades[BiggerLungs.UPGRADE_NAME] = saveInfo.biggerLungs;
                UpgradeBus.Instance.upgradeLevels[BiggerLungs.UPGRADE_NAME] = saveInfo.lungLevel;
            }
            if (saveInfo.doorsHydraulicsBattery)
            {
                UpgradeBus.Instance.activeUpgrades[DoorsHydraulicsBattery.UPGRADE_NAME] = saveInfo.doorsHydraulicsBattery;
                UpgradeBus.Instance.upgradeLevels[DoorsHydraulicsBattery.UPGRADE_NAME] = saveInfo.doorsHydraulicsBatteryLevel;
            }
            if (saveInfo.hunter)
            {
                UpgradeBus.Instance.activeUpgrades[Hunter.UPGRADE_NAME] = saveInfo.hunter;
                UpgradeBus.Instance.upgradeLevels[Hunter.UPGRADE_NAME] = saveInfo.huntLevel;
            }
            if (saveInfo.nightVision)
            {
                UpgradeBus.Instance.activeUpgrades[NightVision.UPGRADE_NAME] = saveInfo.nightVision;
                UpgradeBus.Instance.upgradeLevels[NightVision.UPGRADE_NAME] = saveInfo.nightVisionLevel;
            }
            if (saveInfo.playerHealth)
            {
                UpgradeBus.Instance.activeUpgrades[Stimpack.UPGRADE_NAME] = saveInfo.playerHealth;
                UpgradeBus.Instance.upgradeLevels[Stimpack.UPGRADE_NAME] = saveInfo.playerHealthLevel;
            }
            if (saveInfo.proteinPowder) 
            {
                UpgradeBus.Instance.activeUpgrades[ProteinPowder.UPGRADE_NAME] = saveInfo.proteinPowder;
                UpgradeBus.Instance.upgradeLevels[ProteinPowder.UPGRADE_NAME] = saveInfo.proteinLevel;
            }
            if (saveInfo.runningShoes)
            {
                UpgradeBus.Instance.activeUpgrades[RunningShoes.UPGRADE_NAME] = saveInfo.runningShoes;
                UpgradeBus.Instance.upgradeLevels[RunningShoes.UPGRADE_NAME] = saveInfo.runningLevel;
            }
            if (saveInfo.scannerUpgrade)
            {
                UpgradeBus.Instance.activeUpgrades[BetterScanner.UPGRADE_NAME] = saveInfo.scannerUpgrade;
                UpgradeBus.Instance.upgradeLevels[BetterScanner.UPGRADE_NAME] = saveInfo.scanLevel;
            }
            if (saveInfo.strongLegs)
            {
                UpgradeBus.Instance.activeUpgrades[StrongLegs.UPGRADE_NAME] = saveInfo.strongLegs;
                UpgradeBus.Instance.upgradeLevels[StrongLegs.UPGRADE_NAME] = saveInfo.legLevel;
            }
            if (saveInfo.terminalFlash)
            {
                UpgradeBus.Instance.activeUpgrades[StrongLegs.UPGRADE_NAME] = saveInfo.terminalFlash;
                UpgradeBus.Instance.upgradeLevels[StrongLegs.UPGRADE_NAME] = saveInfo.discoLevel;
            }
            if (saveInfo.walkies) UpgradeBus.Instance.activeUpgrades[WalkieGPS.UPGRADE_NAME] = saveInfo.walkies;
            if (saveInfo.lockSmith) UpgradeBus.Instance.activeUpgrades[LockSmith.UPGRADE_NAME] = saveInfo.lockSmith;
            if (saveInfo.sickBeats) UpgradeBus.Instance.activeUpgrades[SickBeats.UPGRADE_NAME] = saveInfo.sickBeats;
            if (saveInfo.pager) UpgradeBus.Instance.activeUpgrades[FastEncryption.UPGRADE_NAME] = saveInfo.pager;
            if (saveInfo.DestroyTraps) UpgradeBus.Instance.activeUpgrades[MalwareBroadcaster.UPGRADE_NAME] = saveInfo.DestroyTraps;
            if (saveInfo.lightningRod) UpgradeBus.Instance.activeUpgrades[LightningRod.UPGRADE_NAME] = saveInfo.lightningRod;
        }
        public void UpdateUpgradeBus(bool UseLocalSteamID = true, bool checkID = true)
        {
            if(UseLocalSteamID)
            {
                if(playerID == 0 && checkID)
                {
                    logger.LogInfo("SteamID is not available yet, waiting...");
                    StartCoroutine(WaitForSteamID());
                    return;
                }
                if(!LguSave.playerSaves.ContainsKey(playerID))
                {
                    logger.LogInfo($"{playerID} Was not found in save dictionary! Creating new save for ID.");
                    SaveInfo = new SaveInfo();
                    RegisterNewPlayerServerRpc(playerID, JsonConvert.SerializeObject(SaveInfo));
                    return;
                }
                SaveInfo = LguSave.playerSaves[playerID];
                logger.LogInfo($"Successfully loaded save data for ID: {playerID}");
            }
            bool oldHelmet = UpgradeBus.Instance.wearingHelmet;
            UpgradeBus.Instance.activeUpgrades = SaveInfo.activeUpgrades;
            UpgradeBus.Instance.upgradeLevels = SaveInfo.upgradeLevels;

            CheckForOldData(SaveInfo);

            ContractManager.Instance.contractLevel = SaveInfo.contractLevel;
            ContractManager.Instance.contractType = SaveInfo.contractType;
            UpgradeBus.Instance.wearingHelmet = SaveInfo.wearingHelmet;

            UpgradeBus.Instance.SaleData = SaveInfo.SaleData;

            if(oldHelmet != UpgradeBus.Instance.wearingHelmet)
            {
                UpgradeBus.Instance.helmetDesync = true;
                // this will just sync the helmets on StartGame
            }

            UpgradeBus.Instance.LoadSales();

            StartCoroutine(WaitForUpgradeObject());
        }

        private IEnumerator WaitForSteamID()
        {
            yield return new WaitForSeconds(1f);
            int tries = 0;
            while(playerID == 0 && tries < 10)
            {
                tries++;
                playerID = GameNetworkManager.Instance.localPlayerController.playerSteamId;
                yield return new WaitForSeconds(0.5f);
            }
            if (playerID != 0)
            {
                logger.LogInfo($"Loading SteamID: {playerID}");
                UpdateUpgradeBus();
            }
            else
            {
                logger.LogInfo("Timeout reached, loading under ID 0");
                UpdateUpgradeBus(true, false);
            }
        }

        private IEnumerator WaitForUpgradeObject()
        {
            yield return new WaitForSeconds(1f);
            logger.LogInfo("Applying loaded upgrades...");
            foreach (CustomTerminalNode customNode in UpgradeBus.Instance.terminalNodes)
            {
                bool activeUpgrade = UpgradeBus.Instance.activeUpgrades.GetValueOrDefault(customNode.Name, false);
                int upgradeLevel = UpgradeBus.Instance.upgradeLevels.GetValueOrDefault(customNode.Name, 0);
                customNode.Unlocked = activeUpgrade;
                customNode.CurrentUpgrade = upgradeLevel;
                if (activeUpgrade) UpgradeBus.Instance.UpgradeObjects[customNode.Name].GetComponent<BaseUpgrade>().Load();
                if (customNode.Name == NightVision.UPGRADE_NAME)
                {
                    customNode.Unlocked = true;
                }
            }
        }
        void UpdateUpgrades(string name, bool increment = false)
        {
            foreach (CustomTerminalNode node in UpgradeBus.Instance.terminalNodes)
            {
                if (node.Name == name)
                {
                    node.Unlocked = true;
                    if (increment) { node.CurrentUpgrade++; }
                    logger.LogInfo($"Node found and unlocked (level = {node.CurrentUpgrade})");
                    break;
                }
            }
            if (!increment)
            {
                UpgradeBus.Instance.UpgradeObjects[name].GetComponent<BaseUpgrade>().Load();
                logger.LogInfo($"First purchase, executing BaseUpgrade.load()");
            }
            else
            {
                UpgradeBus.Instance.UpgradeObjects[name].GetComponent<TierUpgrade>().Increment();
                logger.LogInfo($"upgrade already unlocked, executing TierUpgrade.Increment()");
            }
            SaveInfo = new SaveInfo();
            UpdateLGUSaveServerRpc(playerID, JsonConvert.SerializeObject(SaveInfo));
        }
        public void HandleUpgrade(string name, bool increment = false)
        {
            if (UpgradeBus.Instance.IndividualUpgrades[name])
            {
                logger.LogInfo($"{name} is registered as a shared upgrade! Calling ServerRpc...");
                HandleUpgradeServerRpc(name, increment);
                return;
            }
            logger.LogInfo($"{name} is not registered as a shared upgrade! Unlocking on this client only...");
            UpdateUpgrades(name, increment);
        }

        [ServerRpc(RequireOwnership = false)]
        private void HandleUpgradeServerRpc(string name, bool increment)
        {
            logger.LogInfo($"Received server request to handle shared upgrade for: {name} increment: {increment}");
            HandleUpgradeClientRpc(name, increment);
        }

        [ClientRpc]
        private void HandleUpgradeClientRpc(string name, bool increment)
        {
            logger.LogInfo($"Received client request to handle shared upgrade for: {name} increment: {increment}");
            UpdateUpgrades(name, increment);
        }

        [ClientRpc]
        public void GenerateSalesClientRpc(int seed)
        {
            UpgradeBus.Instance.GenerateSales(seed);
        }

        [ClientRpc]
        public void DestroyHelmetClientRpc(ulong id)
        {
            if(StartOfRound.Instance.allPlayerObjects.Length <= (int)id)
            {
                logger.LogError($"ID: {id} can not be used to index allPlayerObjects! (Length: {StartOfRound.Instance.allPlayerObjects.Length})");
                return;
            }

            GameObject player = StartOfRound.Instance.allPlayerObjects[(int)id];
            if (player == null) 
            {
                logger.LogError("Player is with helmet is null!");
                return;
            }
            if (player.GetComponent<PlayerControllerB>().IsOwner) return; // owner doesn't have model

            Transform helmet = player.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2).Find("HelmetModel(Clone)");
            if(helmet == null)
            {
                logger.LogError("Unable to find 'HelmetModel(Clone) on player!");
                return;
            }
            Destroy(helmet.gameObject); // this isn't a netobj hence the client side destruction
        }

        [ServerRpc(RequireOwnership = false)]
        public void ReqDestroyHelmetServerRpc(ulong id)
        {
            logger.LogInfo($"Instructing clients to destroy helmet of player : {id}");
            DestroyHelmetClientRpc(id);
        }

        [ClientRpc]
        internal void SpawnAndMoveHelmetClientRpc(NetworkObjectReference netRef, ulong id)
        {
            netRef.TryGet(out NetworkObject obj);
            if (obj == null || obj.IsOwner)
            {
                if (obj == null) logger.LogError("Failed to resolve network object ref in SpawnAndMoveHelmetClientRpc!"); 
                else logger.LogInfo("This client owns the helmet, skipping cosmetic instantiation.");
                return;
            }
            Transform head = obj.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2); // phenomenal
            GameObject go = Instantiate(UpgradeBus.Instance.helmetModel, head);
            go.transform.localPosition = new Vector3(0.01f,0.1f,0.08f);
            logger.LogInfo($"Successfully spawned helmet cosmetic for player: {id}");
        }

        [ServerRpc(RequireOwnership = false)]
        internal void ReqSpawnAndMoveHelmetServerRpc(NetworkObjectReference netRef, ulong id)
        {
            logger.LogInfo($"Instructing clients to spawn helmet on player : {id}");
            SpawnAndMoveHelmetClientRpc(netRef, id);
        }

        [ClientRpc]
        internal void PlayAudioOnPlayerClientRpc(NetworkBehaviourReference netRef, string clip)
        {
            netRef.TryGet(out PlayerControllerB player);
            if (player == null)
            {
                logger.LogError("Unable to resolve network behaviour reference in PlayAudioOnPlayerClientRpc!");
                return;
            }
            player.GetComponentInChildren<AudioSource>().PlayOneShot(UpgradeBus.Instance.SFX[clip]);
            logger.LogInfo($"Playing '{clip}' SFX on player: {player.playerUsername}");
        }

        [ServerRpc(RequireOwnership = false)]
        internal void ReqPlayAudioOnPlayerServerRpc(NetworkBehaviourReference netRef, string clip)
        {
            logger.LogInfo($"Instructing clients to play '{clip}' SFX");
            PlayAudioOnPlayerClientRpc(netRef, clip);
        }

    }

    [Serializable]
    public class SaveInfo
    {
        public Dictionary<string, bool> activeUpgrades = UpgradeBus.Instance.activeUpgrades;
        public Dictionary<string, int> upgradeLevels = UpgradeBus.Instance.upgradeLevels;
        public bool DestroyTraps;
        public bool scannerUpgrade;
        public bool nightVision;
        public bool exoskeleton;
        public bool beekeeper;
        public bool terminalFlash;
        public bool strongLegs;
        public bool proteinPowder;
        public bool runningShoes;
        public bool biggerLungs;
        public bool lockSmith;
        public bool walkies;
        public bool lightningRod;
        public bool pager;
        public bool hunter;
        public bool playerHealth;
        public bool sickBeats;
        public bool doorsHydraulicsBattery;
        public bool marketInfluence;
        public bool bargainConnections;
        public bool lethalDeals;
        public bool quantumDisruptor;

        public int beeLevel;
        public int huntLevel;
        public int proteinLevel;
        public int lungLevel;
        public int backLevel;
        public int runningLevel;
        public int scanLevel;
        public int discoLevel;
        public int legLevel;
        public int nightVisionLevel;
        public int playerHealthLevel;
        public int doorsHydraulicsBatteryLevel;
        public int marketInfluenceLevel;
        public int bargainConnectionsLevel;
        public int quantumDisruptorLevel;

        public bool TPButtonPressed = UpgradeBus.Instance.TPButtonPressed;
        public string contractType = ContractManager.Instance.contractType;
        public string contractLevel = ContractManager.Instance.contractLevel;
        public Dictionary<string, float> SaleData = UpgradeBus.Instance.SaleData;
        public bool wearingHelmet = UpgradeBus.Instance.wearingHelmet;
    }

    [Serializable]
    public class LguSave
    {
        public Dictionary<ulong,SaveInfo> playerSaves = new Dictionary<ulong,SaveInfo>();
    }
}
