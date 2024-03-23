using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items.RadarBooster
{
    internal class ChargingStationManager : MonoBehaviour
    {
        InteractTrigger interactComponent;
        internal float cooldown = 0f;
        void Start()
        {
            GameObject chargeStation = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), transform.position, Quaternion.Euler(Vector3.zero), transform);
            chargeStation.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            chargeStation.tag = nameof(InteractTrigger);
            chargeStation.layer = LayerMask.NameToLayer("Props");
            chargeStation.name = "Charging Station";
            chargeStation.GetComponent<BoxCollider>().isTrigger = true;
            InitializeInteractTrigger(ref chargeStation);
        }

        void InitializeInteractTrigger(ref GameObject chargeStation)
        {
            InteractTrigger interactTrigger = chargeStation.AddComponent<InteractTrigger>();
            interactTrigger.interactable = true;
            interactTrigger.holdInteraction = true;
            interactTrigger.timeToHold = 2f;
            interactTrigger.holdingInteractEvent = new InteractEventFloat();
            interactTrigger.onInteractEarly = new InteractEvent();
            interactTrigger.onStopInteract = new InteractEvent();
            interactTrigger.onInteract = new InteractEvent();
            interactTrigger.onInteract.AddListener(OnChargeInteract);
            interactTrigger.hoverIcon = GetComponent<RadarBoosterItem>().itemProperties.itemIcon;
            interactTrigger.disabledHoverIcon = GetComponent<RadarBoosterItem>().itemProperties.itemIcon;
            interactComponent = interactTrigger;
        }
        void Update()
        {
            PlayerControllerB localPlayer = UpgradeBus.Instance.GetLocalPlayer();
            if (localPlayer == null) return;
            if (cooldown > 0f)
            {
                SetInteractable(interact: false, text: $"On cooldown... [{(int)(cooldown)}s]");
                cooldown -= Time.deltaTime;
                return;
            }
            if (!localPlayer.isHoldingObject)
            {
                SetInteractable(interact: false, text: "Not holding an item to charge up...");
                return;
            }
            GrabbableObject heldObject = localPlayer.currentlyHeldObjectServer;
            if (!heldObject.itemProperties.requiresBattery)
            {
                SetInteractable(interact: false, text: "This item cannot be charged...");
                return;
            }
            SetInteractable(interact: true, text: "Charge item: [LMB]");
        }
        void SetInteractable(bool interact, string text)
        {
            interactComponent.interactable = interact;
            if (interact) interactComponent.hoverTip = text;
            else interactComponent.disabledHoverTip = text;
        }
        void OnChargeInteract(PlayerControllerB interactingPlayer)
        {
            GrabbableObject heldObject = interactingPlayer.currentlyHeldObjectServer;
            heldObject.insertedBattery.charge = Mathf.Clamp(heldObject.insertedBattery.charge + (UpgradeBus.Instance.PluginConfiguration.CHARGING_BOOSTER_CHARGE_PERCENTAGE.Value/100f), 0f, 1f);
            cooldown = ChargingBooster.Instance.chargeCooldown;
            ChargingBooster.Instance.UpdateCooldownServerRpc(new NetworkBehaviourReference(GetComponent<RadarBoosterItem>()));
        }
    }
}
