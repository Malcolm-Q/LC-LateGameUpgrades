using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class PentagramScript : NetworkBehaviour
    {
        InteractTrigger trig;
        bool placed = false;
        Animator anim;
        public GameObject loot;
        void Start()
        {
            trig = GetComponent<InteractTrigger>();
            trig.onInteract.AddListener(Interact);
            anim = GetComponent<Animator>();
        }
        void Interact(PlayerControllerB player)
        {
            placed = true;
            trig.interactable = false;
            trig.disabledHoverTip = "ritual in progress";
            anim.SetTrigger("Ritual");
            GetComponentInChildren<ParticleSystem>().Play();
            GameObject go = Instantiate(loot,transform.position+Vector3.up,Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
        }
        void Update()
        {
            if (placed) return;
            trig.interactable = EvalHeldItem();
        }
        bool EvalHeldItem()
        {
            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            if (player == null) return false;
            if (player.currentlyHeldObjectServer == null) return false;
            ContractObject co = player.currentlyHeldObjectServer.GetComponent<ContractObject>();
            if(co == null) return false;
            if (co.contractType == "exorcism") return true;
            return false;
        }
    }
}
