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
        public bool terminalFlash = false;
        public float flashCooldown = 0f;
        public bool strongLegs = false;
        public bool runningShoes = false;
        public bool biggerLungs = false;
        public float alteredWeight = 1f;
        public AudioClip flashNoise;
        public trapDestroyerScript trapHandler = null;
        
        void Awake()
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
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
            flashCooldown = 0f;
            strongLegs = false;
            runningShoes = false;
            biggerLungs = false;
            trapHandler = null;
            alteredWeight = 1f;
        }
    }
}
