using MoreShipUpgrades.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class MedkitScript : GrabbableObject
    {
        public AudioClip error, use;
        private AudioSource audio;
        private int uses = 0;

        public override void Start()
        {
            audio = GetComponent<AudioSource>();
            base.Start();
        }
        public override void DiscardItem()
        {
            playerHeldBy.activatingItem = false;
            base.DiscardItem();
        }

        public override void ItemActivate(bool used, bool buttonDown = true)
        {
            base.ItemActivate(used, buttonDown);
            if (itemUsedUp)
            {
                HUDManager.Instance.DisplayTip("NO MORE USES!", "This medkit doesn't have anymore supplies!", true, false, "LC_Tip1");
                return;
            }
            if (Mouse.current.leftButton.isPressed)
            {
                int health = UpgradeBus.instance.playerHPs.ContainsKey(playerHeldBy.playerSteamId) ? UpgradeBus.instance.playerHPs[playerHeldBy.playerSteamId] : 100;
                if(playerHeldBy.health >= health)
                {
                    audio.PlayOneShot(error);
                    Debug.Log("LGU: Can't use medkit - full health");
                    return;
                }
                audio.PlayOneShot(use);
                uses++;
                int heal_value = UpgradeBus.instance.cfg.MEDKIT_HEAL_VALUE;
                int potentialHealth = playerHeldBy.health + heal_value;
                if (potentialHealth > health)
                {
                    heal_value -= potentialHealth - health;
                }
                playerHeldBy.DamagePlayer(-heal_value, false, true, CauseOfDeath.Unknown, 0, false, Vector3.zero);
                if(uses >= UpgradeBus.instance.cfg.MEDKIT_USES)
                {
                    itemUsedUp = true;
                    HUDManager.Instance.DisplayTip("NO MORE USES!", "This medkit doesn't have anymore supplies!", true, false, "LC_Tip1");
                }
            }
        }
    }
}
