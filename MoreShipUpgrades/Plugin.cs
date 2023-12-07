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
using BepInEx.Bootstrap;

namespace MoreShipUpgrades
{
    [BepInEx.BepInPlugin(Metadata.GUID,Metadata.NAME,Metadata.VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(Metadata.GUID);
        public static Plugin instance;
        public static ManualLogSource mls;
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
                PortNode.displayText = "A button that when pressed teleports you and your loot back to the ship. Must have Ship Teleporter unlocked!!!\n\nHas a 90% chance to self destruct on use.";
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
                advNode.displayText = "A button that when pressed teleports you and your loot back to the ship. Must have Ship Teleporter unlocked!!!";
                Items.RegisterShopItem(tpButAdvanced,null,null,advNode, tpButAdvanced.creditsWorth);
            }

            //beekeeper
            GameObject beekeeper = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/beekeeper.prefab");
            beekeeper.AddComponent<beekeeperScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(beekeeper);
            if(cfg.BEEKEEPER_ENABLED)
            {
                CustomTerminalNode beeNode = new CustomTerminalNode("Beekeeper", cfg.BEEKEEPER_PRICE, $"Circuit bees do %{Mathf.Round(100 * cfg.BEEKEEPER_DAMAGE_MULTIPLIER)} of their base damage.", beekeeper, 3);
                UpgradeBus.instance.terminalNodes.Add(beeNode);

            }

            //lungs
            GameObject biggerLungs = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/BiggerLungs.prefab");
            biggerLungs.AddComponent<biggerLungScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(biggerLungs);
            if(cfg.BIGGER_LUNGS_ENABLED)
            {
                CustomTerminalNode lungNode = new CustomTerminalNode("Bigger Lungs", cfg.BIGGER_LUNGS_PRICE, $"Stamina Time is {(UpgradeBus.instance.cfg.SPRINT_TIME_INCREASE - 11)} units longer", biggerLungs, 3);
                UpgradeBus.instance.terminalNodes.Add(lungNode);
            }

            //running shoes
            GameObject runningShoes = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/runningShoes.prefab");
            runningShoes.AddComponent<runningShoeScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(runningShoes);
            if(cfg.RUNNING_SHOES_ENABLED)
            {
                CustomTerminalNode node = new CustomTerminalNode("Running Shoes", cfg.RUNNING_SHOES_PRICE, $"You can run {UpgradeBus.instance.cfg.MOVEMENT_SPEED - 4.6f} units faster", runningShoes, 3);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            //strong legs
            GameObject strongLegs = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/strongLegs.prefab");
            strongLegs.AddComponent<strongLegsScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(strongLegs);
            if(cfg.STRONG_LEGS_ENABLED)
            {
                CustomTerminalNode node = new CustomTerminalNode("Strong Legs", cfg.STRONG_LEGS_PRICE, $"Jump {cfg.JUMP_FORCE - 13} units higher.", strongLegs, 3);
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
            if(cfg.MALWARE_BROADCASTER_ENABLED)
            { 
                CustomTerminalNode node = new CustomTerminalNode("Malware Broadcaster", cfg.MALWARE_BROADCASTER_PRICE, desc, destructiveCodes);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            //light footed
            GameObject lightFooted = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/lightFooted.prefab");
            lightFooted.AddComponent<lightFootedScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(lightFooted);
            if(cfg.LIGHT_FOOTED_ENABLED)
            {
                CustomTerminalNode node = new CustomTerminalNode("Light Footed", cfg.LIGHT_FOOTED_PRICE, $"Audible Noise Distance is reduced by {UpgradeBus.instance.cfg.NOISE_REDUCTION} units. \nApplies to both sprinting and walking.", lightFooted, 3);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            //night vision
            GameObject nightVision = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/nightVision.prefab");
            nightVision.AddComponent<nightVisionScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(nightVision);
            if(cfg.NIGHT_VISION_ENABLED) 
            { 
                CustomTerminalNode node = new CustomTerminalNode("Night Vision", cfg.NIGHT_VISION_PRICE, $"Allows you to see in the dark. Press Left-Alt to turn on.  \nDrain speed is {cfg.NIGHT_VIS_DRAIN_SPEED}  \nRegen speed is {cfg.NIGHT_VIS_REGEN_SPEED}", nightVision, 3);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            //terminal flashbang
            GameObject flash = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/terminalFlash.prefab");
            AudioClip flashSFX = UpgradeAssets.LoadAsset<AudioClip>("Assets/ShipUpgrades/flashbangsfx.ogg");
            UpgradeBus.instance.flashNoise = flashSFX;
            flash.AddComponent<terminalFlashScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(flash);
            if(cfg.DISCOMBOBULATOR_ENABLED)
            {
                CustomTerminalNode node = new CustomTerminalNode("Discombobulator", cfg.DISCOMBOBULATOR_PRICE, $"Stun enemies around your ship in a {cfg.DISCOMBOBULATOR_RADIUS} unit radius.", flash, 3);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            //stronger scanner
            GameObject strongScan = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/strongScanner.prefab");
            strongScan.AddComponent<strongerScannerScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(strongScan);
            if(cfg.BETTER_SCANNER_ENABLED)
            {
                string LOS = cfg.REQUIRE_LINE_OF_SIGHT ? "Does not remove" : "Removes";
                string info = $"Increase distance nodes can be scanned by {cfg.NODE_DISTANCE_INCREASE} units.  \nIncrease distance Ship and Entrance can be scanned by {cfg.SHIP_AND_ENTRANCE_DISTANCE_INCREASE} units.  \n";
                info += $"{LOS} LOS requirement";
                CustomTerminalNode node = new CustomTerminalNode("Better Scanner", cfg.BETTER_SCANNER_PRICE, info, strongScan);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            // back muscles
            GameObject exoskel = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/exoskeleton.prefab");
            exoskel.AddComponent<exoskeletonScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(exoskel);
            if(cfg.BACK_MUSCLES_ENABLED)
            {
                CustomTerminalNode node = new CustomTerminalNode("Back Muscles", cfg.BACK_MUSCLES_PRICE, $"Carry weight becomes %{Mathf.Round((cfg.CARRY_WEIGHT_REDUCTION) * 100f)} of original", exoskel, 3);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            // pager
            GameObject pager = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/Pager.prefab");
            pager.AddComponent<pagerScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(pager);
            if (cfg.PAGER_ENABLED)
            {
                CustomTerminalNode node = new CustomTerminalNode("Pager", cfg.PAGER_PRICE, "Type `page <message>` to send a message to each team members chat", pager);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            //lockSmith
            GameObject lockSmith = UpgradeAssets.LoadAsset<GameObject>("Assets/ShipUpgrades/LockSmith.prefab");
            lockSmith.AddComponent<lockSmithScript>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(lockSmith);
            if (cfg.LOCKSMITH_ENABLED)
            {
                CustomTerminalNode node = new CustomTerminalNode("Locksmith", cfg.LOCKSMITH_PRICE,"Allows you to pick door locks by completing a minigame.", lockSmith);
                UpgradeBus.instance.terminalNodes.Add(node);
            }

            UpgradeBus.instance.CreateDeepNodeCopy();
            harmony.PatchAll();

            mls.LogInfo("More Ship Upgrades has been patched");
        }

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
