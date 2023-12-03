using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    public class CustomTerminalNode
    {
        public string Name;
        public int Price;
        public string Description;
        public GameObject Prefab;
        public bool Unlocked = false;
        public int MaxUpgrade;
        public int CurrentUpgrade { get; set; }


        public CustomTerminalNode(string name, int price, string description, GameObject prefab, int maxUpgrade = 0)
        {
            Name = name;
            Price = price;
            Description = description;
            Prefab = prefab;
            MaxUpgrade = maxUpgrade;
        }

        public CustomTerminalNode Copy()
        {
            return new CustomTerminalNode
            (
                Name = this.Name,
                Price = this.Price,
                Description = this.Description,
                Prefab = this.Prefab,
                MaxUpgrade = this.MaxUpgrade
            );
        }
    }
}
