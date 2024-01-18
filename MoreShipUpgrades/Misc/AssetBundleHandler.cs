using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    internal class AssetBundleHandler
    {
        private static readonly LGULogger logger = new LGULogger(typeof(AssetBundleHandler).Name);
        private static Dictionary<string, string> infoJSON;
        static string root = "Assets/ShipUpgrades/";
        public static Dictionary<string, string> samplePaths = new Dictionary<string, string>()
        {
            { "centipede", root+"Samples/SnareFleaSample.asset" },
            { "bunker spider", root+"Samples/BunkerSpiderSample.asset" },
            { "hoarding bug", root+"Samples/HoardingBugSample.asset" },
            { "flowerman", root+"Samples/BrackenSample.asset" },
            { "mouthdog", root+"Samples/EyelessDogSample.asset" },
            { "baboon hawk", root+"Samples/BaboonHawkSample.asset" },
            { "crawler", root+"Samples/ThumperSample.asset" },
        };
        private static Dictionary<string, string> assetPaths = new Dictionary<string, string>()
        {
            { beekeeperScript.UPGRADE_NAME, root+"beekeeper.prefab" },
            { BeatScript.UPGRADE_NAME, root+"SickBeats.prefab" },
            { ContractScript.UPGRADE_NAME, root+"Contract.prefab" },
            { proteinPowderScript.UPGRADE_NAME, root+"ProteinPowder.prefab" },
            { biggerLungScript.UPGRADE_NAME, root+"BiggerLungs.prefab" },
            { runningShoeScript.UPGRADE_NAME, root+"runningShoes.prefab" },
            { strongLegsScript.UPGRADE_NAME, root+"strongLegs.prefab" },
            { trapDestroyerScript.UPGRADE_NAME, root+"destructiveCodes.prefab" },
            { nightVisionScript.UPGRADE_NAME, root+"nightVision.prefab" },
            { terminalFlashScript.UPGRADE_NAME, root+"terminalFlash.prefab" },
            { hunterScript.UPGRADE_NAME, root+"Hunter.prefab" },
            { strongerScannerScript.UPGRADE_NAME, root+"strongScanner.prefab" },
            { lightningRodScript.UPGRADE_NAME, root+"LightningRod.prefab" },
            { walkieScript.UPGRADE_NAME, root+"walkieUpgrade.prefab" },
            { exoskeletonScript.UPGRADE_NAME, root+"exoskeleton.prefab" },
            { "Interns", root+"Intern.prefab" },
            { pagerScript.UPGRADE_NAME, root+"Pager.prefab" },
            { lockSmithScript.UPGRADE_NAME, root+"LockSmith.prefab" },
            { playerHealthScript.UPGRADE_NAME, root+"PlayerHealth.prefab" },
            { ExtendDeadlineScript.UPGRADE_NAME, root+"ExtendDeadline.prefab" },
            { DoorsHydraulicsBattery.UPGRADE_NAME, root+"DoorsHydraulicsBattery.prefab" },
            { ScrapInsurance.COMMAND_NAME, root+"ScrapInsurance.prefab" },

            { "Advanced Portable Tele", root+"TpButtonAdv.asset" },
            { "Portable Tele", root+"TpButton.asset" },
            { "Peeper", root+"coilHead.asset" },
            { "Medkit", root+"MedKitItem.asset" },
            { "HelmetItem", root+"HelmetItem.asset" },
            { "HelmetModel", root+"HelmetModel.prefab" },
            { "MedkitMapItem", root+"MedKitMapItem.asset" },
            { "Night Vision", root+"NightVisionItem.asset" },
            { "Diving Kit", root+"DivingKitItem.asset" },
            { "Store Wheelbarrow", root+"Items/Wheelbarrow/StoreWheelbarrowItem.asset" },
            { "Scrap Wheelbarrow", root+"Items/Wheelbarrow/ScrapWheelbarrowItem.asset" },

            { "Wheelbarrow Sound 0", root+"Items/Wheelbarrow/Wheelbarrow_Move_1.mp3" },
            { "Wheelbarrow Sound 1", root+"Items/Wheelbarrow/Wheelbarrow_Move_2.ogg" },
            { "Wheelbarrow Sound 2", root+"Items/Wheelbarrow/Wheelbarrow_Move_3.ogg" },
            { "Wheelbarrow Sound 3", root+"Items/Wheelbarrow/Wheelbarrow_Move_4.ogg" },
            { "Scrap Wheelbarrow Sound 0", root+"Items/Wheelbarrow/Shopping_Cart_Move_1.ogg" },
            { "Scrap Wheelbarrow Sound 1", root+"Items/Wheelbarrow/Shopping_Cart_Move_2.ogg" },
            { "Scrap Wheelbarrow Sound 2", root+"Items/Wheelbarrow/Shopping_Cart_Move_3.ogg" },
            { "Scrap Wheelbarrow Sound 3", root+"Items/Wheelbarrow/Shopping_Cart_Move_4.ogg" },
            { "HelmetHit", root+"bonk.mp3" },
            { "breakWood", root+"breakWood.mp3" },
            { "Break", root+"break.mp3" },
            { "Error", root+"error.mp3" },
            { "Button Press", root+"ButtonPress2.ogg" },
            { "Flashbang", root+"flashbangsfx.ogg" },
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
            logger.LogDebug($"Loaded asset located in {path}");
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
            logger.LogDebug($"Loaded asset located in {path}");
            return result;
        }

        public static T TryLoadOtherAsset<T>(ref AssetBundle bundle, string path) where T : UnityEngine.Object
        {
            T result = bundle.LoadAsset<T>(path);
            if (result == null)
            {
                logger.LogError($"An error has occurred trying to load asset from {path}");
            }
            logger.LogDebug($"Loaded asset located in {path}");
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
            logger.LogDebug($"Loaded asset located in {path}");
            return result;
        }

        public static Dictionary<string, string> GetInfoJSON(ref AssetBundle bundle)
        {
            if (infoJSON != null) return infoJSON;

            TextAsset infoStringAsset = bundle.LoadAsset<TextAsset>(root+"InfoStrings.json");
            if (!infoStringAsset)
            {
                logger.LogError("An error has occurred trying to load info strings from the bundle");
                return null;
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
            Dictionary<string, string> infoJSON = GetInfoJSON(ref UpgradeBus.instance.UpgradeAssets);

            if (infoJSON == null) return "";

            if (!infoJSON.ContainsKey(key))
            {
                logger.LogError("The key was not present in the info JSON file!");
                return "";
            }
            return infoJSON[key];
        }

        public static GameObject GetPerkGameObject(string upgradeName)
        {
            if (!assetPaths.ContainsKey(upgradeName))
            {
                logger.LogError($"{upgradeName} was not present in the asset dictionary!");
                return null;
            }
            return TryLoadGameObjectAsset(ref UpgradeBus.instance.UpgradeAssets, assetPaths[upgradeName]);
        }

        public static Item GetItemObject(string itemName)
        {
            if (assetPaths.ContainsKey(itemName)) return TryLoadItemAsset(ref UpgradeBus.instance.UpgradeAssets, assetPaths[itemName]);
            if (samplePaths.ContainsKey(itemName)) return TryLoadItemAsset(ref UpgradeBus.instance.UpgradeAssets, samplePaths[itemName]);

            logger.LogError($"{itemName} was not present in the asset or sample dictionary!");
            return null;
        }

        public static AudioClip GetAudioClip(string audioName)
        {
            if (!assetPaths.ContainsKey(audioName))
            {
                logger.LogError($"{audioName} was not present in the asset dictionary!");
                return null;
            }
            return TryLoadAudioClipAsset(ref UpgradeBus.instance.UpgradeAssets, assetPaths[audioName]);
        }
    }
}