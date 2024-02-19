using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    /// <summary>
    /// Component used on designed scrap items to synchronize its value to all players in the game
    /// </summary>
    internal class ScrapValueSyncer : MonoBehaviour
    {
        /// <summary>
        /// Sets the scrap value of the scrap associated with this component to be shown through the scanner and when sold to The Company<para></para>
        /// (or any other uses provided by other mods which use the scrap value of the item)
        /// </summary>
        /// <preCondition>The gameobject that contains this component must also have a component derived from GrabbableObject to change the scrap value</preCondition>
        /// <post>The scrap value of the item will be displayed through its scan node and when selling to the Company</post>
        /// <param name="scrapValue">Value to set on the scrap associated with this component</param>
        internal void SetScrapValue(int scrapValue)
        {
            GrabbableObject prop = GetComponent<GrabbableObject>();
            prop.scrapValue = scrapValue;

            LguScanNodeProperties.UpdateScrapValue(ref prop, scrapValue);
            RoundManager.Instance.totalScrapValueInLevel += scrapValue;
        }
    }
}
