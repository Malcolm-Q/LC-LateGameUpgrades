using MoreShipUpgrades.Managers;
using System.Collections;
using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    internal class ScrapValueSyncer : MonoBehaviour
    {
        int scrapValue = -1;
        void Start()
        {
            StartCoroutine(waitTilDefinedScrapValue());
        }
        internal void SetScrapValue(int scrapValue)
        {
            this.scrapValue = scrapValue;
        }
        bool IsScrapValueNotSet()
        {
            return scrapValue < 0;
        }
        IEnumerator waitTilDefinedScrapValue()
        {
            yield return new WaitWhile(IsScrapValueNotSet); // This should not get stuck if we always set the value right after instantianting the gameObject
            
            GrabbableObject prop = GetComponent<GrabbableObject>();
            if (prop.scrapValue > 0) yield break; // Already defined
            prop.scrapValue = scrapValue;

            ScanNodeProperties node = GetComponentInChildren<ScanNodeProperties>();
            node.scrapValue = scrapValue;
            node.subText = $"VALUE: ${node.scrapValue}";
            RoundManager.Instance.totalScrapValueInLevel += node.scrapValue;
        }
    }
}
