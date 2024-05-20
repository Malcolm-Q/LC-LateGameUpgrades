using GameNetcodeStuff;
using HarmonyLib;
using LethalLib.Extras;
using LethalLib.Modules;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.UpgradeComponents.Items;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace MoreShipUpgrades.Managers
{
    public class UpgradeBus : MonoBehaviour
    {
        internal static UpgradeBus Instance { get; set; }
        internal LategameConfiguration PluginConfiguration { get; set; }
        private static LguLogger logger = new LguLogger(nameof(UpgradeBus));

        internal Dictionary<string, bool> activeUpgrades = new Dictionary<string, bool>();
        internal Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();
        internal bool TPButtonPressed;
        internal Dictionary<string, float> SaleData = new Dictionary<string, float>();

        internal AudioClip flashNoise;
        internal GameObject modStorePrefab;
        internal TerminalNode modStoreInterface;

        internal Dictionary<string, SpawnableMapObjectDef> spawnableMapObjects = new Dictionary<string, SpawnableMapObjectDef>();
        internal Dictionary<string, int> spawnableMapObjectsAmount = new Dictionary<string, int>();
        internal readonly List<Type> upgradeTypes = new();
        internal readonly List<Type> commandTypes = new();
        internal List<CustomTerminalNode> terminalNodes = new List<CustomTerminalNode>();
        internal Dictionary<string, GameObject> UpgradeObjects = new Dictionary<string, GameObject>();
        internal Dictionary<string, Item> ItemsToSync = new Dictionary<string, Item>();
        internal List<Peeper> coilHeadItems = new List<Peeper>();
        internal AssetBundle UpgradeAssets;

        internal GameObject helmetModel;
        internal Helmet helmetScript;
        internal bool wearingHelmet = false;
        internal int helmetHits = 0;

        internal Dictionary<string, AudioClip> SFX = new Dictionary<string, AudioClip>();
        internal bool helmetDesync;
        internal bool IsBeta = false;

        internal int daysExtended = 0;

        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PluginConfiguration = Plugin.Config;
        }

        public Terminal GetTerminal()
        {
            return GameObject.Find("TerminalScript").GetComponent<Terminal>();
        }
        public PlayerControllerB GetLocalPlayer()
        {
            return GameNetworkManager.Instance.localPlayerController;
        }
        public HangarShipDoor GetShipDoors()
        {
            return FindObjectOfType<HangarShipDoor>();
        }

        public void ResetAllValues(bool wipeObjRefs = true)
        {
            if (LguStore.Instance == null) return; // Quitting the game
            ResetPlayerAttributes();
            if (LguStore.Instance.IsServer || LguStore.Instance.IsHost) LguStore.Instance.ResetShipAttributesClientRpc();

            if (PluginConfiguration.BEATS_ENABLED.Value) SickBeats.Instance.EffectsActive = false;
            if (PluginConfiguration.NIGHT_VISION_ENABLED.Value) NightVision.Instance.nightVisionActive = false;
            ContractManager.Instance.ResetAllValues();

            if (PluginConfiguration.DISCOMBOBULATOR_ENABLED.Value) Discombobulator.instance.flashCooldown = 0f;
            if (PluginConfiguration.BACK_MUSCLES_ENABLED.Value) BackMuscles.Instance.alteredWeight = 1f;
            if (wipeObjRefs) {
                UpgradeObjects = new Dictionary<string, GameObject>();
            }
            foreach(CustomTerminalNode node in terminalNodes)
            {
                node.Unlocked = false;
                node.CurrentUpgrade = 0;
                if(node.Name == NightVision.UPGRADE_NAME) { node.Unlocked = true; }
            }

            foreach (string key in activeUpgrades.Keys.ToList())
                activeUpgrades[key] = false;

            foreach (string key in upgradeLevels.Keys.ToList())
                upgradeLevels[key] = 0;
        }
        private void ResetPlayerAttributes()
        {
            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            if (player == null) return; // Disconnecting the game

            logger.LogDebug($"Resetting {player.playerUsername}'s attributes");
            UpgradeObjects.Values.Where(upgrade => upgrade.GetComponent<GameAttributeTierUpgrade>() is IPlayerSync).Do(upgrade => upgrade.GetComponent<GameAttributeTierUpgrade>().UnloadUpgradeAttribute());
        }

        internal void LoadSales()
        {
            if(SaleData.Count == 0)
            {
                logger.LogInfo("Sale data empty, continuing...");
                return;
            }
            foreach(CustomTerminalNode node in terminalNodes)
            {
                if (!SaleData.ContainsKey(node.Name)) continue;
                node.salePerc = SaleData[node.Name];
                if(node.salePerc != 1f)
                {
                    logger.LogInfo($"Loaded sale of {node.salePerc} for {node.Name}.");
                }
            }
        }

        internal void AlterStoreItems()
        {
            if (!PluginConfiguration.PEEPER_ENABLED.Value)
            {
                Items.RemoveShopItem(ItemsToSync["Peeper"]);
                logger.LogInfo("Removing Peeper from store.");
            }
            else if (ItemsToSync["Peeper"].creditsWorth != PluginConfiguration.PEEPER_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["Peeper"], PluginConfiguration.PEEPER_PRICE.Value);

            if (!PluginConfiguration.HELMET_ENABLED.Value)
            {
                logger.LogInfo("Removing helmet from store.");
                Items.RemoveShopItem(ItemsToSync["Helmet"]);
            }
            else if (ItemsToSync["Helmet"].creditsWorth != PluginConfiguration.HELMET_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["Helmet"], PluginConfiguration.HELMET_PRICE.Value);

            if (!PluginConfiguration.DIVEKIT_ENABLED.Value)
            {
                logger.LogInfo("Removing divekit from store.");
                Items.RemoveShopItem(ItemsToSync["Dive"]);
            }
            else if (ItemsToSync["Dive"].creditsWorth != PluginConfiguration.DIVEKIT_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["Dive"], PluginConfiguration.DIVEKIT_PRICE.Value);

            if (!PluginConfiguration.ADVANCED_TELE_ENABLED.Value)
            {
                logger.LogInfo("Removing AdvTele from store.");
                Items.RemoveShopItem(ItemsToSync["AdvTele"]);
            }
            else if (ItemsToSync["AdvTele"].creditsWorth != PluginConfiguration.ADVANCED_TELE_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["AdvTele"], PluginConfiguration.ADVANCED_TELE_PRICE.Value);

            if (!PluginConfiguration.WEAK_TELE_ENABLED.Value)
            {
                logger.LogInfo("Removing Tele from store.");
                Items.RemoveShopItem(ItemsToSync["Tele"]);
            }
            else if (ItemsToSync["Tele"].creditsWorth != PluginConfiguration.WEAK_TELE_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["Tele"], PluginConfiguration.WEAK_TELE_PRICE.Value);

            if (!PluginConfiguration.MEDKIT_ENABLED.Value)
            {
                logger.LogInfo("Removing Medkit from store.");
                Items.RemoveShopItem(ItemsToSync["Medkit"]);
            }
            else if (ItemsToSync["Medkit"].creditsWorth != PluginConfiguration.MEDKIT_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["Medkit"], PluginConfiguration.MEDKIT_PRICE.Value);

            if (!PluginConfiguration.NIGHT_VISION_ENABLED.Value)
            {
                logger.LogInfo("Removing Night Vision from store.");
                Items.RemoveShopItem(ItemsToSync["Night"]);
            }
            else if (ItemsToSync["Night"].creditsWorth != PluginConfiguration.NIGHT_VISION_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["Night"], PluginConfiguration.NIGHT_VISION_PRICE.Value);

            if (!PluginConfiguration.WHEELBARROW_ENABLED.Value)
            {
                logger.LogInfo("Removing Wheelbarrow from store.");
                Items.RemoveShopItem(ItemsToSync["Wheel"]);
            }
            else if (ItemsToSync["Wheel"].creditsWorth != PluginConfiguration.WHEELBARROW_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["Wheel"], PluginConfiguration.WHEELBARROW_PRICE.Value);
        }

        void SyncAvailableContracts()
        {
            if (!PluginConfiguration.DATA_CONTRACT.Value || !PluginConfiguration.CONTRACTS_ENABLED.Value)
            {
                logger.LogInfo("Removing data contract");
                int idx = CommandParser.contracts.IndexOf("Data");
                if (idx != -1)
                {
                    CommandParser.contractInfos.RemoveAt(idx);
                    CommandParser.contracts.RemoveAt(idx);
                }
            }
            if (!PluginConfiguration.EXTRACTION_CONTRACT.Value || !PluginConfiguration.CONTRACTS_ENABLED.Value)
            {
                logger.LogInfo("Removing extraction contract");
                int idx = CommandParser.contracts.IndexOf("Extraction");
                if (idx != -1)
                {
                    CommandParser.contractInfos.RemoveAt(idx);
                    CommandParser.contracts.RemoveAt(idx);
                }
            }
            if (!PluginConfiguration.EXORCISM_CONTRACT.Value || !PluginConfiguration.CONTRACTS_ENABLED.Value)
            {
                logger.LogInfo("Removing exorcism contract");
                int idx = CommandParser.contracts.IndexOf("Exorcism");
                if (idx != -1)
                {
                    CommandParser.contractInfos.RemoveAt(idx);
                    CommandParser.contracts.RemoveAt(idx);
                }
            }
            if (!PluginConfiguration.DEFUSAL_CONTRACT.Value || !PluginConfiguration.CONTRACTS_ENABLED.Value)
            {
                logger.LogInfo("Removing defusal contract");
                int idx = CommandParser.contracts.IndexOf("Defusal");
                if (idx != -1)
                {
                    CommandParser.contractInfos.RemoveAt(idx);
                    CommandParser.contracts.RemoveAt(idx);
                }
            }
            if (!PluginConfiguration.EXTERMINATOR_CONTRACT.Value || !PluginConfiguration.CONTRACTS_ENABLED.Value)
            {
                if(CommandParser.contracts.Count == 1 && PluginConfiguration.CONTRACTS_ENABLED.Value)
                {
                    logger.LogInfo("Why must you do the things you do");
                    logger.LogWarning("User tried to remove all contracts! Leaving exterminator present. Did you mean to disable contracts?");
                }
                else
                {
                    logger.LogInfo("Removing exterminator contract");
                    int idx = CommandParser.contracts.IndexOf("Exterminator");
                    if (idx != -1)
                    {
                        CommandParser.contractInfos.RemoveAt(idx);
                        CommandParser.contracts.RemoveAt(idx);
                    }
                }
            }
        }

        internal void Reconstruct()
        {
            AlterStoreItems();
            BuildCustomNodes();
            SyncAvailableContracts();

            logger.LogInfo("Successfully reconstructed with hosts config.");
        }
        internal void BuildCustomNodes()
        {
            terminalNodes = new List<CustomTerminalNode>();

            foreach (Type type in upgradeTypes)
            {
                MethodInfo method = type.GetMethod(nameof(BaseUpgrade.RegisterTerminalNode), BindingFlags.Static | BindingFlags.Public);
                CustomTerminalNode node = (CustomTerminalNode)method.Invoke(null, null);
                if (node != null) terminalNodes.Add(node);
            }

            terminalNodes.Sort();
        }
        /// <summary>
        /// Generic function where it adds a terminal node for an upgrade that can be purchased multiple times
        /// </summary>
        /// <param name="upgradeName"> Name of the upgrade </param>
        /// <param name="shareStatus"> Wether the upgrade is shared through all players or only for the player who purchased it</param>
        /// <param name="enabled"> Wether the upgrade is enabled for gameplay or not</param>
        /// <param name="initialPrice"> The initial price when purchasing the upgrade for the first time</param>
        /// <param name="prices"> Prices for any subsequent purchases of the upgrade</param>
        /// <param name="infoFormat"> The format of the information displayed when checking the upgrade's info</param>
        internal CustomTerminalNode SetupMultiplePurchasableTerminalNode(string upgradeName,
                                                        bool shareStatus,
                                                        bool enabled,
                                                        int initialPrice,
                                                        int[] prices,
                                                        string overrideName = ""
                                                        )
        {
            GameObject multiPerk = AssetBundleHandler.GetPerkGameObject(upgradeName) ;
            if (!multiPerk) return null;

            if (!enabled) return null;

            string infoString = SetupUpgradeInfo(upgrade: multiPerk.GetComponent<BaseUpgrade>(), shareStatus: shareStatus, price: initialPrice, incrementalPrices: prices);
            string moreInfo = infoString;
            if (multiPerk.GetComponent<BaseUpgrade>() is IUpgradeWorldBuilding component) moreInfo += "\n\n" + component.GetWorldBuildingText(shareStatus) + "\n";

            return new TierTerminalNode(
                name: overrideName != "" ? overrideName : upgradeName,
                unlockPrice: initialPrice,
                description: moreInfo,
                prefab: multiPerk,
                prices: prices,
                maxUpgrade: prices.Length,
                originalName: upgradeName,
                sharedUpgrade: shareStatus);
        }
        /// <summary>
        /// Generic function where it adds a terminal node for an upgrade that can only be bought once
        /// </summary>
        /// <param name="upgradeName"> Name of the upgrade</param>
        /// <param name="shareStatus"> Wether the upgrade is shared through all players or only for the player who purchased it</param>
        /// <param name="enabled"> Wether the upgrade is enabled for gameplay or not</param>
        /// <param name="price"></param>
        /// <param name="info"> The information displayed when checking the upgrade's info</param>
        internal CustomTerminalNode SetupOneTimeTerminalNode(string upgradeName,
                                              bool shareStatus,
                                              bool enabled,
                                              int price,
                                              string overrideName = ""
                                              )
        {
            GameObject oneTimeUpgrade = AssetBundleHandler.GetPerkGameObject(upgradeName);
            if (!oneTimeUpgrade) return null;
            if (!enabled) return null;
            string info = SetupUpgradeInfo(upgrade: oneTimeUpgrade.GetComponent<BaseUpgrade>(), shareStatus: shareStatus, price: price);
            string moreInfo = info;
            if (oneTimeUpgrade.GetComponent<BaseUpgrade>() is IUpgradeWorldBuilding component) moreInfo += "\n\n" + component.GetWorldBuildingText(shareStatus) + "\n";

            return new OneTimeTerminalNode(
                name: overrideName != "" ? overrideName : upgradeName,
                unlockPrice: price,
                description: moreInfo,
                prefab: oneTimeUpgrade,
                originalName: upgradeName,
                sharedUpgrade: shareStatus);
        }

        public string SetupUpgradeInfo(BaseUpgrade upgrade = null, bool shareStatus = false, int price = -1, int[] incrementalPrices = null)
        {
            string info = "";
            if (upgrade is IOneTimeUpgradeDisplayInfo upgradeInfo) info += upgradeInfo.GetDisplayInfo(price) + "\n";
            if (upgrade is ITierUpgradeDisplayInfo tierUpgradeInfo) info += tierUpgradeInfo.GetDisplayInfo(initialPrice: price, maxLevels: incrementalPrices.Length, incrementalPrices: incrementalPrices);
            return info;
        }
        /// <summary>
        /// Function which parses the prices present in a given string and inserts them into an array of integers
        /// </summary>
        /// <param name="upgradePrices">The string which contains the list of prices</param>
        /// <returns>An array of integers with the values that were present in the string</returns>
        internal static int[] ParseUpgradePrices(string upgradePrices)
        {
            string[] priceString = upgradePrices.Split(',').ToArray();
            int[] prices = new int[priceString.Length];

            for (int i = 0; i < priceString.Length; i++)
            {
                if (int.TryParse(priceString[i], out int price))
                {
                    prices[i] = price;
                }
                else
                {
                    logger.LogWarning($"Invalid upgrade price submitted: {prices[i]}");
                    prices[i] = -1;

                }
            }
            if (prices.Length == 1 && prices[0] == -1) { prices = new int[0]; }
            return prices;
        }
    }
}