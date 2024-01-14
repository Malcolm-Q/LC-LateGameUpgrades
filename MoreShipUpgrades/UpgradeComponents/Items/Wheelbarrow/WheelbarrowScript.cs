using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow
{
    internal class WheelbarrowScript : GrabbableObject
    {
        protected enum Restrictions
        {
            None,
            TotalWeight,
            ItemCount,
            All,
        }
        private static LGULogger logger = new LGULogger(nameof(WheelbarrowScript));
        protected Restrictions restriction;
        private System.Random randomNoise;
        /// <summary>
        /// Component responsible to emit sound when the wheelbarrow's moving
        /// </summary>
        private AudioSource wheelsNoise;
        /// <summary>
        /// The sound the wheelbarrow will be playing while in movement
        /// </summary>
        public AudioClip[] wheelsClip;
        /// <summary>
        /// How far the sound can be heard from nearby enemies
        /// </summary>
        protected float noiseRange;
        /// <summary>
        /// How long the last sound was played
        /// </summary>
        private float soundCounter;
        /// <summary>
        /// Maximum amount of items the wheelbarrow allows to store inside
        /// </summary>
        protected int maximumAmountItems;
        /// <summary>
        /// Maximum weight allowed to be stored in the wheelbarrow
        /// </summary>
        protected float maximumWeightAllowed;
        /// <summary>
        /// Current amount of items stored in the wheelbarrow
        /// </summary>
        private int currentAmountItems;
        /// <summary>
        /// Multiplier to the inserted item's weight when inserted into the wheelbarrow to increase the wheelbarrow's total weight
        /// </summary>
        protected float weightReduceMultiplier;
        /// <summary>
        /// Weight of the wheelbarrow when not carrying any items
        /// </summary>
        protected float defaultWeight;
        /// <summary>
        /// How sloppy the player's movement is when moving with a wheelbarrow
        /// </summary>
        protected float sloppiness;
        /// <summary>
        /// Value multiplied on the look sensitivity of the player who's carrying the wheelbarrow
        /// </summary>
        protected float lookSensitivityDrawback;
        /// <summary>
        /// The GameObject responsible to be containing all of the items stored in the wheelbarrow
        /// </summary>
        private BoxCollider container;
        /// <summary>
        /// Trigger responsible to allow interacting with wheelbarrow's container of items
        /// </summary>
        private InteractTrigger[] triggers;
        private Dictionary<Restrictions, Func<bool>> checkMethods;

        private const string ITEM_NAME = "Wheelbarrow";
        private const string SCRAP_ITEM_NAME = "Shopping Cart";
        private const string ITEM_DESCRIPTION = "Allows carrying multiple items";
        private const string NO_ITEMS_TEXT = "No items to deposit...";
        private const string FULL_TEXT = "Too many items in the wheelbarrow";
        private const string TOO_MUCH_WEIGHT_TEXT = "Too much weight in the wheelbarrow...";
        private const string ALL_FULL_TEXT = "Cannot insert any more items in the wheelbarrow...";
        private const string WHEELBARROWCEPTION_TEXT = "You're not allowed to do that...";
        private const string DEPOSIT_TEXT = "Depositing item...";
        private const string START_DEPOSIT_TEXT = "Deposit item: [LMB]";

        /// <summary>
        /// When the item spawns in-game, store the necessary variables for correct behaviours from the prefab asset
        /// </summary>
        public override void Start()
        {
            base.Start();
            randomNoise = new System.Random(StartOfRound.Instance.randomMapSeed + 80);
            maximumAmountItems = UpgradeBus.instance.cfg.WHEELBARROW_MAXIMUM_AMOUNT_ITEMS;
            weightReduceMultiplier = UpgradeBus.instance.cfg.WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER;
            defaultWeight = itemProperties.weight;
            soundCounter = 0f;

            wheelsNoise = GetComponentInChildren<AudioSource>();
            triggers = GetComponentsInChildren<InteractTrigger>();
            foreach (BoxCollider collider in GetComponentsInChildren<BoxCollider>())
            {
                if (collider.name != "PlaceableBounds") continue;

                container = collider;
                break;
            }
            if (wheelsNoise == null) logger.LogError($"Couldn't find {nameof(AudioSource)} component in the prefab...");
            if (container == null) logger.LogError($"Couldn't find {nameof(BoxCollider)} component in the prefab...");
            if (triggers == null) logger.LogError($"Couldn't find {nameof(InteractTrigger)} components in the prefab...");
            foreach (InteractTrigger trigger in triggers)
            {
                trigger.onInteract.AddListener(StoreItemToWheelbarrow);
                trigger.tag = nameof(InteractTrigger); // Necessary for the interact UI to appear
                trigger.interactCooldown = false;
                trigger.cooldownTime = 0;
            }
            checkMethods = new Dictionary<Restrictions, Func<bool>>();
            checkMethods[Restrictions.ItemCount] = CheckWheelbarrowItemCountRestriction;
            checkMethods[Restrictions.TotalWeight] = CheckWheelbarrowWeightRestriction;
            checkMethods[Restrictions.All] = CheckWheelbarrowAllRestrictions;

            SetupItemAttributes();
        }
        public float GetSloppiness()
        {
            return sloppiness;
        }
        public float GetLookSensitivityDrawback()
        {
            return lookSensitivityDrawback;
        }
        public override void Update()
        {
            base.Update();
            UpdateWheelbarrowSounds();
            UpdateInteractTriggers();
        }
        private void UpdateWheelbarrowSounds()
        {
            soundCounter += Time.deltaTime;
            if (!isHeld) return;
            if (wheelsNoise == null) return;
            if (playerHeldBy.thisController.velocity.magnitude == 0f)
            {
                wheelsNoise.Stop();
                return;
            }
            if (soundCounter < 2.0f) return;
            soundCounter = 0f;
            int index = randomNoise.Next(0, wheelsClip.Length);
            wheelsNoise.PlayOneShot(wheelsClip[index], 0.2f);
            WalkieTalkie.TransmitOneShotAudio(wheelsNoise, wheelsClip[index], 0.2f);
            RoundManager.Instance.PlayAudibleNoise(transform.position, noiseRange, 0.2f, 0, isInElevator && StartOfRound.Instance.hangarDoorsClosed);
        }
        private void UpdateInteractTriggers()
        {
            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            if (player == null)
            {
                foreach (InteractTrigger trigger in triggers)
                {
                    trigger.interactable = false;
                }
                return;
            }

            if (!player.isHoldingObject)
            {
                foreach (InteractTrigger trigger in triggers)
                {
                    trigger.interactable = false;
                    trigger.disabledHoverTip = NO_ITEMS_TEXT;
                }
                return;
            }
            GrabbableObject holdingItem = player.currentlyHeldObjectServer;
            if (holdingItem.GetComponent<WheelbarrowScript>() != null)
            {
                foreach (InteractTrigger trigger in triggers)
                {
                    trigger.interactable = false;
                    trigger.disabledHoverTip = WHEELBARROWCEPTION_TEXT;
                }
                return;
            }
            if (CheckWheelbarrowRestrictions()) return;
            foreach (InteractTrigger trigger in triggers)
            {
                trigger.interactable = true;
                trigger.hoverTip = START_DEPOSIT_TEXT;
            }
        }
        private bool CheckWheelbarrowRestrictions()
        {
            if (restriction == Restrictions.None) return false;
            logger.LogDebug(restriction);
            return checkMethods[restriction].Invoke();
        }
        private bool CheckWheelbarrowAllRestrictions()
        {
            bool weightCondition = itemProperties.weight - (defaultWeight - 1f) > 1f + maximumWeightAllowed / 100f;
            bool itemCountCondition = currentAmountItems >= maximumAmountItems;
            logger.LogDebug(weightCondition);
            logger.LogDebug(itemCountCondition);
            if (weightCondition || itemCountCondition)
            {
                foreach (InteractTrigger trigger in triggers)
                {
                    trigger.interactable = false;
                    trigger.disabledHoverTip = ALL_FULL_TEXT;
                }
                return true;
            }
            return false;
        }
        private bool CheckWheelbarrowWeightRestriction()
        {
            if (itemProperties.weight - (defaultWeight - 1f) > 1f + maximumWeightAllowed / 100f)
            {
                foreach (InteractTrigger trigger in triggers)
                {
                    trigger.interactable = false;
                    trigger.disabledHoverTip = TOO_MUCH_WEIGHT_TEXT;
                }
                return true;
            }
            return false;
        }
        private bool CheckWheelbarrowItemCountRestriction()
        {
            if (currentAmountItems >= maximumAmountItems)
            {
                foreach (InteractTrigger trigger in triggers)
                {
                    trigger.interactable = false;
                    trigger.disabledHoverTip = FULL_TEXT;
                }
                return true;
            }
            return false;
        }
        public override void DiscardItem()
        {
            wheelsNoise.Stop();
            base.DiscardItem();
        }
        public override void GrabItem()
        {
            base.GrabItem();
            if (playerHeldBy.isCrouching) playerHeldBy.Crouch(!playerHeldBy.isCrouching);
        }
        /// <summary>
        /// Setups attributes related to the wheelbarrow item
        /// </summary>
        private void SetupItemAttributes()
        {
            itemProperties = Instantiate(itemProperties);
            grabbable = true;
            grabbableToEnemies = true;
            SetupScanNodeProperties();
        }
        /// <summary>
        /// Prepares the Scan Node associated with the Wheelbarrow for user display
        /// </summary>
        private void SetupScanNodeProperties()
        {
            ScanNodeProperties scanNode = GetComponentInChildren<ScanNodeProperties>();
            if (scanNode == null)
            {
                logger.LogError($"Couldn't find {nameof(ScanNodeProperties)} component in the prefab,");
                return;
            }
            scanNode.creatureScanID = -1;
            scanNode.headerText = this is StoreWheelbarrow ? ITEM_NAME : SCRAP_ITEM_NAME;
            scanNode.nodeType = this is StoreWheelbarrow ? 0 : scanNode.nodeType;
            scanNode.subText = this is StoreWheelbarrow ? ITEM_DESCRIPTION : $"Value: {scrapValue}";
        }
        public void DecrementStoredItems()
        {
            currentAmountItems--;
            UpdateWheelbarrowWeightServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void UpdateWheelbarrowWeightServerRpc()
        {
            UpdateWheelbarrowWeightClientRpc();
        }
        [ClientRpc]
        private void UpdateWheelbarrowWeightClientRpc()
        {
            GrabbableObject[] storedItems = GetComponentsInChildren<GrabbableObject>();
            logger.LogDebug(storedItems.Length);
            if (playerHeldBy) playerHeldBy.carryWeight -= Mathf.Clamp(exoskeletonScript.DecreasePossibleWeight(itemProperties.weight - 1f), 0f, 10f);
            itemProperties.weight = defaultWeight;

            for (int i = 0; i < storedItems.Length; i++)
            {
                if (storedItems[i].GetComponent<WheelbarrowScript>() != null) continue;
                GrabbableObject storedItem = storedItems[i];
                itemProperties.weight += (storedItem.itemProperties.weight - 1f) * weightReduceMultiplier;
            }
            if (playerHeldBy) playerHeldBy.carryWeight += Mathf.Clamp(exoskeletonScript.DecreasePossibleWeight(itemProperties.weight - 1f), 0f, 10f);
        }
        /// <summary>
        /// Action when the interaction bar is completely filled on the container of the wheelbarrow.
        /// It will store the item in the wheelbarrow, allowing it to be carried when grabbing the wheelbarrow
        /// </summary>
        /// <param name="playerInteractor"></param>
        private void StoreItemToWheelbarrow(PlayerControllerB playerInteractor)
        {
            logger.LogDebug($"Attempting to store an item from {playerInteractor.playerUsername}");

            Collider triggerCollider = container;
            Vector3 vector = RoundManager.RandomPointInBounds(triggerCollider.bounds);
            vector.y = triggerCollider.bounds.max.y;
            vector.y += playerInteractor.currentlyHeldObjectServer.itemProperties.verticalOffset;
            vector = GetComponent<NetworkObject>().transform.InverseTransformPoint(vector);
            logger.LogDebug($"Applying vector of ({vector.x},{vector.y},{vector.z})");
            playerInteractor.DiscardHeldObject(placeObject: true, parentObjectTo: GetComponent<NetworkObject>(), placePosition: vector, matchRotationOfParent: false);
            currentAmountItems++;
            UpdateWheelbarrowWeightServerRpc();
        }

        public static float CheckIfPlayerCarryingWheelbarrowLookSensitivity(float defaultValue)
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            if (!player.isHoldingObject) return defaultValue;
            if (player.currentlyHeldObjectServer is not WheelbarrowScript) return defaultValue;
            if (player.thisController.velocity.magnitude <= 5.0f) return defaultValue;
            return defaultValue * player.currentlyHeldObjectServer.GetComponent<WheelbarrowScript>().GetLookSensitivityDrawback();
        }
        public static float CheckIfPlayerCarryingWheelbarrowMovement(float defaultValue)
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            if (!player.isHoldingObject) return defaultValue;
            if (player.currentlyHeldObjectServer is not WheelbarrowScript) return defaultValue;
            if (player.thisController.velocity.magnitude <= 5.0f) return defaultValue;
            return defaultValue * player.currentlyHeldObjectServer.GetComponent<WheelbarrowScript>().GetSloppiness(); ;
        }
        public static bool CheckIfPlayerCarryingWheelbarrow(PlayerControllerB instance)
        {
            return instance.isHoldingObject && instance.currentlyHeldObjectServer is WheelbarrowScript;
        }
    }
}
