using GameNetcodeStuff;
using LethalLib.Modules;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.UpgradeComponents.Items;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    public class UpgradeBus : NetworkBehaviour
    {
        public static UpgradeBus instance;
        public PluginConfig cfg;
        public GameObject introScreen;
        private static LGULogger logger = new LGULogger(nameof(UpgradeBus));

        public bool DestroyTraps = false;
        public bool scannerUpgrade = false;
        public bool wearingHelmet = false;
        public bool nightVision = false;
        public bool nightVisionActive = false;
        public float nightVisRange;
        public float nightVisIntensity;
        public bool exoskeleton = false;
        public bool TPButtonPressed = false;
        public bool beekeeper = false;
        public bool terminalFlash = false;
        public bool strongLegs = false;
        public bool runningShoes = false;
        public bool lockSmith = false;
        public bool biggerLungs = false;
        public bool proteinPowder = false;
        public bool lightningRod = false;
        public bool hunter = false;
        public bool playerHealth = false;
        public bool doorsHydraulicsBattery = false;
        public bool bargainConnections = false;

        public int lungLevel = 0;
        public int helmetHits = 0;
        public int huntLevel = 0;
        public int proteinLevel = 0;
        public int beeLevel = 0;
        public int backLevel = 0;
        public int runningLevel = 0;
        public int discoLevel = 0;
        public int legLevel = 0;
        public int scanLevel = 0;
        public int nightVisionLevel = 0;
        public int playerHealthLevel = 0;
        public int doorsHydraulicsBatteryLevel = 0;
        public int bargainConnectionsLevel = 0;

        public float flashCooldown = 0f;
        public float alteredWeight = 1f;

        public string contractLevel = "None";
        public string contractType = "None";
        public string DataMinigameKey = "";
        public string DataMinigameUser = "";
        public string DataMinigamePass = "";

        public Dictionary<string, List<string>> fakeBombOrders = new Dictionary<string, List<string>>();
        public Dictionary<string, float> SaleData = new Dictionary<string, float>();

        public Color nightVisColor;
        public AudioClip flashNoise;
        public GameObject modStorePrefab;
        public TerminalNode modStoreInterface;
        public Terminal terminal;
        public PlayerControllerB localPlayer;
        private HangarShipDoor hangarDoors;

        public List<BoomboxItem> boomBoxes = new List<BoomboxItem>();

        public List<CustomTerminalNode> terminalNodes = new List<CustomTerminalNode>();

        public Dictionary<string, GameObject> UpgradeObjects = new Dictionary<string, GameObject>();

        public Dictionary<string, Item> ItemsToSync = new Dictionary<string, Item>();

        public Dictionary<ulong, int> playerHealthLevels = new Dictionary<ulong, int>();

        public bool increaseHivePrice = false;

        public Dictionary<string,bool> IndividualUpgrades = new Dictionary<string,bool>();
        public string[] internNames, internInterests;

        public List<Peeper> coilHeadItems = new List<Peeper>();
        internal bool walkies;
        internal bool walkieUIActive;
        internal string version;

        public AssetBundle UpgradeAssets;
        internal bool pager;

        public Dictionary<string,GameObject> samplePrefabs = new Dictionary<string,GameObject>();
        public GameObject nightVisionPrefab;
        public bool sickBeats;
        public float staminaDrainCoefficient = 1f;
        public float incomingDamageCoefficient = 1f;
        public int damageBoost;
        public GameObject BoomboxIcon;
        public bool EffectsActive;
        public GameObject helmetModel;
        public Helmet helmetScript;
        public Dictionary<string, AudioClip> SFX = new Dictionary<string, AudioClip>();
        public bool helmetDesync;
        public List<string> bombOrder = new List<string>();
        public string SerialNumber;

        void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            cfg = Plugin.cfg;
            logger = new LGULogger(typeof(UpgradeBus).Name);
        }

        public Terminal GetTerminal()
        {
            if (terminal == null) terminal = GameObject.Find("TerminalScript").GetComponent<Terminal>();

            return terminal;
        }
        public PlayerControllerB GetLocalPlayer() // I keep forgetting we have this
        {
            if (localPlayer == null) localPlayer = GameNetworkManager.Instance.localPlayerController;
            return localPlayer;
        }
        public HangarShipDoor GetShipDoors()
        {
            if (hangarDoors == null) hangarDoors = FindObjectOfType<HangarShipDoor>();
            return hangarDoors;
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
            if(IsHost || IsServer) ResetShipAttributesClientRpc();
            EffectsActive = false;
            DestroyTraps = false;
            scannerUpgrade = false;
            nightVision = false;
            walkies = false;
            nightVisionActive = false;
            exoskeleton = false;
            TPButtonPressed = false;
            beekeeper = false;
            terminalFlash = false;
            strongLegs = false;
            runningShoes = false;
            lockSmith = false;
            biggerLungs = false;
            lightningRod = false;
            pager = false;
            hunter = false;
            playerHealth = false;
            sickBeats = false;
            bargainConnections = false;

            contractType = "None";
            contractLevel = "None";

            huntLevel = 0;
            proteinLevel = 0;
            lungLevel = 0;
            backLevel = 0;
            scanLevel = 0;
            beeLevel = 0;
            runningLevel = 0;
            discoLevel = 0;
            legLevel = 0;
            nightVisionLevel = 0;
            playerHealthLevel = 0;
            bargainConnectionsLevel = 0;
            flashCooldown = 0f;
            alteredWeight = 1f;
            if (wipeObjRefs) {
                UpgradeObjects = new Dictionary<string, GameObject>(); 
                MalwareBroadcaster.instance = null;
                Discombobulator.instance = null;
                FastEncryption.instance = null;
            }
            foreach(CustomTerminalNode node in terminalNodes)
            {
                node.Unlocked = false;
                node.CurrentUpgrade = 0;
                if(node.Name == NightVision.UPGRADE_NAME) { node.Unlocked = true; }
            }

        }
        private void ResetPlayerAttributes()
        {
            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            if (player == null) return; // Disconnecting the game

            logger.LogDebug($"Resetting {player.playerUsername}'s attributes");
            if (cfg.RUNNING_SHOES_ENABLED.Value && runningShoes) UpgradeObjects[RunningShoes.UPGRADE_NAME].GetComponent<GameAttributeTierUpgrade>().UnloadUpgradeAttribute(ref runningShoes, ref runningLevel);
            if (cfg.BIGGER_LUNGS_ENABLED.Value && biggerLungs) UpgradeObjects[BiggerLungs.UPGRADE_NAME].GetComponent<GameAttributeTierUpgrade>().UnloadUpgradeAttribute(ref biggerLungs, ref lungLevel);
            if (cfg.STRONG_LEGS_ENABLED.Value && strongLegs) UpgradeObjects[StrongLegs.UPGRADE_NAME].GetComponent<GameAttributeTierUpgrade>().UnloadUpgradeAttribute(ref strongLegs, ref legLevel);
            if (cfg.PLAYER_HEALTH_ENABLED.Value && playerHealth) UpgradeObjects[Stimpack.UPGRADE_NAME].GetComponent<GameAttributeTierUpgrade>().UnloadUpgradeAttribute(ref playerHealth, ref playerHealthLevel);
        }

        [ClientRpc]
        private void ResetShipAttributesClientRpc()
        {
            HangarShipDoor shipDoors = GetShipDoors();
            if (shipDoors == null) return; // Very edge case

            logger.LogDebug($"Resetting the ship's attributes");
            if (cfg.DOOR_HYDRAULICS_BATTERY_ENABLED.Value && doorsHydraulicsBattery) UpgradeObjects[Stimpack.UPGRADE_NAME].GetComponent<GameAttributeTierUpgrade>().UnloadUpgradeAttribute(ref doorsHydraulicsBattery, ref doorsHydraulicsBatteryLevel);
        }

        internal void GenerateSales(int seed = -1) // TODO: Save sales
        {
            if (seed == -1) seed = Random.Range(0, 999999);
            logger.LogInfo($"Generating sales with seed: {seed} on this client...");
            Random.InitState(seed);
            SaleData = new Dictionary<string, float>();
            foreach(CustomTerminalNode node in terminalNodes)
            {
                if(Random.value > cfg.SALE_PERC.Value)
                {
                    node.salePerc = Random.Range(0.60f, 0.90f);
                    logger.LogInfo($"Set sale percentage to: {node.salePerc} for {node.Name}.");
                }
                else
                {
                    node.salePerc = 1f;
                }
                if(IsHost || IsServer)
                {
                    SaleData.Add(node.Name, node.salePerc);
                }
            }
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
                node.salePerc = SaleData[node.Name];
                if(node.salePerc != 1f)
                {
                    logger.LogInfo($"Loaded sale of {node.salePerc} for {node.Name}.");
                }
            }
        }

        internal void AlterStoreItems()
        {
            if (!cfg.PEEPER_ENABLED.Value)
            {
                Items.RemoveShopItem(ItemsToSync["Peeper"]);
                logger.LogInfo("Removing Peeper from store.");
            }
            else if (ItemsToSync["Peeper"].creditsWorth != cfg.PEEPER_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["Peeper"], cfg.PEEPER_PRICE.Value);

            if (!cfg.HELMET_ENABLED.Value)
            {
                logger.LogInfo("Removing helmet from store.");
                Items.RemoveShopItem(ItemsToSync["Helmet"]);
            }
            else if (ItemsToSync["Helmet"].creditsWorth != cfg.HELMET_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["Helmet"], cfg.HELMET_PRICE.Value);

            if (!cfg.DIVEKIT_ENABLED.Value)
            {
                logger.LogInfo("Removing divekit from store.");
                Items.RemoveShopItem(ItemsToSync["Dive"]);
            }
            else if (ItemsToSync["Dive"].creditsWorth != cfg.DIVEKIT_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["Dive"], cfg.DIVEKIT_PRICE.Value);

            if (!cfg.ADVANCED_TELE_ENABLED.Value)
            {
                logger.LogInfo("Removing AdvTele from store.");
                Items.RemoveShopItem(ItemsToSync["AdvTele"]);
            }
            else if (ItemsToSync["AdvTele"].creditsWorth != cfg.ADVANCED_TELE_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["AdvTele"], cfg.ADVANCED_TELE_PRICE.Value);

            if (!cfg.WEAK_TELE_ENABLED.Value)
            {
                logger.LogInfo("Removing Tele from store.");
                Items.RemoveShopItem(ItemsToSync["Tele"]);
            }
            else if (ItemsToSync["Tele"].creditsWorth != cfg.WEAK_TELE_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["Tele"], cfg.WEAK_TELE_PRICE.Value);

            if (!cfg.MEDKIT_ENABLED.Value)
            {
                logger.LogInfo("Removing Medkit from store.");
                Items.RemoveShopItem(ItemsToSync["Medkit"]);
            }
            else if (ItemsToSync["Medkit"].creditsWorth != cfg.MEDKIT_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["Medkit"], cfg.MEDKIT_PRICE.Value);

            if (!cfg.NIGHT_VISION_ENABLED.Value)
            {
                logger.LogInfo("Removing Night Vision from store.");
                Items.RemoveShopItem(ItemsToSync["Night"]);
            }
            else if (ItemsToSync["Night"].creditsWorth != cfg.NIGHT_VISION_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["Night"], cfg.NIGHT_VISION_PRICE.Value);

            if (!cfg.WHEELBARROW_ENABLED.Value)
            {
                logger.LogInfo("Removing Wheelbarrow from store.");
                Items.RemoveShopItem(ItemsToSync["Wheel"]);
            }
            else if (ItemsToSync["Wheel"].creditsWorth != cfg.WHEELBARROW_PRICE.Value) Items.UpdateShopItemPrice(ItemsToSync["Wheel"], cfg.WHEELBARROW_PRICE.Value);
        }

        void SyncAvailableContracts()
        {
            if (!cfg.DATA_CONTRACT.Value)
            {
                logger.LogInfo("Removing data contract");
                int idx = CommandParser.contracts.IndexOf("data");
                if (idx != -1)
                {
                    CommandParser.contractInfos.RemoveAt(idx);
                    CommandParser.contracts.RemoveAt(idx);
                }
            }
            if (!cfg.EXTRACTION_CONTRACT.Value)
            {
                logger.LogInfo("Removing extraction contract");
                int idx = CommandParser.contracts.IndexOf("extraction");
                if (idx != -1)
                {
                    CommandParser.contractInfos.RemoveAt(idx);
                    CommandParser.contracts.RemoveAt(idx);
                }
            }
            if (!cfg.EXORCISM_CONTRACT.Value)
            {
                logger.LogInfo("Removing exorcism contract");
                int idx = CommandParser.contracts.IndexOf("exorcism");
                if (idx != -1)
                {
                    CommandParser.contractInfos.RemoveAt(idx);
                    CommandParser.contracts.RemoveAt(idx);
                }
            }
            if (!cfg.DEFUSAL_CONTRACT.Value)
            {
                logger.LogInfo("Removing defusal contract");
                int idx = CommandParser.contracts.IndexOf("defusal");
                if (idx != -1)
                {
                    CommandParser.contractInfos.RemoveAt(idx);
                    CommandParser.contracts.RemoveAt(idx);
                }
            }
            if (!cfg.EXTERMINATOR_CONTRACT.Value)
            {
                if(CommandParser.contracts.Count == 1)
                {
                    logger.LogInfo("Why must you do the things you do");
                    logger.LogWarning("User tried to remove all contracts! Leaving exterminator present. Did you mean to disable contracts?");
                }
                else
                {
                    logger.LogInfo("Removing exterminator contract");
                    int idx = CommandParser.contracts.IndexOf("exterminator");
                    CommandParser.contractInfos.RemoveAt(idx);
                    CommandParser.contracts.RemoveAt(idx);
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

            SetupBargainConnectionsTerminalNode();
            terminalNodes.Sort();
        }
        void SetupBargainConnectionsTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(BargainConnections.UPGRADE_NAME,
                                                true,
                                                cfg.BARGAIN_CONNECTIONS_ENABLED,
                                                cfg.BARGAIN_CONNECTIONS_PRICE,
                                                ParseUpgradePrices(cfg.BARGAIN_CONNECTIONS_PRICES));
        }
        private void SetupShutterBatteriesTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(DoorsHydraulicsBattery.UPGRADE_NAME,
                                                true,
                                                cfg.DOOR_HYDRAULICS_BATTERY_ENABLED.Value,
                                                cfg.DOOR_HYDRAULICS_BATTERY_PRICE.Value,
                                                ParseUpgradePrices(cfg.DOOR_HYDRAULICS_BATTERY_PRICES.Value));
        }
        private void SetupSickBeatsTerminalNode()
        {
            SetupOneTimeTerminalNode(
                SickBeats.UPGRADE_NAME,
                cfg.SHARED_UPGRADES.Value ? true : !cfg.BEATS_INDIVIDUAL.Value,
                cfg.BEATS_ENABLED.Value,
                cfg.BEATS_PRICE.Value);
        }

        private void SetupBeekeperTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(
                Beekeeper.UPGRADE_NAME,
                cfg.SHARED_UPGRADES.Value ? true : !cfg.BEEKEEPER_INDIVIDUAL.Value,
                cfg.BEEKEEPER_ENABLED.Value,
                cfg.BEEKEEPER_PRICE.Value,
                ParseUpgradePrices(cfg.BEEKEEPER_UPGRADE_PRICES.Value));
        }

        private void SetupProteinPowderTerminalNode() 
        {
            SetupMultiplePurchasableTerminalNode(ProteinPowder.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES.Value ? true : !cfg.PROTEIN_INDIVIDUAL.Value,
                                                cfg.PROTEIN_ENABLED.Value,
                                                cfg.PROTEIN_PRICE.Value,
                                                ParseUpgradePrices(cfg.PROTEIN_UPGRADE_PRICES.Value));
        }
        private void SetupBiggerLungsTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(BiggerLungs.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES.Value ? true : !cfg.BIGGER_LUNGS_INDIVIDUAL.Value,
                                                cfg.BIGGER_LUNGS_ENABLED.Value,
                                                cfg.BIGGER_LUNGS_PRICE.Value,
                                                ParseUpgradePrices(cfg.BIGGER_LUNGS_UPGRADE_PRICES.Value));
        }
        private void SetupRunningShoesTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(RunningShoes.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES.Value ? true : !cfg.RUNNING_SHOES_INDIVIDUAL.Value,
                                                cfg.RUNNING_SHOES_ENABLED.Value,
                                                cfg.RUNNING_SHOES_PRICE.Value,
                                                ParseUpgradePrices(cfg.RUNNING_SHOES_UPGRADE_PRICES.Value));
        }
        private void SetupStrongLegsTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(StrongLegs.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES.Value ? true : !cfg.STRONG_LEGS_INDIVIDUAL.Value,
                                                cfg.STRONG_LEGS_ENABLED.Value,
                                                cfg.STRONG_LEGS_PRICE.Value,
                                                ParseUpgradePrices(cfg.STRONG_LEGS_UPGRADE_PRICES.Value));
        }
        private void SetupMalwareBroadcasterTerminalNode()
        {

            SetupOneTimeTerminalNode(MalwareBroadcaster.UPGRADE_NAME,
                                    cfg.SHARED_UPGRADES.Value ? true : !cfg.MALWARE_BROADCASTER_INDIVIDUAL.Value,
                                    cfg.MALWARE_BROADCASTER_ENABLED.Value,
                                    cfg.MALWARE_BROADCASTER_PRICE.Value);
        }
        private void SetupNightVisionBatteryTerminalNode()
        {
            CustomTerminalNode node = SetupMultiplePurchasableTerminalNode(NightVision.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES.Value ? true : !cfg.NIGHT_VISION_INDIVIDUAL.Value,
                                                cfg.NIGHT_VISION_ENABLED.Value,
                                                0,
                                                ParseUpgradePrices(cfg.NIGHT_VISION_UPGRADE_PRICES.Value));
            if(node != null) node.Unlocked = true;
        }
        private void SetupDiscombobulatorTerminalNode()
        {
            AudioClip flashSFX = AssetBundleHandler.TryLoadAudioClipAsset(ref UpgradeAssets, "Assets/ShipUpgrades/flashbangsfx.ogg");
            if (!flashSFX) return;

            flashNoise = flashSFX;
            SetupMultiplePurchasableTerminalNode(Discombobulator.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES.Value ? true : !cfg.DISCOMBOBULATOR_INDIVIDUAL.Value,
                                                cfg.DISCOMBOBULATOR_ENABLED.Value,
                                                cfg.DISCOMBOBULATOR_PRICE.Value,
                                                ParseUpgradePrices(cfg.DISCO_UPGRADE_PRICES.Value));
        }
        private void SetupHunterTerminalNode()
        {
            Hunter.SetupTierList();
            SetupMultiplePurchasableTerminalNode(Hunter.UPGRADE_NAME,
                                                true,
                                                cfg.HUNTER_ENABLED.Value,
                                                cfg.HUNTER_PRICE.Value,
                                                ParseUpgradePrices(cfg.HUNTER_UPGRADE_PRICES.Value));
        }
        private void SetupBetterScannerTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(BetterScanner.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES.Value ? true : !cfg.BETTER_SCANNER_INDIVIDUAL.Value,
                                                cfg.BETTER_SCANNER_ENABLED.Value,
                                                cfg.BETTER_SCANNER_PRICE.Value,
                                                new int[] { cfg.BETTER_SCANNER_PRICE2.Value, cfg.BETTER_SCANNER_PRICE3.Value }
                                                );
        }
        private void SetupLightningRodTerminalNode()
        {
            SetupOneTimeTerminalNode(LightningRod.UPGRADE_NAME,
                                    true,
                                    cfg.LIGHTNING_ROD_ENABLED.Value,
                                    cfg.LIGHTNING_ROD_PRICE.Value);
        }
        private void SetupWalkieGPSTerminalNode()
        {
            SetupOneTimeTerminalNode(WalkieGPS.UPGRADE_NAME,
                                    true,
                                    cfg.WALKIE_ENABLED.Value,
                                    cfg.WALKIE_PRICE.Value);
        }
        private void SetupBackMusclesTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(BackMuscles.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES.Value ? true : !cfg.BACK_MUSCLES_INDIVIDUAL.Value,
                                                cfg.BACK_MUSCLES_ENABLED.Value,
                                                cfg.BACK_MUSCLES_PRICE.Value,
                                                ParseUpgradePrices(cfg.BACK_MUSCLES_UPGRADE_PRICES.Value));
        }
        private void SetupPagerTerminalNode()
        {
            SetupOneTimeTerminalNode(FastEncryption.UPGRADE_NAME,
                                    true,
                                    cfg.PAGER_ENABLED.Value,
                                    cfg.PAGER_PRICE.Value);
        }
        private void SetupLocksmithTerminalNode()
        {
            SetupOneTimeTerminalNode(LockSmith.UPGRADE_NAME,
                                    cfg.SHARED_UPGRADES.Value ? true : !cfg.LOCKSMITH_INDIVIDUAL.Value,
                                    cfg.LOCKSMITH_ENABLED.Value,
                                    cfg.LOCKSMITH_PRICE.Value);
        }
        private void SetupPlayerHealthTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(Stimpack.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES.Value ? true : !cfg.PLAYER_HEALTH_INDIVIDUAL.Value,
                                                cfg.PLAYER_HEALTH_ENABLED.Value,
                                                cfg.PLAYER_HEALTH_PRICE.Value,
                                                ParseUpgradePrices(cfg.PLAYER_HEALTH_UPGRADE_PRICES.Value));
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
        private CustomTerminalNode SetupOneTimeTerminalNode(string upgradeName,
                                              bool shareStatus,
                                              bool enabled,
                                              int price
                                              )
        {
            GameObject oneTimeUpgrade = AssetBundleHandler.GetPerkGameObject(upgradeName);
            if (!oneTimeUpgrade) return null;

            IndividualUpgrades.Add(upgradeName, shareStatus);
            if (!enabled) return null;
            string info = SetupUpgradeInfo(upgrade: oneTimeUpgrade.GetComponent<BaseUpgrade>(), shareStatus: shareStatus, price: price);

            CustomTerminalNode node = new CustomTerminalNode(upgradeName, price, info, oneTimeUpgrade);
            terminalNodes.Add(node);
            return node;
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