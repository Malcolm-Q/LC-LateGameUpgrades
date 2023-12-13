using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    public class terminalFlashScript : BaseUpgrade
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.UpgradeObjects.Add("Discombobulator", gameObject);
        }

        public override void load()
        {
            UpgradeBus.instance.terminalFlash = true;
            UpgradeBus.instance.flashScript = this;
            
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Discombobulator is active!\nType 'cooldown' into the terminal for info!!!</color>";
        }

        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Discombobulator")) { UpgradeBus.instance.UpgradeObjects.Add("Discombobulator", gameObject); }
        }

        public override void Increment()
        {
            UpgradeBus.instance.discoLevel++;
        }

        public override void Unwind()
        {
            UpgradeBus.instance.terminalFlash = false;
            UpgradeBus.instance.discoLevel = 0;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Discombobulator has been disabled.</color>";
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
            Collider[] array = Physics.OverlapSphere(terminal.transform.position, UpgradeBus.instance.cfg.DISCOMBOBULATOR_RADIUS, 524288);
            if(array.Length > 0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    EnemyAICollisionDetect component = array[i].GetComponent<EnemyAICollisionDetect>();
                    if (component != null)
                    {
                        component.mainScript.SetEnemyStunned(true, UpgradeBus.instance.cfg.DISCOMBOBULATOR_STUN_DURATION + (UpgradeBus.instance.cfg.DISCOMBOBULATOR_INCREMENT * UpgradeBus.instance.discoLevel), null);
                    }
                }
            }
        }

        private IEnumerator ResetRange(Terminal terminal)
        {
            yield return new WaitForSeconds(2f);
            terminal.terminalAudio.maxDistance = 17f;
        }

    }
}
