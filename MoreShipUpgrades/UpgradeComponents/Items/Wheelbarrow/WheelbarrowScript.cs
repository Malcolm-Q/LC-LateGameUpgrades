using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

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
        internal bool dropAllItemsKeySet;
        internal Key dropAllItemsKey;
        internal bool dropAllItemsMouseButtonSet;
        internal MouseButton dropAllitemsMouseButton;
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
        private float totalWeight;
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
        protected bool playSounds;
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
        private const string WITHDRAW_ITEM_TEXT = "Withdraw item: [LMB]";

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
            totalWeight = defaultWeight;
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
                trigger.onInteract.AddListener(InteractWheelbarrow);
                trigger.tag = nameof(InteractTrigger); // Necessary for the interact UI to appear
                trigger.interactCooldown = false;
                trigger.cooldownTime = 0;
            }
            checkMethods = new Dictionary<Restrictions, Func<bool>>();
            checkMethods[Restrictions.ItemCount] = CheckWheelbarrowItemCountRestriction;
            checkMethods[Restrictions.TotalWeight] = CheckWheelbarrowWeightRestriction;
            checkMethods[Restrictions.All] = CheckWheelbarrowAllRestrictions;
            string controlBind = UpgradeBus.instance.cfg.WHEELBARROW_DROP_ALL_CONTROL_BIND;
            if (Enum.TryParse(controlBind, out Key toggle))
            {
                dropAllItemsKey = toggle;
                dropAllItemsKeySet = true;
            }
            else dropAllItemsKeySet = false;
            if (Enum.TryParse(controlBind, out MouseButton mouseButton))
            {
                dropAllitemsMouseButton = mouseButton;
                dropAllItemsMouseButtonSet = true;
            }
            else dropAllItemsMouseButtonSet = false;
            if (!dropAllItemsKeySet && !dropAllItemsMouseButtonSet)
            {
                logger.LogWarning("No configuration was found to set a control bind for dropping all items, defaulting to middle mouse button");
                dropAllItemsMouseButtonSet = true;
                dropAllitemsMouseButton = MouseButton.Middle;
            }

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
            UpdateWheelbarrowDrop();
            UpdateInteractTriggers();
        }
        private void UpdateWheelbarrowDrop()
        {
            if (!isHeld) return;
            if (playerHeldBy != UpgradeBus.instance.GetLocalPlayer()) return;
            if (currentAmountItems <= 0) return;
            if (!DropAllItemsControlBindPressed()) return;
            DropAllItemsInWheelbarrowServerRpc();
        }
        private bool DropAllItemsControlBindPressed()
        {
            if (dropAllItemsKeySet) return Keyboard.current[dropAllItemsKey].wasPressedThisFrame;
            if (dropAllItemsMouseButtonSet)
            {
                switch(dropAllitemsMouseButton)
                {
                    case MouseButton.Left: return Mouse.current.leftButton.wasPressedThisFrame;
                    case MouseButton.Right: return Mouse.current.rightButton.wasPressedThisFrame;
                    case MouseButton.Middle: return Mouse.current.middleButton.wasPressedThisFrame;
                    case MouseButton.Back: return Mouse.current.backButton.wasPressedThisFrame;
                    case MouseButton.Forward: return Mouse.current.forwardButton.wasPressedThisFrame;
                }
            }
            return false;
        }
        [ServerRpc(RequireOwnership = false)]
        private void DropAllItemsInWheelbarrowServerRpc()
        {
            DropAllItemsInWheelbarrowClientRpc();
        }
        [ClientRpc]
        private void DropAllItemsInWheelbarrowClientRpc()
        {
            logger.LogDebug("Dropping all items from the wheelbarrow...");
            GrabbableObject[] storedItems = GetComponentsInChildren<GrabbableObject>();
            for(int i = 0; i < storedItems.Length; i++)
            {
                if (storedItems[i] == this) continue; // Don't drop the wheelbarrow
                logger.LogDebug($"Dropping {storedItems[i].itemProperties.itemName}");
                DropItem(ref storedItems[i]);
            }
            UpdateWheelbarrowWeightServerRpc();
        }
        /// <summary>
        /// Copy paste from PlayerControllerB.DropAllHeldItems applied on a singular grabbable object script
        /// </summary>
        /// <param name="grabbableObject"></param>
        private void DropItem(ref GrabbableObject grabbableObject)
        {
            grabbableObject.parentObject = null;
            grabbableObject.heldByPlayerOnServer = false;
            if (isInElevator)
            {
                grabbableObject.transform.SetParent(playerHeldBy.playersManager.elevatorTransform, worldPositionStays: true);
            }
            else
            {
                grabbableObject.transform.SetParent(playerHeldBy.playersManager.propsContainer, worldPositionStays: true);
            }

            playerHeldBy.SetItemInElevator(playerHeldBy.isInHangarShipRoom, isInElevator, grabbableObject);
            grabbableObject.EnablePhysics(enable: true);
            grabbableObject.EnableItemMeshes(enable: true);
            grabbableObject.transform.localScale = grabbableObject.originalScale;
            grabbableObject.isHeld = false;
            grabbableObject.isPocketed = false;
            grabbableObject.startFallingPosition = grabbableObject.transform.parent.InverseTransformPoint(grabbableObject.transform.position);
            grabbableObject.FallToGround(randomizePosition: true);
            grabbableObject.fallTime = UnityEngine.Random.Range(-0.3f, 0.05f);
            grabbableObject.hasHitGround = false;
            if (!grabbableObject.itemProperties.syncDiscardFunction)
            {
                grabbableObject.playerHeldBy = null;
            }
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
            
            
            if (playSounds) wheelsNoise.PlayOneShot(wheelsClip[index], 0.2f);
            if (playSounds) WalkieTalkie.TransmitOneShotAudio(wheelsNoise, wheelsClip[index], 0.2f);
            RoundManager.Instance.PlayAudibleNoise(transform.position, noiseRange, 0.8f, 0, isInElevator && StartOfRound.Instance.hangarDoorsClosed);
        }
        private void UpdateInteractTriggers()
        {
            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            if (player == null)
            {
                SetInteractTriggers(false, NO_ITEMS_TEXT);
                return;
            }

            if (!player.isHoldingObject)
            {
                SetInteractTriggers(false, NO_ITEMS_TEXT);
                return;
            }

            GrabbableObject holdingItem = player.currentlyHeldObjectServer;
            if (holdingItem.GetComponent<WheelbarrowScript>() != null)
            {
                SetInteractTriggers(false, WHEELBARROWCEPTION_TEXT);
                return;
            }
            if (CheckWheelbarrowRestrictions()) return;
            SetInteractTriggers(true, START_DEPOSIT_TEXT);
        }
        private void SetInteractTriggers(bool interactable = false, string hoverTip = START_DEPOSIT_TEXT)
        {
            foreach (InteractTrigger trigger in triggers)
            {
                trigger.interactable = interactable;
                if (interactable) trigger.hoverTip = hoverTip;
                else trigger.disabledHoverTip = hoverTip;
            }
        }
        private bool CheckWheelbarrowRestrictions()
        {
            if (restriction == Restrictions.None) return false;
            return checkMethods[restriction].Invoke();
        }
        private bool CheckWheelbarrowAllRestrictions()
        {
            bool weightCondition = totalWeight > 1f + maximumWeightAllowed / 100f;
            bool itemCountCondition = currentAmountItems >= maximumAmountItems;
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
            if (totalWeight > 1f + maximumWeightAllowed / 100f)
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
            if (playerHeldBy && GameNetworkManager.Instance.localPlayerController == playerHeldBy)
            {
                logger.LogDebug("Updating player's weight on drop");
                playerHeldBy.carryWeight += Mathf.Clamp(BackMuscles.DecreasePossibleWeight(itemProperties.weight - 1f), 0, 10f);
                playerHeldBy.carryWeight -= Mathf.Clamp(BackMuscles.DecreasePossibleWeight(totalWeight - 1f), 0, 10f);
            }

            GrabbableObject[] storedItems = GetComponentsInChildren<GrabbableObject>();
            for (int i = 0; i < storedItems.Length; i++)
            {
                if (storedItems[i] is WheelbarrowScript) continue;
                playerHeldBy.SetItemInElevator(playerHeldBy.isInHangarShipRoom, playerHeldBy.isInElevator, storedItems[i]);
            }

            base.DiscardItem();
        }
        public override void GrabItem()
        {
            base.GrabItem();
            if (playerHeldBy.isCrouching && GameNetworkManager.Instance.localPlayerController == playerHeldBy)
            {
                playerHeldBy.Crouch(!playerHeldBy.isCrouching);
            }
            if (playerHeldBy && GameNetworkManager.Instance.localPlayerController == playerHeldBy)
            {
                logger.LogDebug("Updating player's weight on drop");
                playerHeldBy.carryWeight -= Mathf.Clamp(BackMuscles.DecreasePossibleWeight(itemProperties.weight - 1f), 0, 10f);
                playerHeldBy.carryWeight += Mathf.Clamp(BackMuscles.DecreasePossibleWeight(totalWeight - 1f), 0, 10f);
            }
        }
        /// <summary>
        /// Setups attributes related to the wheelbarrow item
        /// </summary>
        private void SetupItemAttributes()
        {
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
            logger.LogDebug(nameof(UpdateWheelbarrowWeightClientRpc));
            logger.LogDebug(GameNetworkManager.Instance.localPlayerController.playerUsername);
            GrabbableObject[] storedItems = GetComponentsInChildren<GrabbableObject>();
            if (isHeld && playerHeldBy == UpgradeBus.instance.GetLocalPlayer()) playerHeldBy.carryWeight -= Mathf.Clamp(BackMuscles.DecreasePossibleWeight(totalWeight - 1f), 0f, 10f);
            totalWeight = defaultWeight;
            currentAmountItems = 0;
            for (int i = 0; i < storedItems.Length; i++)
            {
                if (storedItems[i].GetComponent<WheelbarrowScript>() != null) continue;
                currentAmountItems++;
                GrabbableObject storedItem = storedItems[i];
                totalWeight += (storedItem.itemProperties.weight - 1f) * weightReduceMultiplier;
            }
            logger.LogDebug($"There's currently {(totalWeight - 1f)*100} lbs in the wheelcart");
            if (isHeld && playerHeldBy == UpgradeBus.instance.GetLocalPlayer()) playerHeldBy.carryWeight += Mathf.Clamp(BackMuscles.DecreasePossibleWeight(totalWeight - 1f), 0f, 10f);
        }
        /// <summary>
        /// Action when the interaction bar is completely filled on the container of the wheelbarrow.
        /// It will store the item in the wheelbarrow, allowing it to be carried when grabbing the wheelbarrow
        /// </summary>
        /// <param name="playerInteractor"></param>
        private void InteractWheelbarrow(PlayerControllerB playerInteractor)
        {
            if (playerInteractor.isHoldingObject)
            {
                StoreItemInWheelbarrow(ref playerInteractor);
                return;
            }
        }
        private void StoreItemInWheelbarrow(ref PlayerControllerB playerInteractor)
        {
            logger.LogDebug($"Attempting to store an item from {playerInteractor.playerUsername}");
            Collider triggerCollider = container;
            Vector3 vector = RoundManager.RandomPointInBounds(triggerCollider.bounds);
            vector.y = triggerCollider.bounds.max.y;
            vector.y += playerInteractor.currentlyHeldObjectServer.itemProperties.verticalOffset;
            vector = GetComponent<NetworkObject>().transform.InverseTransformPoint(vector);
            logger.LogDebug($"Applying vector of ({vector.x},{vector.y},{vector.z})");
            playerInteractor.DiscardHeldObject(placeObject: true, parentObjectTo: GetComponent<NetworkObject>(), placePosition: vector, matchRotationOfParent: false);
            UpdateWheelbarrowWeightServerRpc();
        }

        public static float CheckIfPlayerCarryingWheelbarrowLookSensitivity(float defaultValue)
        {
            if (!UpgradeBus.instance.cfg.WHEELBARROW_ENABLED && !UpgradeBus.instance.cfg.SCRAP_WHEELBARROW_ENABLED) return defaultValue;
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            if (!player.isHoldingObject) return defaultValue;
            if (player.currentlyHeldObjectServer is not WheelbarrowScript) return defaultValue;
            if (player.thisController.velocity.magnitude <= 5.0f) return defaultValue;
            return defaultValue * player.currentlyHeldObjectServer.GetComponent<WheelbarrowScript>().GetLookSensitivityDrawback();
        }
        public static float CheckIfPlayerCarryingWheelbarrowMovement(float defaultValue)
        {
            if (!UpgradeBus.instance.cfg.WHEELBARROW_ENABLED && !UpgradeBus.instance.cfg.SCRAP_WHEELBARROW_ENABLED) return defaultValue;
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

        public static bool CheckIfItemInWheelbarrow(GrabbableObject item)
        {
            if (item == null) return false;
            return item.GetComponentInParent<WheelbarrowScript>() != null;
        }
    }
}
