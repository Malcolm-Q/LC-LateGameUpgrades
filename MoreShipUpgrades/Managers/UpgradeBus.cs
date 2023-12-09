using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    public class UpgradeBus : NetworkBehaviour
    {
        public static UpgradeBus instance;
        public PluginConfig cfg;

        public bool DestroyTraps = false;
        public bool softSteps = false;
        public bool scannerUpgrade = false;
        public bool nightVision = false;
        public bool nightVisionActive = false;
        public float nightVisRange;
        public float nightVisIntensity;
        public bool exoskeleton = false;
        public bool TPButtonPressed = false;
        public bool beekeeper = false;
        public bool terminalFlash = false;
        public bool strongLegs = false;
        public bool runningShoes = false;
        public bool lockSmith = false;
        public bool biggerLungs = false;
        public bool pager = false;

        public int lungLevel = 0;
        public int beeLevel = 0;
        public int backLevel = 0;
        public int runningLevel = 0;
        public int lightLevel = 0;
        public int discoLevel = 0;
        public int legLevel = 0;
        public int nightVisionLevel = 0;

        public float flashCooldown = 0f;
        public float alteredWeight = 1f;

        public trapDestroyerScript trapHandler = null;
        public terminalFlashScript flashScript = null;
        public lockSmithScript lockScript = null;
        public pagerScript pageScript = null;

        public Color nightVisColor;
        public AudioClip flashNoise;
        public GameObject modStorePrefab;
        public TerminalNode modStoreInterface;

        public List<CustomTerminalNode> terminalNodes = new List<CustomTerminalNode>();
        public List<CustomTerminalNode> terminalNodesOriginal = new List<CustomTerminalNode>();

        public Dictionary<string, GameObject> UpgradeObjects = new Dictionary<string, GameObject>();

        public Dictionary<ulong,float> beePercs = new Dictionary<ulong,float>();

        void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            cfg = Plugin.cfg;
        }

        public TerminalNode ConstructNode()
        {
            modStoreInterface = new TerminalNode();
            modStoreInterface.clearPreviousText = true;
            foreach (CustomTerminalNode terminalNode in terminalNodes)
            {
                if (!terminalNode.Unlocked) { modStoreInterface.displayText += $"\n{terminalNode.Name} // {terminalNode.Price}  "; }
                else if (terminalNode.MaxUpgrade == 0) { modStoreInterface.displayText += $"\n{terminalNode.Name} // UNLOCKED  "; }
                else if (terminalNode.MaxUpgrade > terminalNode.CurrentUpgrade) { modStoreInterface.displayText += $"\n{terminalNode.Name} // {terminalNode.Price} // LVL {terminalNode.CurrentUpgrade + 1}"; }
                else { modStoreInterface.displayText += $"\n{terminalNode.Name} // MAX LVL"; }
            }
            if (modStoreInterface.displayText == "")
            {
                modStoreInterface.displayText = "No upgrades available";
            }
            modStoreInterface.displayText += "\n\n";
            return modStoreInterface;
        }

        public void ResetAllValues()
        {
            DestroyTraps = false;
            softSteps = false;
            scannerUpgrade = false;
            nightVision = false;
            nightVisionActive = false;
            exoskeleton = false;
            TPButtonPressed = false;
            beekeeper = false;
            terminalFlash = false;
            strongLegs = false;
            runningShoes = false;
            pager = false;
            lockSmith = false;
            biggerLungs = false;
            lungLevel = 0;
            backLevel = 0;
            beeLevel = 0;
            runningLevel = 0;
            lightLevel = 0;
            discoLevel = 0;
            legLevel = 0;
            nightVisionLevel = 0;
            flashCooldown = 0f;
            alteredWeight = 1f;
            trapHandler = null;
            flashScript = null;
            UpgradeObjects = new Dictionary<string, GameObject>();
            terminalNodes = terminalNodesOriginal;
        }

        public void CreateDeepNodeCopy()
        {
            terminalNodesOriginal = terminalNodes.Select(node => node.Copy()).ToList();
        }
    }
}