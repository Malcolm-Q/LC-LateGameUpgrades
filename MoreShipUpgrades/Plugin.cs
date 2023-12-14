using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LethalLib.Modules;
using MoreShipUpgrades.UpgradeComponents;
using MoreShipUpgrades.Managers;
using System.IO;
using System.Reflection;
using MoreShipUpgrades.Misc;
using BepInEx.Bootstrap;
using Newtonsoft.Json;

namespace MoreShipUpgrades
{
    [BepInEx.BepInPlugin(Metadata.GUID,Metadata.NAME,Metadata.VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(Metadata.GUID);
        public static Plugin instance;
        public static ManualLogSource mls;
        public static PluginConfig cfg { get; private set; }


        void Awake()
        {
            // TODO: Move item info strings to a json file for the love of god.

            cfg = new(base.Config);
            cfg.InitBindings();

            mls = BepInEx.Logging.Logger.CreateLogSource(Metadata.NAME);
            instance = this;

            // netcode patching stuff
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }

            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "shipupgrades");
            AssetBundle UpgradeAssets = AssetBundle.LoadFromFile(assetDir);

            string infoStrings = GetJsonContent();
            Dictionary<string,string> infoJson = JsonConvert.DeserializeObject<Dictionary<string,string>>(UpgradeAssets.LoadAsset<TextAsset>("Assets/ShipUpgrades/InfoStrings.json").text);

            GameObject busGO = new GameObject("UpgradeBus");
            busGO.AddComponent<UpgradeBus>();

            UpgradeBus.instance.version = Metadata.VERSION;

            UpgradeBus.instance.internNames = infoJson["InternNames"].Split(",");
            UpgradeBus.instance.internInterests = infoJson["InternInterests"].Split(",");

            GameObject modStore = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/LGUStore.prefab");
            modStore.AddComponent<LGUStore>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(modStore);
            UpgradeBus.instance.modStorePrefab = modStore;

            //TP button sfx 
            AudioClip itemBreak = UpgradeAssets.LoadAsset<AudioClip>("Assets/ShipUpgrades/break.mp3");
            AudioClip error = UpgradeAssets.LoadAsset<AudioClip>("Assets/ShipUpgrades/error.mp3");
            AudioClip buttonPressed = UpgradeAssets.LoadAsset<AudioClip>("Assets/ShipUpgrades/ButtonPress2.ogg");

            // intro screen
            UpgradeBus.instance.introScreen = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/IntroScreen.prefab");
            UpgradeBus.instance.introScreen.AddComponent<IntroScreenScript>();

            //tp button
            Item tpBut = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/TpButton.asset");
            tpBut.itemName = "Portable Tele";
            TPButtonScript tpScript = tpBut.spawnPrefab.AddComponent<TPButtonScript>();
            tpScript.itemProperties = tpBut;
            tpScript.grabbable = true;
            tpScript.grabbableToEnemies = true;
            tpScript.ItemBreak = itemBreak;
            tpScript.useCooldown = 2f;
            tpScript.error = error;
            tpScript.buttonPress = buttonPressed;
            tpBut.creditsWorth = cfg.WEAK_TELE_PRICE;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(tpBut.spawnPrefab);

            if(cfg.WEAK_TELE_ENABLED)
            {
                TerminalNode PortNode = new TerminalNode();
                PortNode.displayText = string.Format(infoJson["Portable Tele"], (int)(cfg.CHANCE_TO_BREAK * 100));
                Items.RegisterShopItem(tpBut,null,null,PortNode, tpBut.creditsWorth);
            }

            //TP button advanced
            Item tpButAdvanced = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/TpButtonAdv.asset");
            tpButAdvanced.creditsWorth = cfg.ADVANCED_TELE_PRICE;
            tpButAdvanced.itemName = "Advanced Portable Tele";
            AdvTPButtonScript butScript = tpButAdvanced.spawnPrefab.AddComponent<AdvTPButtonScript>();
            butScript.itemProperties = tpButAdvanced;
            butScript.grabbable = true;
            butScript.useCooldown = 2f;
            butScript.grabbableToEnemies = true;
            butScript.ItemBreak = itemBreak;
            butScript.error = error;
            butScript.buttonPress = buttonPressed;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(tpButAdvanced.spawnPrefab);

            if(cfg.ADVANCED_TELE_ENABLED)
            {
                TerminalNode advNode = new TerminalNode();
                advNode.displayText = string.Format(infoJson["Advanced Portable Tele"], (int)(cfg.ADV_CHANCE_TO_BREAK * 100));
                Items.RegisterShopItem(tpButAdvanced,null,null,advNode, tpButAdvanced.creditsWorth);
            }

            //Night Vision Item
            Item nightVisionItem = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/NightVisionItem.asset");
            nightVisionItem.creditsWorth = cfg.NIGHT_VISION_PRICE;
            nightVisionItem.spawnPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            NightVisionItemScript visScript = nightVisionItem.spawnPrefab.AddComponent<NightVisionItemScript>(); 
            visScript.itemProperties = nightVisionItem;
            visScript.grabbable = true;
            visScript.useCooldown = 2f;
            visScript.grabbableToEnemies=true;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(nightVisionItem.spawnPrefab);
            if(cfg.NIGHT_VISION_ENABLED)
            {
                TerminalNode nightNode = new TerminalNode();
                string grantStatus = cfg.NIGHT_VISION_INDIVIDUAL ? "one" : "all";
                string loseOnDeath = cfg.LOSE_NIGHT_VIS_ON_DEATH ? "be" : "not be";
                nightNode.displayText = string.Format(infoJson["Night Vision"], grantStatus, loseOnDeath, cfg.NIGHT_VIS_DRAIN_SPEED, cfg.NIGHT_VIS_REGEN_SPEED);
                Items.RegisterShopItem(nightVisionItem,null,null,nightNode, nightVisionItem.creditsWorth);
            }

            //coil head Item
            Item Peeper = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/coilHead.asset");
            Peeper.creditsWorth = cfg.PEEPER_PRICE;
            Peeper.spawnPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            coilHeadItem peepScript = Peeper.spawnPrefab.AddComponent<coilHeadItem>(); 
            peepScript.itemProperties = Peeper;
            peepScript.grabbable = true;
            peepScript.grabbableToEnemies=true;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(Peeper.spawnPrefab);
            if(cfg.PEEPER_ENABLED)
            {
                TerminalNode peepNode = new TerminalNode();
                peepNode.displayText = "Looks at coil heads, don't lose it\n";
                Items.RegisterShopItem(Peeper,null,null,peepNode, Peeper.creditsWorth);
            }

            //beekeeper
            GameObject beekeeper = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/beekeeper.prefab");
            beekeeper.AddComponent<beekeeperScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(beekeeper);
            bool shareStatus = cfg.SHARED_UPGRADES ? false : cfg.BEEKEEPER_INDIVIDUAL;
            UpgradeBus.instance.IndividualUpgrades.Add("Beekeeper", shareStatus);
            if(cfg.BEEKEEPER_ENABLED)
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
                if(prices.Length == 1 && prices[0] == -1) { prices = new int[0]; }

                string infoString = string.Format(infoJson["Beekeeper"],1,cfg.BEEKEEPER_PRICE,(int)(100 * cfg.BEEKEEPER_DAMAGE_MULTIPLIER));
                for(int i = 0; i < prices.Length; i++)
                {
                    infoString += string.Format(infoJson["Beekeeper"], i + 2, prices[i],(int)(100 * (cfg.BEEKEEPER_DAMAGE_MULTIPLIER - ((i+1) * cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT))));
                }
                CustomTerminalNode beeNode = new CustomTerminalNode("Beekeeper", cfg.BEEKEEPER_PRICE, infoString, beekeeper, prices, prices.Length);
                UpgradeBus.instance.terminalNodes.Add(beeNode);
            }

