using System;
using UnityEngine;

namespace MoreShipUpgrades.Misc.TerminalNodes
{
    /// <summary>
    /// Terminal node used to display the lgu upgrade store
    /// </summary>
    public abstract class CustomTerminalNode : IComparable
    {
        public string Name {get; set;}
        public int[] Prices { get; set; }
        public int UnlockPrice { get; set; }
        public string SimplifiedDescription { get; set; }
        public string Description { get; set; }
        public GameObject Prefab { get; set; }
        public bool Unlocked { get; set; } = false;
        public int MaxUpgrade { get; set; }
        public int CurrentUpgrade { get; set; }

        public float salePerc { get; set; } = 1f;


        public CustomTerminalNode(string name, int unlockPrice, string description, GameObject prefab, int[] prices = null, int maxUpgrade = 0, string simplifiedDescription = "")
        {
            if (prices == null) { prices = new int[0]; }
            Name = name;
            Prices = prices;
            SimplifiedDescription = simplifiedDescription;
            Description = description;
            Prefab = prefab;
            MaxUpgrade = maxUpgrade;
            UnlockPrice = unlockPrice;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return -1;
            if (obj is not CustomTerminalNode) return -1;
            CustomTerminalNode other = obj as CustomTerminalNode;
            return Name.CompareTo(other.Name);
        }

        internal abstract string GetTerminalNodeText();
    }
}
