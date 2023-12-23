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
        private static string MODULE_NAME = "Asset Bundle Handler";
        private static Dictionary<string, string> infoJSON;
        public static Dictionary<string, string> samplePaths = new Dictionary<string, string>()
        {
            { "snare flea", "Assets/ShipUpgrades/Samples/SnareFleaSample.asset" },
            { "bunker spider", "Assets/ShipUpgrades/Samples/BunkerSpiderSample.asset" },
            { "hoarding bug", "Assets/ShipUpgrades/Samples/HoardingBugSample.asset" },
            { "bracken", "Assets/ShipUpgrades/Samples/BrackenSample.asset" },
            { "eyeless dog", "Assets/ShipUpgrades/Samples/EyelessDogSample.asset" },
            { "baboon hawk", "Assets/ShipUpgrades/Samples/BaboonHawkSample.asset" },
            { "thumper", "Assets/ShipUpgrades/Samples/ThumperSample.asset" },
        };
        private static Dictionary<string, string> assetPaths = new Dictionary<string, string>()
        {
            { "Beekeeper", "Assets/ShipUpgrades/beekeeper.prefab" },
            { "Protein Powder", "Assets/ShipUpgrades/ProteinPowder.prefab" },
            { "Bigger Lungs", "Assets/ShipUpgrades/BiggerLungs.prefab" },
            { "Running Shoes", "Assets/ShipUpgrades/runningShoes.prefab" },
            { "Strong Legs", "Assets/ShipUpgrades/strongLegs.prefab" },
            { "Malware Broadcaster", "Assets/ShipUpgrades/destructiveCodes.prefab" },
            { "Light Footed", "Assets/ShipUpgrades/lightFooted.prefab" },
            { "NV Headset Batteries", "Assets/ShipUpgrades/nightVision.prefab" },
            { "Discombobulator", "Assets/ShipUpgrades/terminalFlash.prefab" },
            { "Hunter", "Assets/ShipUpgrades/Hunter.prefab" },
            { "Better Scanner", "Assets/ShipUpgrades/strongScanner.prefab" },
            { lightningRodScript.UPGRADE_NAME, "Assets/ShipUpgrades/LightningRod.prefab" },
            { "Walkie GPS", "Assets/ShipUpgrades/walkieUpgrade.prefab" },
            { "Back Muscles", "Assets/ShipUpgrades/exoskeleton.prefab" },
            { "Interns", "Assets/ShipUpgrades/Intern.prefab" },
            { "Fast Encryption", "Assets/ShipUpgrades/Pager.prefab" },
            { "Locksmith", "Assets/ShipUpgrades/LockSmith.prefab" },
            { playerHealthScript.UPGRADE_NAME, "Assets/ShipUpgrades/PlayerHealth.prefab" },

            { "Advanced Portable Tele", "Assets/ShipUpgrades/TpButtonAdv.asset" },
            { "Portable Tele", "Assets/ShipUpgrades/TpButton.asset" },
            { "Peeper", "Assets/ShipUpgrades/coilHead.asset" },
            { "Medkit", "Assets/ShipUpgrades/MedKitItem.asset" },
            { "Night Vision", "Assets/ShipUpgrades/NightVisionItem.asset" },
            { "Medkit", "Assets/ShipUpgrades/MedKitItem.asset" },

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
                Plugin.mls.LogError(string.Format("[{0}] An error has occurred trying to load asset from {1}\n", MODULE_NAME, path));
            }
            Plugin.mls.LogInfo(string.Format("[{0}] Loaded asset located in {1}", MODULE_NAME, path));
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
                Plugin.mls.LogError(string.Format("[{0}] An error has occurred trying to load asset from {1}\n", MODULE_NAME, path));
            }
            Plugin.mls.LogInfo(string.Format("[{0}] Loaded asset located in {1}", MODULE_NAME, path));
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
                Plugin.mls.LogError(string.Format("[{0}] An error has occurred trying to load asset from {1}\n", MODULE_NAME, path));
            }
            Plugin.mls.LogInfo(string.Format("[{0}] Loaded asset located in {1}", MODULE_NAME, path));
            return result;
        }

        public static Dictionary<string, string> GetInfoJSON(ref AssetBundle bundle) 
        {
            if (infoJSON != null) return infoJSON;

            TextAsset infoStringAsset = bundle.LoadAsset<TextAsset>("Assets/ShipUpgrades/InfoStrings.json");
            if (!infoStringAsset)
            {
                Plugin.mls.LogError(string.Format("[{0}] An error has occurred trying to load info strings from the bundle\n", MODULE_NAME));
                return null;
            }

            infoJSON = JsonConvert.DeserializeObject<Dictionary<string, string>>(infoStringAsset.text);
            if (infoJSON == null)
            {
                Plugin.mls.LogError(string.Format("[{0}] An error has occurred trying to deserialize info strings into a dictionary\n", MODULE_NAME));
            }
            return infoJSON;
        }

        public static string GetInfoFromJSON(string key)
        {
            Dictionary<string, string> infoJSON = GetInfoJSON(ref UpgradeBus.instance.UpgradeAssets);

            if (infoJSON == null) return "";

            if (!infoJSON.ContainsKey(key))
            {
                Plugin.mls.LogError(string.Format("[{0}] The key was not present in the info JSON file!\n", MODULE_NAME));
                return "";
            }
            return infoJSON[key];
        }

        public static GameObject GetPerkGameObject(string upgradeName)
        {
            if (!assetPaths.ContainsKey(upgradeName)) 
            {
                Plugin.mls.LogError(string.Format("[{0}] {1} was not present in the asset dictionary!\n", MODULE_NAME, upgradeName));
                return null;
            }
            return TryLoadGameObjectAsset(ref UpgradeBus.instance.UpgradeAssets, assetPaths[upgradeName]);
        }

        public static Item GetItemObject(string itemName)
        {
            if (assetPaths.ContainsKey(itemName)) return TryLoadItemAsset(ref UpgradeBus.instance.UpgradeAssets, assetPaths[itemName]);
            if (samplePaths.ContainsKey(itemName)) return TryLoadItemAsset(ref UpgradeBus.instance.UpgradeAssets, samplePaths[itemName]);

            Plugin.mls.LogError(string.Format("[{0}] {1} was not present in the asset or sample dictionary!\n", MODULE_NAME, itemName));
            return null;
        }

        public static AudioClip GetAudioClip(string audioName)
        {
            if (!assetPaths.ContainsKey(audioName))
            {
                Plugin.mls.LogError(string.Format("[{0}] {1} was not present in the asset dictionary!\n", MODULE_NAME, audioName));
                return null;
            }
            return TryLoadAudioClipAsset(ref UpgradeBus.instance.UpgradeAssets, assetPaths[audioName]);
        }
    }
}
