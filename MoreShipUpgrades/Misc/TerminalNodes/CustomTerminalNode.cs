using MoreShipUpgrades.Managers;
using System;
using System.Security.Policy;
using Unity.Netcode;
using UnityEngine;
using static Unity.Audio.Handle;

namespace MoreShipUpgrades.Misc.TerminalNodes
{
    /// <summary>
    /// Terminal node used to display the lgu upgrade store
    /// </summary>
    public abstract class CustomTerminalNode : IComparable
    {
        public string Name {get; set;}
        public string OriginalName { get; set;}
        public int[] Prices { get; set; }
        public int UnlockPrice { get; set; }
        public string Description { get; set; }
        public GameObject Prefab { get; set; }
        public bool Unlocked { get; set; } = false;
        public int MaxUpgrade { get; set; }
        public int CurrentUpgrade { get; set; }
        public bool SharedUpgrade { get; set; }
        public float salePerc { get; set; } = 1f;


        protected CustomTerminalNode(string name, int unlockPrice, string description, GameObject prefab, int[] prices = null, int maxUpgrade = 0, string originalName = "", bool sharedUpgrade = false)
        {
            if (prices == null) { prices = new int[0]; }
            Name = name;
            Prices = prices;
            Description = description;
            Prefab = prefab;
            MaxUpgrade = maxUpgrade;
            UnlockPrice = unlockPrice;
            OriginalName = originalName;
            SharedUpgrade = sharedUpgrade;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return -1;
            if (obj is not CustomTerminalNode) return -1;
            CustomTerminalNode other = obj as CustomTerminalNode;
            return Name.CompareTo(other.Name);
        }

        public int GetCurrentPrice()
        {
            if (!Unlocked) return (int)((UnlockPrice - ItemProgressionManager.GetCurrentContribution(this)) * salePerc);
            if (CurrentUpgrade >= MaxUpgrade) return int.MaxValue;
            return (int)((Prices[CurrentUpgrade] - ItemProgressionManager.GetCurrentContribution(this)) * salePerc);
        }
    }
}
