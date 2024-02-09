using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    internal class ScrapValueSyncer : MonoBehaviour
    {
        internal void SetScrapValue(int scrapValue)
        {
            GrabbableObject prop = GetComponent<GrabbableObject>();
            prop.scrapValue = scrapValue;

            LGUScanNodeProperties.UpdateScrapValue(ref prop, scrapValue);
            RoundManager.Instance.totalScrapValueInLevel += scrapValue;
        }
    }
}
