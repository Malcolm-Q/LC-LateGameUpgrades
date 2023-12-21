using GameNetcodeStuff;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents;
using Newtonsoft.Json;
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

        public bool DestroyTraps = false;
        public bool softSteps = false;
        public bool scannerUpgrade = false;
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

        public int lungLevel = 0;
        public int huntLevel = 0;
        public int proteinLevel = 0;
        public int beeLevel = 0;
        public int backLevel = 0;
        public int runningLevel = 0;
        public int lightLevel = 0;
        public int discoLevel = 0;
        public int legLevel = 0;
        public int scanLevel = 0;
        public int nightVisionLevel = 0;

        public float flashCooldown = 0f;
        public float alteredWeight = 1f;

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

        public List<CustomTerminalNode> terminalNodes = new List<CustomTerminalNode>();

        public Dictionary<string, GameObject> UpgradeObjects = new Dictionary<string, GameObject>();

        private Dictionary<string, System.Func<int, float>> infoFunctions = new Dictionary<string, System.Func<int, float>>()
        {
            { "Back Muscles", level => (instance.cfg.CARRY_WEIGHT_REDUCTION - (level * instance.cfg.CARRY_WEIGHT_INCREMENT)) * 100 },
            { "Discombobulator", level => instance.cfg.DISCOMBOBULATOR_STUN_DURATION + (level * instance.cfg.DISCOMBOBULATOR_INCREMENT) },
            { "Light Footed", level => instance.cfg.NOISE_REDUCTION + (level * instance.cfg.NOISE_REDUCTION_INCREMENT)},
            { "Strong Legs", level =>  instance.cfg.JUMP_FORCE + (level * instance.cfg.JUMP_FORCE_INCREMENT) - 13f },
            { "Running Shoes", level => instance.cfg.MOVEMENT_SPEED + (level * instance.cfg.MOVEMENT_INCREMENT) - 4.6f },
            { "Bigger Lungs", level => instance.cfg.SPRINT_TIME_INCREASE + (level * instance.cfg.SPRINT_TIME_INCREMENT) - 11f },
            { "Protein Powder", level => instance.cfg.PROTEIN_UNLOCK_FORCE + 1 + (instance.cfg.PROTEIN_INCREMENT * level) },
            { "Beekeeper", level => 100 * (instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (level * instance.cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT)) },
        };

        public Dictionary<ulong,float> beePercs = new Dictionary<ulong,float>();
        public Dictionary<ulong,int> forceMults = new Dictionary<ulong,int>();

        public Dictionary<string,bool> IndividualUpgrades = new Dictionary<string,bool>();
        public string[] internNames, internInterests;

        public List<coilHeadItem> coilHeadItems = new List<coilHeadItem>();
        internal bool walkies;
        internal bool walkieUIActive;
        internal string version;

        public AssetBundle UpgradeAssets;
        internal bool pager;
        internal pagerScript pageScript;

        public Dictionary<string,GameObject> samplePrefabs = new Dictionary<string,GameObject>();


        void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            cfg = Plugin.cfg;
        }

        public Terminal GetTerminal()
        {
            if (terminal == null) terminal = GameObject.Find("TerminalScript").GetComponent<Terminal>();

            return terminal;
        }

        public TerminalNode ConstructNode()
        {
            modStoreInterface = new TerminalNode();
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
            DestroyTraps = false;
            softSteps = false;
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
            huntLevel = 0;
            proteinLevel = 0;
            lungLevel = 0;
            backLevel = 0;
            scanLevel = 0;
            beeLevel = 0;
            runningLevel = 0;
            lightLevel = 0;
            discoLevel = 0;
            legLevel = 0;
            nightVisionLevel = 0;
            flashCooldown = 0f;
            alteredWeight = 1f;
            trapHandler = null;
            flashScript = null;
            pageScript = null;
            if (wipeObjRefs) { UpgradeObjects = new Dictionary<string, GameObject>(); }
            foreach(CustomTerminalNode node in terminalNodes)
            {
                node.Unlocked = false;
                node.CurrentUpgrade = 0;
                if(node.Name == "NV Headset Batteries") { node.Unlocked = true; }
            }
            PlayerControllerB[] players = GameObject.FindObjectsOfType<PlayerControllerB>();
            foreach (PlayerControllerB player in players)
            {
                player.movementSpeed = 4.6f;
                player.sprintTime = 11;
                player.jumpForce = 13;
            }
        }

        internal void GenerateSales(int seed = -1)
        {
            Random.InitState(seed);
            foreach(CustomTerminalNode node in terminalNodes)
            {
                if(node.Name == "Interns") { continue; }
                if(Random.value > UpgradeBus.instance.cfg.SALE_PERC)
                {
                    node.salePerc = Random.Range(0.60f, 0.90f);
                }
                else
                {
                    node.salePerc = 1f;
                }
            }
        }

        internal void AlterStoreItems()
        {
            // nothing works :(
            Terminal[] terms = GameObject.FindObjectsOfType<Terminal>();
            foreach(Terminal term in terms)
            {
                List<CompatibleNoun> words = new List<CompatibleNoun>();
                foreach(CompatibleNoun word in term.terminalNodes.allKeywords[0].compatibleNouns)
                {
                    if(word.noun.word == "peeper")
                    {
                        if(cfg.PEEPER_ENABLED)words.Add(word);
                        word.result.itemCost = cfg.PEEPER_PRICE;
                    }
                    else if(word.noun.word == "night-vision-goggles")
                    {
                        if(cfg.NIGHT_VISION_ENABLED)words.Add(word);
                        word.result.itemCost = cfg.NIGHT_VISION_PRICE;
                    }
                    else if(word.noun.word == "portable-tele")
                    {
                        if(cfg.WEAK_TELE_ENABLED)words.Add(word);
                        word.result.itemCost = cfg.WEAK_TELE_PRICE;
                    }
                    else if(word.noun.word == "advanced-portable-tele")
                    {
                        if(cfg.ADVANCED_TELE_ENABLED)words.Add(word);
                        word.result.itemCost = cfg.ADVANCED_TELE_PRICE;
                    }
                    else
                    {
                        words.Add(word);
                    }
                    term.terminalNodes.allKeywords[0].compatibleNouns = words.ToArray();
                }
            }
        }

        internal void Reconstruct()
        {
            //AlterStoreItems();
            BuildCustomNodes();
        }
        internal void BuildCustomNodes()
        {
            terminalNodes = new List<CustomTerminalNode>();

            Dictionary<string,string> infoJson = JsonConvert.DeserializeObject<Dictionary<string,string>>(UpgradeAssets.LoadAsset<TextAsset>("Assets/ShipUpgrades/InfoStrings.json").text);

            IndividualUpgrades = new Dictionary<string, bool>();

            SetupBeekeperTerminalNode(ref infoJson);

            SetupProteinPowderTerminalNode(ref infoJson);

            SetupBiggerLungsTerminalNode(ref infoJson);

            SetupRunningShoesTerminalNode(ref infoJson);

            SetupStrongLegsTerminalNode(ref infoJson);

            SetupMalwareBroadcasterTerminalNode(ref infoJson);

            SetupLightFootedTerminalNode(ref infoJson);

            SetupNightVisionBatteryTerminalNode(ref infoJson);

            SetupDiscombobulatorTerminalNode(ref infoJson);

            SetupBetterScannerTerminalNode(ref infoJson);

            SetupLightningRodTerminalNode(ref infoJson);

            SetupWalkieGPSTerminalNode(ref infoJson);

            SetupBackMusclesTerminalNode(ref infoJson);

            SetupInternsTerminalNode(ref infoJson);

            SetupPagerTerminalNode();

            SetupLocksmithTerminalNode();
        }

        private void SetupBeekeperTerminalNode(ref Dictionary<string, string> infoJSON)
        {
            SetupMultiplePurchasableTerminalNode("Beekeeper",
                                                "Assets/ShipUpgrades/beekeeper.prefab",
                                                cfg.SHARED_UPGRADES ? true : cfg.BEEKEEPER_INDIVIDUAL,
                                                cfg.BEEKEEPER_ENABLED,
                                                cfg.BEEKEEPER_PRICE,
                                                ParseUpgradePrices(cfg.BEEKEEPER_UPGRADE_PRICES),
                                                infoJSON["Beekeeper"]);
        }

        private void SetupProteinPowderTerminalNode(ref Dictionary<string, string> infoJSON) 
        {
            SetupMultiplePurchasableTerminalNode("Protein Powder",
                                                "Assets/ShipUpgrades/ProteinPowder.prefab",
                                                cfg.SHARED_UPGRADES ? true : cfg.PROTEIN_INDIVIDUAL,
                                                cfg.PROTEIN_ENABLED,
                                                cfg.PROTEIN_PRICE,
                                                ParseUpgradePrices(cfg.PROTEIN_UPGRADE_PRICES),
                                                infoJSON["Protein Powder"]);
        }
        private void SetupBiggerLungsTerminalNode(ref Dictionary<string, string> infoJSON)
        {
            SetupMultiplePurchasableTerminalNode("Bigger Lungs",
                                                "Assets/ShipUpgrades/BiggerLungs.prefab",
                                                cfg.SHARED_UPGRADES ? true : cfg.BIGGER_LUNGS_INDIVIDUAL,
                                                cfg.BIGGER_LUNGS_ENABLED,
                                                cfg.BIGGER_LUNGS_PRICE,
                                                ParseUpgradePrices(cfg.RUNNING_SHOES_UPGRADE_PRICES),
                                                infoJSON["Running Shoes"]);
        }
        private void SetupRunningShoesTerminalNode(ref Dictionary<string, string> infoJSON)
        {
            SetupMultiplePurchasableTerminalNode("Running Shoes",
                                                "Assets/ShipUpgrades/runningShoes.prefab",
                                                cfg.SHARED_UPGRADES ? true : cfg.RUNNING_SHOES_INDIVIDUAL,
                                                cfg.RUNNING_SHOES_ENABLED,
                                                cfg.RUNNING_SHOES_PRICE,
                                                ParseUpgradePrices(cfg.RUNNING_SHOES_UPGRADE_PRICES),
                                                infoJSON["Running Shoes"]);
        }
        private void SetupStrongLegsTerminalNode(ref Dictionary<string, string> infoJSON)
        {
            SetupMultiplePurchasableTerminalNode("Strong Legs",
                                                "Assets/ShipUpgrades/strongLegs.prefab",
                                                cfg.SHARED_UPGRADES ? true : cfg.STRONG_LEGS_INDIVIDUAL,
                                                cfg.STRONG_LEGS_ENABLED,
                                                cfg.STRONG_LEGS_PRICE,
                                                ParseUpgradePrices(cfg.STRONG_LEGS_UPGRADE_PRICES),
                                                infoJSON["Strong Legs"]);
        }
        private void SetupMalwareBroadcasterTerminalNode(ref Dictionary<string, string> infoJSON)
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

            SetupOneTimeTerminalNode("Malware Broadcaster",
                                    "Assets/ShipUpgrades/destructiveCodes.prefab",
                                    cfg.SHARED_UPGRADES ? true : cfg.MALWARE_BROADCASTER_INDIVIDUAL,
                                    cfg.MALWARE_BROADCASTER_ENABLED,
                                    cfg.MALWARE_BROADCASTER_PRICE,
                                    desc);
        }
        private void SetupLightFootedTerminalNode(ref Dictionary<string, string> infoJSON)
        {
            SetupMultiplePurchasableTerminalNode("Light Footed",
                                                "Assets/ShipUpgrades/lightFooted.prefab",
                                                cfg.SHARED_UPGRADES ? true : cfg.LIGHT_FOOTED_INDIVIDUAL,
                                                cfg.LIGHT_FOOTED_ENABLED,
                                                cfg.LIGHT_FOOTED_PRICE,
                                                ParseUpgradePrices(cfg.STRONG_LEGS_UPGRADE_PRICES),
                                                infoJSON["Light Footed"]);
        }
        private void SetupNightVisionBatteryTerminalNode(ref Dictionary<string, string> infoJSON)
        {

            // night vision
            GameObject nightVision = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/nightVision.prefab");
            if (!nightVision) return;

            bool shareStatus = cfg.SHARED_UPGRADES ? true : cfg.NIGHT_VISION_INDIVIDUAL;
            IndividualUpgrades.Add("NV Headset Batteries", shareStatus);
            if (!cfg.NIGHT_VISION_ENABLED) return;

            int[] prices = ParseUpgradePrices(cfg.NIGHT_VISION_UPGRADE_PRICES);

            string infoString = "";

            float drain = (cfg.NIGHT_BATTERY_MAX - (cfg.NIGHT_BATTERY_MAX * cfg.NIGHT_VIS_STARTUP)) / cfg.NIGHT_VIS_DRAIN_SPEED;
            float regen = cfg.NIGHT_BATTERY_MAX / cfg.NIGHT_VIS_REGEN_SPEED;
            infoString += string.Format(infoJSON["NV Headset Batteries"], 1, "0", drain, regen);
            for (int i = 0; i < prices.Length; i++)
            {
                float regenAdjustment = Mathf.Clamp(cfg.NIGHT_VIS_REGEN_SPEED + (cfg.NIGHT_VIS_REGEN_INCREMENT * (i + 1)), 0, 1000);
                float drainAdjustment = Mathf.Clamp(cfg.NIGHT_VIS_DRAIN_SPEED - (cfg.NIGHT_VIS_DRAIN_INCREMENT * (i + 1)), 0, 1000);
                float batteryLife = cfg.NIGHT_BATTERY_MAX + (cfg.NIGHT_VIS_BATTERY_INCREMENT * (i + 1));

                string drainTime = "infinite";
                if (drainAdjustment != 0) drainTime = ((batteryLife - (batteryLife * cfg.NIGHT_VIS_STARTUP)) / drainAdjustment).ToString("F2");

                string regenTime = "infinite";
                if (regenAdjustment != 0) regenTime = (batteryLife / regenAdjustment).ToString("F2");

                infoString += string.Format(infoJSON["NV Headset Batteries"], i + 2, prices[i], drainTime, regenTime);
            }
            CustomTerminalNode node = new CustomTerminalNode(
                "NV Headset Batteries",
                cfg.NIGHT_VISION_PRICE,
                infoString,
                nightVision,
                prices,
                prices.Length
                );
            node.Unlocked = true;
            terminalNodes.Add(node);
        }
        private void SetupDiscombobulatorTerminalNode(ref Dictionary<string, string> infoJSON)
        {
            AudioClip flashSFX = AssetBundleHandler.TryLoadAudioClipAsset(ref UpgradeAssets, "Assets/ShipUpgrades/flashbangsfx.ogg");
            if (!flashSFX) return;

            flashNoise = flashSFX;
            SetupMultiplePurchasableTerminalNode("Discombobulator",
                                                "Assets/ShipUpgrades/terminalFlash.prefab",
                                                cfg.SHARED_UPGRADES ? true : cfg.DISCOMBOBULATOR_INDIVIDUAL,
                                                cfg.DISCOMBOBULATOR_ENABLED,
                                                cfg.DISCOMBOBULATOR_PRICE,
                                                ParseUpgradePrices(cfg.DISCO_UPGRADE_PRICES),
                                                infoJSON["Discombobulator"]);
        }
        private void SetupBetterScannerTerminalNode(ref Dictionary<string, string> infoJSON)
        {
            GameObject strongScan = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/strongScanner.prefab");
            if (!strongScan) return;

            //hunter
            GameObject hunter = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/Hunter.prefab");
            if (hunter != null) 
            {
                IndividualUpgrades.Add("Hunter", true);
                if (cfg.HUNTER_ENABLED)
                {
                    string infoString = "Not implemented, if you see this @_kieth and tell him how stupid he is.";
                    CustomTerminalNode node = new CustomTerminalNode(
                        "Hunter",
                        cfg.HUNTER_PRICE,
                        infoString,
                        hunter,
                        new int[] { cfg.HUNTER_PRICE2, cfg.HUNTER_PRICE3 },
                        2);
                    terminalNodes.Add(node);
                }
            }

            string infoString = string.Format(infoJSON["Better Scanner1"], 1, cfg.BETTER_SCANNER_PRICE, cfg.NODE_DISTANCE_INCREASE, cfg.SHIP_AND_ENTRANCE_DISTANCE_INCREASE);
            infoString += string.Format(infoJSON["Better Scanner2"], 2, cfg.BETTER_SCANNER_PRICE2);
            string enemStatus = cfg.BETTER_SCANNER_ENEMIES ? " and enemies" : "";
            infoString += string.Format(infoJSON["Better Scanner3"], 3, cfg.BETTER_SCANNER_PRICE3, enemStatus);
            infoString += "hives and scrap command display the location of the most valuable hives and scrap on the map.\n";
            CustomTerminalNode node = new CustomTerminalNode(
                "Better Scanner",
                cfg.BETTER_SCANNER_PRICE,
                infoString,
                strongScan,
                new int[] { cfg.BETTER_SCANNER_PRICE2, cfg.BETTER_SCANNER_PRICE3 },
                2);
            terminalNodes.Add(node);
        }
        private void SetupLightningRodTerminalNode(ref Dictionary<string, string> infoJSON)
        {
            SetupOneTimeTerminalNode(lightningRodScript.UPGRADE_NAME,
                                    "Assets/ShipUpgrades/LightningRod.prefab",
                                    true,
                                    cfg.LIGHTNING_ROD_ENABLED,
                                    cfg.LIGHTNING_ROD_PRICE,
                                    string.Format(infoJSON[lightningRodScript.UPGRADE_NAME], cfg.LIGHTNING_ROD_PRICE, cfg.LIGHTNING_ROD_DIST));
        }
        private void SetupWalkieGPSTerminalNode(ref Dictionary<string, string> infoJSON)
        {
            GameObject walkie = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/walkieUpgrade.prefab");
            if (!walkie) return;

            IndividualUpgrades.Add("Walkie GPS", true);
            if (!cfg.WALKIE_ENABLED) return;

            CustomTerminalNode node = new CustomTerminalNode("Walkie GPS", cfg.WALKIE_PRICE, "Displays your location and time when holding a walkie talkie.\nEspecially useful for fog.", walkie);
            terminalNodes.Add(node);
        }
        private void SetupBackMusclesTerminalNode(ref Dictionary<string, string> infoJSON)
        {
            SetupMultiplePurchasableTerminalNode("Back Muscles",
                                                "Assets/ShipUpgrades/exoskeleton.prefab",
                                                cfg.SHARED_UPGRADES ? true : cfg.BACK_MUSCLES_INDIVIDUAL,
                                                cfg.BACK_MUSCLES_ENABLED,
                                                cfg.BACK_MUSCLES_PRICE,
                                                ParseUpgradePrices(cfg.BACK_MUSCLES_UPGRADE_PRICES),
                                                infoJSON["Back Muscles"]);
        }
        private void SetupInternsTerminalNode(ref Dictionary<string, string> infoJSON)
        {
            SetupOneTimeTerminalNode("Interns",
                                    "Assets/ShipUpgrades/Intern.prefab",
                                    cfg.SHARED_UPGRADES ? true : cfg.INTERN_INDIVIDUAL,
                                    cfg.INTERN_ENABLED,
                                    cfg.INTERN_PRICE,
                                    string.Format(infoJSON["Interns"], cfg.INTERN_PRICE));
        }
        private void SetupPagerTerminalNode()
        {
            SetupOneTimeTerminalNode("Fast Encryption",
                                    "Assets/ShipUpgrades/Pager.prefab",
                                    true,
                                    cfg.PAGER_ENABLED,
                                    cfg.PAGER_PRICE,
                                    "Unrestrict the transmitter");
        }
        private void SetupLocksmithTerminalNode()
        {
            SetupOneTimeTerminalNode("Locksmith",
                                    "Assets/ShipUpgrades/LockSmith.prefab",
                                    cfg.SHARED_UPGRADES ? true : cfg.LOCKSMITH_INDIVIDUAL,
                                    cfg.LOCKSMITH_ENABLED,
                                    cfg.LOCKSMITH_PRICE,
                                    "Allows you to pick door locks by completing a minigame.");
        }
        /// <summary>
        /// Generic function where it adds a terminal node for an upgrade that can be purchased multiple times
        /// </summary>
        /// <param name="upgradeName"> Name of the upgrade </param>
        /// <param name="path"> Path to the asset associated to this upgrade </param>
        /// <param name="shareStatus"> Wether the upgrade is shared through all players or only for the player who purchased it</param>
        /// <param name="enabled"> Wether the upgrade is enabled for gameplay or not</param>
        /// <param name="initialPrice"> The initial price when purchasing the upgrade for the first time</param>
        /// <param name="prices"> Prices for any subsequent purchases of the upgrade</param>
        /// <param name="infoFormat"> The format of the information displayed when checking the upgrade's info</param>
        private void SetupMultiplePurchasableTerminalNode(string upgradeName,
                                                        string path,
                                                        bool shareStatus,
                                                        bool enabled,
                                                        int initialPrice,
                                                        int[] prices,
                                                        string infoFormat
                                                        )
        {
            GameObject multiPerk = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, path);
            if (!multiPerk) return;

            IndividualUpgrades.Add(upgradeName, shareStatus);

            if (!enabled) return;

            string infoString = string.Format(infoFormat, 1, initialPrice, infoFunctions[upgradeName](0));
            for (int i = 0; i < prices.Length; i++)
            {
                float infoResult = infoFunctions[upgradeName](i);
                if (infoResult % 1 == 0) // It's an Integer
                    infoString += string.Format(infoFormat, i + 2, prices[i], Mathf.RoundToInt(infoFunctions[upgradeName](i+1)));
                else
                    infoString += string.Format(infoFormat, i + 2, prices[i], infoFunctions[upgradeName](i + 1));
            }
            CustomTerminalNode node = new CustomTerminalNode(upgradeName, initialPrice, infoString, multiPerk, prices, prices.Length);
            terminalNodes.Add(node);
        }
        /// <summary>
        /// Generic function where it adds a terminal node for an upgrade that can only be bought once
        /// </summary>
        /// <param name="upgradeName"> Name of the upgrade</param>
        /// <param name="path">Path to the asset associated to this upgrade</param>
        /// <param name="shareStatus"> Wether the upgrade is shared through all players or only for the player who purchased it</param>
        /// <param name="enabled"> Wether the upgrade is enabled for gameplay or not</param>
        /// <param name="price"></param>
        /// <param name="info"> The information displayed when checking the upgrade's info</param>
        private void SetupOneTimeTerminalNode(string upgradeName,
                                              string path,
                                              bool shareStatus,
                                              bool enabled,
                                              int price,
                                              string info
                                              )
        {
            GameObject oneTimeUpgrade = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, path);
            if (!oneTimeUpgrade) return;

            IndividualUpgrades.Add(upgradeName, shareStatus);
            if (!enabled) return;

            CustomTerminalNode node = new CustomTerminalNode(upgradeName, price, info, oneTimeUpgrade);
            terminalNodes.Add(node);
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
                    Debug.LogWarning($"[LGU] Invalid upgrade price submitted: {prices[i]}");
                    prices[i] = -1;

                }
            }
            if (prices.Length == 1 && prices[0] == -1) { prices = new int[0]; }

            return prices;
        }
    }
}