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
using MoreShipUpgrades.Compat;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using HarmonyLib;

namespace MoreShipUpgrades.Managers
{
    /// <summary>
    /// Manager which handles Lategame Upgrades saving and upgrade purchases
    /// </summary>
    public class LguStore : NetworkBehaviour
    {
        public static LguStore Instance { get; internal set; }
        /// <summary>
        /// Client's Lategame Upgrades save data
        /// </summary>
        public SaveInfo SaveInfo { get; internal set; }
        /// <summary>
        /// Lategame upgrades saves of all connected players
        /// </summary>
        public LguSave LguSave { get; internal set; }
        /// <summary>
        /// Old save format of Lategame Upgrades
        /// </summary>
        public LGUSaveV1 oldSave;
        /// <summary>
        /// Player identifier of the client
        /// </summary>
        private ulong playerID = 0;
        static LguLogger logger = new LguLogger(nameof(LguStore));
        /// <summary>
        /// Key used to store Lategame Upgrades relevant data
        /// </summary>
        const string saveDataKey = "LGU_SAVE_DATA";
        /// <summary>
        /// Wether this client already received the save from host client or not
        /// </summary>
        private bool receivedSave;

        private void Start()
        {
            Instance = this;
            if (NetworkManager.IsHost)
            {
                FetchLGUSaveFile();
                UpdateUpgradeBus();
                UpgradeBus.Instance.Reconstruct();
                HandleSpawns();
            }
            else
            {
                StartCoroutine(WaitForConfigSync());
            }
        }
        /// <summary>
        /// Waits for set amount of time before reconstructing necessary components from the configuration received by host
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitForConfigSync()
        {
            yield return new WaitForSeconds(2.5f);
            UpgradeBus.Instance.Reconstruct();
        }
        /// <summary>
        /// Retrieves the save file relevant to Lategame Upgrades to retrieve the data of the player's upgrades and other attributes saved
        /// </summary>
        void FetchLGUSaveFile()
        {
            string saveFile = GameNetworkManager.Instance.currentSaveFileName;
            int saveNum = GameNetworkManager.Instance.saveFileNum;
            string filePath = Path.Combine(Application.persistentDataPath, $"LGU_{saveNum}.json");
            if (File.Exists(filePath))
            {
                string tempJson = File.ReadAllText(filePath);
                oldSave = JsonConvert.DeserializeObject<LGUSaveV1>(tempJson);
                File.Delete(filePath);
            }
            string json = (string)ES3.Load(key: saveDataKey, defaultValue: null, filePath: saveFile);
            if (json != null)
            {
                logger.LogInfo($"Loading save file for slot {saveNum}.");
                LguSave = JsonConvert.DeserializeObject<LguSave>(json);
            }
            else
            {
                logger.LogInfo($"No save file found for slot {saveNum}. Creating new.");
                LguSave = new LguSave();
            }
        }
        /// <summary>
        /// Remote Procedure Call used by clients to notify the server to save everyone's state into the save file
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        public void ServerSaveFileServerRpc()
        {
            ServerSaveFile();
        }

        /// <summary>
        /// Stores Lategame Upgrades' relevant save data into the game's current save file.
        /// </summary>
        void ServerSaveFile()
        {
            string saveFile = GameNetworkManager.Instance.currentSaveFileName;
            string json = JsonConvert.SerializeObject(LguSave);
            ES3.Save(key: saveDataKey, value: json, filePath: saveFile);
        }

        /// <summary>
        /// Remove Procedure Call used by clients to notify the server to update a client's save with provided save data
        /// </summary>
        /// <param name="id">Identifier of the client we wish to update the save of</param>
        /// <param name="json">Save data of the client to replace with the current one</param>
        [ServerRpc(RequireOwnership = false)]
        public void UpdateLGUSaveServerRpc(ulong id, string json)
        {
            LguSave.playerSaves[id] = JsonConvert.DeserializeObject<SaveInfo>(json);
            logger.LogInfo($"Received and updated save info for client: {id}");
        }
        /// <summary>
        /// Spawns all relevant upgrade and command managers into the scene
        /// </summary>
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

            GameObject intern = Instantiate(AssetBundleHandler.GetPerkGameObject(Interns.NAME));
            intern.hideFlags = HideFlags.HideAndDontSave;
            intern.GetComponent<NetworkObject>().Spawn();

            GameObject extendDeadline = Instantiate(AssetBundleHandler.GetPerkGameObject(ExtendDeadlineScript.NAME));
            intern.hideFlags = HideFlags.HideAndDontSave;
            extendDeadline.GetComponent<NetworkObject>().Spawn();

