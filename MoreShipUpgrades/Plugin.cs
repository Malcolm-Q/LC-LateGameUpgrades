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
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

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

            SetupModStore(ref UpgradeAssets);

            SetupIntroScreen(ref UpgradeAssets);

            SetupItems(ref UpgradeAssets, ref infoJson);
            
            SetupPerks(ref UpgradeAssets);


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

        private void SetupModStore(ref AssetBundle bundle)
        {
            GameObject modStore = AssetBundleHandler.TryLoadGameObjectAsset(ref bundle, "Assets/ShipUpgrades/LGUStore.prefab");
            if (modStore == null) return;

            modStore.AddComponent<LGUStore>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(modStore);
            UpgradeBus.instance.modStorePrefab = modStore;
        }
        private void SetupIntroScreen(ref AssetBundle bundle)
        {
            UpgradeBus.instance.introScreen = AssetBundleHandler.TryLoadGameObjectAsset(ref bundle, "Assets/ShipUpgrades/IntroScreen.prefab");
            if (UpgradeBus.instance.introScreen != null) UpgradeBus.instance.introScreen.AddComponent<IntroScreenScript>();
        }
        private void SetupItems(ref AssetBundle bundle, ref Dictionary<string, string> infoJSON)
        {
            SetupTeleporterButtons(ref bundle, ref infoJSON);
            SetupNightVision(ref bundle, ref infoJSON);
            SetupPeeper(ref bundle);
            SetupSamples(ref bundle);
        }
        private void SetupSamples(ref AssetBundle bundle)
        {
            Item fleaSample = AssetBundleHandler.TryLoadItemAsset(ref bundle, "Assets/ShipUpgrades/SnareFleaSample.asset");
            Item spiderSample = AssetBundleHandler.TryLoadItemAsset(ref bundle, "Assets/ShipUpgrades/BunkerSpiderSample.asset");
            Item hoardSample = AssetBundleHandler.TryLoadItemAsset(ref bundle, "Assets/ShipUpgrades/HoardingBugSample.asset");
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(fleaSample.spawnPrefab);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(spiderSample.spawnPrefab);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(hoardSample.spawnPrefab);
            UpgradeBus.instance.samplePrefabs.Add("snare flea", fleaSample.spawnPrefab);
            UpgradeBus.instance.samplePrefabs.Add("bunker spider", spiderSample.spawnPrefab);
            UpgradeBus.instance.samplePrefabs.Add("hoarding bug", hoardSample.spawnPrefab);
        }
        private void SetupTeleporterButtons(ref AssetBundle bundle, ref Dictionary<string, string> infoJSON)
        {
            // Teleporter Button SFX
            AudioClip itemBreak = AssetBundleHandler.TryLoadAudioClipAsset(ref bundle, "Assets/ShipUpgrades/break.mp3");
            AudioClip error = AssetBundleHandler.TryLoadAudioClipAsset(ref bundle, "Assets/ShipUpgrades/error.mp3");
            AudioClip buttonPressed = AssetBundleHandler.TryLoadAudioClipAsset(ref bundle, "Assets/ShipUpgrades/ButtonPress2.ogg");

            if (itemBreak == null || error == null || buttonPressed == null) return;

            SetupRegularTeleporterButton(ref bundle, ref itemBreak, ref error, ref buttonPressed, ref infoJSON);
            SetupAdvancedTeleporterButton(ref bundle, ref itemBreak, ref error, ref buttonPressed, ref infoJSON);
        }
        private void SetupRegularTeleporterButton(ref AssetBundle bundle, ref AudioClip itemBreak, ref AudioClip error, ref AudioClip buttonPressed, ref Dictionary<string, string> infoJSON)
        {
            Item tpBut = AssetBundleHandler.TryLoadItemAsset(ref bundle, "Assets/ShipUpgrades/TpButton.asset");
            if (tpBut == null) return;

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

            if (!cfg.WEAK_TELE_ENABLED) return;

            TerminalNode PortNode = new TerminalNode();
            PortNode.displayText = string.Format(infoJSON["Portable Tele"], (int)(cfg.CHANCE_TO_BREAK * 100));
            Items.RegisterShopItem(tpBut, null, null, PortNode, tpBut.creditsWorth);
        }
        private void SetupAdvancedTeleporterButton(ref AssetBundle bundle, ref AudioClip itemBreak, ref AudioClip error, ref AudioClip buttonPressed, ref Dictionary<string, string> infoJSON)
        {
            Item tpButAdvanced = AssetBundleHandler.TryLoadItemAsset(ref bundle, "Assets/ShipUpgrades/TpButtonAdv.asset");
            if (tpButAdvanced == null) return;

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

            if (!cfg.ADVANCED_TELE_ENABLED) return;

            TerminalNode advNode = new TerminalNode();
            advNode.displayText = string.Format(infoJSON["Advanced Portable Tele"], (int)(cfg.ADV_CHANCE_TO_BREAK * 100));
            Items.RegisterShopItem(tpButAdvanced, null, null, advNode, tpButAdvanced.creditsWorth);
        }
        private void SetupNightVision(ref AssetBundle bundle, ref Dictionary<string, string> infoJSON)
        {
            Item nightVisionItem = AssetBundleHandler.TryLoadItemAsset(ref bundle, "Assets/ShipUpgrades/NightVisionItem.asset");
            if (nightVisionItem == null) return;

            nightVisionItem.creditsWorth = cfg.NIGHT_VISION_PRICE;
            nightVisionItem.spawnPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            nightVisionItem.itemId = 492014;
            NightVisionItemScript visScript = nightVisionItem.spawnPrefab.AddComponent<NightVisionItemScript>();
            visScript.itemProperties = nightVisionItem;
            visScript.grabbable = true;
            visScript.useCooldown = 2f;
            visScript.grabbableToEnemies = true;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(nightVisionItem.spawnPrefab);

            if (!cfg.NIGHT_VISION_ENABLED) return;

            TerminalNode nightNode = new TerminalNode();
            string grantStatus = cfg.NIGHT_VISION_INDIVIDUAL || UpgradeBus.instance.cfg.SHARED_UPGRADES ? "one" : "all";
            string loseOnDeath = cfg.LOSE_NIGHT_VIS_ON_DEATH ? "be" : "not be";
            nightNode.displayText = string.Format(infoJSON["Night Vision"], grantStatus, loseOnDeath);
            Items.RegisterShopItem(nightVisionItem, null, null, nightNode, nightVisionItem.creditsWorth);
        }
        private void SetupPeeper(ref AssetBundle bundle)
        {
            Item Peeper = AssetBundleHandler.TryLoadItemAsset(ref bundle, "Assets/ShipUpgrades/coilHead.asset");
            if (Peeper == null) return;

            Peeper.creditsWorth = cfg.PEEPER_PRICE;
            Peeper.twoHanded = false;
            Peeper.itemId = 492015;
            Peeper.twoHandedAnimation = false;
            Peeper.spawnPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            coilHeadItem peepScript = Peeper.spawnPrefab.AddComponent<coilHeadItem>();
            peepScript.itemProperties = Peeper;
            peepScript.grabbable = true;
            peepScript.grabbableToEnemies = true;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(Peeper.spawnPrefab);

            if (!cfg.PEEPER_ENABLED) return;

            TerminalNode peepNode = new TerminalNode();
            peepNode.displayText = "Looks at coil heads, don't lose it\n";
            Items.RegisterShopItem(Peeper, null, null, peepNode, Peeper.creditsWorth);
        }
        private void SetupPerks(ref AssetBundle bundle)
        {
            SetupBeekeeper(ref bundle);
            SetupProteinPowder(ref bundle);
            SetupBiggerLungs(ref bundle);
            SetupRunningShoes(ref bundle);
            SetupStrongLegs(ref bundle);
            SetupMalwareBroadcaster(ref bundle);
            SetupLightFooted(ref bundle);
            SetupNightVisionBattery(ref bundle);
            SetupDiscombobulator(ref bundle);
            SetupBetterScanner(ref bundle);
            SetupWalkieGPS(ref bundle);
            SetupBackMuscles(ref bundle);
            SetupInterns(ref bundle);
            SetupPager(ref bundle);
            SetupLightningRod(ref bundle);
            SetupLocksmith(ref bundle);
            SetupPlayerHealth(ref bundle);
        }
        private void SetupBeekeeper(ref AssetBundle bundle)
        {
            SetupGenericPerk<beekeeperScript>(ref bundle, "Assets/ShipUpgrades/beekeeper.prefab");
        }
        private void SetupProteinPowder(ref AssetBundle bundle) 
        {
            SetupGenericPerk<proteinPowderScript>(ref bundle, "Assets/ShipUpgrades/ProteinPowder.prefab");
        }
        private void SetupBiggerLungs(ref AssetBundle bundle)
        {
            SetupGenericPerk<biggerLungScript>(ref bundle, "Assets/ShipUpgrades/BiggerLungs.prefab");
        }
        private void SetupRunningShoes(ref AssetBundle bundle) 
        {
            SetupGenericPerk<runningShoeScript>(ref bundle, "Assets/ShipUpgrades/runningShoes.prefab");
        }
        private void SetupStrongLegs(ref AssetBundle bundle) 
        {
            SetupGenericPerk<strongLegsScript>(ref bundle, "Assets/ShipUpgrades/strongLegs.prefab");
        }
        private void SetupMalwareBroadcaster(ref AssetBundle bundle)
        {
            SetupGenericPerk<trapDestroyerScript>(ref bundle, "Assets/ShipUpgrades/destructiveCodes.prefab");
        }
        private void SetupLightFooted(ref AssetBundle bundle) 
        {
            SetupGenericPerk<lightFootedScript>(ref bundle, "Assets/ShipUpgrades/lightFooted.prefab");
        }
        private void SetupNightVisionBattery(ref AssetBundle bundle) 
        {
            SetupGenericPerk<nightVisionScript>(ref bundle, "Assets/ShipUpgrades/nightVision.prefab");
        }
        private void SetupDiscombobulator(ref AssetBundle bundle)
        {
            AudioClip flashSFX = AssetBundleHandler.TryLoadAudioClipAsset(ref bundle, "Assets/ShipUpgrades/flashbangsfx.ogg");
            if (flashSFX != null) UpgradeBus.instance.flashNoise = flashSFX;

            SetupGenericPerk<terminalFlashScript>(ref bundle, "Assets/ShipUpgrades/terminalFlash.prefab");
        }
        private void SetupBetterScanner(ref AssetBundle bundle)
        {
            SetupGenericPerk<strongerScannerScript>(ref bundle, "Assets/ShipUpgrades/strongScanner.prefab");
        }
        private void SetupWalkieGPS(ref AssetBundle bundle)
        {
            SetupGenericPerk<walkieScript>(ref bundle, "Assets/ShipUpgrades/walkieUpgrade.prefab");
        }
        private void SetupBackMuscles(ref AssetBundle bundle)
        {
            SetupGenericPerk<exoskeletonScript>(ref bundle, "Assets/ShipUpgrades/exoskeleton.prefab");
        }
        private void SetupInterns(ref AssetBundle bundle)
        {
            SetupGenericPerk<defibScript>(ref bundle, "Assets/ShipUpgrades/Intern.prefab");
        }
        private void SetupPager(ref AssetBundle bundle)
        {
            SetupGenericPerk<pagerScript>(ref bundle, "Assets/ShipUpgrades/Pager.prefab");
        }
        private void SetupLightningRod(ref AssetBundle bundle)
        {
            SetupGenericPerk<lightningRodScript>(ref bundle, "Assets/ShipUpgrades/LightningRod.prefab");
        }
        private void SetupLocksmith(ref AssetBundle bundle)
        {
            SetupGenericPerk<lockSmithScript>(ref bundle, "Assets/ShipUpgrades/LockSmith.prefab");
        }
        private void SetupPlayerHealth(ref AssetBundle bundle)
        {
            SetupGenericPerk<playerHealthScript>(ref bundle, "Assets/ShipUpgrades/PlayerHealth.prefab");
        }
        /// <summary>
        /// Generic function where it adds a script (specificed through the type) into an GameObject asset 
        /// which is present in a provided asset bundle in a given path and registers it as a network prefab.
        /// </summary>
        /// <typeparam name="T"> The script we wish to include into the GameObject asset</typeparam>
        /// <param name="bundle"> The asset bundle where the asset is located</param>
        /// <param name="path"> The path to access the asset in the asset bundle</param>
        private void SetupGenericPerk<T>(ref AssetBundle bundle, string path) where T : Component
        {
            GameObject perk = AssetBundleHandler.TryLoadGameObjectAsset(ref bundle, path);
            if (!perk) return;

            perk.AddComponent<T>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(perk);
        }
    }
}
