using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

namespace MoreShipUpgrades.Managers
{
    internal class ContractManager : NetworkBehaviour
    {
        #region Static Variables
        static LguLogger logger = new LguLogger(nameof(ContractManager));
        internal static ContractManager Instance {get; set; }
        /// <summary>
        /// Terminal node for "route" commands used to select the available moons for contract placements
        /// </summary>
        private static TerminalKeyword routeKeyword;
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
        internal int lastContractIndex = -1;
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
        internal Dictionary<string, List<string>> fakeBombOrders = new Dictionary<string, List<string>>();
        /// <summary>
        /// The correct combination to defuse the bomb for loot
        /// </summary>
        internal List<string> bombOrder = new List<string>();
        /// <summary>
        /// Serial number associated with the bomb item
        /// </summary>
        internal string SerialNumber;
        #endregion
        #endregion

        void Awake()
        {
            Instance = this;
        }

        [ServerRpc(RequireOwnership = false)]
        internal void ReqSyncContractDetailsServerRpc(string contractLvl, int contractType)
        {
            SyncContractDetailsClientRpc(contractLvl, contractType);
            logger.LogInfo("Syncing contract details on all clients...");
        }

        [ClientRpc]
        internal void SyncContractDetailsClientRpc(string contractLvl = "None", int contractType = -1)
        {
            contractLevel = contractLvl;
            if (contractType == -1) this.contractType = "None";
            else
            {
                this.contractType = CommandParser.contracts[contractType];
                lastContractIndex = contractType;
            }
            fakeBombOrders = new Dictionary<string, List<string>>();
            logger.LogInfo($"New contract details received. level: {contractLvl}, type: {contractType}");
        }

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

        internal static (string, LevelWeatherType) PickWeather(string levelName, string weather = "")
        {
            SelectableLevel[] availableLevels = StartOfRound.Instance.levels;
            SelectableLevel selectedLevel = Array.Find(availableLevels, x => x.PlanetName.ToLower().Contains(levelName));
            if (selectedLevel == null) return (null, LevelWeatherType.None);
            LevelWeatherType selectedWeather = selectedLevel.overrideWeather ? selectedLevel.overrideWeatherType : selectedLevel.currentWeather;
            switch(weather)
            {
                case "clear":
                case "none":
                    {
                        return (selectedLevel.PlanetName, LevelWeatherType.None);
                    }
                case "": return RandomizeWeather(ref selectedLevel);
            }
            LevelWeatherType newSelectedWeather = selectedLevel.randomWeathers.Select(x => x.weatherType).FirstOrDefault(x => x.ToString().ToLower().Contains(weather));
            return (selectedLevel.PlanetName, newSelectedWeather);
        }

        internal static (string, LevelWeatherType) RandomizeWeather(ref SelectableLevel level)
        {
            LevelWeatherType selectedWeather = level.overrideWeather ? level.overrideWeatherType : level.currentWeather;
            LevelWeatherType[] allowedWeathers = level.randomWeathers.Select(x => x.weatherType).Where(x => x != selectedWeather).ToArray();
            int selectedWeatherValue = UnityEngine.Random.Range(0, allowedWeathers.Length+1);
            if (selectedWeatherValue == allowedWeathers.Length)
            {
                if (selectedWeather == LevelWeatherType.None)
                {
                    LevelWeatherType newSelectedWeather = allowedWeathers[UnityEngine.Random.Range(0, allowedWeathers.Length)];
                    return (level.PlanetName, newSelectedWeather);
                }
                else return (level.PlanetName, LevelWeatherType.None);
            }
            else
            {
                LevelWeatherType newSelectedWeather = allowedWeathers[selectedWeatherValue];
                return (level.PlanetName, newSelectedWeather);
            }
        }
        internal void ResetAllValues()
        {
            contractType = "None";
            contractLevel = "None";
        }
    }
}
