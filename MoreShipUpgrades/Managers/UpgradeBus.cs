using GameNetcodeStuff;
using LethalLib.Modules;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.Items;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
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
        public bool lightningRodActive = false;
        public bool hunter = false;
        public bool playerHealth = false;
        public bool doorsHydraulicsBattery = false;

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

        public float flashCooldown = 0f;
        public float alteredWeight = 1f;

        public string contractLevel = "None";
        public string contractType = "None";
        public string DataMinigameKey = "";
        public string DataMinigameUser = "";
        public string DataMinigamePass = "";

        public Dictionary<string, List<string>> fakeBombOrders = new Dictionary<string, List<string>>();
        public Dictionary<string, float> SaleData = new Dictionary<string, float>();

        public trapDestroyerScript trapHandler = null;
        public terminalFlashScript flashScript = null;
        public lockSmithScript lockScript = null;
        public defibScript internScript = null;
        public walkieScript walkieHandler = null;

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

        private Dictionary<string, System.Func<int, float>> infoFunctions = new Dictionary<string, System.Func<int, float>>()
        {
            { exoskeletonScript.UPGRADE_NAME, level => (instance.cfg.CARRY_WEIGHT_REDUCTION - (level * instance.cfg.CARRY_WEIGHT_INCREMENT)) * 100 },
            { terminalFlashScript.UPGRADE_NAME, level => instance.cfg.DISCOMBOBULATOR_STUN_DURATION + (level * instance.cfg.DISCOMBOBULATOR_INCREMENT) },
            { strongLegsScript.UPGRADE_NAME, level =>  instance.cfg.JUMP_FORCE_UNLOCK + (level * instance.cfg.JUMP_FORCE_INCREMENT) },
            { runningShoeScript.UPGRADE_NAME, level => instance.cfg.MOVEMENT_SPEED_UNLOCK + (level * instance.cfg.MOVEMENT_INCREMENT) },
            { biggerLungScript.UPGRADE_NAME, level => instance.cfg.SPRINT_TIME_INCREASE_UNLOCK + (level * instance.cfg.SPRINT_TIME_INCREMENT) },
            { proteinPowderScript.UPGRADE_NAME, level => instance.cfg.PROTEIN_UNLOCK_FORCE + 1 + (instance.cfg.PROTEIN_INCREMENT * level) },
            { beekeeperScript.UPGRADE_NAME, level => 100 * (instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (level * instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT)) },
            { playerHealthScript.UPGRADE_NAME, level => instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_UNLOCK + (level)*instance.cfg.PLAYER_HEALTH_ADDITIONAL_HEALTH_INCREMENT },
            { DoorsHydraulicsBattery.UPGRADE_NAME, level => instance.cfg.DOOR_HYDRAULICS_BATTERY_INITIAL + (level)*instance.cfg.DOOR_HYDRAULICS_BATTERY_INCREMENTAL },
        };

        public Dictionary<string, Item> ItemsToSync = new Dictionary<string, Item>();

        private Dictionary<string, System.Func<int, int, string>> complexInfoFunctions = new Dictionary<string, System.Func<int,int, string>>()
        {
            { strongerScannerScript.UPGRADE_NAME, (level, price) => strongerScannerScript.GetBetterScannerInfo(level, price) },
            { hunterScript.UPGRADE_NAME, (level, price) => hunterScript.GetHunterInfo(level, price)},
            { nightVisionScript.UPGRADE_NAME, (level, price) => nightVisionScript.GetNightVisionInfo(level, price) },
        };

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
        internal pagerScript pageScript;

        public bool insurance;

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
            if(IsHost || IsServer) ResetShipAttributesServerRpc();
            EffectsActive = false;
            insurance = false;
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
            lightningRodActive = false;
            pager = false;
            hunter = false;
            playerHealth = false;
            sickBeats = false;

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
            flashCooldown = 0f;
            alteredWeight = 1f;
            if (wipeObjRefs) {
                UpgradeObjects = new Dictionary<string, GameObject>(); 
                trapHandler = null;
                flashScript = null;
                pageScript = null;
            }
            foreach(CustomTerminalNode node in terminalNodes)
            {
                node.Unlocked = false;
                node.CurrentUpgrade = 0;
                if(node.Name == nightVisionScript.UPGRADE_NAME) { node.Unlocked = true; }
            }

        }
        private void ResetPlayerAttributes()
        {
            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            if (player == null) return; // Disconnecting the game

            logger.LogDebug($"Resetting {player.playerUsername}'s attributes");
            if (cfg.RUNNING_SHOES_ENABLED && runningShoes) runningShoeScript.ResetRunningShoesBuff(ref player);
            if (cfg.BIGGER_LUNGS_ENABLED && biggerLungs) biggerLungScript.ResetBiggerLungsBuff(ref player);
            if (cfg.STRONG_LEGS_ENABLED && strongLegs) strongLegsScript.ResetStrongLegsBuff(ref player);
            if (cfg.PLAYER_HEALTH_ENABLED && playerHealth) playerHealthScript.ResetStimpackBuff(ref player);
        }

        [ClientRpc]
        private void ResetShipAttributesClientRpc()
        {
            HangarShipDoor shipDoors = GetShipDoors();
            if (shipDoors == null) return; // Very edge case

            logger.LogDebug($"Resetting the ship's attributes");
            if (cfg.DOOR_HYDRAULICS_BATTERY_ENABLED && doorsHydraulicsBattery) DoorsHydraulicsBattery.ResetDoorsHydraulicsBattery(ref shipDoors);
        }

        [ServerRpc]
        private void ResetShipAttributesServerRpc()
        {
            ResetShipAttributesClientRpc();
        }

        internal void GenerateSales(int seed = -1) // TODO: Save sales
        {
            if (seed == -1) seed = Random.Range(0, 999999);
            logger.LogInfo($"Generating sales with seed: {seed} on this client...");
            Random.InitState(seed);
            SaleData = new Dictionary<string, float>();
            foreach(CustomTerminalNode node in terminalNodes)
            {
                if(Random.value > cfg.SALE_PERC)
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
            if (!cfg.PEEPER_ENABLED)
            {
                Items.RemoveShopItem(ItemsToSync["Peeper"]);
                logger.LogInfo("Removing Peeper from store.");
            }
            else if (ItemsToSync["Peeper"].creditsWorth != cfg.PEEPER_PRICE) Items.UpdateShopItemPrice(ItemsToSync["Peeper"], cfg.PEEPER_PRICE);

            if (!cfg.HELMET_ENABLED)
            {
                logger.LogInfo("Removing helmet from store.");
                Items.RemoveShopItem(ItemsToSync["Helmet"]);
            }
            else if (ItemsToSync["Helmet"].creditsWorth != cfg.HELMET_PRICE) Items.UpdateShopItemPrice(ItemsToSync["Helmet"], cfg.HELMET_PRICE);

            if (!cfg.DIVEKIT_ENABLED)
            {
                logger.LogInfo("Removing divekit from store.");
                Items.RemoveShopItem(ItemsToSync["Dive"]);
            }
            else if (ItemsToSync["Dive"].creditsWorth != cfg.DIVEKIT_PRICE) Items.UpdateShopItemPrice(ItemsToSync["Dive"], cfg.DIVEKIT_PRICE);

            if (!cfg.ADVANCED_TELE_ENABLED)
            {
                logger.LogInfo("Removing AdvTele from store.");
                Items.RemoveShopItem(ItemsToSync["AdvTele"]);
            }
            else if (ItemsToSync["AdvTele"].creditsWorth != cfg.ADVANCED_TELE_PRICE) Items.UpdateShopItemPrice(ItemsToSync["AdvTele"], cfg.ADVANCED_TELE_PRICE);

            if (!cfg.WEAK_TELE_ENABLED)
            {
                logger.LogInfo("Removing Tele from store.");
                Items.RemoveShopItem(ItemsToSync["Tele"]);
            }
            else if (ItemsToSync["Tele"].creditsWorth != cfg.WEAK_TELE_PRICE) Items.UpdateShopItemPrice(ItemsToSync["Tele"], cfg.WEAK_TELE_PRICE);

            if (!cfg.MEDKIT_ENABLED)
            {
                logger.LogInfo("Removing Medkit from store.");
                Items.RemoveShopItem(ItemsToSync["Medkit"]);
            }
            else if (ItemsToSync["Medkit"].creditsWorth != cfg.MEDKIT_PRICE) Items.UpdateShopItemPrice(ItemsToSync["Medkit"], cfg.MEDKIT_PRICE);

            if (!cfg.NIGHT_VISION_ENABLED)
            {
                logger.LogInfo("Removing Night Vision from store.");
                Items.RemoveShopItem(ItemsToSync["Night"]);
            }
            else if (ItemsToSync["Night"].creditsWorth != cfg.NIGHT_VISION_PRICE) Items.UpdateShopItemPrice(ItemsToSync["Night"], cfg.NIGHT_VISION_PRICE);

            if (!cfg.WHEELBARROW_ENABLED)
            {
                logger.LogInfo("Removing Wheelbarrow from store.");
                Items.RemoveShopItem(ItemsToSync["Wheel"]);
            }
            else if (ItemsToSync["Wheel"].creditsWorth != cfg.WHEELBARROW_PRICE) Items.UpdateShopItemPrice(ItemsToSync["Wheel"], cfg.WHEELBARROW_PRICE);
        }

        void SyncAvailableContracts()
        {
            if (!cfg.DATA_CONTRACT)
            {
                logger.LogInfo("Removing data contract");
                int idx = CommandParser.contracts.IndexOf("data");
                CommandParser.contractInfos.RemoveAt(idx);
                CommandParser.contracts.RemoveAt(idx);
            }
            if (!cfg.EXTRACTION_CONTRACT)
            {
                logger.LogInfo("Removing extraction contract");
                int idx = CommandParser.contracts.IndexOf("extraction");
                CommandParser.contractInfos.RemoveAt(idx);
                CommandParser.contracts.RemoveAt(idx);
            }
            if (!cfg.EXORCISM_CONTRACT)
            {
                logger.LogInfo("Removing exorcism contract");
                int idx = CommandParser.contracts.IndexOf("exorcism");
                CommandParser.contractInfos.RemoveAt(idx);
                CommandParser.contracts.RemoveAt(idx);
            }
            if (!cfg.DEFUSAL_CONTRACT)
            {
                logger.LogInfo("Removing defusal contract");
                int idx = CommandParser.contracts.IndexOf("defusal");
                CommandParser.contractInfos.RemoveAt(idx);
                CommandParser.contracts.RemoveAt(idx);
            }
            if (!cfg.EXTERMINATOR_CONTRACT)
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
            terminalNodes.Sort();
        }
        private void SetupShutterBatteriesTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(DoorsHydraulicsBattery.UPGRADE_NAME,
                                                true,
                                                cfg.DOOR_HYDRAULICS_BATTERY_ENABLED,
                                                cfg.DOOR_HYDRAULICS_BATTERY_PRICE,
                                                ParseUpgradePrices(cfg.DOOR_HYDRAULICS_BATTERY_PRICES),
                                                "LVL {0} - ${1} - Increases the door's hydraulic capacity to remain closed by {2} units\n");
        }
        private void SetupSickBeatsTerminalNode()
        {
            string txt = $"Sick Beats - ${cfg.BEATS_PRICE}\nPlayers within a {cfg.BEATS_RADIUS} unit radius from an active boombox will have the following effects:\n\n";
            if (cfg.BEATS_SPEED) txt += $"Movement speed increased by {cfg.BEATS_SPEED_INC}\n";
            if (cfg.BEATS_DMG) txt += $"Damage inflicted increased by {cfg.BEATS_DMG_INC}\n";
            if (cfg.BEATS_DEF) txt += $"Incoming Damage multiplied by {cfg.BEATS_DEF_CO}\n";
            if (cfg.BEATS_STAMINA) txt += $"Stamina Drain multiplied by {cfg.BEATS_STAMINA_CO}\n";
            SetupOneTimeTerminalNode(
                BeatScript.UPGRADE_NAME,
                cfg.SHARED_UPGRADES ? true : !cfg.BEATS_INDIVIDUAL,
                cfg.BEATS_ENABLED,
                cfg.BEATS_PRICE,
                txt);
        }

        private void SetupBeekeperTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(
                beekeeperScript.UPGRADE_NAME,
                cfg.SHARED_UPGRADES ? true : !cfg.BEEKEEPER_INDIVIDUAL,
                cfg.BEEKEEPER_ENABLED,
                cfg.BEEKEEPER_PRICE,
                ParseUpgradePrices(cfg.BEEKEEPER_UPGRADE_PRICES),
                AssetBundleHandler.GetInfoFromJSON(beekeeperScript.UPGRADE_NAME));
        }

        private void SetupProteinPowderTerminalNode() 
        {
            SetupMultiplePurchasableTerminalNode(proteinPowderScript.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES ? true : !cfg.PROTEIN_INDIVIDUAL,
                                                cfg.PROTEIN_ENABLED,
                                                cfg.PROTEIN_PRICE,
                                                ParseUpgradePrices(cfg.PROTEIN_UPGRADE_PRICES),
                                                AssetBundleHandler.GetInfoFromJSON(proteinPowderScript.UPGRADE_NAME));
        }
        private void SetupBiggerLungsTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(biggerLungScript.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES ? true : !cfg.BIGGER_LUNGS_INDIVIDUAL,
                                                cfg.BIGGER_LUNGS_ENABLED,
                                                cfg.BIGGER_LUNGS_PRICE,
                                                ParseUpgradePrices(cfg.BIGGER_LUNGS_UPGRADE_PRICES),
                                                AssetBundleHandler.GetInfoFromJSON(biggerLungScript.UPGRADE_NAME));
        }
        private void SetupRunningShoesTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(runningShoeScript.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES ? true : !cfg.RUNNING_SHOES_INDIVIDUAL,
                                                cfg.RUNNING_SHOES_ENABLED,
                                                cfg.RUNNING_SHOES_PRICE,
                                                ParseUpgradePrices(cfg.RUNNING_SHOES_UPGRADE_PRICES),
                                                AssetBundleHandler.GetInfoFromJSON(runningShoeScript.UPGRADE_NAME));
        }
        private void SetupStrongLegsTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(strongLegsScript.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES ? true : !cfg.STRONG_LEGS_INDIVIDUAL,
                                                cfg.STRONG_LEGS_ENABLED,
                                                cfg.STRONG_LEGS_PRICE,
                                                ParseUpgradePrices(cfg.STRONG_LEGS_UPGRADE_PRICES),
                                                AssetBundleHandler.GetInfoFromJSON(strongLegsScript.UPGRADE_NAME));
        }
        private void SetupMalwareBroadcasterTerminalNode()
        {
            string desc;
            if (cfg.DESTROY_TRAP)
            {
                if (cfg.EXPLODE_TRAP)
                {
                    desc = "Broadcasted codes now explode map hazards.";
                }
                else
                {
                    desc = "Broadcasted codes now destroy map hazards.";
                }
            }
            else { desc = $"Broadcasted codes now disable map hazards for {cfg.DISARM_TIME} seconds."; }

            SetupOneTimeTerminalNode(trapDestroyerScript.UPGRADE_NAME,
                                    cfg.SHARED_UPGRADES ? true : !cfg.MALWARE_BROADCASTER_INDIVIDUAL,
                                    cfg.MALWARE_BROADCASTER_ENABLED,
                                    cfg.MALWARE_BROADCASTER_PRICE,
                                    desc);
        }
        private void SetupNightVisionBatteryTerminalNode()
        {
            CustomTerminalNode node = SetupMultiplePurchasableTerminalNode(nightVisionScript.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES ? true : !cfg.NIGHT_VISION_INDIVIDUAL,
                                                cfg.NIGHT_VISION_ENABLED,
                                                0,
                                                ParseUpgradePrices(cfg.NIGHT_VISION_UPGRADE_PRICES));
            if(node != null) node.Unlocked = true;
        }
        private void SetupDiscombobulatorTerminalNode()
        {
            AudioClip flashSFX = AssetBundleHandler.TryLoadAudioClipAsset(ref UpgradeAssets, "Assets/ShipUpgrades/flashbangsfx.ogg");
            if (!flashSFX) return;

            flashNoise = flashSFX;
            SetupMultiplePurchasableTerminalNode(terminalFlashScript.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES ? true : !cfg.DISCOMBOBULATOR_INDIVIDUAL,
                                                cfg.DISCOMBOBULATOR_ENABLED,
                                                cfg.DISCOMBOBULATOR_PRICE,
                                                ParseUpgradePrices(cfg.DISCO_UPGRADE_PRICES),
                                                AssetBundleHandler.GetInfoFromJSON(terminalFlashScript.UPGRADE_NAME));
        }
        private void SetupHunterTerminalNode()
        {
            hunterScript.SetupTierList();
            SetupMultiplePurchasableTerminalNode(hunterScript.UPGRADE_NAME,
                                                true,
                                                cfg.HUNTER_ENABLED,
                                                cfg.HUNTER_PRICE,
                                                ParseUpgradePrices(cfg.HUNTER_UPGRADE_PRICES));
        }
        private void SetupBetterScannerTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(strongerScannerScript.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES ? true : !cfg.BETTER_SCANNER_INDIVIDUAL,
                                                cfg.BETTER_SCANNER_ENABLED,
                                                cfg.BETTER_SCANNER_PRICE,
                                                new int[] { cfg.BETTER_SCANNER_PRICE2, cfg.BETTER_SCANNER_PRICE3 }
                                                );
        }
        private void SetupLightningRodTerminalNode()
        {
            SetupOneTimeTerminalNode(lightningRodScript.UPGRADE_NAME,
                                    true,
                                    cfg.LIGHTNING_ROD_ENABLED,
                                    cfg.LIGHTNING_ROD_PRICE,
                                    string.Format(AssetBundleHandler.GetInfoFromJSON(lightningRodScript.UPGRADE_NAME), cfg.LIGHTNING_ROD_PRICE, cfg.LIGHTNING_ROD_DIST));
        }
        private void SetupWalkieGPSTerminalNode()
        {
            GameObject walkie = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/walkieUpgrade.prefab");
            if (!walkie) return;

            IndividualUpgrades.Add(walkieScript.UPGRADE_NAME, true);
            if (!cfg.WALKIE_ENABLED) return;

            CustomTerminalNode node = new CustomTerminalNode(walkieScript.UPGRADE_NAME, cfg.WALKIE_PRICE, "Displays your location and time when holding a walkie talkie.\nEspecially useful for fog.", walkie);
            terminalNodes.Add(node);
        }
        private void SetupBackMusclesTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(exoskeletonScript.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES ? true : !cfg.BACK_MUSCLES_INDIVIDUAL,
                                                cfg.BACK_MUSCLES_ENABLED,
                                                cfg.BACK_MUSCLES_PRICE,
                                                ParseUpgradePrices(cfg.BACK_MUSCLES_UPGRADE_PRICES),
                                                AssetBundleHandler.GetInfoFromJSON(exoskeletonScript.UPGRADE_NAME));
        }
        private void SetupPagerTerminalNode()
        {
            SetupOneTimeTerminalNode(pagerScript.UPGRADE_NAME,
                                    true,
                                    cfg.PAGER_ENABLED,
                                    cfg.PAGER_PRICE,
                                    "Unrestrict the transmitter");
        }
        private void SetupLocksmithTerminalNode()
        {
            SetupOneTimeTerminalNode(lockSmithScript.UPGRADE_NAME,
                                    cfg.SHARED_UPGRADES ? true : !cfg.LOCKSMITH_INDIVIDUAL,
                                    cfg.LOCKSMITH_ENABLED,
                                    cfg.LOCKSMITH_PRICE,
                                    "Allows you to pick door locks by completing a minigame.");
        }
        private void SetupPlayerHealthTerminalNode()
        {
            SetupMultiplePurchasableTerminalNode(playerHealthScript.UPGRADE_NAME,
                                                cfg.SHARED_UPGRADES ? true : !cfg.PLAYER_HEALTH_INDIVIDUAL,
                                                cfg.PLAYER_HEALTH_ENABLED,
                                                cfg.PLAYER_HEALTH_PRICE,
                                                ParseUpgradePrices(cfg.PLAYER_HEALTH_UPGRADE_PRICES),
                                                AssetBundleHandler.GetInfoFromJSON(playerHealthScript.UPGRADE_NAME));
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
                                                        int[] prices,
                                                        string infoFormat = ""
                                                        )
        {
            GameObject multiPerk = AssetBundleHandler.GetPerkGameObject(upgradeName) ;
            if (!multiPerk) return null;

            IndividualUpgrades.Add(upgradeName, shareStatus);

            if (!enabled) return null;

            string infoString = "";
            if (infoFunctions.ContainsKey(upgradeName))
            {
                infoString = string.Format(infoFormat, 1, initialPrice, infoFunctions[upgradeName](0));
                for (int i = 0; i < prices.Length; i++)
                {
                    float infoResult = infoFunctions[upgradeName](i);
                    if (infoResult % 1 == 0) // It's an Integer
                        infoString += string.Format(infoFormat, i + 2, prices[i], Mathf.RoundToInt(infoFunctions[upgradeName](i + 1)));
                    else
                        infoString += string.Format(infoFormat, i + 2, prices[i], infoFunctions[upgradeName](i + 1));
                }
            }
            else if (complexInfoFunctions.ContainsKey(upgradeName))
            {
                infoString = complexInfoFunctions[upgradeName](1, initialPrice);
                for(int i = 0; i < prices.Length; i++)
                    infoString += complexInfoFunctions[upgradeName](i+2, prices[i]);
            }
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
                                              int price,
                                              string info
                                              )
        {
            GameObject oneTimeUpgrade = AssetBundleHandler.GetPerkGameObject(upgradeName);
            if (!oneTimeUpgrade) return null;

            IndividualUpgrades.Add(upgradeName, shareStatus);
            if (!enabled) return null;

            CustomTerminalNode node = new CustomTerminalNode(upgradeName, price, info, oneTimeUpgrade);
            terminalNodes.Add(node);
            return node;
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