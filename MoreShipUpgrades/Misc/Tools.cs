using System;
using System.Collections.Generic;

namespace MoreShipUpgrades.Misc
{
    internal class Tools
    {
        public static void ShuffleList<T>(List<T> list)
        {
            if(list == null) throw new ArgumentNullException("list");

            Random random = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
