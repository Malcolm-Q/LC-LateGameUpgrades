using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using MoreShipUpgrades.Managers;
using System.IO;
using System.Reflection;
using MoreShipUpgrades.Misc;
using BepInEx.Bootstrap;
using Newtonsoft.Json;
using MoreShipUpgrades.Patches;
using MoreShipUpgrades.UpgradeComponents;
using MoreShipUpgrades.UpgradeComponents.Wheelbarrow;

namespace MoreShipUpgrades
{
    [BepInEx.BepInPlugin(Metadata.GUID,Metadata.NAME,Metadata.VERSION)]
    [BepInDependency("evaisa.lethallib")]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(Metadata.GUID);
        public static Plugin instance;
        public static ManualLogSource mls;
        private AudioClip itemBreak, buttonPressed, error;
        private AudioClip[] wheelbarrowSound, shoppingCartSound;

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

            GameObject busGO = new GameObject("UpgradeBus");
            busGO.AddComponent<UpgradeBus>();

            UpgradeBus.instance.version = Metadata.VERSION;
            UpgradeBus.instance.UpgradeAssets = UpgradeAssets;

            UpgradeBus.instance.internNames = AssetBundleHandler.GetInfoFromJSON("InternNames").Split(",");
            UpgradeBus.instance.internInterests = AssetBundleHandler.GetInfoFromJSON("InternInterests").Split(",");

            SetupModStore(ref UpgradeAssets);

            SetupIntroScreen(ref UpgradeAssets);

            SetupItems();
            
            SetupPerks();

            harmony.PatchAll();

            mls.LogDebug("LGU has been patched");
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
                        mls.LogDebug($"Failed to send info to ModSync, go yell at Minx");
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
        private void SetupItems()
        {
            SetupTeleporterButtons();
            SetupNightVision();
            SetupMedkit();
            SetupPeeper();
            SetupSamples();
            SetupDivingKit();
            SetupWheelbarrows();
        }
        private void SetupSamples()
        {
            Dictionary<string, int> MINIMUM_VALUES = new Dictionary<string, int>()
            {
                { "centipede", cfg.SNARE_FLEA_SAMPLE_MINIMUM_VALUE },
                { "bunker spider", cfg.BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE },
                { "hoarding bug", cfg.HOARDING_BUG_SAMPLE_MINIMUM_VALUE },
                { "flowerman", cfg.BRACKEN_SAMPLE_MINIMUM_VALUE },
                { "mouthdog", cfg.EYELESS_DOG_SAMPLE_MINIMUM_VALUE },
                { "baboon hawk", cfg.BABOON_HAWK_SAMPLE_MINIMUM_VALUE },
                { "crawler", cfg.THUMPER_SAMPLE_MINIMUM_VALUE },
            };
            Dictionary<string, int> MAXIMUM_VALUES = new Dictionary<string, int>()
            {
                { "centipede", cfg.SNARE_FLEA_SAMPLE_MAXIMUM_VALUE },
                { "bunker spider", cfg.BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE },
                { "hoarding bug", cfg.HOARDING_BUG_SAMPLE_MAXIMUM_VALUE },
                { "flowerman", cfg.BRACKEN_SAMPLE_MAXIMUM_VALUE },
                { "mouthdog", cfg.EYELESS_DOG_SAMPLE_MAXIMUM_VALUE },
                { "baboon hawk", cfg.BABOON_HAWK_SAMPLE_MAXIMUM_VALUE },
                { "crawler", cfg.THUMPER_SAMPLE_MAXIMUM_VALUE },
            };
            foreach (string creatureName in AssetBundleHandler.samplePaths.Keys)
            {
                Item sample = AssetBundleHandler.GetItemObject(creatureName);
                SampleItem sampleScript = sample.spawnPrefab.AddComponent<SampleItem>();
                sampleScript.grabbable = true;
                sampleScript.grabbableToEnemies = true;
                sampleScript.itemProperties = sample;
                sampleScript.itemProperties.minValue = MINIMUM_VALUES[creatureName];
                sampleScript.itemProperties.maxValue = MAXIMUM_VALUES[creatureName];
                LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(sample.spawnPrefab);
                UpgradeBus.instance.samplePrefabs.Add(creatureName, sample.spawnPrefab);
            }
        }
        private void SetupTeleporterButtons()
        {
            // Teleporter Button SFX
            itemBreak = AssetBundleHandler.GetAudioClip("Break");
            error = AssetBundleHandler.GetAudioClip("Error");
            buttonPressed = AssetBundleHandler.GetAudioClip("Button Press");

            if (itemBreak == null || error == null || buttonPressed == null) return;

            SetupRegularTeleporterButton();
            SetupAdvancedTeleporterButton();
        }
        private void SetupRegularTeleporterButton()
        {
            Item tpBut = AssetBundleHandler.GetItemObject("Portable Tele");
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

            TerminalNode PortNode = ScriptableObject.CreateInstance<TerminalNode>();
            PortNode.displayText = string.Format(AssetBundleHandler.GetInfoFromJSON("Portable Tele"), (int)(cfg.CHANCE_TO_BREAK * 100));
            LethalLib.Modules.Items.RegisterShopItem(tpBut, null, null, PortNode, tpBut.creditsWorth);
        }
        private void SetupAdvancedTeleporterButton()
        {
            Item tpButAdvanced = AssetBundleHandler.GetItemObject("Advanced Portable Tele");
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

            TerminalNode advNode = ScriptableObject.CreateInstance<TerminalNode>();
            advNode.displayText = string.Format(AssetBundleHandler.GetInfoFromJSON("Advanced Portable Tele"), (int)(cfg.ADV_CHANCE_TO_BREAK * 100));
            LethalLib.Modules.Items.RegisterShopItem(tpButAdvanced, null, null, advNode, tpButAdvanced.creditsWorth);
        }

