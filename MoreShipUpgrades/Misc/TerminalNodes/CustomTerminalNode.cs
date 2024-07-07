using MoreShipUpgrades.Managers;
using System;
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
        public bool Unlocked { get; set; }
        public int MaxUpgrade { get; set; }
        public int CurrentUpgrade { get; set; }
        public bool SharedUpgrade { get; set; }
        public float SalePercentage { get; set; } = 1f;
        public bool Visible {  get; set; }
        protected CustomTerminalNode(string name, int unlockPrice, string description, GameObject prefab, int[] prices = null, int maxUpgrade = 0, string originalName = "", bool sharedUpgrade = false)
        {
            if (prices == null) { prices = []; }
            Name = name;
            Prices = prices;
            Description = description;
            Prefab = prefab;
            MaxUpgrade = maxUpgrade;
            UnlockPrice = unlockPrice;
            OriginalName = originalName;
            SharedUpgrade = sharedUpgrade;
            Visible = true;
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
            if (!Unlocked) return (int)((UnlockPrice - ItemProgressionManager.GetCurrentContribution(this)) * SalePercentage);
            if (CurrentUpgrade >= MaxUpgrade) return int.MaxValue;
            return (int)((Prices[CurrentUpgrade] - ItemProgressionManager.GetCurrentContribution(this)) * SalePercentage);
        }

        public int GetCurrentLevel()
        {
            if (!Unlocked) return 0;
            return Mathf.Clamp(CurrentUpgrade, 0, MaxUpgrade) + 1;
        }

        public int GetRemainingLevels()
        {
            int remainingLevels = Unlocked ? 0 : 1;
            remainingLevels += MaxUpgrade != 0 ? Mathf.Max(0, MaxUpgrade - CurrentUpgrade) : 0;
            return remainingLevels;
        }

        public void ToggleVisibility(bool visible)
        {
            Visible = visible;
        }

        public bool IsVisible() 
        {
            return Visible;
        }
    }
}
