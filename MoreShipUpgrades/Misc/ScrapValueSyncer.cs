using MoreShipUpgrades.Managers;
using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    /*
     * This should be set up to be more modular in the future with an itemName dictionary
     * but for this hotfix this will work. also nothing else needs this at the moment.
     */
    internal class ScrapValueSyncer : MonoBehaviour
    {
        void Start()
        {
            PhysicsProp prop = GetComponent<PhysicsProp>();
            prop.scrapValue = UpgradeBus.instance.cfg.CONTRACT_EXOR_REWARD;

            ScanNodeProperties node = GetComponentInChildren<ScanNodeProperties>();
            node.scrapValue = UpgradeBus.instance.cfg.CONTRACT_EXOR_REWARD;
            node.subText = $"VALUE: ${node.scrapValue}";
        }
    }
}
