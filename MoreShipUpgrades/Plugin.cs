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
using LethalLib.Extras;

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
        string root = "Assets/ShipUpgrades/";

        public static PluginConfig cfg { get; private set; }


        void Awake()
        {
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

            SetupContractMapObjects(ref UpgradeAssets);


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

        private void SetupContractMapObjects(ref AssetBundle bundle)
        {
            AnimationCurve curve = new AnimationCurve(new Keyframe(0,1), new Keyframe(1,1)); // always spawn 1

            SetupScavContract(ref bundle, curve);
            SetupExterminatorContract(ref bundle, curve);
            SetupDataContract(ref bundle, curve);
            SetupExorcismContract(ref bundle, curve);
            SetupBombContract(ref bundle, curve);
        }

        void SetupBombContract(ref AssetBundle bundle, AnimationCurve curve)
        {
            Item bomb = AssetBundleHandler.TryLoadItemAsset(ref bundle, root + "BombItem.asset");
            if (bomb == null) return;

            ContractObject coNest = bomb.spawnPrefab.AddComponent<ContractObject>();
            coNest.contractType = "defusal";

            BombDefusalScript bombScript = bomb.spawnPrefab.AddComponent<BombDefusalScript>();
            bombScript.snip = AssetBundleHandler.TryLoadAudioClipAsset(ref bundle, root + "scissors.mp3");
            bombScript.tick = AssetBundleHandler.TryLoadAudioClipAsset(ref bundle, root + "tick.mp3");


            Utilities.FixMixerGroups(bomb.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(bomb.spawnPrefab);
            Items.RegisterItem(bomb);

            SpawnableMapObjectDef mapObjDefBug = ScriptableObject.CreateInstance<SpawnableMapObjectDef>();
            mapObjDefBug.spawnableMapObject = new SpawnableMapObject();
            mapObjDefBug.spawnableMapObject.prefabToSpawn = bomb.spawnPrefab;
            MapObjects.RegisterMapObject(mapObjDefBug, Levels.LevelTypes.All, (level) => curve);
        }


        void SetupExorcismContract(ref AssetBundle bundle, AnimationCurve curve)
        {
            Item contractLoot = AssetBundleHandler.TryLoadItemAsset(ref bundle, root + "ExorcLootItem.asset");
            Items.RegisterItem(contractLoot);
            Utilities.FixMixerGroups(contractLoot.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(contractLoot.spawnPrefab);

            Item mainItem = AssetBundleHandler.TryLoadItemAsset(ref bundle,root + "PentagramItem.asset");

            string[] ritualItems = new string[] { "Heart.asset", "Crucifix.asset", "candelabraItem.asset", "Teddy Bear.asset", "Bones.asset" };
            foreach(string ritualItem in ritualItems)
            {
                Item exorItem = AssetBundleHandler.TryLoadItemAsset(ref bundle,root + "RitualItems/" +ritualItem);
                ContractObject exorCo = exorItem.spawnPrefab.AddComponent<ContractObject>();
                exorCo.contractType = "exorcism";
                Items.RegisterItem(exorItem);
                Utilities.FixMixerGroups(exorItem.spawnPrefab);
                NetworkPrefabs.RegisterNetworkPrefab(exorItem.spawnPrefab);

                SpawnableMapObjectDef mapObjDefRitual = ScriptableObject.CreateInstance<SpawnableMapObjectDef>();
                mapObjDefRitual.spawnableMapObject = new SpawnableMapObject();
                mapObjDefRitual.spawnableMapObject.prefabToSpawn = exorItem.spawnPrefab;
                MapObjects.RegisterMapObject(mapObjDefRitual, Levels.LevelTypes.All, (level) => new AnimationCurve(new Keyframe(0,3),new Keyframe(1,3)));

                Items.RegisterShopItem(exorItem, 0);
            }

            if (mainItem == null || contractLoot == null) return;

            ContractObject co = mainItem.spawnPrefab.AddComponent<ContractObject>();
            co.contractType = "exorcism";

            PentagramScript pentScript = mainItem.spawnPrefab.AddComponent<PentagramScript>();
            pentScript.loot = contractLoot.spawnPrefab;
            pentScript.chant = AssetBundleHandler.TryLoadAudioClipAsset(ref bundle, root + "ritualSFX.mp3");
            pentScript.portal = AssetBundleHandler.TryLoadAudioClipAsset(ref bundle, root + "portal.mp3");

            Utilities.FixMixerGroups(mainItem.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(mainItem.spawnPrefab);
            Items.RegisterItem(mainItem);

            SpawnableMapObjectDef mapObjDef = ScriptableObject.CreateInstance<SpawnableMapObjectDef>();
            mapObjDef.spawnableMapObject = new SpawnableMapObject();
            mapObjDef.spawnableMapObject.prefabToSpawn = mainItem.spawnPrefab;
            MapObjects.RegisterMapObject(mapObjDef, Levels.LevelTypes.All, (level) => curve);
            Items.RegisterShopItem(mainItem, 0);
        }


        void SetupExterminatorContract(ref AssetBundle bundle, AnimationCurve curve)
        {
            Item bugLoot = AssetBundleHandler.TryLoadItemAsset(ref bundle, root + "EggLootItem.asset");
            Items.RegisterItem(bugLoot);
            Utilities.FixMixerGroups(bugLoot.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(bugLoot.spawnPrefab);

            Item nest = AssetBundleHandler.TryLoadItemAsset(ref bundle,root + "HoardingEggItem.asset");
            if (nest == null || bugLoot == null) return;

            ContractObject coNest = nest.spawnPrefab.AddComponent<ContractObject>();
            coNest.contractType = "exterminator";

            BugNestScript nestScript = nest.spawnPrefab.AddComponent<BugNestScript>();
            nestScript.loot = bugLoot.spawnPrefab;

            Utilities.FixMixerGroups(nest.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(nest.spawnPrefab);
            Items.RegisterItem(nest);

            SpawnableMapObjectDef mapObjDefBug = ScriptableObject.CreateInstance<SpawnableMapObjectDef>();
            mapObjDefBug.spawnableMapObject = new SpawnableMapObject();
            mapObjDefBug.spawnableMapObject.prefabToSpawn = nest.spawnPrefab;
            MapObjects.RegisterMapObject(mapObjDefBug, Levels.LevelTypes.All, (level) => curve);
        }

        void SetupScavContract(ref AssetBundle bundle, AnimationCurve curve)
        {
            Item scav = AssetBundleHandler.TryLoadItemAsset(ref bundle, root + "ScavItem.asset");
            if (scav == null) return;

            ContractObject co = scav.spawnPrefab.AddComponent<ContractObject>();
            co.contractType = "extraction";

            ExtractPlayerScript extractScript = scav.spawnPrefab.AddComponent<ExtractPlayerScript>();
            TextAsset scavAudioPaths = AssetBundleHandler.TryLoadOtherAsset<TextAsset>(ref bundle, root + "scavSounds/scavAudio.json");
            Dictionary<string, string[]> scavAudioDict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(scavAudioPaths.text);
            ExtractPlayerScript.clipDict.Add("lost", CreateAudioClipArray(scavAudioDict["lost"], ref bundle));
            ExtractPlayerScript.clipDict.Add("heal", CreateAudioClipArray(scavAudioDict["heal"], ref bundle));
            ExtractPlayerScript.clipDict.Add("safe", CreateAudioClipArray(scavAudioDict["safe"], ref bundle));
            ExtractPlayerScript.clipDict.Add("held", CreateAudioClipArray(scavAudioDict["held"], ref bundle));

            Utilities.FixMixerGroups(scav.spawnPrefab);
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(scav.spawnPrefab);
            Items.RegisterItem(scav);

            SpawnableMapObjectDef mapObjDef = ScriptableObject.CreateInstance<SpawnableMapObjectDef>();
            mapObjDef.spawnableMapObject = new SpawnableMapObject();
            mapObjDef.spawnableMapObject.prefabToSpawn = scav.spawnPrefab;
            MapObjects.RegisterMapObject(mapObjDef, Levels.LevelTypes.All, (level) => curve);
        }

        void SetupDataContract(ref AssetBundle bundle, AnimationCurve curve)
        {
            Item dataLoot = AssetBundleHandler.TryLoadItemAsset(ref bundle, root + "DiscItem.asset");
            Items.RegisterItem(dataLoot);
            Utilities.FixMixerGroups(dataLoot.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(dataLoot.spawnPrefab);

            Item pc = AssetBundleHandler.TryLoadItemAsset(ref bundle,root + "DataPCItem.asset");
            if (pc == null || dataLoot == null) return;

            ContractObject coPC = pc.spawnPrefab.AddComponent<ContractObject>();
            coPC.contractType = "data";

            DataPCScript dataScript = pc.spawnPrefab.AddComponent<DataPCScript>();
            dataScript.error = AssetBundleHandler.TryLoadAudioClipAsset(ref bundle, root + "winError.mp3");
            dataScript.startup = AssetBundleHandler.TryLoadAudioClipAsset(ref bundle, root + "startup.mp3");
            dataScript.loot = dataLoot.spawnPrefab;

            Utilities.FixMixerGroups(pc.spawnPrefab);
            NetworkPrefabs.RegisterNetworkPrefab(pc.spawnPrefab);
            Items.RegisterItem(pc);

            SpawnableMapObjectDef mapObjDefPC = ScriptableObject.CreateInstance<SpawnableMapObjectDef>();
            mapObjDefPC.spawnableMapObject = new SpawnableMapObject();
            mapObjDefPC.spawnableMapObject.prefabToSpawn = pc.spawnPrefab;
            MapObjects.RegisterMapObject(mapObjDefPC, Levels.LevelTypes.All, (level) => curve);
        }

        private AudioClip[] CreateAudioClipArray(string[] paths, ref AssetBundle bundle)
        {
            AudioClip[] clips = new AudioClip[paths.Length];
            for(int i = 0; i < paths.Length; i++)
            {
                clips[i] = AssetBundleHandler.TryLoadAudioClipAsset(ref bundle, paths[i]);
            }
            return clips;
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
            SetupHelmet();
            SetupDivingKit();
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

        private void SetupHelmet()
        {
            Item helmet = AssetBundleHandler.GetItemObject("HelmetItem");
            UpgradeBus.instance.helmetModel = AssetBundleHandler.GetPerkGameObject("HelmetModel");
            if (helmet == null) return;

            UpgradeBus.instance.SFX.Add("helmet",AssetBundleHandler.GetAudioClip("HelmetHit"));
            UpgradeBus.instance.SFX.Add("breakWood",AssetBundleHandler.GetAudioClip("breakWood"));

            HelmetScript helmScript = helmet.spawnPrefab.AddComponent<HelmetScript>();
            helmScript.itemProperties = helmet;
            helmScript.grabbable = true;
            helmScript.grabbableToEnemies = true;
            helmet.creditsWorth = cfg.HELMET_PRICE;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(helmet.spawnPrefab);

            if (!cfg.HELMET_ENABLED) return;

            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            node.displayText = string.Format(AssetBundleHandler.GetInfoFromJSON("Helmet"), cfg.HELMET_HITS_BLOCKED);
            Items.RegisterShopItem(helmet, null, null, node, helmet.creditsWorth);

            UpgradeBus.instance.ItemsToSync.Add("Helmet", helmet);
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
            Items.RegisterShopItem(tpBut, null, null, PortNode, tpBut.creditsWorth);

            UpgradeBus.instance.ItemsToSync.Add("Tele", tpBut);
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
            Items.RegisterShopItem(tpButAdvanced, null, null, advNode, tpButAdvanced.creditsWorth);

            UpgradeBus.instance.ItemsToSync.Add("AdvTele", tpButAdvanced);
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
            Items.RegisterShopItem(nightVisionItem, null, null, nightNode, nightVisionItem.creditsWorth);

            UpgradeBus.instance.ItemsToSync.Add("Night", nightVisionItem);
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
            Items.RegisterShopItem(DiveItem, null, null,medNode, DiveItem.creditsWorth);

            UpgradeBus.instance.ItemsToSync.Add("Dive",DiveItem);
        }
        private void SetupMedkit()
        {
            Item MedKitItem = AssetBundleHandler.GetItemObject("Medkit");
            if (MedKitItem == null) return;
            AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 3), new Keyframe(1f, 3));

            MedKitItem.creditsWorth = cfg.MEDKIT_PRICE;
            MedKitItem.itemId = 492016;
            MedkitScript medScript = MedKitItem.spawnPrefab.AddComponent<MedkitScript>();
            medScript.itemProperties = MedKitItem;
            medScript.grabbable = true;
            medScript.useCooldown = 2f;
            medScript.grabbableToEnemies = true;
            medScript.error = error;
            medScript.use = buttonPressed;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(MedKitItem.spawnPrefab);

            if (cfg.MEDKIT_ENABLED)
            {
                TerminalNode medNode = ScriptableObject.CreateInstance<TerminalNode>();
                medNode.displayText = string.Format("MEDKIT - ${0}\n\nLeft click to heal yourself for {1} health.\nCan be used {2} times.\n", cfg.MEDKIT_PRICE, cfg.MEDKIT_HEAL_VALUE, cfg.MEDKIT_USES);
                Items.RegisterShopItem(MedKitItem, null, null,medNode, MedKitItem.creditsWorth);
            }

            Item MedKitMapItem = AssetBundleHandler.GetItemObject("MedkitMapItem");
            if (MedKitMapItem == null) return;
            MedkitScript medMapScript = MedKitMapItem.spawnPrefab.AddComponent<MedkitScript>();
            ContractObject co = MedKitMapItem.spawnPrefab.AddComponent<ContractObject>();
            co.contractType = "extraction";
            medMapScript.itemProperties = MedKitMapItem;
            medMapScript.grabbable = true;
            medMapScript.useCooldown = 2f;
            medMapScript.grabbableToEnemies = true;
            medMapScript.error = error;
            medMapScript.use = buttonPressed;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(MedKitMapItem.spawnPrefab);

            SpawnableMapObjectDef mapObjDef = ScriptableObject.CreateInstance<SpawnableMapObjectDef>();
            mapObjDef.spawnableMapObject = new SpawnableMapObject();
            mapObjDef.spawnableMapObject.prefabToSpawn = MedKitMapItem.spawnPrefab;
            MapObjects.RegisterMapObject(mapObjDef, Levels.LevelTypes.All, (level) => curve);

            UpgradeBus.instance.ItemsToSync.Add("Medkit",MedKitItem);
        }
        private void SetupPeeper()
        {
            Item Peeper = AssetBundleHandler.GetItemObject("Peeper");
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

            TerminalNode peepNode = ScriptableObject.CreateInstance<TerminalNode>();
            peepNode.displayText = "Looks at coil heads, don't lose it\n";
            Items.RegisterShopItem(Peeper, null, null, peepNode, Peeper.creditsWorth);

            UpgradeBus.instance.ItemsToSync.Add("Peeper", Peeper);
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
            SetupContract();
            SetupSickBeats();
            SetupExtendDeadline();
        }

        private void SetupSickBeats()
        {
            SetupGenericPerk<BeatScript>(BeatScript.UPGRADE_NAME);
        }

        private void SetupContract()
        {
            SetupGenericPerk<ContractScript>(ContractScript.UPGRADE_NAME);
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
