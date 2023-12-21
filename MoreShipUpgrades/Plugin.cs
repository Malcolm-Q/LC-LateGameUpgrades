using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
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
    [BepInDependency("evaisa.lethallib","0.6.0")]
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

            Dictionary<string,string> infoJson = JsonConvert.DeserializeObject<Dictionary<string,string>>(UpgradeAssets.LoadAsset<TextAsset>("Assets/ShipUpgrades/InfoStrings.json").text);

            GameObject busGO = new GameObject("UpgradeBus");
            busGO.AddComponent<UpgradeBus>();

            UpgradeBus.instance.version = Metadata.VERSION;
            UpgradeBus.instance.UpgradeAssets = UpgradeAssets;

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

            // samples
            Item fleaSample = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/SnareFleaSample.asset");
            Item spiderSample = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/BunkerSpiderSample.asset");
            Item hoardSample = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/HoardingBugSample.asset");
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(fleaSample.spawnPrefab);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(spiderSample.spawnPrefab);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(hoardSample.spawnPrefab);
            UpgradeBus.instance.samplePrefabs.Add("snare flea", fleaSample.spawnPrefab);
            UpgradeBus.instance.samplePrefabs.Add("bunker spider", spiderSample.spawnPrefab);
            UpgradeBus.instance.samplePrefabs.Add("hoarding bug", hoardSample.spawnPrefab);

            //tp button
            Item tpBut = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/TpButton.asset");
            tpBut.itemName = "Portable Tele";
            tpBut.itemId = 492012;
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
            tpButAdvanced.itemId = 492013;
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
            nightVisionItem.itemId = 492014;
            NightVisionItemScript visScript = nightVisionItem.spawnPrefab.AddComponent<NightVisionItemScript>(); 
            visScript.itemProperties = nightVisionItem;
            visScript.grabbable = true;
            visScript.useCooldown = 2f;
            visScript.grabbableToEnemies=true;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(nightVisionItem.spawnPrefab);
            if (cfg.NIGHT_VISION_ENABLED)
            {
                TerminalNode nightNode = new TerminalNode();
                string grantStatus = cfg.NIGHT_VISION_INDIVIDUAL || UpgradeBus.instance.cfg.SHARED_UPGRADES ? "one" : "all";
                string loseOnDeath = cfg.LOSE_NIGHT_VIS_ON_DEATH ? "be" : "not be";
                nightNode.displayText = string.Format(infoJson["Night Vision"], grantStatus, loseOnDeath);
                Items.RegisterShopItem(nightVisionItem,null,null,nightNode, nightVisionItem.creditsWorth);
            }

            //coil head Item
            Item Peeper = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/coilHead.asset");
            Peeper.creditsWorth = cfg.PEEPER_PRICE;
            Peeper.twoHanded = false;
            Peeper.itemId = 492015;
            Peeper.twoHandedAnimation = false;
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

             //hunter
            GameObject hunter = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/Hunter.prefab");
            hunter.AddComponent<hunterScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(hunter);

            //protein powder
            GameObject prot = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/ProteinPowder.prefab");
            prot.AddComponent<proteinPowderScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(prot);
            
            //lungs
            GameObject biggerLungs = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/BiggerLungs.prefab");
            biggerLungs.AddComponent<biggerLungScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(biggerLungs);

            //running shoes
            GameObject runningShoes = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/runningShoes.prefab");
            runningShoes.AddComponent<runningShoeScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(runningShoes);

            //strong legs
            GameObject strongLegs = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/strongLegs.prefab");
            strongLegs.AddComponent<strongLegsScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(strongLegs);

            //destructive codes
            GameObject destructiveCodes = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/destructiveCodes.prefab");
            destructiveCodes.AddComponent<trapDestroyerScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(destructiveCodes);

            //light footed
            GameObject lightFooted = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/lightFooted.prefab");
            lightFooted.AddComponent<lightFootedScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(lightFooted);

            //night vision
            GameObject nightVision = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/nightVision.prefab");
            nightVision.AddComponent<nightVisionScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(nightVision);

            //terminal flashbang
            GameObject flash = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/terminalFlash.prefab");
            AudioClip flashSFX = UpgradeAssets.LoadAsset<AudioClip>("Assets/ShipUpgrades/flashbangsfx.ogg");
            UpgradeBus.instance.flashNoise = flashSFX;
            flash.AddComponent<terminalFlashScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(flash);

            //stronger scanner
            GameObject strongScan = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/strongScanner.prefab");
            strongScan.AddComponent<strongerScannerScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(strongScan);

            // walkie
            GameObject walkie = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/walkieUpgrade.prefab");
            walkie.AddComponent<walkieScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(walkie);

            // back muscles
            GameObject exoskel = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/exoskeleton.prefab");
            exoskel.AddComponent<exoskeletonScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(exoskel);

            // interns
            GameObject intern = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/Intern.prefab");
            intern.AddComponent<defibScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(intern);

            // pager
            GameObject pager = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/Pager.prefab");
            pager.AddComponent<pagerScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(pager);

            // Lightning Rod
            GameObject lightningRod = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/LightningRod.prefab");
            lightningRod.AddComponent<lightningRodScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(lightningRod);

            //lockSmith
            GameObject lockSmith = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/LockSmith.prefab");
            lockSmith.AddComponent<lockSmithScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(lockSmith);

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
    }
}
