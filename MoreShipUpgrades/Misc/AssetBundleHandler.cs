using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;
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
        public static Dictionary<string, string> samplePaths = new Dictionary<string, string>()
        {
            { "centipede", "Assets/ShipUpgrades/Samples/SnareFleaSample.asset" },
            { "bunker spider", "Assets/ShipUpgrades/Samples/BunkerSpiderSample.asset" },
            { "hoarding bug", "Assets/ShipUpgrades/Samples/HoardingBugSample.asset" },
            { "flowerman", "Assets/ShipUpgrades/Samples/BrackenSample.asset" },
            { "mouthdog", "Assets/ShipUpgrades/Samples/EyelessDogSample.asset" },
            { "baboon hawk", "Assets/ShipUpgrades/Samples/BaboonHawkSample.asset" },
            { "crawler", "Assets/ShipUpgrades/Samples/ThumperSample.asset" },
        };
        private static Dictionary<string, string> assetPaths = new Dictionary<string, string>()
        {
            { beekeeperScript.UPGRADE_NAME, "Assets/ShipUpgrades/beekeeper.prefab" },
            { proteinPowderScript.UPGRADE_NAME, "Assets/ShipUpgrades/ProteinPowder.prefab" },
            { biggerLungScript.UPGRADE_NAME, "Assets/ShipUpgrades/BiggerLungs.prefab" },
            { runningShoeScript.UPGRADE_NAME, "Assets/ShipUpgrades/runningShoes.prefab" },
            { strongLegsScript.UPGRADE_NAME, "Assets/ShipUpgrades/strongLegs.prefab" },
            { trapDestroyerScript.UPGRADE_NAME, "Assets/ShipUpgrades/destructiveCodes.prefab" },
            { nightVisionScript.UPGRADE_NAME, "Assets/ShipUpgrades/nightVision.prefab" },
            { terminalFlashScript.UPGRADE_NAME, "Assets/ShipUpgrades/terminalFlash.prefab" },
            { hunterScript.UPGRADE_NAME, "Assets/ShipUpgrades/Hunter.prefab" },
            { strongerScannerScript.UPGRADE_NAME, "Assets/ShipUpgrades/strongScanner.prefab" },
            { lightningRodScript.UPGRADE_NAME, "Assets/ShipUpgrades/LightningRod.prefab" },
            { walkieScript.UPGRADE_NAME, "Assets/ShipUpgrades/walkieUpgrade.prefab" },
            { exoskeletonScript.UPGRADE_NAME, "Assets/ShipUpgrades/exoskeleton.prefab" },
            { "Interns", "Assets/ShipUpgrades/Intern.prefab" },
            { pagerScript.UPGRADE_NAME, "Assets/ShipUpgrades/Pager.prefab" },
            { lockSmithScript.UPGRADE_NAME, "Assets/ShipUpgrades/LockSmith.prefab" },
            { playerHealthScript.UPGRADE_NAME, "Assets/ShipUpgrades/PlayerHealth.prefab" },
            { ExtendDeadlineScript.UPGRADE_NAME, "Assets/ShipUpgrades/ExtendDeadline.prefab" },

            { "Advanced Portable Tele", "Assets/ShipUpgrades/TpButtonAdv.asset" },
            { "Portable Tele", "Assets/ShipUpgrades/TpButton.asset" },
            { "Peeper", "Assets/ShipUpgrades/coilHead.asset" },
            { "Medkit", "Assets/ShipUpgrades/MedKitItem.asset" },
            { "Night Vision", "Assets/ShipUpgrades/NightVisionItem.asset" },
            { "Diving Kit", "Assets/ShipUpgrades/DivingKitItem.asset" },

            { "Break", "Assets/ShipUpgrades/break.mp3" },
            { "Error", "Assets/ShipUpgrades/error.mp3" },
            { "Button Press", "Assets/ShipUpgrades/ButtonPress2.ogg" },
            { "Flashbang", "Assets/ShipUpgrades/flashbangsfx.ogg" },
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

            TextAsset infoStringAsset = bundle.LoadAsset<TextAsset>("Assets/ShipUpgrades/InfoStrings.json");
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