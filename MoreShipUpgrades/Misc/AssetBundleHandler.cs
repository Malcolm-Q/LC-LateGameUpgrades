using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    internal static class AssetBundleHandler
    {
        static readonly LguLogger logger = new LguLogger(typeof(AssetBundleHandler).Name);
        static Dictionary<string, string> infoJSON;
        const string root = "Assets/ShipUpgrades/";

        const string upgrades = root + "Upgrades/";

        const string commands = root + "Commands/";

        const string items = root + "Items/";
        const string sampleItems = items + "Samples/";
        const string storeItems = items + "Store/";
        const string contractItems = items + "Contracts/";
        const string extractionItems = contractItems + "Extraction/";
        const string defusalItems = contractItems + "Defusal/";
        const string dataItems = contractItems + "Data Retrieval/";
        const string exorcismItems = contractItems + "Exorcism/";
        const string ritualItems = exorcismItems + "Ritual Items/";
        const string exterminatorItems = contractItems + "Exterminator/";

        const string UI = root + "UI/";
        const string introScreen = UI + "Intro Screen UI/";

        const string models = root + "Models/";
        const string prefabs = models + "Prefabs/";

        const string SFX = root + "Sound Effects/";
        const string contractSFX = SFX + "Contracts/";
        const string defusalSFX = contractSFX + "Defusal/";
        const string exorcismSFX = contractSFX + "Exorcism/";
        const string extractionSFX = contractSFX + "Extraction/";
        const string dataSFX = contractSFX + "Data Retrieval/";
        const string DiscombobulatorSFX = SFX + "Discombobulator/";
        const string ShoppingCartSFX = SFX + "Shopping Cart/";
        const string WheelbarrowSFX = SFX + "Wheelbarrow/";
        const string PortableTeleSFX = SFX + "Portable Teleporter/";
        const string HelmetSFX = SFX + "Helmet/";
        internal static readonly Dictionary<string, string> samplePaths = new Dictionary<string, string>()
        {
            { "centipede", sampleItems+ "SnareFleaSample.asset" },
            { "bunker spider", sampleItems+ "BunkerSpiderSample.asset" },
            { "hoarding bug", sampleItems+ "HoardingBugSample.asset" },
            { "flowerman", sampleItems+ "BrackenSample.asset" },
            { "mouthdog", sampleItems+ "EyelessDogSample.asset" },
            { "baboon hawk", sampleItems+ "BaboonHawkSample.asset" },
            { "crawler", sampleItems+ "ThumperSample.asset" },
        };
        static Dictionary<string, string> SFXPaths = new Dictionary<string, string>()
        {
            { "Wheelbarrow Sound 0", WheelbarrowSFX+"Wheelbarrow_Move_1.mp3" },
            { "Wheelbarrow Sound 1", WheelbarrowSFX+"Wheelbarrow_Move_2.ogg" },
            { "Wheelbarrow Sound 2", WheelbarrowSFX+"Wheelbarrow_Move_3.ogg" },
            { "Wheelbarrow Sound 3", WheelbarrowSFX+"Wheelbarrow_Move_4.ogg" },
            { "Scrap Wheelbarrow Sound 0", ShoppingCartSFX+"Shopping_Cart_Move_1.ogg" },
            { "Scrap Wheelbarrow Sound 1", ShoppingCartSFX+"Shopping_Cart_Move_2.ogg" },
            { "Scrap Wheelbarrow Sound 2", ShoppingCartSFX+"Shopping_Cart_Move_3.ogg" },
            { "Scrap Wheelbarrow Sound 3", ShoppingCartSFX+"Shopping_Cart_Move_4.ogg" },
            { "HelmetHit", HelmetSFX+"bonk.mp3" },
            { "breakWood", HelmetSFX+"breakWood.mp3" },
            { "Break", PortableTeleSFX+"break.mp3" },
            { "Error", PortableTeleSFX+"error.mp3" },
            { "Button Press", PortableTeleSFX+"ButtonPress2.ogg" },
            { "Flashbang", DiscombobulatorSFX+"flashbangsfx.ogg" },
            { "Data Startup", contractSFX + "startup.mp3" },
            { "Bomb Cut", defusalSFX + "scissors.mp3" },
            { "Bomb Tick", defusalSFX + "tick.mp3" },
            { "Ritual Fail", exorcismSFX + "ritualSFX.mp3" },
            { "Ritual Success", exorcismSFX + "portal.mp3" },
            { "Ritual Idle", exorcismSFX + "whisper.mp3" },
            { "Laptop Error", dataSFX + "winError.mp3" },
            { "Laptop Start", dataSFX + "startup.mp3" },
        };
        static Dictionary<string, string> assetPaths = new Dictionary<string, string>()
        {
            { Beekeeper.UPGRADE_NAME, upgrades+"beekeeper.prefab" },
            { SickBeats.UPGRADE_NAME, upgrades+"SickBeats.prefab" },
            { ProteinPowder.UPGRADE_NAME, upgrades+"ProteinPowder.prefab" },
            { BiggerLungs.UPGRADE_NAME, upgrades+"BiggerLungs.prefab" },
            { RunningShoes.UPGRADE_NAME, upgrades+"runningShoes.prefab" },
            { StrongLegs.UPGRADE_NAME, upgrades+"strongLegs.prefab" },
            { MalwareBroadcaster.UPGRADE_NAME, upgrades+"destructiveCodes.prefab" },
            { NightVision.UPGRADE_NAME, upgrades+"nightVision.prefab" },
            { Discombobulator.UPGRADE_NAME, upgrades+"terminalFlash.prefab" },
            { Hunter.UPGRADE_NAME, upgrades+"Hunter.prefab" },
            { BetterScanner.UPGRADE_NAME, upgrades+"strongScanner.prefab" },
            { LightningRod.UPGRADE_NAME, upgrades+"LightningRod.prefab" },
            { WalkieGPS.UPGRADE_NAME, upgrades+"walkieUpgrade.prefab" },
            { BackMuscles.UPGRADE_NAME, upgrades+"exoskeleton.prefab" },
            { "Interns", commands+"Intern.prefab" },
            { FastEncryption.UPGRADE_NAME, upgrades+"Pager.prefab" },
            { LockSmith.UPGRADE_NAME, upgrades+"LockSmith.prefab" },
            { Stimpack.UPGRADE_NAME, upgrades+"PlayerHealth.prefab" },
            { ExtendDeadlineScript.NAME, commands+"ExtendDeadline.prefab" },
            { DoorsHydraulicsBattery.UPGRADE_NAME, upgrades+"DoorsHydraulicsBattery.prefab" },
            { ScrapInsurance.COMMAND_NAME, commands+"ScrapInsurance.prefab" },
            { MarketInfluence.UPGRADE_NAME, upgrades+"MarketInfluence.prefab" },
            { BargainConnections.UPGRADE_NAME, upgrades+"BargainConnections.prefab" },
            { LethalDeals.UPGRADE_NAME, upgrades+"LethalDeals.prefab" },
            { QuantumDisruptor.UPGRADE_NAME, upgrades+"QuantumDisruptor.prefab" },
            { ChargingBooster.UPGRADE_NAME, upgrades + "ChargingBooster.prefab" },
            { "Intro Screen", introScreen + "IntroScreen.prefab" },
            { FasterDropPod.UPGRADE_NAME, upgrades+"FasterDropPod.prefab" },
            { Sigurd.UPGRADE_NAME, upgrades+"Sigurd.prefab" },
            { EfficientEngines.UPGRADE_NAME, upgrades+"EfficientEngines.prefab" },
            { ClimbingGloves.UPGRADE_NAME, upgrades+"ClimbingGloves.prefab" },
            { LithiumBatteries.UPGRADE_NAME, upgrades+"LithiumBatteries.prefab" },
            { AluminiumCoils.UPGRADE_NAME, upgrades+"AluminiumCoils.prefab" },
            { DeepPockets.UPGRADE_NAME, upgrades+"DeeperPockets.prefab" },

            { "Advanced Portable Tele", storeItems+"TpButtonAdv.asset" },
            { "Portable Tele", storeItems+"TpButton.asset" },
            { "Peeper", storeItems+"coilHead.asset" },
            { "Medkit", storeItems+"MedKitItem.asset" },
            { "HelmetItem", storeItems+"HelmetItem.asset" },
            { "HelmetModel", prefabs+"HelmetModel.prefab" },
            { "MedkitMapItem", extractionItems+"MedKitMapItem.asset" },
            { "Night Vision", storeItems+"NightVisionItem.asset" },
            { "Diving Kit", storeItems+"DivingKitItem.asset" },
            { "Store Wheelbarrow", items + "Wheelbarrow/StoreWheelbarrowItem.asset" },
            { "Scrap Wheelbarrow", items + "Wheelbarrow/ScrapWheelbarrowItem.asset" },
            { "Bomb", defusalItems + "BombItem.asset" },
            { "Demon Tome", exorcismItems + "ExorcLootItem.asset" },
            { "Pentagram", exorcismItems + "PentagramItem.asset" },
            { "RitualItem0", ritualItems + "Heart.asset" },
            { "RitualItem1", ritualItems + "Crucifix.asset" },
            { "RitualItem2", ritualItems + "candelabraItem.asset" },
            { "RitualItem3", ritualItems + "Teddy Bear.asset" },
            { "RitualItem4", ritualItems + "Bones.asset" },
            { "HoardingBugEggsLoot", exterminatorItems + "EggLootItem.asset" },
            { "HoardingBugEggs", exterminatorItems + "HoardingEggItem.asset" },
            { "Scavenger", extractionItems + "ScavItem.asset" },
            { "Scavenger Sounds", extractionSFX + "scavSounds/scavAudio.json" },
            { "Floppy Disk", dataItems + "DiscItem.asset" },
            { "Laptop", dataItems + "DataPCItem.asset" },
        };
        /// <summary>
        /// Tries to load an asset from provided asset bundle through a given path into a GameObject
        /// <para>
        /// If the asset requested does not exist in the bundle, it will be logged for easier tracking of what asset is missing from the bundle
        /// </para>
        /// </summary>
        /// <param name="bundle">The asset bundle we wish to gather the asset from</param>
        /// <param name="path">The path to the asset we wish to load</param>
        /// <returns>The asset's gameObject if it's present in the asset bundle, otherwise null</returns>
        public static GameObject TryLoadGameObjectAsset(ref AssetBundle bundle, string path)
        {
            GameObject result = bundle.LoadAsset<GameObject>(path);
            if (result == null)
            {
                logger.LogError($"An error has occurred trying to load asset from {path}");
            }
            return result;
        }
        /// <summary>
        /// Tries to load an asset from provided asset bundle through a given path into a AudioClip
        /// <para>
        /// If the asset requested does not exist in the bundle, it will be logged for easier tracking of what asset is missing from the bundle
        /// </para>
        /// </summary>
        /// <param name="bundle">The asset bundle we wish to gather the asset from</param>
        /// <param name="path">The path to the asset we wish to load</param>
        /// <returns>The asset's AudioClip if it's present in the asset bundle, otherwise null</returns>
        public static AudioClip TryLoadAudioClipAsset(ref AssetBundle bundle, string path)
        {
            AudioClip result = bundle.LoadAsset<AudioClip>(path);
            if (result == null)
            {
                logger.LogError($"An error has occurred trying to load asset from {path}");
            }
            return result;
        }

        public static T TryLoadOtherAsset<T>(ref AssetBundle bundle, string path) where T : UnityEngine.Object
        {
            T result = bundle.LoadAsset<T>(path);
            if (result == null)
            {
                logger.LogError($"An error has occurred trying to load asset from {path}");
            }
            return result;
        }

        /// <summary>
        /// Tries to load an asset from provided asset bundle through a given path into a Item
        /// <para>
        /// If the asset requested does not exist in the bundle, it will be logged for easier tracking of what asset is missing from the bundle
        /// </para>
        /// </summary>
        /// <param name="bundle">The asset bundle we wish to gather the asset from</param>
        /// <param name="path">The path to the asset we wish to load</param>
        /// <returns>The asset's Item if it's present in the asset bundle, otherwise null</returns>
        public static Item TryLoadItemAsset(ref AssetBundle bundle, string path)
        {
            Item result = bundle.LoadAsset<Item>(path);
            if (result == null)
            {
                logger.LogError($"An error has occurred trying to load asset from {path}");
            }
            return result;
        }

        public static Dictionary<string, string> GetInfoJSON(ref AssetBundle bundle)
        {
            if (infoJSON != null) return infoJSON;

            TextAsset infoStringAsset = bundle.LoadAsset<TextAsset>(upgrades+"InfoStrings.json");
            if (!infoStringAsset)
            {
                logger.LogError("An error has occurred trying to load info strings from the bundle");
                return new();
            }

            infoJSON = JsonConvert.DeserializeObject<Dictionary<string, string>>(infoStringAsset.text);
            if (infoJSON == null)
            {
                logger.LogError("An error has occurred trying to deserialize info strings into a dictionary");
            }
            return infoJSON;
        }

        public static string GetInfoFromJSON(string key)
        {
            Dictionary<string, string> infoFormat = GetInfoJSON(ref UpgradeBus.Instance.UpgradeAssets);

            if (infoFormat == null) return "";

            if (!infoFormat.ContainsKey(key))
            {
                logger.LogError("The key was not present in the info JSON file!");
                return "";
            }
            return infoFormat[key];
        }

        public static GameObject GetPerkGameObject(string upgradeName)
        {
            if (!assetPaths.ContainsKey(upgradeName))
            {
                logger.LogError($"{upgradeName} was not present in the asset dictionary!");
                return null;
            }
            return TryLoadGameObjectAsset(ref UpgradeBus.Instance.UpgradeAssets, assetPaths[upgradeName]);
        }

        public static Item GetItemObject(string itemName)
        {
            if (assetPaths.ContainsKey(itemName)) return TryLoadItemAsset(ref UpgradeBus.Instance.UpgradeAssets, assetPaths[itemName]);
            if (samplePaths.ContainsKey(itemName)) return TryLoadItemAsset(ref UpgradeBus.Instance.UpgradeAssets, samplePaths[itemName]);

            logger.LogError($"{itemName} was not present in the asset or sample dictionary!");
            return null;
        }

        public static AudioClip GetAudioClip(string audioName)
        {
            if (!SFXPaths.ContainsKey(audioName))
            {
                logger.LogError($"{audioName} was not present in the asset dictionary!");
                return null;
            }
            return TryLoadAudioClipAsset(ref UpgradeBus.Instance.UpgradeAssets, SFXPaths[audioName]);
        }
        public static T GetGenericAsset<T>(string assetName) where T : UnityEngine.Object
        {
            if (!assetPaths.ContainsKey(assetName))
            {
                logger.LogError($"{assetName} was not present in the asset dictionary!");
                return default;
            }
            return TryLoadOtherAsset<T>(ref UpgradeBus.Instance.UpgradeAssets, assetPaths[assetName]);
        }
    }
}