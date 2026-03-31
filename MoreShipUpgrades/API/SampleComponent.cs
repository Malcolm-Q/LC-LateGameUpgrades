using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship;
using UnityEngine;

namespace MoreShipUpgrades.API
{
    internal class SampleComponent : MonoBehaviour
    {
        private static int usedMapSeed = -1;
        private static System.Random random = null;
        public void Start()
        {
            if (usedMapSeed < 0 || random == null || usedMapSeed != StartOfRound.Instance.randomMapSeed)
            {
                usedMapSeed = StartOfRound.Instance.randomMapSeed;
                random = new System.Random(usedMapSeed + 105);
            }
            Item itemProperties = GetComponent<GrabbableObject>().itemProperties;
            int value = random.Next(minValue: itemProperties.minValue, maxValue: itemProperties.maxValue);
            if (UpgradeBus.Instance.PluginConfiguration.AffectSamples)
			{
                Plugin.mls.LogDebug($"Midas Touch affecting samples is enabled, increasing the original scrap value ({value})...");
				value = MidasTouch.IncreaseScrapValueInteger(value);
                Plugin.mls.LogDebug($"Set the sample scrap value to {value}...");
            }
            GetComponent<ScrapValueSyncer>().SetScrapValue(value);
        }
    }
}
