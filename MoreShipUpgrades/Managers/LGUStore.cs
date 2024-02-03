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

        private static Dictionary<string, Func<SaveInfo, bool>> conditions = new Dictionary<string, Func<SaveInfo, bool>>
        {
            {trapDestroyerScript.UPGRADE_NAME, saveInfo => saveInfo.DestroyTraps },
            {terminalFlashScript.UPGRADE_NAME, SaveInfo => SaveInfo.terminalFlash },
            {biggerLungScript.UPGRADE_NAME, SaveInfo => SaveInfo.biggerLungs },
            {runningShoeScript.UPGRADE_NAME, SaveInfo => SaveInfo.runningShoes },
            {nightVisionScript.UPGRADE_NAME, SaveInfo => SaveInfo.nightVision },
            {strongLegsScript.UPGRADE_NAME, SaveInfo => SaveInfo.strongLegs },
            {strongerScannerScript.UPGRADE_NAME, SaveInfo => SaveInfo.scannerUpgrade },
            {beekeeperScript.UPGRADE_NAME, SaveInfo => SaveInfo.beekeeper },
            {exoskeletonScript.UPGRADE_NAME, SaveInfo => SaveInfo.exoskeleton },
            {lockSmithScript.UPGRADE_NAME, SaveInfo => SaveInfo.lockSmith },
            {walkieScript.UPGRADE_NAME, SaveInfo => SaveInfo.walkies },
            {proteinPowderScript.UPGRADE_NAME, SaveInfo => SaveInfo.proteinPowder },
            {pagerScript.UPGRADE_NAME, SaveInfo => SaveInfo.pager },
            {hunterScript.UPGRADE_NAME, SaveInfo => SaveInfo.hunter },
            {lightningRodScript.UPGRADE_NAME, SaveInfo => SaveInfo.lightningRod },
            {playerHealthScript.UPGRADE_NAME, SaveInfo => SaveInfo.playerHealth },
            {DoorsHydraulicsBattery.UPGRADE_NAME, SaveInfo => SaveInfo.doorsHydraulicsBattery },
            {BeatScript.UPGRADE_NAME, SaveInfo => SaveInfo.sickBeats }
        };

        private static Dictionary<string, Func<SaveInfo, int>> levelConditions = new Dictionary<string, Func<SaveInfo, int>>
        {
            { trapDestroyerScript.UPGRADE_NAME, saveInfo => 0 },
            { terminalFlashScript.UPGRADE_NAME, saveInfo => saveInfo.discoLevel },
            { biggerLungScript.UPGRADE_NAME, saveInfo => saveInfo.lungLevel },
            { runningShoeScript.UPGRADE_NAME, saveInfo => saveInfo.runningLevel },
            { nightVisionScript.UPGRADE_NAME, saveInfo => saveInfo.nightVisionLevel },
            { strongLegsScript.UPGRADE_NAME, saveInfo => saveInfo.legLevel },
            { strongerScannerScript.UPGRADE_NAME, saveInfo => saveInfo.scanLevel },
            { beekeeperScript.UPGRADE_NAME, saveInfo => saveInfo.beeLevel },
            { exoskeletonScript.UPGRADE_NAME, saveInfo => saveInfo.backLevel },
            { lockSmithScript.UPGRADE_NAME, saveInfo => 0 },
            { walkieScript.UPGRADE_NAME, saveInfo => 0 },
            { proteinPowderScript.UPGRADE_NAME, SaveInfo => SaveInfo.proteinLevel },
            { pagerScript.UPGRADE_NAME, SaveInfo => 0 },
            { hunterScript.UPGRADE_NAME, SaveInfo => SaveInfo.huntLevel },
            { lightningRodScript.UPGRADE_NAME, saveInfo => 0},
            { playerHealthScript.UPGRADE_NAME, saveInfo => saveInfo.playerHealthLevel },
            { DoorsHydraulicsBattery.UPGRADE_NAME, saveInfo => saveInfo.doorsHydraulicsBatteryLevel},
            { BeatScript.UPGRADE_NAME, saveInfo => 0 },
        };
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

            GameObject intern = Instantiate(AssetBundleHandler.GetPerkGameObject(defibScript.UPGRADE_NAME));
            intern.GetComponent<NetworkObject>().Spawn();

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
                Color col = UpgradeBus.instance.cfg.NIGHT_VIS_COLOR;
                UpgradeBus.instance.cfg = cfg;
                UpgradeBus.instance.cfg.NIGHT_VIS_COLOR = col; //
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
            if(UpgradeBus.instance.cfg.SHARED_UPGRADES && saves.Count > 0)
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
        public void DeleteUpgradesServerRpc()
        {
            logger.LogInfo($"Deleting Upgrades...");
            int i = 0;
            BaseUpgrade[] upgradeObjects = GameObject.FindObjectsOfType<BaseUpgrade>();
            foreach (BaseUpgrade upgrade in upgradeObjects)
            {
                i++;
                GameNetworkManager.Destroy(upgrade.gameObject);
            }
            logger.LogInfo($"Deleted {i} Upgrade Objects.");
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
            UpgradeBus.instance.DestroyTraps = saveInfo.DestroyTraps;
            UpgradeBus.instance.scannerUpgrade = saveInfo.scannerUpgrade;
            UpgradeBus.instance.nightVision = saveInfo.nightVision;
            UpgradeBus.instance.exoskeleton = saveInfo.exoskeleton;
            UpgradeBus.instance.TPButtonPressed = saveInfo.TPButtonPressed;
            UpgradeBus.instance.beekeeper = saveInfo.beekeeper;
            UpgradeBus.instance.terminalFlash = saveInfo.terminalFlash;
            UpgradeBus.instance.strongLegs = saveInfo.strongLegs;
            UpgradeBus.instance.runningShoes = saveInfo.runningShoes;
            UpgradeBus.instance.biggerLungs = saveInfo.biggerLungs;
            UpgradeBus.instance.lockSmith = saveInfo.lockSmith;
            UpgradeBus.instance.proteinPowder = saveInfo.proteinPowder;
            UpgradeBus.instance.lightningRod = saveInfo.lightningRod;
            UpgradeBus.instance.pager = saveInfo.pager;
            UpgradeBus.instance.hunter = saveInfo.hunter;
            UpgradeBus.instance.playerHealth = saveInfo.playerHealth;
            UpgradeBus.instance.wearingHelmet = saveInfo.wearingHelmet;
            UpgradeBus.instance.sickBeats = saveInfo.sickBeats;
            UpgradeBus.instance.doorsHydraulicsBattery = saveInfo.doorsHydraulicsBattery;

            UpgradeBus.instance.beeLevel = saveInfo.beeLevel;
            UpgradeBus.instance.huntLevel = saveInfo.huntLevel;
            UpgradeBus.instance.proteinLevel = saveInfo.proteinLevel;
            UpgradeBus.instance.lungLevel = saveInfo.lungLevel;
            UpgradeBus.instance.walkies = saveInfo.walkies;
            UpgradeBus.instance.backLevel = saveInfo.backLevel;
            UpgradeBus.instance.runningLevel = saveInfo.runningLevel;
            UpgradeBus.instance.discoLevel = saveInfo.discoLevel;
            UpgradeBus.instance.legLevel = saveInfo.legLevel;
            UpgradeBus.instance.scanLevel = saveInfo.scanLevel;
            UpgradeBus.instance.nightVisionLevel = saveInfo.nightVisionLevel;
            UpgradeBus.instance.playerHealthLevel = saveInfo.playerHealthLevel;
            UpgradeBus.instance.doorsHydraulicsBatteryLevel = saveInfo.doorsHydraulicsBatteryLevel;

            UpgradeBus.instance.contractLevel = saveInfo.contractLevel;
            UpgradeBus.instance.contractType = saveInfo.contractType;

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
                if (conditions.TryGetValue(customNode.Name, out var condition) && condition(saveInfo))
                {
                    logger.LogInfo($"Activated {customNode.Name}!");
                    customNode.Unlocked = true;
                    levelConditions.TryGetValue(customNode.Name, out var level);
                    customNode.CurrentUpgrade = level.Invoke(saveInfo);
                    UpgradeBus.instance.UpgradeObjects[customNode.Name].GetComponent<BaseUpgrade>().load();
                }
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
                UpgradeBus.instance.UpgradeObjects[name].GetComponent<BaseUpgrade>().load(); 
                logger.LogInfo($"First purchase, executing BaseUpgrade.load()");
            }
            else 
            {
                UpgradeBus.instance.UpgradeObjects[name].GetComponent<BaseUpgrade>().Increment(); 
                logger.LogInfo($"upgrade already unlocked, executing BaseUpgrade.Increment()");
            }
            saveInfo = new SaveInfo();
            UpdateLGUSaveServerRpc(playerID, JsonConvert.SerializeObject(saveInfo));
        }

        [ServerRpc(RequireOwnership = false)]
        public void EnableNightVisionServerRpc()
        {
            logger.LogInfo("Enabling night vision for all clients...");
            EnableNightVisionClientRpc();
        }

        [ClientRpc]
        private void EnableNightVisionClientRpc()
        {
            logger.LogInfo("Request to enable night vision on this client received.");
            UpgradeBus.instance.UpgradeObjects[nightVisionScript.UPGRADE_NAME].GetComponent<nightVisionScript>().EnableOnClient();
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
                UpgradeBus.instance.UpgradeObjects[name].GetComponent<BaseUpgrade>().load(); 
                logger.LogInfo($"First purchase, executing BaseUpgrade.load()");
            }
            else 
            {
                UpgradeBus.instance.UpgradeObjects[name].GetComponent<BaseUpgrade>().Increment(); 
                logger.LogInfo($"upgrade already unlocked, executing BaseUpgrade.Increment()");
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
        public void CoordinateInterceptionClientRpc()
        {
            logger.LogInfo("Setting lighting to intercepted on this client...");
            lightningRodScript.instance.LightningIntercepted = true;
            FindObjectOfType<StormyWeather>(true).staticElectricityParticle.gameObject.SetActive(false);
        }

        [ClientRpc]
        public void SyncValuesClientRpc(int value, NetworkBehaviourReference netRef)
        {
            netRef.TryGet(out MonsterSample prop);
            if (prop != null)
            {
                prop.scrapValue = value;
                prop.itemProperties.creditsWorth = value;
                prop.GetComponentInChildren<ScanNodeProperties>().subText = $"Value: ${value}";
                logger.LogInfo($"Successfully synced values of {prop.itemProperties.itemName}");
            }
            else logger.LogInfo("Unable to resolve net ref for SyncValuesClientRpc!");
        }

        [ServerRpc(RequireOwnership = false)]
        public void SpawnNightVisionItemOnDeathServerRpc(Vector3 position)
        {
            GameObject go = Instantiate(UpgradeBus.instance.nightVisionPrefab, position + Vector3.up, Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
            logger.LogInfo("Request to spawn night vision goggles received.");
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

        [ClientRpc]
        public void ExtendDeadlineClientRpc(int days)
        {
            float before = TimeOfDay.Instance.timeUntilDeadline;
            TimeOfDay.Instance.timeUntilDeadline += TimeOfDay.Instance.totalTime * days;
            TimeOfDay.Instance.UpdateProfitQuotaCurrentTime();
            TimeOfDay.Instance.SyncTimeClientRpc(TimeOfDay.Instance.globalTime, (int)TimeOfDay.Instance.timeUntilDeadline);
            logger.LogDebug($"Previous time: {before}, new time: {TimeOfDay.Instance.timeUntilDeadline}");
        }

        [ServerRpc(RequireOwnership = false)]
        public void ExtendDeadlineServerRpc(int days)
        {
            ExtendDeadlineClientRpc(days);
        }

    }

    [Serializable]
    public class SaveInfo
    {
        public bool DestroyTraps = UpgradeBus.instance.DestroyTraps;
        public bool scannerUpgrade = UpgradeBus.instance.scannerUpgrade;
        public bool nightVision = UpgradeBus.instance.nightVision;
        public bool exoskeleton = UpgradeBus.instance.exoskeleton;
        public bool TPButtonPressed = UpgradeBus.instance.TPButtonPressed;
        public bool beekeeper = UpgradeBus.instance.beekeeper;
        public bool terminalFlash = UpgradeBus.instance.terminalFlash;
        public bool strongLegs = UpgradeBus.instance.strongLegs;
        public bool proteinPowder = UpgradeBus.instance.proteinPowder;
        public bool runningShoes = UpgradeBus.instance.runningShoes;
        public bool biggerLungs = UpgradeBus.instance.biggerLungs;
        public bool lockSmith = UpgradeBus.instance.lockSmith;
        public bool walkies = UpgradeBus.instance.walkies;
        public bool lightningRod = UpgradeBus.instance.lightningRod;
        public bool pager = UpgradeBus.instance.pager;
        public bool hunter = UpgradeBus.instance.hunter;
        public bool playerHealth = UpgradeBus.instance.playerHealth;
        public bool wearingHelmet = UpgradeBus.instance.wearingHelmet;
        public bool sickBeats = UpgradeBus.instance.sickBeats;
        public bool doorsHydraulicsBattery = UpgradeBus.instance.doorsHydraulicsBattery;

        public int beeLevel = UpgradeBus.instance.beeLevel;
        public int huntLevel = UpgradeBus.instance.huntLevel;
        public int proteinLevel = UpgradeBus.instance.proteinLevel;
        public int lungLevel = UpgradeBus.instance.lungLevel;
        public int backLevel = UpgradeBus.instance.backLevel;
        public int runningLevel = UpgradeBus.instance.runningLevel;
        public int scanLevel = UpgradeBus.instance.scanLevel;
        public int discoLevel = UpgradeBus.instance.discoLevel;
        public int legLevel = UpgradeBus.instance.legLevel;
        public int nightVisionLevel = UpgradeBus.instance.nightVisionLevel;
        public int playerHealthLevel = UpgradeBus.instance.playerHealthLevel;
        public int doorsHydraulicsBatteryLevel = UpgradeBus.instance.doorsHydraulicsBatteryLevel;
        public string contractType = UpgradeBus.instance.contractType;
        public string contractLevel = UpgradeBus.instance.contractLevel;
        public Dictionary<string, float> SaleData = UpgradeBus.instance.SaleData;
    }

    [Serializable]
    public class LGUSave
    {
        public Dictionary<ulong,SaveInfo> playerSaves = new Dictionary<ulong,SaveInfo>();
    }
}
