using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    public class UpgradeBus : NetworkBehaviour
    {
        public static UpgradeBus instance;
        public bool DestroyTraps = false;
        public bool softSteps = false;
        public bool scannerUpgrade = false;
        public bool nightVision = false;
        public bool nightVisionActive = false;
        public Color nightVisColor;
        public float nightVisRange;
        public float nightVisIntensity;
        public bool exoskeleton = false;
        public bool TPButtonPressed = false;
        public bool beekeeper = false;
        public int beeLevel = 0;
        public bool terminalFlash = false;
        public float flashCooldown = 0f;
        public bool strongLegs = false;
        public bool runningShoes = false;
        public bool lockSmith = false;
        public bool biggerLungs = false;
        public bool pager = false;
        public int lungLevel = 0;
        public int backLevel = 0;
        public int runningLevel = 0;
        public int lightLevel = 0;
        public int discoLevel = 0;
        //public int scanLevel = 0; problematic, balance / refactor and then introduce tiered upgrades.
        public int legLevel = 0;
        public float alteredWeight = 1f;
        public AudioClip flashNoise;
        public trapDestroyerScript trapHandler = null;
        public terminalFlashScript flashScript = null;
        public lockSmithScript lockScript = null;
        public pagerScript pageScript = null;
        public PluginConfig cfg;
        public GameObject modStorePrefab;
        public TerminalNode modStoreInterface;
        public List<CustomTerminalNode> terminalNodes = new List<CustomTerminalNode>();
        public List<CustomTerminalNode> terminalNodesOriginal = new List<CustomTerminalNode>();
        public Dictionary<string, GameObject> UpgradeObjects = new Dictionary<string, GameObject>();
        
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
                else { modStoreInterface.displayText += $"\n{terminalNode.Name} // MAX LVL";  }
            }
            if(modStoreInterface.displayText == "")
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
            beeLevel = 0;
            terminalFlash = false;
            flashCooldown = 0f;
            strongLegs = false;
            runningShoes = false;
            biggerLungs = false;
            lungLevel = 0;
            backLevel = 0;
            runningLevel = 0;
            lightLevel = 0;
            discoLevel = 0;
            legLevel = 0;
            alteredWeight = 1f;
            trapHandler = null;
            flashScript = null;
            UpgradeObjects = new Dictionary<string, GameObject>();
            terminalNodes = terminalNodesOriginal;

            try { LGUStore.instance.DeleteUpgradesServerRpc(); }
            catch (Exception ex)
            { 
                BaseUpgrade[] upgradeObjects = GameObject.FindObjectsOfType<BaseUpgrade>();
                foreach(BaseUpgrade upgrade in upgradeObjects)
                {
                    Destroy(upgrade.gameObject);
                }
            }
        }

        public void CreateDeepNodeCopy()
        {
            terminalNodesOriginal = terminalNodes.Select(node => node.Copy()).ToList();
        }
    }
}
