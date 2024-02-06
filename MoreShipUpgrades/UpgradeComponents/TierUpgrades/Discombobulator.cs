using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades
{
    class Discombobulator : TierUpgrade, IUpgradeWorldBuilding
    {
        public const string UPGRADE_NAME = "Discombobulator";
        public static string PRICES_DEFAULT = "330,460,620";
        private static LGULogger logger = new LGULogger(nameof(Discombobulator));
        public static Discombobulator instance;
        internal const string WORLD_BUILDING_TEXT = "\n\nService key for the Ship's terminal which allows {0} to legally use the Ship's 'Discombobulator' module." +
            " Comes with a list of opt-in maintenance procedures that promise to optimze the discharge and refractory of the system." +
            " Said document contains no mention of whatever it might be that it was included in the Ship's design to discombobulate.\n\n";
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            base.Start();
        }

        void Update()
        {
            if (UpgradeBus.instance.flashCooldown > 0f)
            {
                UpgradeBus.instance.flashCooldown -= Time.deltaTime;
            }
        }
        public override void Load()
        {
            base.Load();
            UpgradeBus.instance.terminalFlash = true;
            instance = this;
        }

        public override void Increment()
        {
            UpgradeBus.instance.discoLevel++;
        }

        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.terminalFlash = false;
            UpgradeBus.instance.discoLevel = 0;
        }

        [ServerRpc(RequireOwnership = false)]
        public void PlayAudioAndUpdateCooldownServerRpc()
        {
            PlayAudioAndUpdateCooldownClientRpc();
        }

        [ClientRpc]
        private void PlayAudioAndUpdateCooldownClientRpc()
        {
            Terminal terminal = UpgradeBus.instance.GetTerminal();
            terminal.terminalAudio.maxDistance = 100f;
            terminal.terminalAudio.PlayOneShot(UpgradeBus.instance.flashNoise);
            StartCoroutine(ResetRange(terminal));
            UpgradeBus.instance.flashCooldown = UpgradeBus.instance.cfg.DISCOMBOBULATOR_COOLDOWN;
            Collider[] array = Physics.OverlapSphere(terminal.transform.position, UpgradeBus.instance.cfg.DISCOMBOBULATOR_RADIUS, 524288);
            if (array.Length <= 0) return;
            for (int i = 0; i < array.Length; i++)
            {
                EnemyAICollisionDetect component = array[i].GetComponent<EnemyAICollisionDetect>();
                if (component == null) continue;
                EnemyAI enemy = component.mainScript;
                if (CanDealDamage())
                {
                    int forceValue = UpgradeBus.instance.cfg.DISCOMBOBULATOR_INITIAL_DAMAGE + UpgradeBus.instance.cfg.DISCOMBOBULATOR_DAMAGE_INCREASE * (UpgradeBus.instance.discoLevel - UpgradeBus.instance.cfg.DISCOMBOBULATOR_DAMAGE_LEVEL);
                    enemy.HitEnemy(forceValue);
                }
                if (!enemy.isEnemyDead) enemy.SetEnemyStunned(true, UpgradeBus.instance.cfg.DISCOMBOBULATOR_STUN_DURATION + UpgradeBus.instance.cfg.DISCOMBOBULATOR_INCREMENT * UpgradeBus.instance.discoLevel, null);
            }
        }

        private bool CanDealDamage()
        {
            return UpgradeBus.instance.cfg.DISCOMBOBULATOR_DAMAGE_LEVEL > 0 && UpgradeBus.instance.discoLevel + 1 >= UpgradeBus.instance.cfg.DISCOMBOBULATOR_DAMAGE_LEVEL;
        }
        private IEnumerator ResetRange(Terminal terminal)
        {
            yield return new WaitForSeconds(2f);
            terminal.terminalAudio.maxDistance = 17f;
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "your crew" : "you");
        }

        public override string GetDisplayInfo(int initialPrice = -1, int maxLevels = -1, int[] incrementalPrices = null)
        {
            System.Func<int, float> infoFunction = level => UpgradeBus.instance.cfg.DISCOMBOBULATOR_STUN_DURATION + (level * UpgradeBus.instance.cfg.DISCOMBOBULATOR_INCREMENT);
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