            //protein powder
            GameObject prot = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/ProteinPowder.prefab");
            prot.AddComponent<proteinPowderScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prot);
            shareStatus = cfg.SHARED_UPGRADES ? false : cfg.PROTEIN_INDIVIDUAL;
            UpgradeBus.instance.IndividualUpgrades.Add("Protein Powder", shareStatus);
            if(cfg.PROTEIN_ENABLED)
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
                if(prices.Length == 1 && prices[0] == -1) { prices = new int[0]; }

                string infoString = string.Format(infoJson["Protein Powder"],1,cfg.PROTEIN_PRICE,cfg.PROTEIN_UNLOCK_FORCE);
                for(int i = 0; i < prices.Length; i++)
                {
                    infoString += string.Format(infoJson["Protein Powder"], i + 2, prices[i],cfg.PROTEIN_UNLOCK_FORCE + 1 + (cfg.PROTEIN_INCREMENT *i));
                }
                CustomTerminalNode protNode = new CustomTerminalNode("Protein Powder", cfg.PROTEIN_PRICE, infoString, prot, prices, prices.Length);
                UpgradeBus.instance.terminalNodes.Add(protNode);
            }


            //lungs
            GameObject biggerLungs = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/BiggerLungs.prefab");
            biggerLungs.AddComponent<biggerLungScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(biggerLungs);
            shareStatus = cfg.SHARED_UPGRADES ? false : cfg.BIGGER_LUNGS_INDIVIDUAL;
            UpgradeBus.instance.IndividualUpgrades.Add("Bigger Lungs", shareStatus);
            if(cfg.BIGGER_LUNGS_ENABLED)
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
                if(prices.Length == 1 && prices[0] == -1) { prices = new int[0]; }
                string infoString = string.Format(infoJson["Bigger Lungs"],1,cfg.BIGGER_LUNGS_PRICE,cfg.SPRINT_TIME_INCREASE-11f);
                for(int i = 0; i < prices.Length; i++)
                {
                    infoString += string.Format(infoJson["Bigger Lungs"], i + 2, prices[i], (cfg.SPRINT_TIME_INCREASE + ((i+1) * cfg.SPRINT_TIME_INCREMENT))-11f);
                }

                CustomTerminalNode lungNode = new CustomTerminalNode("Bigger Lungs", cfg.BIGGER_LUNGS_PRICE, infoString, biggerLungs, prices, prices.Length);
                UpgradeBus.instance.terminalNodes.Add(lungNode);
            }

            //running shoes
            GameObject runningShoes = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/runningShoes.prefab");
            runningShoes.AddComponent<runningShoeScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(runningShoes);
            shareStatus = cfg.SHARED_UPGRADES ? false : cfg.RUNNING_SHOES_INDIVIDUAL;
            UpgradeBus.instance.IndividualUpgrades.Add("Running Shoes", shareStatus);
            if(cfg.RUNNING_SHOES_ENABLED)
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
                if(prices.Length == 1 && prices[0] == -1) { prices = new int[0]; }
                string infoString = string.Format(infoJson["Running Shoes"],1,cfg.RUNNING_SHOES_PRICE,cfg.MOVEMENT_SPEED - 4.6f);
                for(int i = 0; i < prices.Length; i++)
                {
                    infoString += string.Format(infoJson["Running Shoes"], i + 2, prices[i], (cfg.MOVEMENT_SPEED + ((i+1) * cfg.MOVEMENT_INCREMENT)) - 4.6f);
                }
                CustomTerminalNode node = new CustomTerminalNode("Running Shoes", cfg.RUNNING_SHOES_PRICE, infoString, runningShoes, prices, prices.Length);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            //strong legs
            GameObject strongLegs = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/strongLegs.prefab");
            strongLegs.AddComponent<strongLegsScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(strongLegs);
            shareStatus = cfg.SHARED_UPGRADES ? false : cfg.STRONG_LEGS_INDIVIDUAL;
            UpgradeBus.instance.IndividualUpgrades.Add("Strong Legs", shareStatus);
            if(cfg.STRONG_LEGS_ENABLED)
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
                if(prices.Length == 1 && prices[0] == -1) { prices = new int[0]; }
                string infoString = string.Format(infoJson["Strong Legs"],1,cfg.STRONG_LEGS_PRICE,cfg.JUMP_FORCE-13f);
                for(int i = 0; i < prices.Length; i++)
                {
                    infoString += string.Format(infoJson["Strong Legs"], i + 2, prices[i], (cfg.JUMP_FORCE + ((i+1) * cfg.JUMP_FORCE_INCREMENT))-13f);
                }
                CustomTerminalNode node = new CustomTerminalNode("Strong Legs", cfg.STRONG_LEGS_PRICE, infoString, strongLegs, prices, prices.Length);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            //destructive codes
            GameObject destructiveCodes = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/destructiveCodes.prefab");
            destructiveCodes.AddComponent<trapDestroyerScript>();
            string desc = "";
            if(cfg.DESTROY_TRAP)
            {
                if(cfg.EXPLODE_TRAP)
                {
                    desc = "Broadcasted codes now explode map hazards.";
                }
                else
                {
                    desc = "Broadcasted codes now destroy map hazards.";
                }
            }
            else { desc = $"Broadcasted codes now disable map hazards for {cfg.DISARM_TIME} seconds."; }
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(destructiveCodes);
            shareStatus = cfg.SHARED_UPGRADES ? false : cfg.MALWARE_BROADCASTER_INDIVIDUAL;
            UpgradeBus.instance.IndividualUpgrades.Add("Malware Broadcaster", shareStatus);
            if(cfg.MALWARE_BROADCASTER_ENABLED)
            { 
                CustomTerminalNode node = new CustomTerminalNode("Malware Broadcaster", cfg.MALWARE_BROADCASTER_PRICE, desc, destructiveCodes);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            //light footed
            GameObject lightFooted = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/lightFooted.prefab");
            lightFooted.AddComponent<lightFootedScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(lightFooted);
            shareStatus = cfg.SHARED_UPGRADES ? false : cfg.LIGHT_FOOTED_INDIVIDUAL;
            UpgradeBus.instance.IndividualUpgrades.Add("Light Footed", shareStatus);
            if(cfg.LIGHT_FOOTED_ENABLED)
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
                if(prices.Length == 1 && prices[0] == -1) { prices = new int[0]; }
                string infoString = string.Format(infoJson["Light Footed"],1,cfg.LIGHT_FOOTED_PRICE,cfg.NOISE_REDUCTION);
                for(int i = 0; i < prices.Length; i++)
                {
                    infoString += string.Format(infoJson["Light Footed"], i + 2, prices[i], cfg.NOISE_REDUCTION + ((i+1) * cfg.NOISE_REDUCTION_INCREMENT));
                }
                CustomTerminalNode node = new CustomTerminalNode("Light Footed", cfg.LIGHT_FOOTED_PRICE, infoString, lightFooted, prices, prices.Length);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            //night vision
            GameObject nightVision = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/nightVision.prefab");
            nightVision.AddComponent<nightVisionScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(nightVision);
            shareStatus = cfg.SHARED_UPGRADES ? false : cfg.NIGHT_VISION_INDIVIDUAL;
            UpgradeBus.instance.IndividualUpgrades.Add("NV Headset Batteries", shareStatus);
            if(cfg.NIGHT_VISION_ENABLED) 
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
                if(prices.Length == 1 && prices[0] == -1) { prices = new int[0]; }
                string infoString = "";
                for(int i = 0; i < prices.Length; i++)
                {
                    float regenAdjustment = Mathf.Pow(1 + cfg.NIGHT_VIS_REGEN_INCREASE_PERCENT / 100f, i) * 0.01f;
                    float drainAdjustment = Mathf.Pow(1 - cfg.NIGHT_VIS_DRAIN_DECREASE_PERCENT / 100f, i) * 0.01f;

                    infoString += string.Format(infoJson["NV Headset Batteries"], i + 1, prices[i], cfg.NIGHT_VIS_DRAIN_SPEED - drainAdjustment, cfg.NIGHT_VIS_REGEN_SPEED + regenAdjustment);
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
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            //terminal flashbang
            GameObject flash = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/terminalFlash.prefab");
            AudioClip flashSFX = UpgradeAssets.LoadAsset<AudioClip>("Assets/ShipUpgrades/flashbangsfx.ogg");
            UpgradeBus.instance.flashNoise = flashSFX;
            flash.AddComponent<terminalFlashScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(flash);
            shareStatus = cfg.SHARED_UPGRADES ? false : cfg.DISCOMBOBULATOR_INDIVIDUAL;
            UpgradeBus.instance.IndividualUpgrades.Add("Discombobulator", shareStatus);
            if(cfg.DISCOMBOBULATOR_ENABLED)
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
                if(prices.Length == 1 && prices[0] == -1) { prices = new int[0]; }
                string infoString = string.Format(infoJson["Discombobulator"],1,cfg.DISCOMBOBULATOR_PRICE,cfg.DISCOMBOBULATOR_RADIUS);
                for(int i = 0; i < prices.Length; i++)
                {
                    infoString += string.Format(infoJson["Discombobulator"], i + 2, prices[i], cfg.DISCOMBOBULATOR_STUN_DURATION + ((i+1) * cfg.DISCOMBOBULATOR_INCREMENT));
                }
                CustomTerminalNode node = new CustomTerminalNode("Discombobulator", cfg.DISCOMBOBULATOR_PRICE, infoString, flash, prices, prices.Length);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            //stronger scanner
            GameObject strongScan = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/strongScanner.prefab");
            strongScan.AddComponent<strongerScannerScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(strongScan);
            shareStatus = cfg.SHARED_UPGRADES ? false : cfg.BETTER_SCANNER_INDIVIDUAL;
            UpgradeBus.instance.IndividualUpgrades.Add("Better Scanner", shareStatus);
            if(cfg.BETTER_SCANNER_ENABLED)
            {
                string LOS = cfg.REQUIRE_LINE_OF_SIGHT ? "Does not remove" : "Removes";
                string info = $"Increase distance nodes can be scanned by {cfg.NODE_DISTANCE_INCREASE} units.  \nIncrease distance Ship and Entrance can be scanned by {cfg.SHIP_AND_ENTRANCE_DISTANCE_INCREASE} units.\n{LOS} LOS requirement\n";
                CustomTerminalNode node = new CustomTerminalNode("Better Scanner", cfg.BETTER_SCANNER_PRICE, info, strongScan);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            // walkie
            GameObject walkie = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/walkieUpgrade.prefab");
            walkie.AddComponent<walkieScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(walkie);
            UpgradeBus.instance.IndividualUpgrades.Add("Walkie GPS", true);
            if(cfg.WALKIE_ENABLED)
            {
                CustomTerminalNode node = new CustomTerminalNode("Walkie GPS", cfg.WALKIE_PRICE, "Displays your location and time when holding a walkie talkie.\nEspecially useful for fog.", walkie);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            // back muscles
            GameObject exoskel = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/exoskeleton.prefab");
            exoskel.AddComponent<exoskeletonScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(exoskel);
            shareStatus = cfg.SHARED_UPGRADES ? false : cfg.BACK_MUSCLES_INDIVIDUAL;
            UpgradeBus.instance.IndividualUpgrades.Add("Back Muscles", shareStatus);
            if(cfg.BACK_MUSCLES_ENABLED)
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
                if(prices.Length == 1 && prices[0] == -1) { prices = new int[0]; }
                string infoString = string.Format(infoJson["Back Muscles"],1,cfg.BACK_MUSCLES_PRICE,cfg.CARRY_WEIGHT_REDUCTION);
                for(int i = 0; i < prices.Length; i++)
                {
                    infoString += string.Format(infoJson["Back Muscles"], i + 2, prices[i], cfg.CARRY_WEIGHT_REDUCTION + ((i+1) * cfg.CARRY_WEIGHT_INCREMENT));
                }
                CustomTerminalNode node = new CustomTerminalNode("Back Muscles", cfg.BACK_MUSCLES_PRICE, infoString, exoskel, prices, prices.Length);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            // interns
            GameObject intern = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/Pager.prefab");
            intern.AddComponent<defibScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(intern);
            shareStatus = cfg.SHARED_UPGRADES ? false : cfg.INTERN_INDIVIDUAL;
            UpgradeBus.instance.IndividualUpgrades.Add("Interns", shareStatus);
            if (cfg.INTERN_ENABLED)
            {
                CustomTerminalNode node = new CustomTerminalNode("Interns", cfg.INTERN_PRICE, string.Format(infoJson["Interns"], cfg.INTERN_PRICE), intern);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            //lockSmith
            GameObject lockSmith = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/LockSmith.prefab");
            lockSmith.AddComponent<lockSmithScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(lockSmith);
            shareStatus = cfg.SHARED_UPGRADES ? false : cfg.LOCKSMITH_INDIVIDUAL;
            UpgradeBus.instance.IndividualUpgrades.Add("Locksmith", shareStatus);
            if (cfg.LOCKSMITH_ENABLED)
            {
                CustomTerminalNode node = new CustomTerminalNode("Locksmith", cfg.LOCKSMITH_PRICE,"Allows you to pick door locks by completing a minigame.", lockSmith);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            UpgradeBus.instance.GenerateSales();

            harmony.PatchAll();

            mls.LogInfo("LGU has been patched");
        }

        // I don't even know if modsync is still used but here we are.
        public void sendModInfo()
        {
            foreach (var plugin in Chainloader.PluginInfos)
            {
                if (plugin.Value.Metadata.GUID.Contains("ModSync"))
                {
                    try
                    {
                        List<string> list = new List<string>
                        {
                            "malco",
                            "LateGameUpgrades"
                        };
                        plugin.Value.Instance.BroadcastMessage("getModInfo", list, UnityEngine.SendMessageOptions.DontRequireReceiver);
                    }
                    catch (Exception e)
                    {
                        // ignore mod if error, removing dependency
                        mls.LogInfo($"Failed to send info to ModSync, go yell at Minx");
                    }
                    break;
                }

            }
        }

        private string GetJsonContent()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MoreShipUpgrades.Misc.InfoStrings.json";

            using (StreamReader reader = new StreamReader(assembly.GetManifestResourceStream(resourceName)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
