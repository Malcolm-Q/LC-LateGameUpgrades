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
                beekeeper.creditsWorth = cfg.BEEKEEPER_PRICE;
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(beekeeper.spawnPrefab);
                TerminalNode beeNode = new TerminalNode();
                beeNode.displayText = "Upgrades your suit to a space beekeeping suit. Circuit bees can no longer harass you!";
                beeNode.clearPreviousText = true;
                Items.RegisterShopItem(beekeeper,null,null,beeNode, beekeeper.creditsWorth);
                upgradeItems.Add(beekeeper);
            }

            //lungs
            if(cfg.BIGGER_LUNGS_ENABLED)
            {
                Item biggerLungs = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/BiggerLungs.asset");
                biggerLungs.spawnPrefab.AddComponent<biggerLungScript>();
                biggerLungs.creditsWorth = cfg.BIGGER_LUNGS_PRICE;
                TerminalNode lungNode = new TerminalNode();
                lungNode.displayText = "Upgrades your suit to a space beekeeping suit. Circuit bees can no longer harass you!";
                lungNode.clearPreviousText = true;
                Items.RegisterShopItem(biggerLungs,null,null,lungNode, biggerLungs.creditsWorth);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(biggerLungs.spawnPrefab);
                upgradeItems.Add(biggerLungs);
            }

            //running shoes
            if(cfg.RUNNING_SHOES_ENABLED)
            {
                Item runningShoes = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/runningShoes.asset");
                runningShoes.spawnPrefab.AddComponent<runningShoeScript>();
                runningShoes.creditsWorth = cfg.RUNNING_SHOES_PRICE;
                TerminalNode runNode = new TerminalNode();
                runNode.displayText = "Move faster!";
                runNode.clearPreviousText = true;
                Items.RegisterShopItem(runningShoes,null,null,runNode, runningShoes.creditsWorth);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(runningShoes.spawnPrefab);
                upgradeItems.Add(runningShoes);
            }

            //strong legs
            if(cfg.STRONG_LEGS_ENABLED)
            {
                Item strongLegs = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/strongLegs.asset");
                strongLegs.spawnPrefab.AddComponent<strongLegsScript>();
                strongLegs.creditsWorth = cfg.STRONG_LEGS_PRICE;
                TerminalNode legNode = new TerminalNode();
                legNode.displayText = "Jump Higher. Never fail those pesky jumps and be forced to listen in silence as your friends mock you.";
                legNode.clearPreviousText = true;
                Items.RegisterShopItem(strongLegs,null,null,legNode, strongLegs.creditsWorth);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(strongLegs.spawnPrefab);
                upgradeItems.Add(strongLegs);
            }

            //destructive codes
            if(cfg.MALWARE_BROADCASTER_ENABLED)
            {
                Item destructiveCodes = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/destructiveCodes.asset");
                destructiveCodes.itemName = "Malware Broadcaster";
                destructiveCodes.spawnPrefab.AddComponent<trapDestroyerScript>();
                destructiveCodes.creditsWorth = cfg.MALWARE_BROADCASTER_PRICE;
                TerminalNode destNode = new TerminalNode();
                destNode.displayText = "When broadcasting a code turrets and mines aren't disabled, they're destroyed.";
                destNode.clearPreviousText = true;
                Items.RegisterShopItem(destructiveCodes,null,null,destNode, destructiveCodes.creditsWorth);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(destructiveCodes.spawnPrefab);
                upgradeItems.Add(destructiveCodes);
            }

            //light footed
            if(cfg.LIGHT_FOOTED_ENABLED)
            {
                Item lightFooted = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/LightFooted.asset");
                lightFooted.spawnPrefab.AddComponent<lightFootedScript>();
                lightFooted.creditsWorth = cfg.LIGHT_FOOTED_PRICE;
                TerminalNode lightNode = new TerminalNode();
                lightNode.displayText = "Enemies must be closer to you to hear your footsteps.\nApplies to both sprinting and walking.";
                lightNode.clearPreviousText= true;
                Items.RegisterShopItem(lightFooted,null,null,lightNode, lightFooted.creditsWorth);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(lightFooted.spawnPrefab);
                upgradeItems.Add(lightFooted);
            }

            //night vision
            if(cfg.NIGHT_VISION_ENABLED) 
            { 
                Item nightVision = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/NightVision.asset");
                nightVision.spawnPrefab.AddComponent<nightVisionScript>();
                nightVision.creditsWorth = cfg.NIGHT_VISION_PRICE;
                TerminalNode nightNode = new TerminalNode();
                nightNode.displayText = "Press Left-Alt to toggle night vision!";
                nightNode.clearPreviousText = true;
                Items.RegisterShopItem(nightVision,null,null,nightNode, nightVision.creditsWorth);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(nightVision.spawnPrefab);
                upgradeItems.Add(nightVision);
            }

            //terminal flashbang
            if(cfg.DISCOMBOBULATOR_ENABLED)
            {
                Item flash = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/terminalFlash.asset");
                flash.itemName = "Discombobulator";
                flash.creditsWorth = cfg.DISCOMBOBULATOR_PRICE;
                AudioClip flashSFX = UpgradeAssets.LoadAsset<AudioClip>("Assets/ShipUpgrades/flashbangsfx.ogg");
                UpgradeBus.instance.flashNoise = flashSFX;
                flash.spawnPrefab.AddComponent<terminalFlashScript>();
                TerminalNode flashNode = new TerminalNode();
                flashNode.displayText = "Stun any enemies in a wide radius around your ship! Type cooldown into the terminal to view status and initattack to execute.";
                flashNode.clearPreviousText= true;
                Items.RegisterShopItem(flash,null,null,flashNode, flash.creditsWorth);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(flash.spawnPrefab);
                upgradeItems.Add(flash);
            }

            //stronger scanner
            if(cfg.BETTER_SCANNER_ENABLED)
            {
                Item strongScan = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/strongScanner.asset");
                strongScan.itemName = "Better Scanner";
                strongScan.spawnPrefab.AddComponent<strongerScannerScript>();
                strongScan.creditsWorth = cfg.BETTER_SCANNER_PRICE;
                TerminalNode betScanNode = new TerminalNode();
                betScanNode.displayText = "Allows your scanner to find the entrance and ship from further away, increases the distance you can ping objects and enemies, and allows you to ping through walls.";
                betScanNode .clearPreviousText= true;
                Items.RegisterShopItem(strongScan,null,null,betScanNode, strongScan.creditsWorth);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(strongScan.spawnPrefab);
                upgradeItems.Add(strongScan);
            }

            // back muscles
            if(cfg.BACK_MUSCLES_ENABLED)
            {
                Item exoskel = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/exoskeleton.asset");
                exoskel.spawnPrefab.AddComponent<exoskeletonScript>();
                exoskel.itemName = "Back Muscles";
                exoskel.creditsWorth = cfg.BACK_MUSCLES_PRICE;
                TerminalNode exoNode = new TerminalNode();
                exoNode.displayText = "Lift things with ease and no longer worry about your carry weight.";
                exoNode.clearPreviousText= true;
                Items.RegisterShopItem(exoskel,null,null,exoNode, exoskel.creditsWorth);
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(exoskel.spawnPrefab);
                upgradeItems.Add(exoskel);
            }
            harmony.PatchAll();

            mls.LogInfo("More Ship Upgrades has been patched");
        }
    }
}
