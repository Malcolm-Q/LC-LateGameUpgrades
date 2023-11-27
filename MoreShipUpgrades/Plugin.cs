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

namespace MoreShipUpgrades
{
    [BepInEx.BepInPlugin(GUID,NAME,VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string GUID = "com.malco.lethalcompany.moreshipupgrades";
        private const string NAME = "More Ship Upgrades";
        private const string VERSION = "1.1.2";

        private readonly Harmony harmony = new Harmony(GUID);
        public static Plugin instance;
        public static ManualLogSource mls;
        public static List<Item> upgradeItems = new List<Item>();


        void Awake()
        {
            // TODO: Make item loading and registration more modular and concise. 
            mls = BepInEx.Logging.Logger.CreateLogSource(GUID);
            instance = this;

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
            tpBut.creditsWorth = 300;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(tpBut.spawnPrefab);
            TerminalNode PortNode = new TerminalNode();
            PortNode.displayText = "A button that when pressed teleports you and your loot back to the ship. Must have Ship Teleporter unlocked!!!\n\nHas a 90% chance to self destruct on use.";
            Items.RegisterShopItem(tpBut,null,null,PortNode, tpBut.creditsWorth);
            upgradeItems.Add(tpBut);

            //TP button advanced
            Item tpButAdvanced = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/TpButtonAdv.asset");
            tpButAdvanced.creditsWorth = 1750;
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

            //beekeeper
            Item beekeeper = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/beekeeper.asset");
            beekeeper.spawnPrefab.AddComponent<beekeeperScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(beekeeper.spawnPrefab);
            TerminalNode beeNode = new TerminalNode();
            beeNode.displayText = "Upgrades your suit to a space beekeeping suit. Circuit bees can no longer harass you!";
            beeNode.clearPreviousText = true;
            Items.RegisterShopItem(beekeeper,null,null,beeNode, beekeeper.creditsWorth);
            upgradeItems.Add(beekeeper);

            //lungs
            Item biggerLungs = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/BiggerLungs.asset");
            biggerLungs.spawnPrefab.AddComponent<biggerLungScript>();
            TerminalNode lungNode = new TerminalNode();
            lungNode.displayText = "Upgrades your suit to a space beekeeping suit. Circuit bees can no longer harass you!";
            lungNode.clearPreviousText = true;
            Items.RegisterShopItem(biggerLungs,null,null,lungNode, biggerLungs.creditsWorth);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(biggerLungs.spawnPrefab);
            upgradeItems.Add(biggerLungs);

            //running shoes
            Item runningShoes = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/runningShoes.asset");
            runningShoes.spawnPrefab.AddComponent<runningShoeScript>();
            TerminalNode runNode = new TerminalNode();
            runNode.displayText = "Move faster!";
            runNode.clearPreviousText = true;
            Items.RegisterShopItem(runningShoes,null,null,runNode, runningShoes.creditsWorth);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(runningShoes.spawnPrefab);
            upgradeItems.Add(runningShoes);

            //strong legs
            Item strongLegs = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/strongLegs.asset");
            strongLegs.spawnPrefab.AddComponent<strongLegsScript>();
            TerminalNode legNode = new TerminalNode();
            legNode.displayText = "Jump Higher. Never fail those pesky jumps and be forced to listen in silence as your friends mock you.";
            legNode.clearPreviousText = true;
            Items.RegisterShopItem(strongLegs,null,null,legNode, strongLegs.creditsWorth);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(strongLegs.spawnPrefab);
            upgradeItems.Add(strongLegs);

            //destructive codes
            Item destructiveCodes = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/destructiveCodes.asset");
            destructiveCodes.itemName = "Malware Broadcaster";
            destructiveCodes.spawnPrefab.AddComponent<trapDestroyerScript>();
            TerminalNode destNode = new TerminalNode();
            destNode.displayText = "When broadcasting a code turrets and mines aren't disabled, they're destroyed.";
            destNode.clearPreviousText = true;
            Items.RegisterShopItem(destructiveCodes,null,null,destNode, destructiveCodes.creditsWorth);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(destructiveCodes.spawnPrefab);
            upgradeItems.Add(destructiveCodes);

            //light footed
            Item lightFooted = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/LightFooted.asset");
            lightFooted.spawnPrefab.AddComponent<lightFootedScript>();
            TerminalNode lightNode = new TerminalNode();
            lightNode.displayText = "Enemies must be closer to you to hear your footsteps.\nApplies to both sprinting and walking.";
            lightNode.clearPreviousText= true;
            Items.RegisterShopItem(lightFooted,null,null,lightNode, lightFooted.creditsWorth);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(lightFooted.spawnPrefab);
            upgradeItems.Add(lightFooted);

            //night vision
            Item nightVision = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/NightVision.asset");
            nightVision.spawnPrefab.AddComponent<nightVisionScript>();
            TerminalNode nightNode = new TerminalNode();
            nightNode.displayText = "Press Left-Alt to toggle night vision!";
            nightNode.clearPreviousText = true;
            Items.RegisterShopItem(nightVision,null,null,nightNode, nightVision.creditsWorth);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(nightVision.spawnPrefab);
            upgradeItems.Add(nightVision);

            //terminal flashbang
            Item flash = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/terminalFlash.asset");
            flash.itemName = "Discombobulator";
            AudioClip flashSFX = UpgradeAssets.LoadAsset<AudioClip>("Assets/ShipUpgrades/flashbangsfx.ogg");
            UpgradeBus.instance.flashNoise = flashSFX;
            flash.spawnPrefab.AddComponent<terminalFlashScript>();
            TerminalNode flashNode = new TerminalNode();
            flashNode.displayText = "Stun any enemies in a wide radius around your ship! Type cooldown into the terminal to view status and initattack to execute.";
            flashNode.clearPreviousText= true;
            Items.RegisterShopItem(flash,null,null,flashNode, flash.creditsWorth);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(flash.spawnPrefab);
            upgradeItems.Add(flash);

            //stronger scanner
            Item strongScan = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/strongScanner.asset");
            strongScan.itemName = "Better Scanner";
            strongScan.spawnPrefab.AddComponent<strongerScannerScript>();
            TerminalNode betScanNode = new TerminalNode();
            betScanNode.displayText = "Allows your scanner to find the entrance and ship from further away, increases the distance you can ping objects and enemies, and allows you to ping through walls.";
            betScanNode .clearPreviousText= true;
            Items.RegisterShopItem(strongScan,null,null,betScanNode, strongScan.creditsWorth);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(strongScan.spawnPrefab);
            upgradeItems.Add(strongScan);

            // back muscles
            Item exoskel = UpgradeAssets.LoadAsset<Item>("Assets/ShipUpgrades/exoskeleton.asset");
            exoskel.spawnPrefab.AddComponent<exoskeletonScript>();
            exoskel.itemName = "Back Muscles";
            TerminalNode exoNode = new TerminalNode();
            exoNode.displayText = "Lift things with ease and no longer worry about your carry weight.";
            exoNode.clearPreviousText= true;
            Items.RegisterShopItem(exoskel,null,null,exoNode, exoskel.creditsWorth);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(exoskel.spawnPrefab);
            upgradeItems.Add(exoskel);

            harmony.PatchAll();

            mls.LogInfo("More Ship Upgrades has been patched");
        }
    }
}
