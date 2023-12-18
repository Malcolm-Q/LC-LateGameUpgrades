using System;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;

namespace MoreShipUpgrades.Misc
{
    public class BaseUpgrade : NetworkBehaviour
    {
        public static string PRICES_CONFIGURATION = "Price of each additional upgrade";

        public static string INDIVIDUAL_CONFIGURATION = "Individual Purchase";
        public static string INDIVIDUAL_DESCRIPTION = "If true: upgrade will apply only to the client that purchased it.";

        public virtual void Increment()
        {

        }

        public virtual void load()
        {

        }

        public virtual void Register()
        {

        }

        public virtual void Unwind()
        {

        }
    }
}
