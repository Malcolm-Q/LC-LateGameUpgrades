using MoreShipUpgrades.Managers;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Misc.Upgrades
{
    abstract class BaseUpgrade : NetworkBehaviour
    {
        protected string upgradeName = "Base Upgrade";

        public static string INDIVIDUAL_SECTION = "Individual Purchase";
        public static bool INDIVIDUAL_DEFAULT = true;
        public static string INDIVIDUAL_DESCRIPTION = "If true: upgrade will apply only to the client that purchased it. (Overriden by 'Convert all upgrades to be shared' option in Misc section)";

        public static string PRICES_SECTION = "Price of each additional upgrade";
        public static string PRICES_DESCRIPTION = "Value must be seperated by commas EX: '123,321,222'";

        public abstract void Load();

        public abstract void Register();

        public abstract void Unwind();
    }
}
