using GameNetcodeStuff;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class biggerLungScript : NetworkBehaviour
    {
        private PlayerControllerB[] players;
        void Start()
        {
            players = GameObject.FindObjectsOfType<PlayerControllerB>();
            foreach (PlayerControllerB player in players)
            {
                player.sprintTime = 17f;
            }
            transform.parent = GameObject.Find("HangarShip").transform;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Bigger Lungs is active!";
        }
    }
}
