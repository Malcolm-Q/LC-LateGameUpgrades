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
        private bool isOnCooldown = false;
        private float remainingCooldownTime;
        private float cooldownDuration;

        void Start()
        {
            StartCoroutine(lateApply());
        }

        private IEnumerator lateApply()
        {
            yield return new WaitForSeconds(1);
            DontDestroyOnLoad(gameObject);
            cooldownDuration = UpgradeBus.instance.cfg.PAGER_COOLDOWN_DURATION; // Use the configuration value
            load();
        }

        public override void load()
        {
            UpgradeBus.instance.pager = true;
            UpgradeBus.instance.pageScript = this;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Pager is active!</color>";
            if (!UpgradeBus.instance.UpgradeObjects.ContainsKey("Pager"))
            {
                UpgradeBus.instance.UpgradeObjects.Add("Pager", gameObject);
            }
            else
            {
                Plugin.mls.LogWarning("Pager is already in upgrade dict.");
            }
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
            else
            {
                HUDManager.Instance.chatText.text += $"\n<color=#FF0000>Pager on cooldown - {Mathf.RoundToInt(remainingCooldownTime)} seconds remaining.</color>";
            }
        }

        [ClientRpc]
        public void ReceiveChatClientRpc(string msg)
        {
            HUDManager.Instance.chatText.text += $"\n<color=#FF0000>Terminal</color><color=#0000FF>:</color> <color=#FF00FF>{msg}</color>";
            HUDManager.Instance.PingHUDElement(HUDManager.Instance.Chat, 4f, 1f, 0.2f);
            HUDManager.Instance.chatText.text += $"\n<color=#FF0000>Pager cooldown - {Mathf.RoundToInt(cooldownDuration)} seconds.</color>";
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
