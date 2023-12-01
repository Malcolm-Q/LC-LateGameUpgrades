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
        public int CurrentUpgrade;


        public CustomTerminalNode(string name, int price, string description, GameObject prefab, int maxUpgrade = 0)
        {
            Name = name;
            Price = price;
            Description = description;
            Prefab = prefab;
            MaxUpgrade = maxUpgrade;
        }
    }
}
