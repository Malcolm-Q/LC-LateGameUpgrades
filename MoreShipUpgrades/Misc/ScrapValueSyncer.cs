using MoreShipUpgrades.Managers;
using System.Collections;
using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    internal class ScrapValueSyncer : MonoBehaviour
    {
        internal void SetScrapValue(int scrapValue)
        {
            GrabbableObject prop = GetComponent<GrabbableObject>();
            prop.scrapValue = scrapValue;

            ScanNodeProperties node = GetComponentInChildren<ScanNodeProperties>();
            node.scrapValue = scrapValue;
            node.subText = $"VALUE: ${scrapValue}";
            RoundManager.Instance.totalScrapValueInLevel += scrapValue;
        }
    }
}
