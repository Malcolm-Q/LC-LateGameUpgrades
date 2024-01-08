using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class WheelbarrowScript : GrabbableObject
    {
        private static LGULogger logger = new LGULogger(nameof(WheelbarrowScript));
        /// <summary>
        /// Maximum amount of items the wheelbarrow allows to store inside
        /// </summary>
        private int maximumAmountItems;
        /// <summary>
        /// Multiplier to the inserted item's weight when inserted into the wheelbarrow to increase the wheelbarrow's total weight
        /// </summary>
        private float weightReduceMultiplier;
        /// <summary>
        /// Weight of the wheelbarrow when not carrying any items
        /// </summary>
        private float defaultWeight;
        /// <summary>
        /// The GameObject responsible to be containing all of the items stored in the wheelbarrow
        /// </summary>
        private PlaceableObjectsSurface container;
        /// <summary>
        /// Trigger responsible to allow interacting with wheelbarrow's container of items
        /// </summary>
        private InteractTrigger trigger;

        private const string ITEM_NAME = "Wheelbarrow";
        private const string ITEM_DESCRIPTION = "Allows carrying multiple items";
        private const string FULL_TEXT = "Too many items in the wheelbarrow";
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

            trigger = GetComponentInChildren<InteractTrigger>();
            container = GetComponentInChildren<PlaceableObjectsSurface>();
            if (container == null) logger.LogError($"Couldn't find {nameof(PlaceableObjectsSurface)} component in the prefab...");
            if (trigger == null) logger.LogError($"Couldn't find {nameof(InteractTrigger)} component in the prefab...");
            trigger.onInteractEarly.AddListener(TryToStoreItemToWheelbarrow);
            trigger.onInteract.AddListener(StoreItemToWheelbarrow);
            trigger.tag = nameof(InteractTrigger); // Necessary for the interact UI to appear
            SetupItemAttributes();
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
            scanNode.headerText = ITEM_NAME;
            scanNode.nodeType = 0;
            scanNode.subText = ITEM_DESCRIPTION;
        }
        /// <summary>
        /// Whenever a player grabs the wheelbarrow, update the weight value of the item according to the amount of items present in it
        /// </summary>
        public override void GrabItem()
        {
            GrabbableObject[] storedItems = GetComponentsInChildren<GrabbableObject>();
            if (storedItems.Length <= 0) return;

            playerHeldBy.carryWeight -= Mathf.Clamp(exoskeletonScript.DecreasePossibleWeight(itemProperties.weight - 1f), 0f, 10f);
            itemProperties.weight = defaultWeight;

            for (int i = 0; i < storedItems.Length; i++)
            {
                if (storedItems[i].GetComponent<WheelbarrowScript>() != null) continue;
                logger.LogDebug(storedItems[i].itemProperties.itemName);
                logger.LogDebug(storedItems[i].itemProperties.weight);
                GrabbableObject storedItem = storedItems[i];
                itemProperties.weight += (storedItem.itemProperties.weight - 1f) * weightReduceMultiplier;
            }
            playerHeldBy.carryWeight += Mathf.Clamp(exoskeletonScript.DecreasePossibleWeight(itemProperties.weight - 1f), 0f, 10f);
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

            if (!(GameNetworkManager.Instance != null && playerInteractor == GameNetworkManager.Instance.localPlayerController)) return;

            Collider triggerCollider = container.placeableBounds;
            Vector3 vector = RoundManager.RandomPointInBounds(triggerCollider.bounds);
            vector.y = triggerCollider.bounds.min.y;
            RaycastHit raycastHit;
            if (Physics.Raycast(new Ray(vector + Vector3.up * 3f, Vector3.down), out raycastHit, 8f, 1048640, QueryTriggerInteraction.Collide))
            {
                vector = raycastHit.point;
            }
            vector.y += playerInteractor.currentlyHeldObjectServer.itemProperties.verticalOffset;
            vector += triggerCollider.transform.position - container.parentTo.transform.position;
            vector = container.transform.InverseTransformPoint(vector);
            logger.LogDebug($"Applying vector of ({vector.x},{vector.y},{vector.z})");
            playerInteractor.DiscardHeldObject(true, container.parentTo, vector, false);
        }
        /// <summary>
        /// Action which is triggered when the player starts interacting with wheelbarrow
        /// It will check if it has enough capacity to hold another item.
        /// Otherwise it will let the interaction continue
        /// </summary>
        /// <param name="playerInteractor"></param>
        private void TryToStoreItemToWheelbarrow(PlayerControllerB playerInteractor)
        {
            GrabbableObject[] storedItems = GetComponentsInChildren<GrabbableObject>();
            if (storedItems.Length >= maximumAmountItems + 1) // Because the wheelbarrow itself is a GrabbableObject
            {
                trigger.holdTip = FULL_TEXT;
                trigger.enabled = false;
                return;
            }
            trigger.hoverTip = START_DEPOSIT_TEXT;
            trigger.holdTip = DEPOSIT_TEXT;
            trigger.enabled = true;
        }
    }
}
