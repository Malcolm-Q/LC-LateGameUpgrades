using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    public class pagerScript : BaseUpgrade
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void load()
        {
            UpgradeBus.instance.pager = true;
            UpgradeBus.instance.pageScript = this;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Fast Encryption is active!</color>";
        }

        public override void Register()
        {
            if (!UpgradeBus.instance.UpgradeObjects.ContainsKey("Fast Encryption")) { UpgradeBus.instance.UpgradeObjects.Add("Fast Encryption", gameObject); }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ReqBroadcastChatServerRpc(string msg)
        {
            ReceiveChatClientRpc(msg);
        }

        [ClientRpc]
        public void ReceiveChatClientRpc(string msg)
        {
            SignalTranslator translator = GameObject.FindObjectOfType<SignalTranslator>();
            if(translator != null )
            {
                HUDManager.Instance.UIAudio.PlayOneShot(translator.startTransmissionSFX);
            }
            HUDManager.Instance.chatText.text += $"\n<color=#FF0000>Terminal</color><color=#0000FF>:</color> <color=#FF00FF>{msg}</color>";
            HUDManager.Instance.PingHUDElement(HUDManager.Instance.Chat, 4f, 1f, 0.2f);
        }
    }
}
