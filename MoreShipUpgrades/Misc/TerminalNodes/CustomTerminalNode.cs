using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.Misc.TerminalNodes
{
    /// <summary>
    /// Terminal node used to display the lgu upgrade store
    /// </summary>
    public class CustomTerminalNode : IComparable
    {
        public string Name;
        public int[] Prices;
        public int UnlockPrice;
        public string Description;
        public GameObject Prefab;
        public bool Unlocked = false;
        public int MaxUpgrade;
        public int CurrentUpgrade { get; set; }

        public float salePerc = 1f;


        public CustomTerminalNode(string name, int unlockPrice, string description, GameObject prefab, int[] prices = null, int maxUpgrade = 0)
        {
            if (prices == null) { prices = new int[0]; }
            Name = name;
            Prices = prices;
            Description = description;
            Prefab = prefab;
            MaxUpgrade = maxUpgrade;
            UnlockPrice = unlockPrice;
        }

        public CustomTerminalNode Copy()
        {
            return new CustomTerminalNode
            (
                Name,
                UnlockPrice,
                Description,
                Prefab,
                Prices,
                MaxUpgrade
            );
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return -1;
            if (obj is not CustomTerminalNode) return -1;
            CustomTerminalNode other = obj as CustomTerminalNode;
            return Name.CompareTo(other.Name);
        }
    }
}
