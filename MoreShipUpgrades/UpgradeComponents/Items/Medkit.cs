﻿using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents.Items
{
    /// <summary>
    /// Logical class which represents an item that can heal the player when activated
    /// </summary>
    internal class Medkit : GrabbableObject
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static LGULogger logger = new LGULogger(nameof(Medkit));
        /// <summary>
        /// Sounds played when interacting with the medkit
        /// </summary>
        public AudioClip error, use;
        /// <summary>
        /// Source of the sounds played when interacting with the medkit
        /// </summary>
        private AudioSource audio;
        /// <summary>
        /// Instance responsible of managing the player's HUD
        /// </summary>
        private HUDManager hudManager;
        /// <summary>
        /// Current amount of heals the instance of this class has made
        /// </summary>
        private int uses = 0;

        /// <summary>
        /// Maximum amount of heals the instance of the class allows
        /// </summary>
        protected int maximumUses;
        /// <summary>
        /// How much a given instance of the class heals when interacted
        /// </summary>
        protected int healAmount;

        public override void Start()
        {
            audio = GetComponent<AudioSource>();
            hudManager = HUDManager.Instance;
            maximumUses = UpgradeBus.instance.cfg.MEDKIT_USES;
            healAmount = UpgradeBus.instance.cfg.MEDKIT_HEAL_VALUE;
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
            AttemptToHealPlayer();
        }
        /// <summary>
        /// Attempts to provide healing to the player that activated the medkit item
        /// </summary>
        private void AttemptToHealPlayer()
        {
            int maximumHealth = UpgradeBus.instance.playerHPs.ContainsKey(playerHeldBy.playerSteamId) ? UpgradeBus.instance.playerHPs[playerHeldBy.playerSteamId] : 100;
            if (!CanUseMedkit(maximumHealth)) return;
            UseMedkit(maximumHealth);
        }
        /// <summary>
        /// <para>Consumes an use of the medkit to heal the player that is holding it.</para>
        /// <para>If it has run out of uses to heal, a tip will be displayed to the player to warn that it cannot heal anymore.</para>
        /// </summary>
        /// <param name="maximumHealth"></param>
        private void UseMedkit(int maximumHealth)
        {
            audio.PlayOneShot(use);
            uses++;
            int heal_value = healAmount;
            int potentialHealth = playerHeldBy.health + heal_value;
            if (potentialHealth > maximumHealth)
            {
                heal_value -= potentialHealth - maximumHealth;
            }
            playerHeldBy.DamagePlayer(-heal_value, false, true, CauseOfDeath.Unknown, 0, false, Vector3.zero);
            if (uses >= maximumUses)
            {
                itemUsedUp = true;
                hudManager.DisplayTip("NO MORE USES!", "This medkit doesn't have anymore supplies!", true, false, "LC_Tip1");
            }
            if (playerHeldBy.health >= 20) playerHeldBy.MakeCriticallyInjured(false);
        }
        /// <summary>
        /// Checks if it's in conditions of consume a use of the medkit
        /// </summary>
        /// <param name="maximumHealth">Maximum health allowed for the player that is using the medkit</param>
        /// <returns></returns>
        private bool CanUseMedkit(int maximumHealth)
        {
            if (itemUsedUp)
            {
                hudManager.DisplayTip("NO MORE USES!", "This medkit doesn't have anymore supplies!", true, false, "LC_Tip1");
                return false;
            }
            if (!Mouse.current.leftButton.isPressed) return false;

            if (playerHeldBy.health >= maximumHealth)
            {
                audio.PlayOneShot(error);
                logger.LogDebug("LGU: Can't use medkit - full health");
                return false;
            }
            return true;
        }
    }
}
