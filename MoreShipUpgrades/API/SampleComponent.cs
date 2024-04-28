using MoreShipUpgrades.Misc;
using System;
using System.Collections.Generic;
using System.Text;
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
            GetComponent<ScrapValueSyncer>().SetScrapValue(random.Next(minValue: itemProperties.minValue, maxValue: itemProperties.maxValue));
        }
    }
}
