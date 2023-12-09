using GameNetcodeStuff;
using JetBrains.Annotations;
using MoreShipUpgrades.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class NightVisionItemScript : GrabbableObject
    {
        public override void DiscardItem()
        {
            this.playerHeldBy.activatingItem = false;
            base.DiscardItem();
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (Mouse.current.leftButton.isPressed)
            {
                if (UpgradeBus.instance.nightVision) {
                    HUDManager.Instance.chatText.text += "<color=#FF0000>Night vision is already active!</color>";
                    return;
                }
                UpgradeBus.instance.UpgradeObjects["Night Vision"].GetComponent<nightVisionScript>().EnableOnClient();
                playerHeldBy.DespawnHeldObject();
            }
        }
    }
}
