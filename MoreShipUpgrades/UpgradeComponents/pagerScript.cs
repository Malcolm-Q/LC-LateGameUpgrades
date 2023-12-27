using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    public class pagerScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Fast Encryption";

        void Start()
        {
            upgradeName = UPGRADE_NAME;
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void load()
        {
            base.load();

            UpgradeBus.instance.pager = true;
            UpgradeBus.instance.pageScript = this;
        }

        public override void Register()
        {
            base.Register();
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
