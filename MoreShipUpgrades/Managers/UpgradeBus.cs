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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    public class UpgradeBus : MonoBehaviour
    {
        internal static UpgradeBus Instance { get; set; }
        internal PluginConfig PluginConfiguration { get; set; }
        internal GameObject IntroScreen { get; set; }
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
        internal List<CustomTerminalNode> terminalNodes = new List<CustomTerminalNode>();
        internal Dictionary<string, GameObject> UpgradeObjects = new Dictionary<string, GameObject>();
        internal Dictionary<string, Item> ItemsToSync = new Dictionary<string, Item>();
        internal Dictionary<string,bool> IndividualUpgrades = new Dictionary<string,bool>();
        internal List<Peeper> coilHeadItems = new List<Peeper>();
        internal AssetBundle UpgradeAssets;
        internal string version;

        internal GameObject helmetModel;
        internal Helmet helmetScript;
        internal bool wearingHelmet = false;
        internal int helmetHits = 0;

        internal Dictionary<string, AudioClip> SFX = new Dictionary<string, AudioClip>();
        internal bool helmetDesync;

        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PluginConfiguration = PluginConfig.Instance;
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
        public TerminalNode ConstructNode()
        {
            modStoreInterface = ScriptableObject.CreateInstance<TerminalNode>();
            modStoreInterface.clearPreviousText = true;
            foreach (CustomTerminalNode terminalNode in terminalNodes)
            {
                string saleStatus = terminalNode.salePerc < 1f ? $"- {((1-terminalNode.salePerc)*100).ToString("F0")}% OFF!" : "";
                if (!terminalNode.Unlocked) { modStoreInterface.displayText += $"\n{terminalNode.Name} // ${(int)(terminalNode.UnlockPrice * terminalNode.salePerc)} {saleStatus}  "; }
                else if (terminalNode.MaxUpgrade == 0) { modStoreInterface.displayText += $"\n{terminalNode.Name} // UNLOCKED  "; }
                else if (terminalNode.MaxUpgrade > terminalNode.CurrentUpgrade) { modStoreInterface.displayText += $"\n{terminalNode.Name} // ${(int)(terminalNode.Prices[terminalNode.CurrentUpgrade]*terminalNode.salePerc)} // LVL {terminalNode.CurrentUpgrade + 1} {saleStatus}  "; }
                else { modStoreInterface.displayText += $"\n{terminalNode.Name} // MAX LVL"; }
            }
            if (modStoreInterface.displayText == "")
            {
                modStoreInterface.displayText = "No upgrades available";
            }
            modStoreInterface.displayText += "\n\n";
            return modStoreInterface;
        }

        public void ResetAllValues(bool wipeObjRefs = true)
        {
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
                int idx = CommandParser.contracts.IndexOf("data");
                if (idx != -1)
                {
                    CommandParser.contractInfos.RemoveAt(idx);
                    CommandParser.contracts.RemoveAt(idx);
                }
            }
            if (!PluginConfiguration.EXTRACTION_CONTRACT.Value || !PluginConfiguration.CONTRACTS_ENABLED.Value)
            {
                logger.LogInfo("Removing extraction contract");
                int idx = CommandParser.contracts.IndexOf("extraction");
                if (idx != -1)
                {
                    CommandParser.contractInfos.RemoveAt(idx);
                    CommandParser.contracts.RemoveAt(idx);
                }
            }
            if (!PluginConfiguration.EXORCISM_CONTRACT.Value || !PluginConfiguration.CONTRACTS_ENABLED.Value)
            {
                logger.LogInfo("Removing exorcism contract");
                int idx = CommandParser.contracts.IndexOf("exorcism");
                if (idx != -1)
                {
                    CommandParser.contractInfos.RemoveAt(idx);
                    CommandParser.contracts.RemoveAt(idx);
                }
            }
            if (!PluginConfiguration.DEFUSAL_CONTRACT.Value || !PluginConfiguration.CONTRACTS_ENABLED.Value)
            {
                logger.LogInfo("Removing defusal contract");
                int idx = CommandParser.contracts.IndexOf("defusal");
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
                    int idx = CommandParser.contracts.IndexOf("exterminator");
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

            IndividualUpgrades = new Dictionary<string, bool>();

            SetupBeekeperTerminalNode();

            SetupProteinPowderTerminalNode();

            SetupBiggerLungsTerminalNode();

            SetupRunningShoesTerminalNode();

            SetupStrongLegsTerminalNode();

            SetupMalwareBroadcasterTerminalNode();

            SetupNightVisionBatteryTerminalNode();

            SetupDiscombobulatorTerminalNode();

            SetupHunterTerminalNode();

            SetupBetterScannerTerminalNode();

            SetupLightningRodTerminalNode();

            SetupWalkieGPSTerminalNode();

            SetupBackMusclesTerminalNode();

            SetupPlayerHealthTerminalNode();

            SetupPagerTerminalNode();

            SetupLocksmithTerminalNode();

            SetupSickBeatsTerminalNode();

            SetupShutterBatteriesTerminalNode();
            SetupMarketInfluenceTerminalNode();
            SetupBargainConnectionsTerminalNode();
            SetupQuantumDisruptorTerminalNode();
            SetupLethalDealsTerminalNode();
            SetupFasterDropPodTerminalNode();
            SetupChargingBoosterTerminalNode();
            SetupSigurdTerminalNode();
            SetupEfficientEnginesNode();
            SetupClimbingGlovesTerminalNode();
            terminalNodes.Sort();
        }
        void SetupEfficientEnginesNode()
        {
            SetupMultiplePurchasableTerminalNode(EfficientEngines.UPGRADE_NAME,
                                                true,
                                                PluginConfiguration.EFFICIENT_ENGINES_ENABLED.Value,
                                                PluginConfiguration.EFFICIENT_ENGINES_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.EFFICIENT_ENGINES_PRICES.Value));
        }
        void SetupClimbingGlovesTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(ClimbingGloves.UPGRADE_NAME,
                                                PluginConfiguration.CLIMBING_GLOVES_INDIVIDUAL.Value,
                                                PluginConfiguration.CLIMBING_GLOVES_ENABLED.Value,
                                                PluginConfiguration.CLIMBING_GLOVES_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.CLIMBING_GLOVES_PRICES.Value));
        }
        void SetupMarketInfluenceTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(MarketInfluence.UPGRADE_NAME,
                                                true,
                                                PluginConfiguration.MARKET_INFLUENCE_ENABLED.Value,
                                                PluginConfiguration.MARKET_INFLUENCE_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.MARKET_INFLUENCE_PRICES.Value));
        }
        void SetupBargainConnectionsTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(BargainConnections.UPGRADE_NAME,
                                                true,
                                                PluginConfiguration.BARGAIN_CONNECTIONS_ENABLED.Value,
                                                PluginConfiguration.BARGAIN_CONNECTIONS_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.BARGAIN_CONNECTIONS_PRICES.Value));
        }
        void SetupLethalDealsTerminalNode()
        {
            SetupOneTimeTerminalNode(LethalDeals.UPGRADE_NAME,
                                    true,
                                    PluginConfiguration.LETHAL_DEALS_ENABLED.Value,
                                    PluginConfiguration.LETHAL_DEALS_PRICE.Value);
        }
        private void SetupQuantumDisruptorTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(QuantumDisruptor.UPGRADE_NAME,
                                                true,
                                                PluginConfiguration.QUANTUM_DISRUPTOR_ENABLED.Value,
                                                PluginConfiguration.QUANTUM_DISRUPTOR_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.QUANTUM_DISRUPTOR_PRICES.Value));
        }
        private void SetupShutterBatteriesTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(DoorsHydraulicsBattery.UPGRADE_NAME,
                                                true,
                                                PluginConfiguration.DOOR_HYDRAULICS_BATTERY_ENABLED.Value,
                                                PluginConfiguration.DOOR_HYDRAULICS_BATTERY_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.DOOR_HYDRAULICS_BATTERY_PRICES.Value));
        }
        private void SetupSickBeatsTerminalNode()
        {
            SetupOneTimeTerminalNode(
                SickBeats.UPGRADE_NAME,
                PluginConfiguration.SHARED_UPGRADES.Value || !PluginConfiguration.BEATS_INDIVIDUAL.Value,
                PluginConfiguration.BEATS_ENABLED.Value,
                PluginConfiguration.BEATS_PRICE.Value);
        }

        private void SetupBeekeperTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(
                Beekeeper.UPGRADE_NAME,
                PluginConfiguration.SHARED_UPGRADES.Value || !PluginConfiguration.BEEKEEPER_INDIVIDUAL.Value,
                PluginConfiguration.BEEKEEPER_ENABLED.Value,
                PluginConfiguration.BEEKEEPER_PRICE.Value,
                ParseUpgradePrices(PluginConfiguration.BEEKEEPER_UPGRADE_PRICES.Value));
        }

        private void SetupProteinPowderTerminalNode() 
        {
            SetupMultiplePurchasableTerminalNode(ProteinPowder.UPGRADE_NAME,
                                                PluginConfiguration.SHARED_UPGRADES.Value || !PluginConfiguration.PROTEIN_INDIVIDUAL.Value,
                                                PluginConfiguration.PROTEIN_ENABLED.Value,
                                                PluginConfiguration.PROTEIN_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.PROTEIN_UPGRADE_PRICES.Value));
        }
        private void SetupBiggerLungsTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(BiggerLungs.UPGRADE_NAME,
                                                PluginConfiguration.SHARED_UPGRADES.Value || !PluginConfiguration.BIGGER_LUNGS_INDIVIDUAL.Value,
                                                PluginConfiguration.BIGGER_LUNGS_ENABLED.Value,
                                                PluginConfiguration.BIGGER_LUNGS_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.BIGGER_LUNGS_UPGRADE_PRICES.Value));
        }
        private void SetupRunningShoesTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(RunningShoes.UPGRADE_NAME,
                                                PluginConfiguration.SHARED_UPGRADES.Value || !PluginConfiguration.RUNNING_SHOES_INDIVIDUAL.Value,
                                                PluginConfiguration.RUNNING_SHOES_ENABLED.Value,
                                                PluginConfiguration.RUNNING_SHOES_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.RUNNING_SHOES_UPGRADE_PRICES.Value));
        }
        private void SetupStrongLegsTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(StrongLegs.UPGRADE_NAME,
                                                PluginConfiguration.SHARED_UPGRADES.Value || !PluginConfiguration.STRONG_LEGS_INDIVIDUAL.Value,
                                                PluginConfiguration.STRONG_LEGS_ENABLED.Value,
                                                PluginConfiguration.STRONG_LEGS_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.STRONG_LEGS_UPGRADE_PRICES.Value));
        }
        private void SetupMalwareBroadcasterTerminalNode()
        {

            SetupOneTimeTerminalNode(MalwareBroadcaster.UPGRADE_NAME,
                                    PluginConfiguration.SHARED_UPGRADES.Value || !PluginConfiguration.MALWARE_BROADCASTER_INDIVIDUAL.Value,
                                    PluginConfiguration.MALWARE_BROADCASTER_ENABLED.Value,
                                    PluginConfiguration.MALWARE_BROADCASTER_PRICE.Value);
        }
        private void SetupNightVisionBatteryTerminalNode()
        {
            CustomTerminalNode node = SetupMultiplePurchasableTerminalNode(NightVision.UPGRADE_NAME,
                                                PluginConfiguration.SHARED_UPGRADES.Value || !PluginConfiguration.NIGHT_VISION_INDIVIDUAL.Value,
                                                PluginConfiguration.NIGHT_VISION_ENABLED.Value,
                                                0,
                                                ParseUpgradePrices(PluginConfiguration.NIGHT_VISION_UPGRADE_PRICES.Value));
            if(node != null) node.Unlocked = true;
        }
        private void SetupDiscombobulatorTerminalNode()
        {
            AudioClip flashSFX = AssetBundleHandler.GetAudioClip("Flashbang");
            if (!flashSFX) return;

            flashNoise = flashSFX;
            SetupMultiplePurchasableTerminalNode(Discombobulator.UPGRADE_NAME,
                                                PluginConfiguration.SHARED_UPGRADES.Value || !PluginConfiguration.DISCOMBOBULATOR_INDIVIDUAL.Value,
                                                PluginConfiguration.DISCOMBOBULATOR_ENABLED.Value,
                                                PluginConfiguration.DISCOMBOBULATOR_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.DISCO_UPGRADE_PRICES.Value));
        }
        private void SetupHunterTerminalNode()
        {
            Hunter.SetupLevels();
            SetupMultiplePurchasableTerminalNode(Hunter.UPGRADE_NAME,
                                                true,
                                                PluginConfiguration.HUNTER_ENABLED.Value,
                                                PluginConfiguration.HUNTER_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.HUNTER_UPGRADE_PRICES.Value));
        }
        private void SetupBetterScannerTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(BetterScanner.UPGRADE_NAME,
                                                PluginConfiguration.SHARED_UPGRADES.Value || !PluginConfiguration.BETTER_SCANNER_INDIVIDUAL.Value,
                                                PluginConfiguration.BETTER_SCANNER_ENABLED.Value,
                                                PluginConfiguration.BETTER_SCANNER_PRICE.Value,
                                                new int[] { PluginConfiguration.BETTER_SCANNER_PRICE2.Value, PluginConfiguration.BETTER_SCANNER_PRICE3.Value }
                                                );
        }
        private void SetupLightningRodTerminalNode()
        {
            SetupOneTimeTerminalNode(LightningRod.UPGRADE_NAME,
                                    true,
                                    PluginConfiguration.LIGHTNING_ROD_ENABLED.Value,
                                    PluginConfiguration.LIGHTNING_ROD_PRICE.Value);
        }
        private void SetupWalkieGPSTerminalNode()
        {
            SetupOneTimeTerminalNode(WalkieGPS.UPGRADE_NAME,
                                    true,
                                    PluginConfiguration.WALKIE_ENABLED.Value,
                                    PluginConfiguration.WALKIE_PRICE.Value);
        }
        private void SetupBackMusclesTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(BackMuscles.UPGRADE_NAME,
                                                PluginConfiguration.SHARED_UPGRADES.Value || !PluginConfiguration.BACK_MUSCLES_INDIVIDUAL.Value,
                                                PluginConfiguration.BACK_MUSCLES_ENABLED.Value,
                                                PluginConfiguration.BACK_MUSCLES_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.BACK_MUSCLES_UPGRADE_PRICES.Value));
        }
        private void SetupPagerTerminalNode()
        {
            SetupOneTimeTerminalNode(FastEncryption.UPGRADE_NAME,
                                    true,
                                    PluginConfiguration.PAGER_ENABLED.Value,
                                    PluginConfiguration.PAGER_PRICE.Value);
        }
        private void SetupLocksmithTerminalNode()
        {
            SetupOneTimeTerminalNode(LockSmith.UPGRADE_NAME,
                                    PluginConfiguration.SHARED_UPGRADES.Value || !PluginConfiguration.LOCKSMITH_INDIVIDUAL.Value,
                                    PluginConfiguration.LOCKSMITH_ENABLED.Value,
                                    PluginConfiguration.LOCKSMITH_PRICE.Value);
        }
        private void SetupPlayerHealthTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(Stimpack.UPGRADE_NAME,
                                                PluginConfiguration.SHARED_UPGRADES.Value || !PluginConfiguration.PLAYER_HEALTH_INDIVIDUAL.Value,
                                                PluginConfiguration.PLAYER_HEALTH_ENABLED.Value,
                                                PluginConfiguration.PLAYER_HEALTH_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.PLAYER_HEALTH_UPGRADE_PRICES.Value));
        }
        private void SetupFasterDropPodTerminalNode()
        {
            SetupOneTimeTerminalNode(FasterDropPod.UPGRADE_NAME,
                                    true,
                                    PluginConfiguration.FASTER_DROP_POD_ENABLED.Value,
                                    PluginConfiguration.FASTER_DROP_POD_PRICE.Value);
        }
        private void SetupSigurdTerminalNode()
        {
            SetupOneTimeTerminalNode(Sigurd.UPGRADE_NAME,
                                    true,
                                    PluginConfiguration.SIGURD_ENABLED.Value || PluginConfiguration.SIGURD_LAST_DAY_ENABLED.Value,
                                    PluginConfiguration.SIGURD_PRICE.Value);
        }
        void SetupChargingBoosterTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(ChargingBooster.UPGRADE_NAME,
                                                true,
                                                PluginConfiguration.CHARGING_BOOSTER_ENABLED.Value,
                                                PluginConfiguration.CHARGING_BOOSTER_PRICE.Value,
                                                ParseUpgradePrices(PluginConfiguration.CHARGING_BOOSTER_PRICES.Value));
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
        private CustomTerminalNode SetupMultiplePurchasableTerminalNode(string upgradeName,
                                                        bool shareStatus,
                                                        bool enabled,
                                                        int initialPrice,
                                                        int[] prices
                                                        )
        {
            GameObject multiPerk = AssetBundleHandler.GetPerkGameObject(upgradeName) ;
            if (!multiPerk) return null;

            IndividualUpgrades.Add(upgradeName, shareStatus);

            if (!enabled) return null;

            string infoString = SetupUpgradeInfo(upgrade: multiPerk.GetComponent<BaseUpgrade>(), shareStatus: shareStatus, price: initialPrice, incrementalPrices: prices);

            CustomTerminalNode node = new CustomTerminalNode(upgradeName, initialPrice, infoString, multiPerk, prices, prices.Length);
            terminalNodes.Add(node);
            return node;
        }
        /// <summary>
        /// Generic function where it adds a terminal node for an upgrade that can only be bought once
        /// </summary>
        /// <param name="upgradeName"> Name of the upgrade</param>
        /// <param name="shareStatus"> Wether the upgrade is shared through all players or only for the player who purchased it</param>
        /// <param name="enabled"> Wether the upgrade is enabled for gameplay or not</param>
        /// <param name="price"></param>
        /// <param name="info"> The information displayed when checking the upgrade's info</param>
        private void SetupOneTimeTerminalNode(string upgradeName,
                                              bool shareStatus,
                                              bool enabled,
                                              int price
                                              )
        {
            GameObject oneTimeUpgrade = AssetBundleHandler.GetPerkGameObject(upgradeName);
            if (!oneTimeUpgrade) return;

            IndividualUpgrades.Add(upgradeName, shareStatus);
            if (!enabled) return;
            string info = SetupUpgradeInfo(upgrade: oneTimeUpgrade.GetComponent<BaseUpgrade>(), shareStatus: shareStatus, price: price);

            CustomTerminalNode node = new CustomTerminalNode(upgradeName, price, info, oneTimeUpgrade);
            terminalNodes.Add(node);
        }
        private string SetupUpgradeInfo(BaseUpgrade upgrade = null, bool shareStatus = false, int price = -1, int[] incrementalPrices = null)
        {
            string info = "";
            if (upgrade is IOneTimeUpgradeDisplayInfo upgradeInfo) info += upgradeInfo.GetDisplayInfo(price) + "\n";
            if (upgrade is ITierUpgradeDisplayInfo tierUpgradeInfo) info += tierUpgradeInfo.GetDisplayInfo(initialPrice: price, maxLevels: incrementalPrices.Length, incrementalPrices: incrementalPrices);
            if (upgrade is IUpgradeWorldBuilding component) info += component.GetWorldBuildingText(shareStatus) + "\n";
            return info;
        }
        /// <summary>
        /// Function which parses the prices present in a given string and inserts them into an array of integers
        /// </summary>
        /// <param name="upgradePrices">The string which contains the list of prices</param>
        /// <returns>An array of integers with the values that were present in the string</returns>
        private int[] ParseUpgradePrices(string upgradePrices)
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