            GameObject insurance = Instantiate(AssetBundleHandler.GetPerkGameObject(ScrapInsurance.COMMAND_NAME));
            intern.hideFlags = HideFlags.HideAndDontSave;
            insurance.GetComponent<NetworkObject>().Spawn();
        }
        /// <summary>
        /// Remote Procedure Call used by clients to notify the server that the game's current run has reached its end and must reset Lategame Upgrade's upgrades and other attributes to default
        /// </summary>
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

        /// <summary>
        /// Remote Procedure Call used by server to notify the clients to reset their Lategame Upgrade's stats to default and clear any save relevant data
        /// </summary>
        [ClientRpc]
        private void ResetUpgradeBusClientRpc()
        {
            logger.LogInfo("Players fired, reseting upgrade bus.");
            UpgradeBus.Instance.ResetAllValues(false);
            if(!IsHost || !IsServer) LguSave = new LguSave();
            SaveInfo = new SaveInfo();
        }

        /// <summary>
        /// Remote Procedure Call used by clients to notify the server to share the host's current save file to other clients in the current game session.
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        public void ShareSaveServerRpc()
        {
            string json = JsonConvert.SerializeObject(LguSave);
            ShareSaveClientRpc(json);
        }

        /// <summary>
        /// Remote Procedure Call used by server to notify the clients to use the provided save file as the save file of the current game session.
        /// </summary>
        /// <param name="json">Structure which contains Lategame Upgrade's relevant save data</param>
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
        /// <summary>
        /// Remote Procedure Call used by clients to notify the server to change the current amount of credits in the current game session to the provided amount
        /// </summary>
        /// <param name="credits">New amount of credits to set on the current game session</param>
        [ServerRpc(RequireOwnership = false)]
        public void SyncCreditsServerRpc(int credits)
        {
            logger.LogInfo($"Request to sync credits to ${credits} received, calling ClientRpc...");
            SyncCreditsClientRpc(credits);
        }
        /// <summary>
        /// Remote Procedure Call used by server to notify the clients to change the current amount of credits in the current game session to the provided amount
        /// </summary>
        /// <param name="newCredits">New amount of credits to set on the current game session</param>
        [ClientRpc]
        public void SyncCreditsClientRpc(int newCredits)
        {
            SyncCredits(newCredits);
        }

        /// <summary>
        /// Changes the current amount of credits in the current game session to the provided amount
        /// </summary>
        /// <param name="newCredits">New amount of credits to set on the current game session</param>
        void SyncCredits(int newCredits)
        {
            logger.LogInfo($"Credits have been synced to ${newCredits}");
            Terminal terminal = UpgradeBus.Instance.GetTerminal();
            terminal.groupCredits = newCredits;
        }

        /// <summary>
        /// Remote Procedure Call used by the clients to notify the server that a new player has joined the current game session and to save its data as the one provided
        /// </summary>
        /// <param name="id">Identifier of the client that joined the game session for the first time</param>
        /// <param name="json">Lategame Upgrade's relevant save data associated with the new client</param>
        [ServerRpc(RequireOwnership =false)]
        private void RegisterNewPlayerServerRpc(ulong id, string json)
        {
            LguSave.playerSaves.Add(id,JsonConvert.DeserializeObject<SaveInfo>(json));
        }
        /// <summary>
        /// Method to get old data from json and store in the new dictionaries
        /// </summary>
        /// <param name="saveInfo">Save that might contain old data</param>
        void CheckForOldData(SaveInfoV1 saveInfo)
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

            if (oldSave != null && oldSave.playerSaves.ContainsKey(playerID))
            {
                CheckForOldData(oldSave.playerSaves[playerID]);
                ServerSaveFileServerRpc();
            }

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
            if(!GameNetworkManager.Instance.disableSteam)
            {
                int tries = 0;
                while (playerID == 0 && tries < 10)
                {
                    tries++;
                    PlayerControllerB player = UpgradeBus.Instance.GetLocalPlayer();
                    if (player == null) continue;
                    playerID = player.playerSteamId;
                    yield return new WaitForSeconds(0.5f);
                }
            }
            
