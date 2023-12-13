using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    public class CustomTerminalNode
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
            if(prices == null) {  prices = new int[0]; }
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
                Name = this.Name,
                UnlockPrice = this.UnlockPrice,
                Description = this.Description,
                Prefab = this.Prefab,
                Prices = this.Prices,
                MaxUpgrade = this.MaxUpgrade
            );
        }
    }
}
