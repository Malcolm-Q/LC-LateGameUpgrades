using GameNetcodeStuff;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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

        public int lungLevel = 0;
        public int proteinLevel = 0;
        public int beeLevel = 0;
        public int backLevel = 0;
        public int runningLevel = 0;
        public int lightLevel = 0;
        public int discoLevel = 0;
        public int legLevel = 0;
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

        public Dictionary<ulong,float> beePercs = new Dictionary<ulong,float>();
        public Dictionary<ulong,int> forceMults = new Dictionary<ulong,int>();

        public Dictionary<string,bool> IndividualUpgrades = new Dictionary<string,bool>();
        public string[] internNames, internInterests;

        public List<coilHeadItem> coilHeadItems = new List<coilHeadItem>();
        internal bool walkies;
        internal bool walkieUIActive;
        internal string version;

        public AssetBundle UpgradeAssets;


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
            proteinLevel = 0;
            lungLevel = 0;
            backLevel = 0;
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
            // nothing works
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
            BuildCustomNodes();
        }
        internal void BuildCustomNodes()
        {
            terminalNodes = new List<CustomTerminalNode>();

            Dictionary<string,string> infoJson = JsonConvert.DeserializeObject<Dictionary<string,string>>(UpgradeAssets.LoadAsset<TextAsset>("Assets/ShipUpgrades/InfoStrings.json").text);

            bool shareStatus;

            //Beekeeper
            GameObject beekeeper = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/beekeeper.prefab");
            if (beekeeper != null) 
            {
                shareStatus = cfg.SHARED_UPGRADES ? true : cfg.BEEKEEPER_INDIVIDUAL;
                IndividualUpgrades = new Dictionary<string, bool>
                {
                    { "Beekeeper", shareStatus }
                };
                if (cfg.BEEKEEPER_ENABLED)
                {
                    string[] priceString = cfg.BEEKEEPER_UPGRADE_PRICES.Split(',').ToArray();
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

                    string infoString = string.Format(infoJson["Beekeeper"], 1, cfg.BEEKEEPER_PRICE, (int)(100 * cfg.BEEKEEPER_DAMAGE_MULTIPLIER));
                    for (int i = 0; i < prices.Length; i++)
                    {
                        infoString += string.Format(infoJson["Beekeeper"], i + 2, prices[i], (int)(100 * (cfg.BEEKEEPER_DAMAGE_MULTIPLIER - ((i + 1) * cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT))));
                    }
                    CustomTerminalNode beeNode = new CustomTerminalNode("Beekeeper", cfg.BEEKEEPER_PRICE, infoString, beekeeper, prices, prices.Length);
                    terminalNodes.Add(beeNode);
                }
            }

            // protein
            GameObject prot = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/ProteinPowder.prefab");
            if (prot != null)
            {
                shareStatus = cfg.SHARED_UPGRADES ? true : cfg.PROTEIN_INDIVIDUAL;
                IndividualUpgrades.Add("Protein Powder", shareStatus);
                if (cfg.PROTEIN_ENABLED)
                {
                    string[] priceString = cfg.PROTEIN_UPGRADE_PRICES.Split(',').ToArray();
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

                    string infoString = string.Format(infoJson["Protein Powder"], 1, cfg.PROTEIN_PRICE, cfg.PROTEIN_UNLOCK_FORCE);
                    for (int i = 0; i < prices.Length; i++)
                    {
                        infoString += string.Format(infoJson["Protein Powder"], i + 2, prices[i], cfg.PROTEIN_UNLOCK_FORCE + 1 + (cfg.PROTEIN_INCREMENT * i));
                    }
                    CustomTerminalNode protNode = new CustomTerminalNode("Protein Powder", cfg.PROTEIN_PRICE, infoString, prot, prices, prices.Length);
                    terminalNodes.Add(protNode);
                }
            }

            // lungs
            GameObject biggerLungs = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/BiggerLungs.prefab");
            if (biggerLungs != null) 
            {
                shareStatus = cfg.SHARED_UPGRADES ? true : cfg.BIGGER_LUNGS_INDIVIDUAL;
                IndividualUpgrades.Add("Bigger Lungs", shareStatus);
                if (cfg.BIGGER_LUNGS_ENABLED)
                {
                    string[] priceString = cfg.BIGGER_LUNGS_UPGRADE_PRICES.Split(',').ToArray();
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
                    string infoString = string.Format(infoJson["Bigger Lungs"], 1, cfg.BIGGER_LUNGS_PRICE, cfg.SPRINT_TIME_INCREASE - 11f);
                    for (int i = 0; i < prices.Length; i++)
                    {
                        infoString += string.Format(infoJson["Bigger Lungs"], i + 2, prices[i], (cfg.SPRINT_TIME_INCREASE + ((i + 1) * cfg.SPRINT_TIME_INCREMENT)) - 11f);
                    }

                    CustomTerminalNode lungNode = new CustomTerminalNode("Bigger Lungs", cfg.BIGGER_LUNGS_PRICE, infoString, biggerLungs, prices, prices.Length);
                    terminalNodes.Add(lungNode);
                }
            }


            // running shoes
            GameObject runningShoes = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/runningShoes.prefab");
            if (runningShoes != null) 
            {
                shareStatus = cfg.SHARED_UPGRADES ? true : cfg.RUNNING_SHOES_INDIVIDUAL;
                IndividualUpgrades.Add("Running Shoes", shareStatus);
                if (cfg.RUNNING_SHOES_ENABLED)
                {
                    string[] priceString = cfg.RUNNING_SHOES_UPGRADE_PRICES.Split(',').ToArray();
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
                    string infoString = string.Format(infoJson["Running Shoes"], 1, cfg.RUNNING_SHOES_PRICE, cfg.MOVEMENT_SPEED - 4.6f);
                    for (int i = 0; i < prices.Length; i++)
                    {
                        infoString += string.Format(infoJson["Running Shoes"], i + 2, prices[i], (cfg.MOVEMENT_SPEED + ((i + 1) * cfg.MOVEMENT_INCREMENT)) - 4.6f);
                    }
                    CustomTerminalNode node = new CustomTerminalNode("Running Shoes", cfg.RUNNING_SHOES_PRICE, infoString, runningShoes, prices, prices.Length);
                    terminalNodes.Add(node);
                }
            }

            // strong legs
            GameObject strongLegs = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/strongLegs.prefab");
            if (strongLegs != null) 
            {
                shareStatus = cfg.SHARED_UPGRADES ? true : cfg.STRONG_LEGS_INDIVIDUAL;
                IndividualUpgrades.Add("Strong Legs", shareStatus);
                if (cfg.STRONG_LEGS_ENABLED)
                {
                    string[] priceString = cfg.STRONG_LEGS_UPGRADE_PRICES.Split(',').ToArray();
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
                    string infoString = string.Format(infoJson["Strong Legs"], 1, cfg.STRONG_LEGS_PRICE, cfg.JUMP_FORCE - 13f);
                    for (int i = 0; i < prices.Length; i++)
                    {
                        infoString += string.Format(infoJson["Strong Legs"], i + 2, prices[i], (cfg.JUMP_FORCE + ((i + 1) * cfg.JUMP_FORCE_INCREMENT)) - 13f);
                    }
                    CustomTerminalNode node = new CustomTerminalNode("Strong Legs", cfg.STRONG_LEGS_PRICE, infoString, strongLegs, prices, prices.Length);
                    terminalNodes.Add(node);
                }
            }

            // destructive codes
            GameObject destructiveCodes = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/destructiveCodes.prefab");
            if (destructiveCodes != null) 
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
                shareStatus = cfg.SHARED_UPGRADES ? true : cfg.MALWARE_BROADCASTER_INDIVIDUAL;
                IndividualUpgrades.Add("Malware Broadcaster", shareStatus);
                if (cfg.MALWARE_BROADCASTER_ENABLED)
                {
                    CustomTerminalNode node = new CustomTerminalNode("Malware Broadcaster", cfg.MALWARE_BROADCASTER_PRICE, desc, destructiveCodes);
                    terminalNodes.Add(node);
                }
            }


            // light footed[{0}]
            GameObject lightFooted = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/lightFooted.prefab");
            if (lightFooted != null)
            {
                shareStatus = cfg.SHARED_UPGRADES ? true : cfg.LIGHT_FOOTED_INDIVIDUAL;
                IndividualUpgrades.Add("Light Footed", shareStatus);
                if (cfg.LIGHT_FOOTED_ENABLED)
                {
                    string[] priceString = cfg.LIGHT_FOOTED_UPGRADE_PRICES.Split(',').ToArray();
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
                    string infoString = string.Format(infoJson["Light Footed"], 1, cfg.LIGHT_FOOTED_PRICE, cfg.NOISE_REDUCTION);
                    for (int i = 0; i < prices.Length; i++)
                    {
                        infoString += string.Format(infoJson["Light Footed"], i + 2, prices[i], cfg.NOISE_REDUCTION + ((i + 1) * cfg.NOISE_REDUCTION_INCREMENT));
                    }
                    CustomTerminalNode node = new CustomTerminalNode("Light Footed", cfg.LIGHT_FOOTED_PRICE, infoString, lightFooted, prices, prices.Length);
                    terminalNodes.Add(node);
                }
            }
            

            // night vision
            GameObject nightVision = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/nightVision.prefab");
            if (nightVision != null)
            {
                shareStatus = cfg.SHARED_UPGRADES ? true : cfg.NIGHT_VISION_INDIVIDUAL;
                IndividualUpgrades.Add("NV Headset Batteries", shareStatus);
                if (cfg.NIGHT_VISION_ENABLED)
                {
                    string[] priceString = cfg.NIGHT_VISION_UPGRADE_PRICES.Split(',').ToArray();
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
                    string infoString = "";

                    float drain = (cfg.NIGHT_BATTERY_MAX - (cfg.NIGHT_BATTERY_MAX * cfg.NIGHT_VIS_STARTUP)) / cfg.NIGHT_VIS_DRAIN_SPEED;
                    float regen = cfg.NIGHT_BATTERY_MAX / cfg.NIGHT_VIS_REGEN_SPEED;
                    infoString += string.Format(infoJson["NV Headset Batteries"], 1, "0", drain, regen);
                    for (int i = 0; i < prices.Length; i++)
                    {
                        float regenAdjustment = Mathf.Clamp(cfg.NIGHT_VIS_REGEN_SPEED + (cfg.NIGHT_VIS_REGEN_INCREMENT * (i + 1)), 0, 1000);
                        float drainAdjustment = Mathf.Clamp(cfg.NIGHT_VIS_DRAIN_SPEED - (cfg.NIGHT_VIS_DRAIN_INCREMENT * (i + 1)), 0, 1000);
                        float batteryLife = cfg.NIGHT_BATTERY_MAX + (cfg.NIGHT_VIS_BATTERY_INCREMENT * (i + 1));

                        string drainTime = "infinite";
                        if (drainAdjustment != 0) drainTime = ((batteryLife - (batteryLife * cfg.NIGHT_VIS_STARTUP)) / drainAdjustment).ToString("F2");

                        string regenTime = "infinite";
                        if (regenAdjustment != 0) regenTime = (batteryLife / regenAdjustment).ToString("F2");

                        infoString += string.Format(infoJson["NV Headset Batteries"], i + 2, prices[i], drainTime, regenTime);
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
            }
            


            // discombob
            GameObject flash = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/terminalFlash.prefab");
            AudioClip flashSFX = AssetBundleHandler.TryLoadAudioClipAsset(ref UpgradeAssets, "Assets/ShipUpgrades/flashbangsfx.ogg");
            if (flash != null && flashSFX != null)
            {
                flashNoise = flashSFX;
                shareStatus = cfg.SHARED_UPGRADES ? true : cfg.DISCOMBOBULATOR_INDIVIDUAL;
                IndividualUpgrades.Add("Discombobulator", shareStatus);
                if (cfg.DISCOMBOBULATOR_ENABLED)
                {
                    string[] priceString = cfg.DISCO_UPGRADE_PRICES.Split(',').ToArray();
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
                    string infoString = string.Format(infoJson["Discombobulator"], 1, cfg.DISCOMBOBULATOR_PRICE, cfg.DISCOMBOBULATOR_STUN_DURATION);
                    for (int i = 0; i < prices.Length; i++)
                    {
                        infoString += string.Format(infoJson["Discombobulator"], i + 2, prices[i], cfg.DISCOMBOBULATOR_STUN_DURATION + ((i + 1) * cfg.DISCOMBOBULATOR_INCREMENT));
                    }
                    CustomTerminalNode node = new CustomTerminalNode("Discombobulator", cfg.DISCOMBOBULATOR_PRICE, infoString, flash, prices, prices.Length);
                    terminalNodes.Add(node);
                }
            }

            //stronger scanner
            GameObject strongScan = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/strongScanner.prefab");
            if (strongScan != null) 
            {
                shareStatus = cfg.SHARED_UPGRADES ? true : cfg.BETTER_SCANNER_INDIVIDUAL;
                IndividualUpgrades.Add("Better Scanner", shareStatus);
                if (cfg.BETTER_SCANNER_ENABLED)
                {
                    string LOS = cfg.REQUIRE_LINE_OF_SIGHT ? "Does not remove" : "Removes";
                    string info = $"Increase distance nodes can be scanned by {cfg.NODE_DISTANCE_INCREASE} units.  \nIncrease distance Ship and Entrance can be scanned by {cfg.SHIP_AND_ENTRANCE_DISTANCE_INCREASE} units.\n{LOS} LOS requirement\n";
                    CustomTerminalNode node = new CustomTerminalNode("Better Scanner", cfg.BETTER_SCANNER_PRICE, info, strongScan);
                    terminalNodes.Add(node);
                }
            }


            //lightning
            GameObject lightningRod = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/LightningRod.prefab");
            if (lightningRod != null) 
            {
                shareStatus = true; // It doesn't really make sense making this individual
                IndividualUpgrades.Add(lightningRodScript.UPGRADE_NAME, shareStatus);
                if (cfg.LIGHTNING_ROD_ENABLED)
                {
                    CustomTerminalNode node = new CustomTerminalNode(lightningRodScript.UPGRADE_NAME, cfg.LIGHTNING_ROD_PRICE, string.Format(infoJson[lightningRodScript.UPGRADE_NAME], cfg.LIGHTNING_ROD_PRICE, cfg.LIGHTNING_ROD_DIST), lightningRod);
                    terminalNodes.Add(node);
                }
            }

            // walkie
            GameObject walkie = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/walkieUpgrade.prefab");
            if (walkie != null) 
            {
                IndividualUpgrades.Add("Walkie GPS", true);
                if (cfg.WALKIE_ENABLED)
                {
                    CustomTerminalNode node = new CustomTerminalNode("Walkie GPS", cfg.WALKIE_PRICE, "Displays your location and time when holding a walkie talkie.\nEspecially useful for fog.", walkie);
                    terminalNodes.Add(node);
                }
            }

            // back muscles
            GameObject exoskel = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/exoskeleton.prefab");
            if (exoskel != null) 
            {
                shareStatus = cfg.SHARED_UPGRADES ? true : cfg.BACK_MUSCLES_INDIVIDUAL;
                IndividualUpgrades.Add("Back Muscles", shareStatus);
                if (cfg.BACK_MUSCLES_ENABLED)
                {
                    string[] priceString = cfg.BACK_MUSCLES_UPGRADE_PRICES.Split(',').ToArray();
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
                    string infoString = string.Format(infoJson["Back Muscles"], 1, cfg.BACK_MUSCLES_PRICE, Mathf.RoundToInt(cfg.CARRY_WEIGHT_REDUCTION * 100));
                    for (int i = 0; i < prices.Length; i++)
                    {
                        infoString += string.Format(infoJson["Back Muscles"], i + 2, prices[i], Mathf.RoundToInt((cfg.CARRY_WEIGHT_REDUCTION - ((i + 1) * cfg.CARRY_WEIGHT_INCREMENT)) * 100));
                    }
                    CustomTerminalNode node = new CustomTerminalNode("Back Muscles", cfg.BACK_MUSCLES_PRICE, infoString, exoskel, prices, prices.Length);
                    terminalNodes.Add(node);
                }
            }
            
            // interns
            GameObject intern = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/Pager.prefab");
            if (intern != null) 
            {
                shareStatus = cfg.SHARED_UPGRADES ? true : cfg.INTERN_INDIVIDUAL;
                IndividualUpgrades.Add("Interns", shareStatus);
                if (cfg.INTERN_ENABLED)
                {
                    CustomTerminalNode node = new CustomTerminalNode("Interns", cfg.INTERN_PRICE, string.Format(infoJson["Interns"], cfg.INTERN_PRICE), intern);
                    terminalNodes.Add(node);
                }
            }

            //lockSmith
            GameObject lockSmith = AssetBundleHandler.TryLoadGameObjectAsset(ref UpgradeAssets, "Assets/ShipUpgrades/LockSmith.prefab");
            if (lockSmith != null) 
            {
                shareStatus = cfg.SHARED_UPGRADES ? true : cfg.LOCKSMITH_INDIVIDUAL;
                IndividualUpgrades.Add("Locksmith", shareStatus);
                if (cfg.LOCKSMITH_ENABLED)
                {
                    CustomTerminalNode node = new CustomTerminalNode("Locksmith", cfg.LOCKSMITH_PRICE, "Allows you to pick door locks by completing a minigame.", lockSmith);
                    terminalNodes.Add(node);
                }
            }

        }
    }
}