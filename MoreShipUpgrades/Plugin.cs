using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LethalLib;
using UnityEngine;
using LethalLib.Modules;
using MoreShipUpgrades.UpgradeComponents;
using MoreShipUpgrades.Managers;
using System.IO;
using System.Reflection;
using MoreShipUpgrades.Misc;
using Unity.Netcode;

namespace MoreShipUpgrades
{
    [BepInEx.BepInPlugin(Metadata.GUID,Metadata.NAME,Metadata.VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(Metadata.GUID);
        public static Plugin instance;
        public static ManualLogSource mls;
        public static List<Item> upgradeItems = new List<Item>();
        public static new PluginConfig cfg { get; private set; }


        void Awake()
        {
            // TODO: Make item loading and registration more modular and concise.

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

            string assetDir = System.IO.Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "shipupgrades");
            AssetBundle UpgradeAssets = AssetBundle.LoadFromFile(assetDir);

            GameObject busGO = new GameObject("UpgradeBus");
            busGO.AddComponent<UpgradeBus>();

            GameObject modStore = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/LGUStore.prefab");
            modStore.AddComponent<LGUStore>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(modStore);
            UpgradeBus.instance.modStorePrefab = modStore;

            //TP button sfx 
            AudioClip itemBreak = UpgradeAssets.LoadAsset<AudioClip>("Assets/ShipUpgrades/break.mp3");
            AudioClip error = UpgradeAssets.LoadAsset<AudioClip>("Assets/ShipUpgrades/error.mp3");
            AudioClip buttonPressed = UpgradeAssets.LoadAsset<AudioClip>("Assets/ShipUpgrades/ButtonPress2.ogg");

            //tp button
            if(cfg.WEAK_TELE_ENABLED)
            {
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
                TerminalNode PortNode = new TerminalNode();
                PortNode.displayText = "A button that when pressed teleports you and your loot back to the ship. Must have Ship Teleporter unlocked!!!\n\nHas a 90% chance to self destruct on use.";
                Items.RegisterShopItem(tpBut,null,null,PortNode, tpBut.creditsWorth);
                upgradeItems.Add(tpBut);
            }

            //TP button advanced
            if(cfg.ADVANCED_TELE_ENABLED)
            {
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
                TerminalNode advNode = new TerminalNode();
                advNode.displayText = "A button that when pressed teleports you and your loot back to the ship. Must have Ship Teleporter unlocked!!!";
                Items.RegisterShopItem(tpButAdvanced,null,null,advNode, tpButAdvanced.creditsWorth);
                upgradeItems.Add(tpButAdvanced);
            }    

            //beekeeper
            if(cfg.BEEKEEPER_ENABLED)
            {
                Item beekeeper = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/beekeeper.asset");
                beekeeper.spawnPrefab.AddComponent<beekeeperScript>();
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(beekeeper.spawnPrefab);
                CustomTerminalNode beeNode = new CustomTerminalNode("Beekeeper", cfg.BEEKEEPER_PRICE, $"Circuit bees do %{Mathf.Round(100 * (cfg.BEEKEEPER_DAMAGE_MULTIPLIER - (UpgradeBus.instance.beeLevel * cfg.BEEKEEPER_DAMAGE_MULTIPLIER_INCREMENT)))} of their base damage.", beekeeper.spawnPrefab, 3);
                UpgradeBus.instance.terminalNodes.Add(beeNode);
                upgradeItems.Add(beekeeper);

            }

            //lungs
            if(cfg.BIGGER_LUNGS_ENABLED)
            {
                Item biggerLungs = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/BiggerLungs.asset");
                biggerLungs.spawnPrefab.AddComponent<biggerLungScript>();
                CustomTerminalNode lungNode = new CustomTerminalNode("Bigger Lungs", cfg.BIGGER_LUNGS_PRICE, $"Stamina Time is {UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE - 11 + UpgradeBus.instance.cfg.SPRINT_TIME_INCREMENT} units longer", biggerLungs.spawnPrefab, 3);
                UpgradeBus.instance.terminalNodes.Add(lungNode);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(biggerLungs.spawnPrefab);
                upgradeItems.Add(biggerLungs);
            }

            //running shoes
            if(cfg.RUNNING_SHOES_ENABLED)
            {
                Item runningShoes = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/runningShoes.asset");
                runningShoes.spawnPrefab.AddComponent<runningShoeScript>();
                CustomTerminalNode node = new CustomTerminalNode("Running Shoes", cfg.RUNNING_SHOES_PRICE, $"You can run {UpgradeBus.instance.cfg.MOVEMENT_SPEED - 4.6f + UpgradeBus.instance.cfg.MOVEMENT_INCREMENT} units faster", runningShoes.spawnPrefab, 3);
                UpgradeBus.instance.terminalNodes.Add(node);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(runningShoes.spawnPrefab);
                upgradeItems.Add(runningShoes);
            }

            //strong legs
            if(cfg.STRONG_LEGS_ENABLED)
            {
                Item strongLegs = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/strongLegs.asset");
                strongLegs.spawnPrefab.AddComponent<strongLegsScript>();
                CustomTerminalNode node = new CustomTerminalNode("Strong Legs", cfg.STRONG_LEGS_PRICE, $"Jump {cfg.JUMP_FORCE - 13} units higher.", strongLegs.spawnPrefab, 3);
                UpgradeBus.instance.terminalNodes.Add(node);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(strongLegs.spawnPrefab);
                upgradeItems.Add(strongLegs);
            }

            //destructive codes
            if(cfg.MALWARE_BROADCASTER_ENABLED)
            {
                Item destructiveCodes = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/destructiveCodes.asset");
                destructiveCodes.spawnPrefab.AddComponent<trapDestroyerScript>();
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
                CustomTerminalNode node = new CustomTerminalNode("Malware Broadcaster", cfg.MALWARE_BROADCASTER_PRICE, desc, destructiveCodes.spawnPrefab);
                UpgradeBus.instance.terminalNodes.Add(node);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(destructiveCodes.spawnPrefab);
                upgradeItems.Add(destructiveCodes);
            }

            //light footed
            if(cfg.LIGHT_FOOTED_ENABLED)
            {
                Item lightFooted = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/LightFooted.asset");
                lightFooted.spawnPrefab.AddComponent<lightFootedScript>();
                CustomTerminalNode node = new CustomTerminalNode("Light Footed", cfg.LIGHT_FOOTED_PRICE, $"Audible Noise Distance is reduced by {UpgradeBus.instance.cfg.NOISE_REDUCTION + (UpgradeBus.instance.cfg.NOISE_REDUCTION_INCREMENT * UpgradeBus.instance.lightLevel)} units. \nApplies to both sprinting and walking.", lightFooted.spawnPrefab, 3);
                UpgradeBus.instance.terminalNodes.Add(node);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(lightFooted.spawnPrefab);
                upgradeItems.Add(lightFooted);
            }

            //night vision
            if(cfg.NIGHT_VISION_ENABLED) 
            { 
                Item nightVision = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/NightVision.asset");
                nightVision.spawnPrefab.AddComponent<nightVisionScript>();
                CustomTerminalNode node = new CustomTerminalNode("Night Vision", cfg.NIGHT_VISION_PRICE, $"Allows you to see in the dark. Press Left-Alt to turn on.  \nDrain speed is {cfg.NIGHT_VIS_DRAIN_SPEED}  \nRegen speed is {cfg.NIGHT_VIS_REGEN_SPEED}", nightVision.spawnPrefab);
                UpgradeBus.instance.terminalNodes.Add(node);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(nightVision.spawnPrefab);
                upgradeItems.Add(nightVision);
            }

            //terminal flashbang
            if(cfg.DISCOMBOBULATOR_ENABLED)
            {
                Item flash = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/terminalFlash.asset");
                AudioClip flashSFX = UpgradeAssets.LoadAsset<AudioClip>("Assets/ShipUpgrades/flashbangsfx.ogg");
                UpgradeBus.instance.flashNoise = flashSFX;
                flash.spawnPrefab.AddComponent<terminalFlashScript>();
                CustomTerminalNode node = new CustomTerminalNode("Discombobulator", cfg.DISCOMBOBULATOR_PRICE, $"Stun enemies around your ship in a {cfg.DISCOMBOBULATOR_RADIUS} unit radius.", flash.spawnPrefab, 3);
                UpgradeBus.instance.terminalNodes.Add(node);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(flash.spawnPrefab);
                upgradeItems.Add(flash);
            }

            //stronger scanner
            if(cfg.BETTER_SCANNER_ENABLED)
            {
                Item strongScan = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/strongScanner.asset");
                strongScan.spawnPrefab.AddComponent<strongerScannerScript>();
                string LOS = cfg.REQUIRE_LINE_OF_SIGHT ? "Does not remove" : "Removes";
                string info = $"Increase distance nodes can be scanned by {cfg.NODE_DISTANCE_INCREASE} units.  \nIncrease distance Ship and Entrance can be scanned by {cfg.SHIP_AND_ENTRANCE_DISTANCE_INCREASE} units.  \n";
                info += $"{LOS} LOS requirement";
                CustomTerminalNode node = new CustomTerminalNode("Better Scanner", cfg.BETTER_SCANNER_PRICE, info, strongScan.spawnPrefab);
                UpgradeBus.instance.terminalNodes.Add(node);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(strongScan.spawnPrefab);
                upgradeItems.Add(strongScan);
            }

            // back muscles
            if(cfg.BACK_MUSCLES_ENABLED)
            {
                Item exoskel = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/exoskeleton.asset");
                exoskel.spawnPrefab.AddComponent<exoskeletonScript>();
                CustomTerminalNode node = new CustomTerminalNode("Back Muscles", cfg.BACK_MUSCLES_PRICE, $"Carry weight becomes %{Mathf.Round((cfg.CARRY_WEIGHT_REDUCTION) * 100f)} of original", exoskel.spawnPrefab, 3);
                UpgradeBus.instance.terminalNodes.Add(node);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(exoskel.spawnPrefab);
                upgradeItems.Add(exoskel);
            }
            Debug.Log(UpgradeBus.instance.terminalNodes);
            harmony.PatchAll();

            mls.LogInfo("More Ship Upgrades has been patched");
        }
    }
}
