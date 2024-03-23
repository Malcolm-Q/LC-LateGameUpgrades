using System;
using System.Collections.Generic;

namespace MoreShipUpgrades.Compat
{
    [Serializable]
    public class SaveInfoV1
    {
        public bool DestroyTraps;
        public bool scannerUpgrade;
        public bool nightVision;
        public bool exoskeleton;
        public bool TPButtonPressed;
        public bool beekeeper;
        public bool terminalFlash;
        public bool strongLegs;
        public bool proteinPowder;
        public bool runningShoes;
        public bool biggerLungs;
        public bool lockSmith;
        public bool walkies;
        public bool lightningRod;
        public bool pager;
        public bool hunter;
        public bool playerHealth;
        public bool wearingHelmet;
        public bool sickBeats;
        public bool doorsHydraulicsBattery;

        public int beeLevel;
        public int huntLevel;
        public int proteinLevel;
        public int lungLevel;
        public int backLevel;
        public int runningLevel;
        public int scanLevel;
        public int discoLevel;
        public int legLevel;
        public int nightVisionLevel;
        public int playerHealthLevel;
        public int doorsHydraulicsBatteryLevel;
        public string contractType;
        public string contractLevel;
        public Dictionary<string, float> SaleData;
    }

    [Serializable]
    public class LGUSaveV1
    {
        public Dictionary<ulong, SaveInfoV1> playerSaves = [];
    }
}
