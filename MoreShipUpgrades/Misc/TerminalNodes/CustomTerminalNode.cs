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
        /// <summary>
        /// The name shown in the Lategame Upgrades store
        /// </summary>
        public string Name {get; set;}
        /// <summary>
        /// The original name of the upgrade
        /// </summary>
        public string OriginalName { get; set;}
        /// <summary>
        /// List of prices for levelling up the upgrade
        /// </summary>
        public int[] Prices { get; set; }
        /// <summary>
        /// Price to unlock the upgrade for its initial effect
        /// </summary>
        public int UnlockPrice { get; set; }
        /// <summary>
        /// Description of the upgrade of what it does
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Associated network prefab that is used as manager for the upgrade
        /// </summary>
        public GameObject Prefab { get; set; }
        /// <summary>
        /// Wether it has been purchased the unlock effect of the upgrade
        /// </summary>
        public bool Unlocked { get; set; }
        /// <summary>
        /// The maximum amount of levels the upgrade can reach
        /// </summary>
        public int MaxUpgrade { get; set; }
        /// <summary>
        /// The current amount of levels the upgrade has been purchased on
        /// </summary>
        public int CurrentUpgrade { get; set; }
        /// <summary>
        /// If it's shared between all clients or only to who bought the upgrade
        /// </summary>
        public bool SharedUpgrade { get; set; }
        /// <summary>
        /// Percentage used as discount when purchasing the upgrade
        /// </summary>
        public float SalePercentage { get; set; } = 1f;
        /// <summary>
        /// Wether it should be shown in the Lategame Upgrades store or not
        /// </summary>
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
        /// <summary>
        /// Gets the price associated to purchase the next level of the upgrade
        /// </summary>
        /// <returns>Price associated with the next level of the upgrade</returns>
        public int GetCurrentPrice()
        {
            if (!Unlocked) return (int)((UnlockPrice - ItemProgressionManager.GetCurrentContribution(this)) * SalePercentage);
            if (CurrentUpgrade >= MaxUpgrade) return int.MaxValue;
            return (int)((Prices[CurrentUpgrade] - ItemProgressionManager.GetCurrentContribution(this)) * SalePercentage);
        }
        /// <summary>
        /// Gets the current level of the associated upgrade
        /// </summary>
        /// <returns>Current level of the associated upgrade</returns>
        public int GetCurrentLevel()
        {
            if (!Unlocked) return 0;
            return Mathf.Clamp(CurrentUpgrade, 0, MaxUpgrade) + 1;
        }
        /// <summary>
        /// Gets the amount of levels that are left to purchase for the associated upgrade
        /// </summary>
        /// <returns>Remaining levels of the upgrade to max it out</returns>
        public int GetRemainingLevels()
        {
            int remainingLevels = Unlocked ? 0 : 1;
            remainingLevels += MaxUpgrade != 0 ? Mathf.Max(0, MaxUpgrade - CurrentUpgrade) : 0;
            return remainingLevels;
        }
    }
}
