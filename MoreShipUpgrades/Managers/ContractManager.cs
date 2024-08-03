using LethalLevelLoader;
using LethalLib.Extras;
using LethalLib.Modules;
using MoreShipUpgrades.Compat;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Contracts;
using MoreShipUpgrades.UpgradeComponents.Items.Contracts.BombDefusal;
using MoreShipUpgrades.UpgradeComponents.Items.Contracts.DataRetrieval;
using MoreShipUpgrades.UpgradeComponents.Items.Contracts.Exorcism;
using MoreShipUpgrades.UpgradeComponents.Items.Contracts.Exterminator;
using MoreShipUpgrades.UpgradeComponents.Items.Contracts.Extraction;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    internal class ContractManager : NetworkBehaviour
    {
        #region Static Variables
        static readonly LguLogger logger = new(nameof(ContractManager));
        internal static ContractManager Instance {get; set; }
        /// <summary>
        /// Terminal node for "route" commands used to select the available moons for contract placements
        /// </summary>
        private static TerminalKeyword routeKeyword;
        internal readonly static Dictionary<string, LevelWeatherType> probedWeathers = [];
        #endregion
        #region Variables
        #region Selected Contract
        /// <summary>
        /// Chosen level (moon) of the selected contract
        /// </summary>
        internal string contractLevel = "None";
        /// <summary>
        /// Chose type of the selected contract
        /// </summary>
        internal string contractType = "None";
        /// <summary>
        /// Index of the last picked contract type
        /// </summary>
        internal int lastContractIndex = UNDEFINED_INDEX;
        #endregion
        #region Data Contract
        /// <summary>
        /// Correct IP Address to bruteforce for user credentials
        /// </summary>
        internal string DataMinigameKey = "";
        /// <summary>
        /// Correct username to log in the PC
        /// </summary>
        internal string DataMinigameUser = "";
        /// <summary>
        /// Correct password to log in the PC
        /// </summary>
        internal string DataMinigamePass = "";
        #endregion
        #region Defusal Contract
        /// <summary>
        /// Possible combinations of triggering the bomb to activate on wrong wire sequence
        /// </summary>
        internal Dictionary<string, List<string>> fakeBombOrders = [];
        /// <summary>
        /// The correct combination to defuse the bomb for loot
        /// </summary>
        internal List<string> bombOrder = [];
        /// <summary>
        /// Serial number associated with the bomb item
        /// </summary>
        internal string SerialNumber;
        #endregion
        #endregion

        const int UNDEFINED_INDEX = -1;

        void Awake()
        {
            Instance = this;
        }
        /// <summary>
        /// Remote procedure call used by clients to notify the server to update the current contract details to provided ones
        /// </summary>
        /// <param name="contractLvl">Moon of the new contract</param>
        /// <param name="contractType">Type of the new contract</param>
        [ServerRpc(RequireOwnership = false)]
        internal void ReqSyncContractDetailsServerRpc(string contractLvl, int contractType)
        {
            SyncContractDetailsClientRpc(contractLvl, contractType);
            logger.LogInfo("Syncing contract details on all clients...");
        }

        /// <summary>
        /// Remote procedure call used by server to notify the clients to update the current contract details to provided ones
        /// </summary>
        /// <param name="contractLvl">Moon of the new contract</param>
        /// <param name="contractType">Type of the new contract</param>
        [ClientRpc]
        internal void SyncContractDetailsClientRpc(string contractLvl = "None", int contractType = UNDEFINED_INDEX)
        {
            contractLevel = contractLvl;
            if (contractType == UNDEFINED_INDEX)
            {
                this.contractType = "None";
            }
            else
            {
                this.contractType = CommandParser.contracts[contractType];
                lastContractIndex = contractType;
            }
            fakeBombOrders = [];
            logger.LogInfo($"New contract details received. level: {contractLvl}, type: {contractType}");
            LguStore.Instance.SaveInfo = new SaveInfo();
            LguStore.Instance.UpdateLGUSaveServerRpc(LguStore.Instance.playerID, Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(LguStore.Instance.SaveInfo)), true);
        }
        /// <summary>
        /// Searches through the available moons whose name contains a substring that matches the provided string
        /// </summary>
        /// <param name="moon">Name of a supposed moon</param>
        /// <returns>Full name of the moon if found any occurences, otherwise the name of the last contract moon</returns>
        internal static string GetSpecifiedLevel(string moon)
        {
            string lvl = Instance.contractLevel;
            SelectableLevel[] availableLevels = StartOfRound.Instance.levels;
            for (int i = 0; i < availableLevels.Length; i++)
            {
                logger.LogDebug(availableLevels[i].PlanetName.ToLower());
                if (!availableLevels[i].PlanetName.ToLower().Contains(moon)) continue;
                lvl = availableLevels[i].PlanetName;
                break;
            }
            return lvl;
        }
        /// <summary>
        /// Searches through the available moons and picks an element out of the list that satisfies the criteria
        /// </summary>
        /// <returns>A moon that satifies the criteria, otherwise the name of the last contract moon</returns>
        internal static string RandomLevel()
        {
            string lvl = Instance.contractLevel;
            string lastLevel = null;
            SelectableLevel[] availableLevels = StartOfRound.Instance.levels;
            bool[] usedLevels = new bool[availableLevels.Length];
            for (int i = 0; i < usedLevels.Length; i++)
            {
                usedLevels[i] = false;
            }
            bool allUsed = true;
            while (lvl == Instance.contractLevel)
            {
                int levelIndex = UnityEngine.Random.Range(0, availableLevels.Length);
                if (usedLevels[levelIndex]) continue;
                usedLevels[levelIndex] = true;
                SelectableLevel level = availableLevels[levelIndex];
                lastLevel = level.PlanetName;
                if (level.PlanetName.Contains("Gordion")) continue;
                if (LethalLevelLoaderCompat.Enabled)
                {
                    if (LethalLevelLoaderCompat.IsLocked(ref level)) continue;
                }
                logger.LogDebug($"Picked {level.PlanetName} as possible moon for contract...");
                if (routeKeyword == null) routeKeyword = UpgradeBus.Instance.GetTerminal().terminalNodes.allKeywords.First(k => k.word == "route");
                for (int i = 0; i < routeKeyword.compatibleNouns.Length && lvl == Instance.contractLevel; i++)
                {
                    TerminalNode routeMoon = routeKeyword.compatibleNouns[i].result;
                    int itemCost = routeMoon.itemCost;
                    if (UpgradeBus.Instance.PluginConfiguration.CONTRACT_FREE_MOONS_ONLY.Value && itemCost != 0)
                    {
                        logger.LogDebug($"Criteria algorithm skipped a choice due to configuration only allowing free moons (Choice: {level.PlanetName})");
                        break;
                    }
                    CompatibleNoun[] additionalNodes = routeMoon.terminalOptions;
                    for (int j = 0; j < additionalNodes.Length && lvl == Instance.contractLevel; j++)
                    {
                        TerminalNode confirmNode = additionalNodes[j].result;
                        if (confirmNode == null) continue;
                        if (confirmNode.buyRerouteToMoon != levelIndex) continue;

                        logger.LogDebug($"Criteria algorithm made a choice and decided to assign contract on {level.PlanetName}");
                        lvl = level.PlanetName;
                    }
                }
                if (lvl != Instance.contractLevel) break;
                allUsed = true;
                for (int i = 0; i < usedLevels.Length; i++)
                {
                    allUsed &= usedLevels[i];
                }
                if (allUsed) break;
            }
            if (lvl == Instance.contractLevel && allUsed)
            {
                logger.LogDebug($"Criteria algorithm did not make a choice, we will use the last selected moon ({lastLevel})");
                lvl = lastLevel;
            }
            logger.LogDebug($"{lvl} will be the moon for the random contract...");
            Instance.contractLevel = lvl;
            return lvl;
        }
        /// <summary>
        /// Resets the manager's current state to default
        /// </summary>
        internal void ResetAllValues()
        {
            contractType = "None";
            contractLevel = "None";
        }
        internal static void SetupContractMapObjects(ref AssetBundle bundle)
        {
            AnimationCurve curve = new(new Keyframe(0, 1), new Keyframe(1, 1)); // always spawn 1

            SetupScavContract(ref bundle, curve);
            SetupExterminatorContract(curve);
            SetupDataContract(curve);
            SetupExorcismContract(curve);
            SetupBombContract(curve);
        }

        static void SetupBombContract(AnimationCurve curve)
        {
            Item bomb = AssetBundleHandler.GetItemObject("Bomb");
            bomb.spawnPrefab.AddComponent<ScrapValueSyncer>();
            bomb.isConductiveMetal = false;
            DefusalContract coNest = bomb.spawnPrefab.AddComponent<DefusalContract>();
            coNest.SetPosition = true;

            BombDefusalScript bombScript = bomb.spawnPrefab.AddComponent<BombDefusalScript>();
            bombScript.snip = AssetBundleHandler.GetAudioClip("Bomb Cut");
            bombScript.tick = AssetBundleHandler.GetAudioClip("Bomb Tick");

            RegisterSpawnableContractObject(bomb, curve);
        }

        const int MAXIMUM_RITUAL_ITEMS = 5;
        static void SetupExorcismContract(AnimationCurve curve)
        {
            Item contractLoot = AssetBundleHandler.GetItemObject("Demon Tome");
            contractLoot.spawnPrefab.AddComponent<ScrapValueSyncer>();
            Items.RegisterItem(contractLoot);
            Utilities.FixMixerGroups(contractLoot.spawnPrefab);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(contractLoot.spawnPrefab);

            Item mainItem = AssetBundleHandler.GetItemObject("Pentagram");

            for (int i = 0; i < MAXIMUM_RITUAL_ITEMS; i++)
            {
                Item exorItem = AssetBundleHandler.GetItemObject("RitualItem" + i);
                exorItem.spawnPrefab.AddComponent<ExorcismContract>();
                RegisterSpawnableContractObject(exorItem, new AnimationCurve(new Keyframe(0, 3), new Keyframe(1, 3)));
            }

            ExorcismContract co = mainItem.spawnPrefab.AddComponent<ExorcismContract>();
            co.SetPosition = true;

            PentagramScript pentScript = mainItem.spawnPrefab.AddComponent<PentagramScript>();
            pentScript.loot = contractLoot.spawnPrefab;
            pentScript.chant = AssetBundleHandler.GetAudioClip("Ritual Fail");
            pentScript.portal = AssetBundleHandler.GetAudioClip("Ritual Success");

            RegisterSpawnableContractObject(mainItem, curve);
        }

        static void RegisterSpawnableContractObject(Item item, AnimationCurve curve)
        {
            Utilities.FixMixerGroups(item.spawnPrefab);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(item.spawnPrefab);
            Items.RegisterItem(item);

            SpawnableMapObjectDef mapObjDefBug = ScriptableObject.CreateInstance<SpawnableMapObjectDef>();
            mapObjDefBug.spawnableMapObject = new SpawnableMapObject
            {
                prefabToSpawn = item.spawnPrefab
            };
            MapObjects.RegisterMapObject(mapObjDefBug, Levels.LevelTypes.All, (_) => curve);
        }
        static void SetupExterminatorContract(AnimationCurve curve)
        {
            Item bugLoot = AssetBundleHandler.GetItemObject("HoardingBugEggsLoot");
            bugLoot.spawnPrefab.AddComponent<ScrapValueSyncer>();
            Items.RegisterItem(bugLoot);
            Utilities.FixMixerGroups(bugLoot.spawnPrefab);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(bugLoot.spawnPrefab);

            Item nest = AssetBundleHandler.GetItemObject("HoardingBugEggs");

            ExterminatorContract coNest = nest.spawnPrefab.AddComponent<ExterminatorContract>();
            coNest.SetPosition = true;

            BugNestScript nestScript = nest.spawnPrefab.AddComponent<BugNestScript>();
            nestScript.loot = bugLoot.spawnPrefab;

            RegisterSpawnableContractObject(nest, curve);
        }

        static void SetupScavContract(ref AssetBundle bundle, AnimationCurve curve)
        {
            Item scav = AssetBundleHandler.GetItemObject("Scavenger");
            if (scav == null) return;

            scav.weight = UpgradeBus.Instance.PluginConfiguration.CONTRACT_EXTRACT_WEIGHT.Value;
            ExtractionContract co = scav.spawnPrefab.AddComponent<ExtractionContract>();
            co.SetPosition = true;

            scav.spawnPrefab.AddComponent<ExtractPlayerScript>();
            scav.spawnPrefab.AddComponent<ScrapValueSyncer>();
            TextAsset scavAudioPaths = AssetBundleHandler.GetGenericAsset<TextAsset>("Scavenger Sounds");
            Dictionary<string, string[]> scavAudioDict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(scavAudioPaths.text);
            ExtractPlayerScript.clipDict.Add("lost", CreateAudioClipArray(scavAudioDict["lost"], ref bundle));
            ExtractPlayerScript.clipDict.Add("heal", CreateAudioClipArray(scavAudioDict["heal"], ref bundle));
            ExtractPlayerScript.clipDict.Add("safe", CreateAudioClipArray(scavAudioDict["safe"], ref bundle));
            ExtractPlayerScript.clipDict.Add("held", CreateAudioClipArray(scavAudioDict["held"], ref bundle));

            RegisterSpawnableContractObject(scav, curve);
        }

        static void SetupDataContract(AnimationCurve curve)
        {
            Item dataLoot = AssetBundleHandler.GetItemObject("Floppy Disk");
            dataLoot.spawnPrefab.AddComponent<ScrapValueSyncer>();
            Items.RegisterItem(dataLoot);
            Utilities.FixMixerGroups(dataLoot.spawnPrefab);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(dataLoot.spawnPrefab);

            Item pc = AssetBundleHandler.GetItemObject("Laptop");

            DataRetrievalContract coPC = pc.spawnPrefab.AddComponent<DataRetrievalContract>();
            coPC.SetPosition = true;

            DataPCScript dataScript = pc.spawnPrefab.AddComponent<DataPCScript>();
            dataScript.error = AssetBundleHandler.GetAudioClip("Laptop Error");
            dataScript.startup = AssetBundleHandler.GetAudioClip("Laptop Start");
            dataScript.loot = dataLoot.spawnPrefab;

            RegisterSpawnableContractObject(pc, curve);
        }
        private static AudioClip[] CreateAudioClipArray(string[] paths, ref AssetBundle bundle)
        {
            AudioClip[] clips = new AudioClip[paths.Length];
            for (int i = 0; i < paths.Length; i++)
            {
                clips[i] = AssetBundleHandler.TryLoadAudioClipAsset(ref bundle, paths[i]);
            }
            return clips;
        }
    }
}
