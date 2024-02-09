using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents.Items
{
    /// <summary>
    /// Logical class which represents an item that can heal the player when activated
    /// </summary>
    internal class Medkit : GrabbableObject, IDisplayInfo
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
            maximumUses = UpgradeBus.instance.cfg.MEDKIT_USES.Value;
            healAmount = UpgradeBus.instance.cfg.MEDKIT_HEAL_VALUE.Value;
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
            int health = UpgradeBus.instance.playerHealthLevels.ContainsKey(playerHeldBy.playerSteamId) ? Stimpack.GetHealthFromPlayer(100, playerHeldBy.playerSteamId) : 100;
            if (!CanUseMedkit(health)) return;
            UseMedkit(health);
        }
        /// <summary>
        /// <para>Consumes an use of the medkit to heal the player that is holding it.</para>
        /// <para>If it has run out of uses to heal, a tip will be displayed to the player to warn that it cannot heal anymore.</para>
        /// </summary>
        /// <param name="maximumHealth"></param>
        private void UseMedkit(int maximumHealth)
        {
            audio.PlayOneShot(use);
            int heal_value = healAmount;
            int potentialHealth = playerHeldBy.health + heal_value;
            if (potentialHealth > maximumHealth)
            {
                heal_value -= potentialHealth - maximumHealth;
            }
            playerHeldBy.health += heal_value;

            if (IsHost || IsServer) UpdateMedkitUsesClientRpc();
            else UpdateMedkitUsesServerRpc();

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
        [ServerRpc]
        void UpdateMedkitUsesServerRpc()
        {
            UpdateMedkitUsesClientRpc();
        }
        [ClientRpc]
        void UpdateMedkitUsesClientRpc()
        {
            uses++;
            if (uses < maximumUses) return;

            itemUsedUp = true;
            if (playerHeldBy == UpgradeBus.instance.GetLocalPlayer()) hudManager.DisplayTip("NO MORE USES!", "This medkit doesn't have anymore supplies!", true, false, "LC_Tip1");
        }
        public string GetDisplayInfo()
        {
            return $"MEDKIT - ${UpgradeBus.instance.cfg.MEDKIT_PRICE}\n\n" +
                $"Left click to heal yourself for {UpgradeBus.instance.cfg.MEDKIT_HEAL_VALUE} health.\n" +
                $"Can be used {UpgradeBus.instance.cfg.MEDKIT_USES} times.";
        }
    }
}
