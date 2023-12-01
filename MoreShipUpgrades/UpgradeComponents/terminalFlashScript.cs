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
    public class terminalFlashScript : BaseUpgrade
    {
        void Start()
        {
            StartCoroutine(lateApply());
        }

        private IEnumerator lateApply()
        {
            yield return new WaitForSeconds(1);
            UpgradeBus.instance.terminalFlash = true;
            UpgradeBus.instance.flashScript = this;
            
            transform.parent = GameObject.Find("HangarShip").transform;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Discombobulator is active!\nType 'cooldown' into the terminal for info!!!</color>";
            foreach(CustomTerminalNode node in UpgradeBus.instance.terminalNodes)
            {
                if(node.Name.ToLower() == "discombobulator" && node.Price > 0)
                {
                    node.Price /= 2;
                }
            }
        }

        public override void Increment()
        {
            UpgradeBus.instance.discoLevel++;
            foreach( CustomTerminalNode node in UpgradeBus.instance.terminalNodes )
            {
                if(node.Name.ToLower() == "discombobulator")
                {
                    node.Description = $"Enemies are stunned for {UpgradeBus.instance.cfg.DISCOMBOBULATOR_STUN_DURATION + (UpgradeBus.instance.discoLevel * UpgradeBus.instance.cfg.DISCOMBOBULATOR_INCREMENT)} seconds.";
                }
            }
        }


        [ServerRpc(RequireOwnership = false)]
        public void PlayAudioAndUpdateCooldownServerRpc()
        {
            PlayAudioAndUpdateCooldownClientRpc();
        }

        [ClientRpc]
        private void PlayAudioAndUpdateCooldownClientRpc()
        {
            Terminal terminal = GameObject.Find("TerminalScript").GetComponent<Terminal>();
            terminal.terminalAudio.maxDistance = 100f;
            terminal.terminalAudio.PlayOneShot(UpgradeBus.instance.flashNoise);
            StartCoroutine(ResetRange(terminal));
            UpgradeBus.instance.flashCooldown = UpgradeBus.instance.cfg.DISCOMBOBULATOR_COOLDOWN;
        }

        private IEnumerator ResetRange(Terminal terminal)
        {
            yield return new WaitForSeconds(2f);
            terminal.terminalAudio.maxDistance = 17f;
        }

    }
}
