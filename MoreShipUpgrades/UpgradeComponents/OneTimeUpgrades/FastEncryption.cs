using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using Unity.Netcode;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    class FastEncryption : OneTimeUpgrade
    {
        public static string UPGRADE_NAME = "Fast Encryption";
        public static FastEncryption instance;
        private static LGULogger logger;

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            logger = new LGULogger(upgradeName);
            base.Start();
        }

        public override void Load()
        {
            base.Load();

            UpgradeBus.instance.pager = true;
            instance = this;
        }

        public override void Unwind()
        {
            base.Unwind();
            UpgradeBus.instance.pager = false;
        }

        [ServerRpc(RequireOwnership = false)]
        public void ReqBroadcastChatServerRpc(string msg)
        {
            logger.LogInfo($"Instructing clients to print broadcasted message...");
            ReceiveChatClientRpc(msg);
        }

        [ClientRpc]
        public void ReceiveChatClientRpc(string msg)
        {
            SignalTranslator translator = FindObjectOfType<SignalTranslator>();
            if (translator != null)
            {
                HUDManager.Instance.UIAudio.PlayOneShot(translator.startTransmissionSFX);
                logger.LogError("Unable to find SignalTranslator!");
            }
            logger.LogInfo("Broadcasted messaged received, printing.");
            HUDManager.Instance.chatText.text += $"\n<color=#FF0000>Terminal</color><color=#0000FF>:</color> <color=#FF00FF>{msg}</color>";
            HUDManager.Instance.PingHUDElement(HUDManager.Instance.Chat, 4f, 1f, 0.2f);
        }

        public override string GetDisplayInfo(int price = -1)
        {
            return "Unrestrict the transmitter";
        }
    }
}
