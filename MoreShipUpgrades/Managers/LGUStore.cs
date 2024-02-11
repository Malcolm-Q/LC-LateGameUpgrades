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
using MoreShipUpgrades.UpgradeComponents.Items;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using MoreShipUpgrades.Misc.TerminalNodes;

namespace MoreShipUpgrades.Managers
{
    public class LGUStore : NetworkBehaviour
    {
        public static LGUStore instance;
        public SaveInfo saveInfo;
        public LGUSave lguSave;
        private ulong playerID = 0;
        private static LGULogger logger = new LGULogger(nameof(LGUStore));
        private string saveDataKey = "LGU_SAVE_DATA";
        private bool retrievedCfg;
        private bool receivedSave;

        private void Start()
        {
            instance = this;
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
                    lguSave = JsonConvert.DeserializeObject<LGUSave>(json);
                    UpdateUpgradeBus();
                }
                else
                {
                    logger.LogInfo($"No save file found for slot {saveNum}. Creating new.");
                    lguSave = new LGUSave();
                    UpdateUpgradeBus();
                }
                UpgradeBus.instance.Reconstruct();
                HandleSpawns();
            }
            else
            {
                logger.LogInfo("Requesting hosts config...");
                SendConfigServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ServerSaveFileServerRpc()
        {
            string saveFile = GameNetworkManager.Instance.currentSaveFileName;
            string json = JsonConvert.SerializeObject(lguSave);
            ES3.Save(key: saveDataKey, value: json, filePath: saveFile);
        }

        [ServerRpc(RequireOwnership = false)]
        public void UpdateLGUSaveServerRpc(ulong id, string json)
        {
            lguSave.playerSaves[id] = JsonConvert.DeserializeObject<SaveInfo>(json);
            logger.LogInfo($"Received and updated save info for client: {id}");
        }

        [ServerRpc(RequireOwnership = false)]
        public void ReqSyncContractDetailsServerRpc(string contractLvl, int contractType)
        {
            SyncContractDetailsClientRpc(contractLvl, contractType);
            logger.LogInfo("Syncing contract details on all clients...");
        }

        [ClientRpc]
        public void SyncContractDetailsClientRpc(string contractLvl = "None", int contractType = -1)
        {
            UpgradeBus.instance.contractLevel = contractLvl;
            if (contractType == -1) UpgradeBus.instance.contractType = "None";
            else
            {
                UpgradeBus.instance.contractType = CommandParser.contracts[contractType];
                ContractScript.lastContractIndex = contractType;
            }
            UpgradeBus.instance.fakeBombOrders = new Dictionary<string, List<string>>();
            logger.LogInfo($"New contract details received. level: {contractLvl}, type: {contractType}");
        }

        public void HandleSpawns()
        {
            int i = 0;
            foreach (CustomTerminalNode node in UpgradeBus.instance.terminalNodes)
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
        public void SendConfigServerRpc()
        {
            logger.LogInfo("Client has requested hosts client");
            string json = JsonConvert.SerializeObject(UpgradeBus.instance.cfg);
            SendConfigClientRpc(json);
        }

        [ClientRpc]
        private void SendConfigClientRpc(string json)
        {
            if (retrievedCfg)
            {
                logger.LogInfo("Config has already been received from host on this client, disregarding.");
                return;
            }
            PluginConfig cfg = JsonConvert.DeserializeObject<PluginConfig>(json);
            if( cfg != null && !IsHost && !IsServer)
            {
                logger.LogInfo("Config received, deserializing and constructing...");
                Color col = UpgradeBus.instance.cfg.NIGHT_VIS_COLOR.Value;
                UpgradeBus.instance.cfg = cfg;
                UpgradeBus.instance.cfg.NIGHT_VIS_COLOR.Value = col; //
                UpgradeBus.instance.Reconstruct();
                retrievedCfg = true;
            }
            if(IsHost || IsServer)
            {
                StartCoroutine(WaitALittleToShareTheFile());
            }
        }

        private IEnumerator WaitALittleToShareTheFile()
        {
            yield return new WaitForSeconds(0.5f);
            logger.LogInfo("Now sharing save file with clients...");
            ShareSaveServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        public void PlayersFiredServerRpc()
        {
            ResetUpgradeBusClientRpc();
            UpgradeBus.instance.ResetAllValues(false);
            string saveFile = GameNetworkManager.Instance.currentSaveFileName;
            lguSave = new LGUSave();
            string json = JsonConvert.SerializeObject(lguSave);
            ES3.Save(key: saveDataKey, value: json, filePath: saveFile);
        }

        [ClientRpc]
        private void ResetUpgradeBusClientRpc()
        {
            logger.LogInfo("Players fired, reseting upgrade bus.");
            UpgradeBus.instance.ResetAllValues(false);
            if(!IsHost || !IsServer) lguSave = new LGUSave();
            saveInfo = new SaveInfo();
        }

        [ServerRpc(RequireOwnership = false)]
        public void ShareSaveServerRpc()
        {
            string json = JsonConvert.SerializeObject(lguSave);
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
            lguSave = JsonConvert.DeserializeObject<LGUSave>(json);
            List<ulong> saves = lguSave.playerSaves.Keys.ToList();
            if(UpgradeBus.instance.cfg.SHARED_UPGRADES.Value && saves.Count > 0)
            {
                ulong steamID = lguSave.playerSaves.Keys.ToList<ulong>()[0];
                logger.LogInfo($"SHARED SAVE FILE: Loading index 0 save under steam ID: {steamID}");
                saveInfo = lguSave.playerSaves[steamID];
                UpdateUpgradeBus(false);
            }
            else
            {
                UpdateUpgradeBus();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void PlayerHealthUpdateLevelServerRpc(ulong id, int level) 
        {
            logger.LogInfo("Request to update player max healths received. Calling ClientRpc...");
            PlayerHealthUpdateLevelClientRpc(id, level);
        }

        [ClientRpc]
        private void PlayerHealthUpdateLevelClientRpc(ulong id, int level)
        {
            logger.LogInfo($"Setting max health level for player {id} to {level}");
            if (level == -1)
            {
                UpgradeBus.instance.playerHealthLevels.Remove(id);
                return;
            }

            if (UpgradeBus.instance.playerHealthLevels.ContainsKey(id))
                UpgradeBus.instance.playerHealthLevels[id] = level;
            else UpgradeBus.instance.playerHealthLevels.Add(id, level);
        }

        [ServerRpc(RequireOwnership = false)]
        public void ToggleIncreaseHivePriceServerRpc()
        {
            logger.LogInfo($"Request to increase hive price received, calling client RPC.");
            ToggleIncreaseHivePriceClientRpc();
        }

        [ClientRpc]
        public void ToggleIncreaseHivePriceClientRpc()
        {
            logger.LogInfo($"Increasing hive price...");
            UpgradeBus.instance.increaseHivePrice = true;
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
            Terminal terminal = GameObject.Find("TerminalScript").GetComponent<Terminal>();
            terminal.groupCredits = newCredits;
        }

        [ServerRpc(RequireOwnership =false)]
        private void RegisterNewPlayerServerRpc(ulong id, string json)
        {
            lguSave.playerSaves.Add(id,JsonConvert.DeserializeObject<SaveInfo>(json));
        }
        /// <summary>
        /// Method to get old data from json and store in the new dictionaries
        /// </summary>
        /// <param name="saveInfo">Save that might contain old data</param>
        void CheckForOldData(SaveInfo saveInfo)
        {
            if (saveInfo.exoskeleton)
            {
                UpgradeBus.instance.activeUpgrades[BackMuscles.UPGRADE_NAME] = saveInfo.exoskeleton;
                UpgradeBus.instance.upgradeLevels[BackMuscles.UPGRADE_NAME] = saveInfo.backLevel;
            }
            if (saveInfo.beekeeper)
            {
                UpgradeBus.instance.activeUpgrades[Beekeeper.UPGRADE_NAME] = saveInfo.beekeeper;
                UpgradeBus.instance.upgradeLevels[Beekeeper.UPGRADE_NAME] = saveInfo.beeLevel;
            }
            if (saveInfo.biggerLungs)
            {
                UpgradeBus.instance.activeUpgrades[BiggerLungs.UPGRADE_NAME] = saveInfo.biggerLungs;
                UpgradeBus.instance.upgradeLevels[BiggerLungs.UPGRADE_NAME] = saveInfo.lungLevel;
            }
            if (saveInfo.doorsHydraulicsBattery)
            {
                UpgradeBus.instance.activeUpgrades[DoorsHydraulicsBattery.UPGRADE_NAME] = saveInfo.doorsHydraulicsBattery;
                UpgradeBus.instance.upgradeLevels[DoorsHydraulicsBattery.UPGRADE_NAME] = saveInfo.doorsHydraulicsBatteryLevel;
            }
            if (saveInfo.hunter)
            {
                UpgradeBus.instance.activeUpgrades[Hunter.UPGRADE_NAME] = saveInfo.hunter;
                UpgradeBus.instance.upgradeLevels[Hunter.UPGRADE_NAME] = saveInfo.huntLevel;
            }
            if (saveInfo.nightVision)
            {
                UpgradeBus.instance.activeUpgrades[NightVision.UPGRADE_NAME] = saveInfo.nightVision;
                UpgradeBus.instance.upgradeLevels[NightVision.UPGRADE_NAME] = saveInfo.nightVisionLevel;
            }
            if (saveInfo.playerHealth)
            {
                UpgradeBus.instance.activeUpgrades[Stimpack.UPGRADE_NAME] = saveInfo.playerHealth;
                UpgradeBus.instance.upgradeLevels[Stimpack.UPGRADE_NAME] = saveInfo.playerHealthLevel;
            }
            if (saveInfo.proteinPowder) 
            {
                UpgradeBus.instance.activeUpgrades[ProteinPowder.UPGRADE_NAME] = saveInfo.proteinPowder;
                UpgradeBus.instance.upgradeLevels[ProteinPowder.UPGRADE_NAME] = saveInfo.proteinLevel;
            }
            if (saveInfo.runningShoes)
            {
                UpgradeBus.instance.activeUpgrades[RunningShoes.UPGRADE_NAME] = saveInfo.runningShoes;
                UpgradeBus.instance.upgradeLevels[RunningShoes.UPGRADE_NAME] = saveInfo.runningLevel;
            }
            if (saveInfo.scannerUpgrade)
            {
                UpgradeBus.instance.activeUpgrades[BetterScanner.UPGRADE_NAME] = saveInfo.scannerUpgrade;
                UpgradeBus.instance.upgradeLevels[BetterScanner.UPGRADE_NAME] = saveInfo.scanLevel;
            }
            if (saveInfo.strongLegs)
            {
                UpgradeBus.instance.activeUpgrades[StrongLegs.UPGRADE_NAME] = saveInfo.strongLegs;
                UpgradeBus.instance.upgradeLevels[StrongLegs.UPGRADE_NAME] = saveInfo.legLevel;
            }
            if (saveInfo.terminalFlash)
            {
                UpgradeBus.instance.activeUpgrades[StrongLegs.UPGRADE_NAME] = saveInfo.terminalFlash;
                UpgradeBus.instance.upgradeLevels[StrongLegs.UPGRADE_NAME] = saveInfo.discoLevel;
            }
            if (saveInfo.walkies) UpgradeBus.instance.activeUpgrades[WalkieGPS.UPGRADE_NAME] = saveInfo.walkies;
            if (saveInfo.lockSmith) UpgradeBus.instance.activeUpgrades[LockSmith.UPGRADE_NAME] = saveInfo.lockSmith;
            if (saveInfo.sickBeats) UpgradeBus.instance.activeUpgrades[SickBeats.UPGRADE_NAME] = saveInfo.sickBeats;
            if (saveInfo.pager) UpgradeBus.instance.activeUpgrades[FastEncryption.UPGRADE_NAME] = saveInfo.pager;
            if (saveInfo.DestroyTraps) UpgradeBus.instance.activeUpgrades[MalwareBroadcaster.UPGRADE_NAME] = saveInfo.DestroyTraps;
            if (saveInfo.lightningRod) UpgradeBus.instance.activeUpgrades[LightningRod.UPGRADE_NAME] = saveInfo.lightningRod;

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
                if(!lguSave.playerSaves.ContainsKey(playerID))
                {
                    logger.LogInfo($"{playerID} Was not found in save dictionary! Creating new save for ID.");
                    saveInfo = new SaveInfo();
                    RegisterNewPlayerServerRpc(playerID, JsonConvert.SerializeObject(saveInfo));
                    return;
                }
                saveInfo = lguSave.playerSaves[playerID];
                logger.LogInfo($"Successfully loaded save data for ID: {playerID}");
            }
            bool oldHelmet = UpgradeBus.instance.wearingHelmet;
            UpgradeBus.instance.activeUpgrades = saveInfo.activeUpgrades;
            UpgradeBus.instance.upgradeLevels = saveInfo.upgradeLevels;

            CheckForOldData(saveInfo);

            UpgradeBus.instance.contractLevel = saveInfo.contractLevel;
            UpgradeBus.instance.contractType = saveInfo.contractType;
            UpgradeBus.instance.wearingHelmet = saveInfo.wearingHelmet;

            UpgradeBus.instance.SaleData = saveInfo.SaleData;

            if(oldHelmet != UpgradeBus.instance.wearingHelmet)
            {
                UpgradeBus.instance.helmetDesync = true;
                // this will just sync the helmets on StartGame
            }

            UpgradeBus.instance.LoadSales();

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
            foreach (CustomTerminalNode customNode in UpgradeBus.instance.terminalNodes)
            {
                bool activeUpgrade = UpgradeBus.instance.activeUpgrades.GetValueOrDefault(customNode.Name, false);
                if (!activeUpgrade) continue;
                int upgradeLevel = UpgradeBus.instance.upgradeLevels.GetValueOrDefault(customNode.Name, 0);
                customNode.Unlocked = true;
                customNode.CurrentUpgrade = upgradeLevel + 1;
                UpgradeBus.instance.UpgradeObjects[customNode.Name].GetComponent<BaseUpgrade>().Load();

            }
        }

        public void HandleUpgrade(string name, bool increment = false)
        {
            if (UpgradeBus.instance.IndividualUpgrades[name])
            {
                logger.LogInfo($"{name} is registered as a shared upgrade! Calling ServerRpc...");
                HandleUpgradeServerRpc(name, increment);
                return;
            }
            logger.LogInfo($"{name} is not registered as a shared upgrade! Unlocking on this client only...");
            foreach (CustomTerminalNode node in UpgradeBus.instance.terminalNodes)
            {
                if (node.Name == name)
                {
                    node.Unlocked = true;
                    if(increment) { node.CurrentUpgrade++; }
                    logger.LogInfo($"Node found and unlocked (level = {node.CurrentUpgrade})");
                    break;
                }
            }
            if(!increment)
            {
                UpgradeBus.instance.UpgradeObjects[name].GetComponent<BaseUpgrade>().Load(); 
                logger.LogInfo($"First purchase, executing BaseUpgrade.load()");
            }
            else 
            {
                UpgradeBus.instance.UpgradeObjects[name].GetComponent<TierUpgrade>().Increment(); 
                logger.LogInfo($"upgrade already unlocked, executing TierUpgrade.Increment()");
            }
            saveInfo = new SaveInfo();
            UpdateLGUSaveServerRpc(playerID, JsonConvert.SerializeObject(saveInfo));
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
            foreach (CustomTerminalNode node in UpgradeBus.instance.terminalNodes)
            {
                if (node.Name == name)
                {
                    node.Unlocked = true;
                    if(increment) { node.CurrentUpgrade++; }
                    logger.LogInfo($"Node found and unlocked (level = {node.CurrentUpgrade})");
                    break;
                }
            }
            if(!increment)
            {
                UpgradeBus.instance.UpgradeObjects[name].GetComponent<BaseUpgrade>().Load(); 
                logger.LogInfo($"First purchase, executing BaseUpgrade.load()");
            }
            else 
            {
                UpgradeBus.instance.UpgradeObjects[name].GetComponent<TierUpgrade>().Increment(); 
                logger.LogInfo($"upgrade already unlocked, executing TierUpgrade.Increment()");
            }
            saveInfo = new SaveInfo();
            UpdateLGUSaveServerRpc(playerID, JsonConvert.SerializeObject(saveInfo));
        }

        [ClientRpc]
        public void GenerateSalesClientRpc(int seed)
        {
            UpgradeBus.instance.GenerateSales(seed);
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
                if (obj.IsOwner) logger.LogInfo("This client owns the helmet, skipping cosmetic instantiation.");
                else logger.LogError("Failed to resolve network object ref in SpawnAndMoveHelmetClientRpc!");
                return;
            }
            Transform head = obj.transform.GetChild(0).GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(2); // phenomenal
            GameObject go = Instantiate(UpgradeBus.instance.helmetModel, head);
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
            player.GetComponentInChildren<AudioSource>().PlayOneShot(UpgradeBus.instance.SFX[clip]);
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
        public Dictionary<string, bool> activeUpgrades = UpgradeBus.instance.activeUpgrades;
        public Dictionary<string, int> upgradeLevels = UpgradeBus.instance.upgradeLevels;
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

        public bool TPButtonPressed = UpgradeBus.instance.TPButtonPressed;
        public string contractType = UpgradeBus.instance.contractType;
        public string contractLevel = UpgradeBus.instance.contractLevel;
        public Dictionary<string, float> SaleData = UpgradeBus.instance.SaleData;
        public bool wearingHelmet = UpgradeBus.instance.wearingHelmet;
    }

    [Serializable]
    public class LGUSave
    {
        public Dictionary<ulong,SaveInfo> playerSaves = new Dictionary<ulong,SaveInfo>();
    }
}
