using GameNetcodeStuff;
using LethalLib.Extras;
using LethalLib.Modules;
using MoreShipUpgrades.Input;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.UpgradeComponents.Items.Wheelbarrow
{
    public abstract class WheelbarrowScript : LategameItem
    {
        internal const float VELOCITY_APPLY_EFFECT_THRESHOLD = 5.0f;
        public enum Restrictions
        {
            None,
            TotalWeight,
            ItemCount,
            All,
        }
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
            maximumAmountItems = UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_MAXIMUM_AMOUNT_ITEMS.Value;
            weightReduceMultiplier = UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_WEIGHT_REDUCTION_MULTIPLIER.Value;
            defaultWeight = itemProperties.weight;
            totalWeight = defaultWeight;
            soundCounter = 0f;

            wheelsNoise = GetComponent<AudioSource>();
            triggers = GetComponentsInChildren<InteractTrigger>();
            foreach (BoxCollider collider in GetComponentsInChildren<BoxCollider>())
            {
                if (collider.name != "PlaceableBounds") continue;

                container = collider;
                break;
            }
            foreach (InteractTrigger trigger in triggers)
            {
                trigger.onInteract.AddListener(InteractWheelbarrow);
                trigger.tag = nameof(InteractTrigger); // Necessary for the interact UI to appear
                trigger.interactCooldown = false;
                trigger.cooldownTime = 0;
            }
            checkMethods = new Dictionary<Restrictions, Func<bool>>
            {
                [Restrictions.ItemCount] = CheckWheelbarrowItemCountRestriction,
                [Restrictions.TotalWeight] = CheckWheelbarrowWeightRestriction,
                [Restrictions.All] = CheckWheelbarrowAllRestrictions
            };

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
        public void UpdateWheelbarrowDrop()
        {
            if (!isHeld) return;
            if (playerHeldBy != UpgradeBus.Instance.GetLocalPlayer()) return;
            if (currentAmountItems <= 0) return;
            DropAllItemsInWheelbarrowServerRpc();
        }
        [ServerRpc(RequireOwnership = false)]
        private void DropAllItemsInWheelbarrowServerRpc()
        {
            DropAllItemsInWheelbarrowClientRpc();
        }
        [ClientRpc]
        private void DropAllItemsInWheelbarrowClientRpc()
        {
            GrabbableObject[] storedItems = GetComponentsInChildren<GrabbableObject>();
            for(int i = 0; i < storedItems.Length; i++)
            {
                if (storedItems[i] == this) continue; // Don't drop the wheelbarrow
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
        void EnableInteractTriggers(bool enabled)
        {
            foreach (InteractTrigger trigger in triggers)
                trigger.gameObject.SetActive(enabled);
        }
        private bool CheckWheelbarrowRestrictions()
        {
            if (restriction == Restrictions.None) return false;
            return checkMethods[restriction].Invoke();
        }
        private bool CheckWheelbarrowAllRestrictions()
        {
            bool weightCondition = totalWeight > 1f + (maximumWeightAllowed / 100f);
            bool itemCountCondition = currentAmountItems >= maximumAmountItems;
            if (weightCondition || itemCountCondition)
            {
                SetInteractTriggers(interactable: false, hoverTip: ALL_FULL_TEXT);
                return true;
            }
            return false;
        }
        private bool CheckWheelbarrowWeightRestriction()
        {
            if (totalWeight > 1f + (maximumWeightAllowed / 100f))
            {
                SetInteractTriggers(interactable: false, hoverTip: TOO_MUCH_WEIGHT_TEXT);
                return true;
            }
            return false;
        }
        private bool CheckWheelbarrowItemCountRestriction()
        {
            if (currentAmountItems >= maximumAmountItems)
            {
                SetInteractTriggers(interactable: false, hoverTip: FULL_TEXT);
                return true;
            }
            return false;
        }

        void UpdatePlayerAttributes(bool grabbing)
        {
            if (grabbing)
            {
                playerHeldBy.carryWeight -= Mathf.Clamp(BackMuscles.DecreasePossibleWeight(itemProperties.weight - 1f), 0, 10f);
                playerHeldBy.carryWeight += Mathf.Clamp(BackMuscles.DecreasePossibleWeight(totalWeight - 1f), 0, 10f);

                PlayerManager.instance.SetSensitivityMultiplier(lookSensitivityDrawback);
                PlayerManager.instance.SetSloppyMultiplier(sloppiness);
                PlayerManager.instance.SetHoldingWheelbarrow(true);
            }
            else
            {
                playerHeldBy.carryWeight += Mathf.Clamp(BackMuscles.DecreasePossibleWeight(itemProperties.weight - 1f), 0, 10f);
                playerHeldBy.carryWeight -= Mathf.Clamp(BackMuscles.DecreasePossibleWeight(totalWeight - 1f), 0, 10f);

                PlayerManager.instance.ResetSensitivityMultiplier();
                PlayerManager.instance.ResetSloppyMultiplier();
                PlayerManager.instance.SetHoldingWheelbarrow(false);
            }
        }

        public override void DiscardItem()
        {
            wheelsNoise.Stop();
            if (playerHeldBy && GameNetworkManager.Instance.localPlayerController == playerHeldBy)
            {
                UpdatePlayerAttributes(grabbing: false);
                EnableInteractTriggers(true);
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
            if (playerHeldBy && GameNetworkManager.Instance.localPlayerController == playerHeldBy)
            {
                UpdatePlayerAttributes(grabbing: true);
                EnableInteractTriggers(false);

                if (playerHeldBy.isCrouching) playerHeldBy.Crouch(!playerHeldBy.isCrouching);
            }
        }
        /// <summary>
        /// Setups attributes related to the wheelbarrow item
        /// </summary>
        private void SetupItemAttributes()
        {
            grabbable = true;
            grabbableToEnemies = true;
            itemProperties.toolTips = SetupWheelbarrowTooltips();
            SetupScanNodeProperties();
        }
        private string[] SetupWheelbarrowTooltips()
        {
            string controlBind = IngameKeybinds.Instance.WheelbarrowKey.GetBindingDisplayString();
            return [$"Drop all items: [{controlBind}]"];
        }

        /// <summary>
        /// Prepares the Scan Node associated with the Wheelbarrow for user display
        /// </summary>
        protected abstract void SetupScanNodeProperties();

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
            GrabbableObject[] storedItems = GetComponentsInChildren<GrabbableObject>();
            if (isHeld && playerHeldBy == UpgradeBus.Instance.GetLocalPlayer()) playerHeldBy.carryWeight -= Mathf.Clamp(BackMuscles.DecreasePossibleWeight(totalWeight - 1f), 0f, 10f);
            totalWeight = defaultWeight;
            currentAmountItems = 0;
            for (int i = 0; i < storedItems.Length; i++)
            {
                if (storedItems[i].GetComponent<WheelbarrowScript>() != null) continue;
                currentAmountItems++;
                GrabbableObject storedItem = storedItems[i];
                totalWeight += (storedItem.itemProperties.weight - 1f) * weightReduceMultiplier;
            }
            if (isHeld && playerHeldBy == UpgradeBus.Instance.GetLocalPlayer()) playerHeldBy.carryWeight += Mathf.Clamp(BackMuscles.DecreasePossibleWeight(totalWeight - 1f), 0f, 10f);
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
            }
        }
        private void StoreItemInWheelbarrow(ref PlayerControllerB playerInteractor)
        {
            Collider triggerCollider = container;
            Vector3 vector = RoundManager.RandomPointInBounds(triggerCollider.bounds);
            vector.y = triggerCollider.bounds.max.y;
            vector.y += playerInteractor.currentlyHeldObjectServer.itemProperties.verticalOffset;
            vector = GetComponent<NetworkObject>().transform.InverseTransformPoint(vector);
            playerInteractor.DiscardHeldObject(placeObject: true, parentObjectTo: GetComponent<NetworkObject>(), placePosition: vector, matchRotationOfParent: false);
            UpdateWheelbarrowWeightServerRpc();
        }

        public static float CheckIfPlayerCarryingWheelbarrowLookSensitivity(float defaultValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_ENABLED.Value && !UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_ENABLED.Value) return defaultValue;
            PlayerControllerB player = UpgradeBus.Instance.GetLocalPlayer();
            if (player == null || player.thisController == null) return defaultValue;
            if (player.thisController.velocity.magnitude <= VELOCITY_APPLY_EFFECT_THRESHOLD) return defaultValue;
            return defaultValue * PlayerManager.instance.GetSensitivityMultiplier();
        }
        public static float CheckIfPlayerCarryingWheelbarrowMovement(float defaultValue)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_ENABLED.Value && !UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_ENABLED.Value) return defaultValue;
            PlayerControllerB player = UpgradeBus.Instance.GetLocalPlayer();
            if (player == null || player.thisController == null) return defaultValue;
            if (player.thisController.velocity.magnitude <= VELOCITY_APPLY_EFFECT_THRESHOLD) return defaultValue;
            return defaultValue * PlayerManager.instance.GetSloppyMultiplier();
        }
        public static bool CheckIfPlayerCarryingWheelbarrow()
        {
            return PlayerManager.instance.GetHoldingWheelbarrow();
        }

        public static bool CheckIfItemInWheelbarrow(GrabbableObject item)
        {
            if (item == null) return false;
            return item.GetComponentInParent<WheelbarrowScript>() != null;
        }

        public static new void LoadItem()
        {
            AudioClip[] wheelbarrowSound = AssetBundleHandler.GetAudioClipList("Wheelbarrow Sound", 4);
            AudioClip[] shoppingCartSound = AssetBundleHandler.GetAudioClipList("Scrap Wheelbarrow Sound", 4);
            SetupStoreWheelbarrow(wheelbarrowSound);
            SetupScrapWheelbarrow(shoppingCartSound);
        }
        private static void SetupScrapWheelbarrow(AudioClip[] shoppingCartSound)
        {
            Item wheelbarrow = AssetBundleHandler.GetItemObject("Scrap Wheelbarrow");
            if (wheelbarrow == null) return;
            wheelbarrow.itemId = 492018;
            wheelbarrow.minValue = UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_MINIMUM_VALUE.Value;
            wheelbarrow.maxValue = UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_MAXIMUM_VALUE.Value;
            wheelbarrow.twoHanded = true;
            wheelbarrow.twoHandedAnimation = true;
            wheelbarrow.grabAnim = "HoldJetpack";
            wheelbarrow.floorYOffset = -90;
            wheelbarrow.positionOffset = new Vector3(0f, -1.7f, 0.35f);
            wheelbarrow.allowDroppingAheadOfPlayer = true;
            wheelbarrow.isConductiveMetal = true;
            wheelbarrow.isScrap = true;
            wheelbarrow.weight = 0.99f + (UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_WEIGHT.Value / 100f);
            wheelbarrow.canBeGrabbedBeforeGameStart = true;
            ScrapWheelbarrow barrowScript = wheelbarrow.spawnPrefab.AddComponent<ScrapWheelbarrow>();
            barrowScript.itemProperties = wheelbarrow;
            barrowScript.wheelsClip = shoppingCartSound;
            wheelbarrow.spawnPrefab.AddComponent<ScrapValueSyncer>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(wheelbarrow.spawnPrefab);
            LethalLib.Modules.Items.RegisterItem(wheelbarrow);
            Utilities.FixMixerGroups(wheelbarrow.spawnPrefab);
            int amountToSpawn = UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_ENABLED.Value ? 1 : 0;

            AnimationCurve curve = new(new Keyframe(0, 0), new Keyframe(1f - UpgradeBus.Instance.PluginConfiguration.SCRAP_WHEELBARROW_RARITY.Value, amountToSpawn), new Keyframe(1, amountToSpawn));
            SpawnableMapObjectDef mapObjDef = ScriptableObject.CreateInstance<SpawnableMapObjectDef>();
            mapObjDef.spawnableMapObject = new SpawnableMapObject
            {
                prefabToSpawn = wheelbarrow.spawnPrefab
            };
            MapObjects.RegisterMapObject(mapObjDef, Levels.LevelTypes.All, (_) => curve);
        }
        private static void SetupStoreWheelbarrow(AudioClip[] wheelbarrowSound)
        {
            Item wheelbarrow = AssetBundleHandler.GetItemObject("Store Wheelbarrow");
            if (wheelbarrow == null) return;

            wheelbarrow.itemId = 492019;
            wheelbarrow.creditsWorth = UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_PRICE.Value;
            wheelbarrow.twoHanded = true;
            wheelbarrow.twoHandedAnimation = true;
            wheelbarrow.grabAnim = "HoldJetpack";
            wheelbarrow.floorYOffset = -90;
            wheelbarrow.verticalOffset = 0.3f;
            wheelbarrow.positionOffset = new Vector3(0f, -0.7f, 1.4f);
            wheelbarrow.allowDroppingAheadOfPlayer = true;
            wheelbarrow.isConductiveMetal = true;
            wheelbarrow.weight = 0.99f + (UpgradeBus.Instance.PluginConfiguration.WHEELBARROW_WEIGHT.Value / 100f);
            wheelbarrow.canBeGrabbedBeforeGameStart = true;
            StoreWheelbarrow barrowScript = wheelbarrow.spawnPrefab.AddComponent<StoreWheelbarrow>();
            barrowScript.itemProperties = wheelbarrow;
            barrowScript.wheelsClip = wheelbarrowSound;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(wheelbarrow.spawnPrefab);
            Utilities.FixMixerGroups(wheelbarrow.spawnPrefab);

            UpgradeBus.Instance.ItemsToSync.Add(StoreWheelbarrow.ITEM_NAME, wheelbarrow);

            ItemManager.SetupStoreItem(wheelbarrow);
        }
    }
}
