using MoreShipUpgrades.Managers;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    public class BaseUpgrade : NetworkBehaviour
    {
        protected string upgradeName = "Base Upgrade";

        public static string INDIVIDUAL_SECTION = "Individual Purchase";
        public static bool INDIVIDUAL_DEFAULT = true;
        public static string INDIVIDUAL_DESCRIPTION = "If true: upgrade will apply only to the client that purchased it. (Overriden by 'Convert all upgrades to be shared' option in Misc section)";

        public static string PRICES_SECTION = "Price of each additional upgrade";
        public static string PRICES_DESCRIPTION = "Value must be seperated by commas EX: '123,321,222'";

        public virtual void Increment()
        {

        }

        public virtual void load()
        {
            string loadColour = "#FF0000";
            string loadMessage = $"\n<color={loadColour}>{upgradeName} is active!</color>";
            HUDManager.Instance.chatText.text += loadMessage;
        }

        public virtual void Register()
        {
            if (!UpgradeBus.instance.UpgradeObjects.ContainsKey(upgradeName)) { UpgradeBus.instance.UpgradeObjects.Add(upgradeName, gameObject); }
        }

        public virtual void Unwind()
        {
            string unloadColour = "#FF0000";
            string unloadMessage = $"\n<color={unloadColour}>{upgradeName} has been disabled!</color>";
            HUDManager.Instance.chatText.text += unloadMessage;
        }
    }
}
