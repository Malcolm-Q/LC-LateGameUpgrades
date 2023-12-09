using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    public class pagerScript : BaseUpgrade
    {
        public bool isOnCooldown = false;
        public float remainingCooldownTime;
        public float cooldownDuration;

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.UpgradeObjects.Add("Pager", gameObject);
            cooldownDuration = UpgradeBus.instance.cfg.PAGER_COOLDOWN_DURATION; // Use the configuration value
        }

        public override void load()
        {
            UpgradeBus.instance.pager = true;
            UpgradeBus.instance.pageScript = this;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Pager is active!</color>";
        }

        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Pager")) { UpgradeBus.instance.UpgradeObjects.Add("Pager", gameObject); }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ReqBroadcastChatServerRpc(string msg)
        {
            if (!isOnCooldown)
            {
                isOnCooldown = true;
                remainingCooldownTime = cooldownDuration;
                StartCoroutine(CooldownTimer());
                ReceiveChatClientRpc(msg);
            }
        }

        [ClientRpc]
        public void ReceiveChatClientRpc(string msg)
        {
            HUDManager.Instance.chatText.text += $"\n<color=#FF0000>Terminal</color><color=#0000FF>:</color> <color=#FF00FF>{msg}</color>";
            HUDManager.Instance.PingHUDElement(HUDManager.Instance.Chat, 4f, 1f, 0.2f);
            isOnCooldown = true;
        }

        [ServerRpc(RequireOwnership = false)]
        private void ReqUpdateCooldownServerRpc()
        {
            UpdateCooldownClientRpc();
        }

        [ClientRpc]
        private void UpdateCooldownClientRpc()
        {
            isOnCooldown = false;
        }

        private IEnumerator CooldownTimer()
        {
            while (remainingCooldownTime > 0)
            {
                yield return new WaitForSeconds(1f);
                remainingCooldownTime--;
            }

            isOnCooldown = false;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Pager cooldown finished. Ready to use!</color>";
        }
    }
}
