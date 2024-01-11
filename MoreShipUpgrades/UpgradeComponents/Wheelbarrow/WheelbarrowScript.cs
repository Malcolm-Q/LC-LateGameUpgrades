using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Wheelbarrow
{
    internal class WheelbarrowScript : GrabbableObject
    {
        private static LGULogger logger = new LGULogger(nameof(WheelbarrowScript));
        /// <summary>
        /// Maximum amount of items the wheelbarrow allows to store inside
        /// </summary>
        protected int maximumAmountItems;
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
        /// The GameObject responsible to be containing all of the items stored in the wheelbarrow
        /// </summary>
        private BoxCollider container;
        /// <summary>
        /// Trigger responsible to allow interacting with wheelbarrow's container of items
        /// </summary>
        private InteractTrigger[] triggers;
        private float totalWeight;

        private const string ITEM_NAME = "Wheelbarrow";
        private const string SCRAP_ITEM_NAME = "Shopping Cart";
        private const string ITEM_DESCRIPTION = "Allows carrying multiple items";
        private const string NO_ITEMS_TEXT = "No items to deposit...";
        private const string FULL_TEXT = "Too many items in the wheelbarrow";
        private const string WHEELBARROWCEPTION_TEXT = "You're not allowed to do that...";
        private const string DEPOSIT_TEXT = "Depositing item...";
        private const string START_DEPOSIT_TEXT = "Deposit item: [LMB]";

        /// <summary>
        /// When the item spawns in-game, store the necessary variables for correct behaviours from the prefab asset
        /// </summary>
        public override void Start()
        {
            base.Start();
            maximumAmountItems = UpgradeBus.instance.cfg.WHEELBARROW_MAXIMUM_AMOUNT_ITEMS;
            weightReduceMultiplier = UpgradeBus.instance.cfg.WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER;
            defaultWeight = itemProperties.weight;
            totalWeight = defaultWeight;

            triggers = GetComponentsInChildren<InteractTrigger>();
            logger.LogDebug(triggers.Length);
            foreach (BoxCollider collider in GetComponentsInChildren<BoxCollider>())
            {
                if (collider.name != "PlaceableBounds") continue;

                container = collider;
                break;
            }
            if (container == null) logger.LogError($"Couldn't find {nameof(BoxCollider)} component in the prefab...");
            if (triggers == null) logger.LogError($"Couldn't find {nameof(InteractTrigger)} components in the prefab...");
            foreach (InteractTrigger trigger in  triggers)
            {
                trigger.onInteract.AddListener(StoreItemToWheelbarrow);
                trigger.tag = nameof(InteractTrigger); // Necessary for the interact UI to appear
                trigger.interactCooldown = false;
                trigger.cooldownTime = 0;
            }

            SetupItemAttributes();
        }

        public override void Update()
        {
            base.Update();
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
            if (holdingItem.GetComponent<WheelbarrowScript>() != null )
            {
                foreach (InteractTrigger trigger in triggers)
                {
                    trigger.interactable = false;
                    trigger.disabledHoverTip = WHEELBARROWCEPTION_TEXT;
                }
                return;
            }
            GrabbableObject[] storedItems = GetComponentsInChildren<GrabbableObject>();
            if (storedItems.Length > maximumAmountItems)
            {
                foreach (InteractTrigger trigger in triggers)
                {
                    trigger.interactable = false;
                    trigger.disabledHoverTip = FULL_TEXT;
                }
                return;
            }
            foreach (InteractTrigger trigger in triggers)
            {
                trigger.interactable = true;
                trigger.hoverTip = START_DEPOSIT_TEXT;
            }
        }
        public override void DiscardItem()
        {
            playerHeldBy.drunkness = 0f;
            playerHeldBy.drunknessInertia = 0f;
            base.DiscardItem();
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
            GrabbableObject[] storedItems = GetComponentsInChildren<GrabbableObject>();
            if (storedItems.Length >= maximumAmountItems + 1) return; // Can't store more items;

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

        public static float CheckIfPlayerCarryingWheelbarrow(float defaultValue)
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            if (!player.isHoldingObject) return defaultValue;
            if (player.currentlyHeldObjectServer is not WheelbarrowScript) return defaultValue;
            if (player.thisController.velocity.magnitude <= 0.5f) return defaultValue;
            return defaultValue * 0.4f;
        }
        public static float CheckIfPlayerCarryingWheelbarrowMovement(float defaultValue)
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            if (!player.isHoldingObject) return defaultValue;
            if (player.currentlyHeldObjectServer is not WheelbarrowScript) return defaultValue;
            logger.LogDebug(player.thisController.velocity.magnitude);
            if (player.thisController.velocity.magnitude <= 5.0f) return defaultValue;
            return defaultValue * 10f;
        }
    }
}
