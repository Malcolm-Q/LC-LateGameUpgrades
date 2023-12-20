using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    internal class AssetBundleHandler
    {
        private static string MODULE_NAME = "Asset Bundle Handler";
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
        /// <returns>The asset's gameObject if it's present in the asset bundle, otherwise null</returns>
        public static AudioClip TryLoadAudioClipAsset(ref AssetBundle bundle, string path)
        {
            AudioClip result = bundle.LoadAsset<AudioClip>(path);
            if (result == null)
            {
                Plugin.mls.LogError(string.Format("[{0}] An error has occurred trying to load asset from {1}\n", MODULE_NAME, path));
            }
            return result;
        }
    }
}
