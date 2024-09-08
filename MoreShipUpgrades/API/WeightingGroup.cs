using System;
using System.Collections.Generic;
using System.Linq;

namespace MoreShipUpgrades.API
{
    internal class WeightingGroup<T> : Dictionary<T, double> where T : class
    {
        public T GetItem()
        {
            double totalWeight = 0f;
            foreach (double item in Values)
            {
                double absWeight = Math.Abs(item);
                totalWeight += absWeight;
            }
            if (totalWeight <= 0f)
                return null;
            double roll = new Random().NextDouble();
            double total = 0;

            foreach (KeyValuePair<T, double> pair in this.OrderByDescending(v => v.Value))
            {
                total += pair.Value / totalWeight;
                if (roll <= total)
                    return pair.Key;
            }
            return Keys.FirstOrDefault();
        }
    }
}
