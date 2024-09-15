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
using static MoreShipUpgrades.Managers.ItemProgressionManager;
using MoreShipUpgrades.Misc.Util;
using System.Text;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Enemies;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Ship;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Items;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Player;

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
        LguSaveV1 oldSave;
        /// <summary>
        /// Player identifier of the client
        /// </summary>
        internal ulong playerID = 0;
        static readonly LguLogger logger = new(nameof(LguStore));
        /// <summary>
        /// Key used to store Lategame Upgrades relevant data
        /// </summary>
        const string saveDataKey = "LGU_SAVE_DATA";
        /// <summary>
        /// Wether this client already received the save from host client or not
        /// </summary>
        private bool receivedSave;
        internal bool alreadyReceivedScrapToUpgrade = false;

        private void Awake()
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
            yield return new WaitForSeconds(3.0f);
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
                oldSave = JsonConvert.DeserializeObject<LguSaveV1>(tempJson);
                File.Delete(filePath);
            }
            string json = (string)ES3.Load(key: saveDataKey, filePath: saveFile, defaultValue: null);
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
        internal void ServerSaveFile()
        {
            string saveFile = GameNetworkManager.Instance.currentSaveFileName;
            LguSave.scrapToUpgrade = UpgradeBus.Instance.scrapToCollectionUpgrade;
            LguSave.contributedValues = UpgradeBus.Instance.contributionValues;
            LguSave.discoveredItems = UpgradeBus.Instance.discoveredItems;
            ItemProgressionManager.Save();
            RandomizeUpgradeManager.Save();
            string json = JsonConvert.SerializeObject(LguSave);
            ES3.Save(key: saveDataKey, value: json, filePath: saveFile);
        }

        /// <summary>
        /// Remove Procedure Call used by clients to notify the server to update a client's save with provided save data
        /// </summary>
        /// <param name="id">Identifier of the client we wish to update the save of</param>
        /// <param name="json">Save data of the client to replace with the current one</param>
        [ServerRpc(RequireOwnership = false)]
        public void UpdateLGUSaveServerRpc(ulong id, byte[] json, bool saveFile = false)
        {
            string jsonString = Encoding.ASCII.GetString(json);
            LguSave.playerSaves[id] = JsonConvert.DeserializeObject<SaveInfo>(jsonString);
            logger.LogInfo($"Received and updated save info for client: {id}");
            if (saveFile) ServerSaveFile();
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
            StartCoroutine(WaitForUpgradeObject());
        }

        /// <summary>
        /// Remote Procedure Call used by clients to notify the server to share the host's current save file to other clients in the current game session.
        /// </summary>
        [ServerRpc(RequireOwnership = false)]
        public void ShareSaveServerRpc()
        {
            string json = JsonConvert.SerializeObject(LguSave);
            ShareSaveClientRpc(Encoding.ASCII.GetBytes(json));
            List<StringContainer> scraps = [];
            List<StringContainer> upgrades = [];
            foreach (KeyValuePair<string, List<string>> pair in UpgradeBus.Instance.scrapToCollectionUpgrade)
            {
                foreach (string upgradeName in pair.Value)
                {
                    scraps.Add(new StringContainer() { SomeText = pair.Key });
                    upgrades.Add(new StringContainer() { SomeText = upgradeName });
                }
            }
            SetScrapToUpgradeDictionaryClientRpc([.. scraps], [.. upgrades]);
        }

        [ClientRpc]
        internal void SetScrapToUpgradeDictionaryClientRpc(StringContainer[] scraps, StringContainer[] upgrades)
        {
            if (alreadyReceivedScrapToUpgrade) return;
            for(int i = 0; i < scraps.Length; i++)
                AddScrapToUpgrade(upgrades[i].SomeText, scraps[i].SomeText);
            alreadyReceivedScrapToUpgrade = true;
        }

        /// <summary>
        /// Remote Procedure Call used by server to notify the clients to use the provided save file as the save file of the current game session.
        /// </summary>
        /// <param name="json">Structure which contains Lategame Upgrade's relevant save data</param>
        [ClientRpc]
        public void ShareSaveClientRpc(byte[] json)
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
            LguSave = JsonConvert.DeserializeObject<LguSave>(Encoding.ASCII.GetString(json));
            List<ulong> saves = [.. LguSave.playerSaves.Keys];
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
        /// Remote Procedure Call used by the clients to notify the server that a new player has joined the current game session and to save its data as the one provided
        /// </summary>
        /// <param name="id">Identifier of the client that joined the game session for the first time</param>
        /// <param name="json">Lategame Upgrade's relevant save data associated with the new client</param>
        [ServerRpc(RequireOwnership =false)]
        private void RegisterNewPlayerServerRpc(ulong id, byte[] json)
        {
            LguSave.playerSaves.Add(id,JsonConvert.DeserializeObject<SaveInfo>(Encoding.ASCII.GetString(json)));
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
                UpgradeBus.Instance.activeUpgrades[ShutterBatteries.UPGRADE_NAME] = saveInfo.doorsHydraulicsBattery;
                UpgradeBus.Instance.upgradeLevels[ShutterBatteries.UPGRADE_NAME] = saveInfo.doorsHydraulicsBatteryLevel;
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
                    RegisterNewPlayerServerRpc(playerID, Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(SaveInfo)));
                    StartCoroutine(WaitForUpgradeObject());
                    return;
                }
                SaveInfo = LguSave.playerSaves[playerID];
                logger.LogInfo($"Successfully loaded save data for ID: {playerID}");
            }
            UpgradeBus.Instance.activeUpgrades = SaveInfo.activeUpgrades;
            UpgradeBus.Instance.upgradeLevels = SaveInfo.upgradeLevels;
            UpgradeBus.Instance.discoveredItems = LguSave.discoveredItems;
            UpgradeBus.Instance.contributionValues = LguSave.contributedValues;
            UpgradeBus.Instance.scrapToCollectionUpgrade = LguSave.scrapToUpgrade;

            if (oldSave != null && oldSave.playerSaves.ContainsKey(playerID))
            {
                CheckForOldData(oldSave.playerSaves[playerID]);
                ServerSaveFileServerRpc();
            }

            ContractManager.Instance.contractLevel = SaveInfo.contractLevel;
            ContractManager.Instance.contractType = SaveInfo.contractType;

            UpgradeBus.Instance.SaleData = SaveInfo.SaleData;
            UpgradeBus.Instance.LoadSales();

            StartCoroutine(WaitForUpgradeObject());
        }
        private IEnumerator WaitForSteamID()
        {
            yield return new WaitWhile(() => GameNetworkManager.Instance.localPlayerController == null);
            if (!GameNetworkManager.Instance.disableSteam)
            {
                while (playerID == 0)
                {
                    PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
                    playerID = player.playerSteamId;
                    yield return new WaitForSeconds(1f);
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
            yield return new WaitForSeconds(4f);
            logger.LogInfo("Applying loaded upgrades...");
            foreach (CustomTerminalNode customNode in UpgradeBus.Instance.terminalNodes)
            {
                bool activeUpgrade = UpgradeBus.Instance.activeUpgrades.GetValueOrDefault(customNode.OriginalName, false);
                int upgradeLevel = UpgradeBus.Instance.upgradeLevels.GetValueOrDefault(customNode.OriginalName, 0);
                customNode.Unlocked = activeUpgrade;
                customNode.CurrentUpgrade = upgradeLevel;
                BaseUpgrade comp = UpgradeBus.Instance.UpgradeObjects[customNode.OriginalName].GetComponent<BaseUpgrade>();
                bool free = comp.CanInitializeOnStart;
                if (activeUpgrade || free) comp.Load();
                if (free)
                {
                    customNode.Unlocked = true;
                }
            }
            RandomizeUpgradeManager.SetRandomUpgradeSeed(LguSave.randomUpgradeSeed);
            RandomizeUpgradeManager.RandomizeUpgrades();
        }
        internal void UpdateUpgrades(CustomTerminalNode node, bool increment = false)
        {
            node.Unlocked = true;
            BaseUpgrade upgrade = UpgradeBus.Instance.UpgradeObjects[node.OriginalName].GetComponent<BaseUpgrade>();
            if (increment)
            {
                node.CurrentUpgrade++;
                if (upgrade is TierUpgrade tierUpgrade)
                {
                    logger.LogInfo("upgrade already unlocked, executing TierUpgrade.Increment()");
                    tierUpgrade.Increment();
                }
                else
                {
                    logger.LogInfo("Upgrade cannot be incremented. Skipping...");
                }
            }
            else
            {
                logger.LogInfo("First purchase, executing BaseUpgrade.load()");
                upgrade.Load();
            }
            logger.LogInfo($"Node found ({node.OriginalName}) and unlocked (level = {node.CurrentUpgrade})");

            SetContributionValue(node.OriginalName, 0);

            if (node.SalePercentage != 1f && UpgradeBus.Instance.PluginConfiguration.SALE_APPLY_ONCE.Value)
            {
                node.SalePercentage = 1f;
                UpgradeBus.Instance.SaleData[node.Name] = node.SalePercentage;
            }

            SaveInfo = new SaveInfo();
            UpdateLGUSaveServerRpc(playerID, Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(SaveInfo)));
        }
        public void HandleUpgrade(CustomTerminalNode node, bool increment = false)
        {
            if (node.SharedUpgrade)
            {
                logger.LogInfo($"{node.OriginalName} is registered as a shared upgrade! Calling ServerRpc...");
                HandleUpgradeServerRpc(node.OriginalName, increment);
                return;
            }
            logger.LogInfo($"{node.OriginalName} is not registered as a shared upgrade! Unlocking on this client only...");
            UpdateUpgrades(node, increment);
        }

        [ServerRpc(RequireOwnership = false)]
        private void HandleUpgradeServerRpc(string name, bool increment)
        {
            logger.LogInfo($"Received server request to handle shared upgrade for: {name} increment: {increment}");
            HandleUpgradeClientRpc(name, increment);
        }

        [ClientRpc]
        public void HandleUpgradeClientRpc(string name, bool increment)
        {
            logger.LogInfo($"Received client request to handle shared upgrade for: {name} increment: {increment}");
            foreach (CustomTerminalNode node in UpgradeBus.Instance.terminalNodes)
                if (node.OriginalName == name) UpdateUpgrades(node, increment);
        }
        [ClientRpc]
        public void HandleUpgradeForNoHostClientRpc(string name, bool increment)
        {
            if (IsHost) return;
            logger.LogInfo($"Received client request to handle shared upgrade for: {name} increment: {increment}");
            foreach (CustomTerminalNode node in UpgradeBus.Instance.terminalNodes)
                if (node.OriginalName == name) UpdateUpgrades(node, increment);
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
            UpgradeBus.Instance.SaleData = [];
            foreach (CustomTerminalNode node in UpgradeBus.Instance.terminalNodes)
            {
                if (UnityEngine.Random.value > UpgradeBus.Instance.PluginConfiguration.SALE_PERC.Value)
                {
                    node.SalePercentage = UnityEngine.Random.Range(0.60f, 0.90f);
                    logger.LogInfo($"Set sale percentage to: {node.SalePercentage} for {node.Name}.");
                }
                else
                {
                    node.SalePercentage = 1f;
                }
                if (IsHost || IsServer)
                {
                    logger.LogInfo("Saving node into save");
                    UpgradeBus.Instance.SaleData.Add(node.Name, node.SalePercentage);
                }
            }
            SaveInfo = new SaveInfo();
            UpdateLGUSaveServerRpc(playerID, Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(SaveInfo)), true);
        }

        [ClientRpc]
        internal void SetContributionValueClientRpc(string key, int value)
        {
            SetContributionValue(key, value);
        }

        [ClientRpc]
        internal void DiscoverItemClientRpc(string scrapName)
        {
            DiscoverScrap(scrapName);
        }
        [ServerRpc(RequireOwnership = false)]
        internal void RandomizeUpgradesServerRpc()
        {
            RandomizeUpgradesClientRpc(RandomizeUpgradeManager.GetRandomUpgradeSeed());
        }

        [ClientRpc]
        internal void RandomizeUpgradesClientRpc(int seed)
        {
            RandomizeUpgradeManager.RandomizeUpgrades(seed);
        }

        [ServerRpc(RequireOwnership = false)]
        internal void AddUpgradeSpentCreditsServerRpc(int price)
        {
            PlayerManager.instance.IncreaseUpgradeSpentCredits(price);
        }
    }

    [Serializable]
    public class SaveInfo
    {
        public Dictionary<string, bool> activeUpgrades;
        public Dictionary<string, int> upgradeLevels;

        public string contractType;
        public string contractLevel;
        public Dictionary<string, float> SaleData;

        public string Version = "V2";

        public SaveInfo()
        {
            activeUpgrades = new(UpgradeBus.Instance.activeUpgrades);
            upgradeLevels = new(UpgradeBus.Instance.upgradeLevels);
            SaleData = new(UpgradeBus.Instance.SaleData);

            contractType = ContractManager.Instance.contractType;
            contractLevel = ContractManager.Instance.contractLevel;
        }
    }

    [Serializable]
    public class LguSave
    {
        public Dictionary<ulong, SaveInfo> playerSaves = [];
        public Dictionary<string, List<string>> scrapToUpgrade;
        public Dictionary<string, int> contributedValues;
        public List<string> discoveredItems;
        public int randomUpgradeSeed;
    }
}