        private void SetupNightVision()
        {
            Item nightVisionItem = AssetBundleHandler.GetItemObject("Night Vision");
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
            UpgradeBus.instance.nightVisionPrefab = nightVisionItem.spawnPrefab;

            if (!cfg.NIGHT_VISION_ENABLED) return;

            TerminalNode nightNode = ScriptableObject.CreateInstance<TerminalNode>();
            string grantStatus = cfg.NIGHT_VISION_INDIVIDUAL || UpgradeBus.instance.cfg.SHARED_UPGRADES ? "one" : "all";
            string loseOnDeath = cfg.LOSE_NIGHT_VIS_ON_DEATH ? "be" : "not be";
            nightNode.displayText = string.Format(AssetBundleHandler.GetInfoFromJSON("Night Vision"), grantStatus, loseOnDeath);
            LethalLib.Modules.Items.RegisterShopItem(nightVisionItem, null, null, nightNode, nightVisionItem.creditsWorth);
        }
        private void SetupDivingKit()
        {
            Item DiveItem = AssetBundleHandler.GetItemObject("Diving Kit");
            if (DiveItem == null) return;

            DiveItem.creditsWorth = cfg.DIVEKIT_PRICE;
            DiveItem.itemId = 492015;
            DiveItem.twoHanded = cfg.DIVEKIT_TWO_HANDED;
            DiveItem.weight = cfg.DIVEKIT_WEIGHT;
            DiveItem.itemSpawnsOnGround = true;
            DivingKitScript diveScript = DiveItem.spawnPrefab.AddComponent<DivingKitScript>();
            diveScript.itemProperties = DiveItem;
            diveScript.grabbable = true;
            diveScript.grabbableToEnemies = true;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(DiveItem.spawnPrefab);

            if (!cfg.DIVEKIT_ENABLED) return;

            TerminalNode medNode = ScriptableObject.CreateInstance<TerminalNode>();
            string hands = cfg.DIVEKIT_TWO_HANDED ? "two" : "one";
            medNode.displayText = $"DIVING KIT - ${cfg.DIVEKIT_PRICE}\n\nBreath underwater.\nWeights {Mathf.RoundToInt((DiveItem.weight -1 )*100)} lbs and is {hands} handed.\n\n";
            LethalLib.Modules.Items.RegisterShopItem(DiveItem, null, null,medNode, DiveItem.creditsWorth);
        }
        private void SetupMedkit()
        {
            Item MedKitItem = AssetBundleHandler.GetItemObject("Medkit");
            if (MedKitItem == null) return;

            MedKitItem.creditsWorth = cfg.MEDKIT_PRICE;
            MedKitItem.itemSpawnsOnGround = true;
            MedKitItem.itemId = 492016;
            MedkitScript medScript = MedKitItem.spawnPrefab.AddComponent<MedkitScript>();
            medScript.itemProperties = MedKitItem;
            medScript.grabbable = true;
            medScript.useCooldown = 2f;
            medScript.grabbableToEnemies = true;
            medScript.error = error;
            medScript.use = buttonPressed;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(MedKitItem.spawnPrefab);

            if (!cfg.MEDKIT_ENABLED) return;

            TerminalNode medNode = ScriptableObject.CreateInstance<TerminalNode>();
            medNode.displayText = string.Format("MEDKIT - ${0}\n\nLeft click to heal yourself for {1} health.\nCan be used {2} times.\n", cfg.MEDKIT_PRICE, cfg.MEDKIT_HEAL_VALUE, cfg.MEDKIT_USES);
            LethalLib.Modules.Items.RegisterShopItem(MedKitItem, null, null,medNode, MedKitItem.creditsWorth);
        }
        private void SetupPeeper()
        {
            Item Peeper = AssetBundleHandler.GetItemObject("Peeper");
            if (Peeper == null) return;

            Peeper.creditsWorth = cfg.PEEPER_PRICE;
            Peeper.twoHanded = false;
            Peeper.itemId = 492017;
            Peeper.twoHandedAnimation = false;
            Peeper.spawnPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            coilHeadItem peepScript = Peeper.spawnPrefab.AddComponent<coilHeadItem>();
            peepScript.itemProperties = Peeper;
            peepScript.grabbable = true;
            peepScript.grabbableToEnemies = true;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(Peeper.spawnPrefab);

            if (!cfg.PEEPER_ENABLED) return;

            TerminalNode peepNode = ScriptableObject.CreateInstance<TerminalNode>();
            peepNode.displayText = "Looks at coil heads, don't lose it\n";
            LethalLib.Modules.Items.RegisterShopItem(Peeper, null, null, peepNode, Peeper.creditsWorth);
        }
        private void SetupWheelbarrows()
        {
            wheelbarrowSound = GetAudioClipList("Wheelbarrow Sound", 4);
            shoppingCartSound = GetAudioClipList("Scrap Wheelbarrow Sound", 4);
            SetupStoreWheelbarrow();
            SetupScrapWheelbarrow();
        }
        private AudioClip[] GetAudioClipList(string name, int length)
        {
            AudioClip[] array = new AudioClip[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = AssetBundleHandler.GetAudioClip($"{name} {i}");
            }
            return array;
        }
        private void SetupScrapWheelbarrow() 
        {
            Item wheelbarrow = AssetBundleHandler.GetItemObject("Scrap Wheelbarrow");
            if (wheelbarrow == null) return;
            wheelbarrow.itemId = 492018;
            wheelbarrow.minValue = cfg.SCRAP_WHEELBARROW_MINIMUM_VALUE;
            wheelbarrow.maxValue = cfg.SCRAP_WHEELBARROW_MAXIMUM_VALUE;
            wheelbarrow.twoHanded = true;
            wheelbarrow.twoHandedAnimation = true;
            wheelbarrow.grabAnim = "HoldJetpack";
            wheelbarrow.floorYOffset = -90;
            wheelbarrow.positionOffset = new Vector3(0f, -1.7f, 0.35f);
            wheelbarrow.allowDroppingAheadOfPlayer = true;
            wheelbarrow.isConductiveMetal = true;
            wheelbarrow.isScrap = true;
            wheelbarrow.weight = 1f + (cfg.SCRAP_WHEELBARROW_WEIGHT/100f);
            ScrapWheelbarrow barrowScript = wheelbarrow.spawnPrefab.AddComponent<ScrapWheelbarrow>();
            barrowScript.itemProperties = wheelbarrow;
            barrowScript.wheelsClip = shoppingCartSound;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(wheelbarrow.spawnPrefab);

            if (!cfg.SCRAP_WHEELBARROW_ENABLED) return;

            LethalLib.Modules.Items.RegisterScrap(wheelbarrow, cfg.SCRAP_WHEELBARROW_RARITY, LethalLib.Modules.Levels.LevelTypes.All);
        }
        private void SetupStoreWheelbarrow()
        {
            Item wheelbarrow = AssetBundleHandler.GetItemObject("Store Wheelbarrow");
            if (wheelbarrow == null) return;

            wheelbarrow.itemId = 492019;
            wheelbarrow.creditsWorth = cfg.WHEELBARROW_PRICE;
            wheelbarrow.twoHanded = true;
            wheelbarrow.twoHandedAnimation = true;
            wheelbarrow.grabAnim = "HoldJetpack";
            wheelbarrow.floorYOffset = -90;
            wheelbarrow.verticalOffset =0.6f;
            wheelbarrow.positionOffset = new Vector3(0f, -0.7f, 1.4f);
            wheelbarrow.allowDroppingAheadOfPlayer = true;
            wheelbarrow.isConductiveMetal = true;
            wheelbarrow.weight = 1f + (cfg.WHEELBARROW_WEIGHT/100f);
            StoreWheelbarrow barrowScript = wheelbarrow.spawnPrefab.AddComponent<StoreWheelbarrow>();
            barrowScript.itemProperties = wheelbarrow;
            barrowScript.wheelsClip = wheelbarrowSound;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(wheelbarrow.spawnPrefab);

            if (!cfg.WHEELBARROW_ENABLED) return;

            TerminalNode wheelbarrowNode = ScriptableObject.CreateInstance<TerminalNode>();
            wheelbarrowNode.displayText = $"A portable container which has a maximum capacity of {cfg.WHEELBARROW_MAXIMUM_AMOUNT_ITEMS} and reduces the effective weight of the inserted items by {cfg.WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER*100} %.\nIt weighs {1f + (cfg.WHEELBARROW_WEIGHT/100f)} lbs";
            LethalLib.Modules.Items.RegisterShopItem(wheelbarrow, null, null, wheelbarrowNode, wheelbarrow.creditsWorth);
        }
        private void SetupPerks()
        {
            SetupBeekeeper();
            SetupProteinPowder();
            SetupBiggerLungs();
            SetupRunningShoes();
            SetupStrongLegs();
            SetupMalwareBroadcaster();
            SetupNightVisionBattery();
            SetupDiscombobulator();
            SetupBetterScanner();
            SetupWalkieGPS();
            SetupBackMuscles();
            SetupInterns();
            SetupPager();
            SetupLightningRod();
            SetupLocksmith();
            SetupPlayerHealth();
            SetupHunter();
            SetupExtendDeadline();
        }
        private void SetupBeekeeper()
        {
            SetupGenericPerk<beekeeperScript>(beekeeperScript.UPGRADE_NAME);
        }
        private void SetupHunter()
        {
            SetupGenericPerk<hunterScript>(hunterScript.UPGRADE_NAME);
        }
        private void SetupProteinPowder() 
        {
            SetupGenericPerk<proteinPowderScript>(proteinPowderScript.UPGRADE_NAME);
        }
        private void SetupBiggerLungs()
        {
            SetupGenericPerk<biggerLungScript>(biggerLungScript.UPGRADE_NAME);
        }
        private void SetupRunningShoes() 
        {
            SetupGenericPerk<runningShoeScript>(runningShoeScript.UPGRADE_NAME);
        }
        private void SetupStrongLegs() 
        {
            SetupGenericPerk<strongLegsScript>(strongLegsScript.UPGRADE_NAME);
        }
        private void SetupMalwareBroadcaster()
        {
            SetupGenericPerk<trapDestroyerScript>(trapDestroyerScript.UPGRADE_NAME);
        }
        private void SetupNightVisionBattery() 
        {
            SetupGenericPerk<nightVisionScript>(nightVisionScript.UPGRADE_NAME);
        }
        private void SetupDiscombobulator()
        {
            AudioClip flashSFX = AssetBundleHandler.GetAudioClip("Flashbang");
            if (flashSFX != null) UpgradeBus.instance.flashNoise = flashSFX;

            SetupGenericPerk<terminalFlashScript>(terminalFlashScript.UPGRADE_NAME);
        }
        private void SetupBetterScanner()
        {
            SetupGenericPerk<strongerScannerScript>(strongerScannerScript.UPGRADE_NAME);
        }
        private void SetupWalkieGPS()
        {
            SetupGenericPerk<walkieScript>(walkieScript.UPGRADE_NAME);

        }
        private void SetupBackMuscles()
        {
            SetupGenericPerk<exoskeletonScript>(exoskeletonScript.UPGRADE_NAME);
        }
        private void SetupInterns()
        {
            SetupGenericPerk<defibScript>(defibScript.UPGRADE_NAME);
        }
        private void SetupPager()
        {
            SetupGenericPerk<pagerScript>(pagerScript.UPGRADE_NAME);
        }
        private void SetupLightningRod()
        {
            SetupGenericPerk<lightningRodScript>(lightningRodScript.UPGRADE_NAME);
        }
        private void SetupLocksmith()
        {
            SetupGenericPerk<lockSmithScript>(lockSmithScript.UPGRADE_NAME);
        }
        private void SetupPlayerHealth()
        {
            SetupGenericPerk<playerHealthScript>(playerHealthScript.UPGRADE_NAME);
        }
        private void SetupExtendDeadline()
        {
            SetupGenericPerk<ExtendDeadlineScript>(ExtendDeadlineScript.UPGRADE_NAME);
        }
        /// <summary>
        /// Generic function where it adds a script (specificed through the type) into an GameObject asset 
        /// which is present in a provided asset bundle in a given path and registers it as a network prefab.
        /// </summary>
        /// <typeparam name="T"> The script we wish to include into the GameObject asset</typeparam>
        /// <param name="bundle"> The asset bundle where the asset is located</param>
        /// <param name="path"> The path to access the asset in the asset bundle</param>
        private void SetupGenericPerk<T>(string upgradeName) where T : Component
        {
            GameObject perk = AssetBundleHandler.GetPerkGameObject(upgradeName);
            if (!perk) return;

            perk.AddComponent<T>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(perk);
        }
    }
}
