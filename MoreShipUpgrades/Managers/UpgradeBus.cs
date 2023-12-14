using GameNetcodeStuff;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    public class UpgradeBus : NetworkBehaviour
    {
        public static UpgradeBus instance;
        public PluginConfig cfg;
        public GameObject introScreen;

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
        public bool proteinPowder = false;

        public int lungLevel = 0;
        public int proteinLevel = 0;
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
        public defibScript internScript = null;
        public walkieScript walkieHandler = null;

        public Color nightVisColor;
        public AudioClip flashNoise;
        public GameObject modStorePrefab;
        public TerminalNode modStoreInterface;

        public List<CustomTerminalNode> terminalNodes = new List<CustomTerminalNode>();

        public Dictionary<string, GameObject> UpgradeObjects = new Dictionary<string, GameObject>();

        public Dictionary<ulong,float> beePercs = new Dictionary<ulong,float>();
        public Dictionary<ulong,int> forceMults = new Dictionary<ulong,int>();

        public Dictionary<string,bool> IndividualUpgrades = new Dictionary<string,bool>();
        public string[] internNames, internInterests;

        public List<coilHeadItem> coilHeadItems = new List<coilHeadItem>();
        internal bool walkies;
        internal bool walkieUIActive;
        internal string version;

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
                string saleStatus = terminalNode.salePerc < 1f ? $"- {((1-terminalNode.salePerc)*100).ToString("F0")}% OFF!" : "";
                if (!terminalNode.Unlocked) { modStoreInterface.displayText += $"\n{terminalNode.Name} // ${(int)(terminalNode.UnlockPrice * terminalNode.salePerc)} {saleStatus}  "; }
                else if (terminalNode.MaxUpgrade == 0) { modStoreInterface.displayText += $"\n{terminalNode.Name} // UNLOCKED  "; }
                else if (terminalNode.MaxUpgrade > terminalNode.CurrentUpgrade) { modStoreInterface.displayText += $"\n{terminalNode.Name} // ${(int)(terminalNode.Prices[terminalNode.CurrentUpgrade]*terminalNode.salePerc)} // LVL {terminalNode.CurrentUpgrade + 1} {saleStatus}  "; }
                else { modStoreInterface.displayText += $"\n{terminalNode.Name} // MAX LVL"; }
            }
            if (modStoreInterface.displayText == "")
            {
                modStoreInterface.displayText = "No upgrades available";
            }
            modStoreInterface.displayText += "\n\n";
            return modStoreInterface;
        }

        public void ResetAllValues(bool wipeObjRefs = true)
        {
            DestroyTraps = false;
            softSteps = false;
            scannerUpgrade = false;
            nightVision = false;
            walkies = false;
            nightVisionActive = false;
            exoskeleton = false;
            TPButtonPressed = false;
            beekeeper = false;
            terminalFlash = false;
            strongLegs = false;
            runningShoes = false;
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
            if (wipeObjRefs) { UpgradeObjects = new Dictionary<string, GameObject>(); }
            foreach(CustomTerminalNode node in terminalNodes)
            {
                node.Unlocked = false;
                node.CurrentUpgrade = 0;
                if(node.Name == "NV Headset Batteries") { node.Unlocked = true; }
            }
            PlayerControllerB[] players = GameObject.FindObjectsOfType<PlayerControllerB>();
            foreach (PlayerControllerB player in players)
            {
                player.movementSpeed = 4.6f;
                player.sprintTime = 11;
                player.jumpForce = 13;
            }
        }

        internal void GenerateSales()
        {
            foreach(CustomTerminalNode node in terminalNodes)
            {
                if(node.Name == "Interns") { continue; }
                if(Random.value > UpgradeBus.instance.cfg.SALE_PERC)
                {
                    node.salePerc = Random.Range(0.60f, 0.90f);
                }
                else
                {
                    node.salePerc = 1f;
                }
            }
        }
    }
}