            if (playerID != 0)
            {
                logger.LogInfo($"Loading SteamID: {playerID}");
                UpdateUpgradeBus();
            }
            else
            {
                logger.LogInfo("LAN detected, loading under ID 0");
                UpdateUpgradeBus(true, false);
            }
        }

        private IEnumerator WaitForUpgradeObject()
        {
            yield return new WaitForSeconds(2f);
            logger.LogInfo("Applying loaded upgrades...");
            foreach (CustomTerminalNode customNode in UpgradeBus.Instance.terminalNodes)
            {
                bool activeUpgrade = UpgradeBus.Instance.activeUpgrades.GetValueOrDefault(customNode.Name, false);
                int upgradeLevel = UpgradeBus.Instance.upgradeLevels.GetValueOrDefault(customNode.Name, 0);
                customNode.Unlocked = activeUpgrade;
                customNode.CurrentUpgrade = upgradeLevel;
                BaseUpgrade comp = UpgradeBus.Instance.UpgradeObjects[customNode.Name].GetComponent<BaseUpgrade>();
                bool free = comp.CanInitializeOnStart();
                if (activeUpgrade || free) comp.Load();
                if (customNode.Name == NightVision.UPGRADE_NAME || free)
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
            GenerateSales(seed);
        }

        internal void GenerateSales(int seed = -1)
        {
            if (seed == -1) seed = UnityEngine.Random.Range(0, 999999);
            logger.LogInfo($"Generating sales with seed: {seed} on this client...");
            UnityEngine.Random.InitState(seed);
            UpgradeBus.Instance.SaleData = new Dictionary<string, float>();
            foreach (CustomTerminalNode node in UpgradeBus.Instance.terminalNodes)
            {
                if (UnityEngine.Random.value > UpgradeBus.Instance.PluginConfiguration.SALE_PERC.Value)
                {
                    node.salePerc = UnityEngine.Random.Range(0.60f, 0.90f);
                    logger.LogInfo($"Set sale percentage to: {node.salePerc} for {node.Name}.");
                }
                else
                {
                    node.salePerc = 1f;
                }
                if (IsHost || IsServer)
                {
                    logger.LogInfo("Saving node into save");
                    UpgradeBus.Instance.SaleData.Add(node.Name, node.salePerc);
                }
            }
            SaveInfo = new SaveInfo();
            UpdateLGUSaveServerRpc(playerID, JsonConvert.SerializeObject(SaveInfo));
            if (IsHost || IsServer) ServerSaveFile();
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

        [ClientRpc]
        internal void ResetShipAttributesClientRpc()
        {
            logger.LogDebug($"Resetting the ship's attributes");
            UpgradeBus.Instance.UpgradeObjects.Values.Where(upgrade => upgrade.GetComponent<GameAttributeTierUpgrade>() is IServerSync).Do(upgrade => upgrade.GetComponent<GameAttributeTierUpgrade>().UnloadUpgradeAttribute());
        }
        [ServerRpc(RequireOwnership = false)]
        internal void SyncWeatherServerRpc(string level, LevelWeatherType selectedWeather)
        {
            SyncWeatherClientRpc(level, selectedWeather);
        }

        [ClientRpc]
        internal void SyncWeatherClientRpc(string level, LevelWeatherType selectedWeather)
        {
            SyncWeather(level, selectedWeather);
        }

        internal void SyncWeather(string level, LevelWeatherType selectedWeather)
        {
            SelectableLevel[] availableLevels = StartOfRound.Instance.levels;
            SelectableLevel selectedLevel = availableLevels.First(x => x.PlanetName.Contains(level));
            if (selectedLevel.overrideWeather) selectedLevel.overrideWeatherType = selectedWeather;
            else selectedLevel.currentWeather = selectedWeather;
            ContractManager.probedWeathers[selectedLevel.PlanetName] = selectedWeather;
            if (selectedLevel == StartOfRound.Instance.currentLevel) StartOfRound.Instance.SetMapScreenInfoToCurrentLevel();
        }
        [ServerRpc(RequireOwnership = false)]
        internal void SyncProbeWeathersServerRpc()
        {
            foreach (string level in ContractManager.probedWeathers.Keys.ToList())
            {
                SyncWeatherClientRpc(level, ContractManager.probedWeathers[level]);
            }
        }
    }

    [Serializable]
    public class SaveInfo
    {
        public Dictionary<string, bool> activeUpgrades = UpgradeBus.Instance.activeUpgrades;
        public Dictionary<string, int> upgradeLevels = UpgradeBus.Instance.upgradeLevels;

        public bool TPButtonPressed = UpgradeBus.Instance.TPButtonPressed;
        public string contractType = ContractManager.Instance.contractType;
        public string contractLevel = ContractManager.Instance.contractLevel;
        public Dictionary<string, float> SaleData = UpgradeBus.Instance.SaleData;
        public bool wearingHelmet = UpgradeBus.Instance.wearingHelmet;

        public string Version = "V2";
    }

    [Serializable]
    public class LguSave
    {
        public Dictionary<ulong, SaveInfo> playerSaves = [];
    }
}
