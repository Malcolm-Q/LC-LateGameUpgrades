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
        internal float flashCooldown = 0f;
        private static LguLogger logger = new LguLogger(nameof(Discombobulator));
        public static Discombobulator instance;

        public const string UPGRADE_NAME = "Discombobulator";
        public const string PRICES_DEFAULT = "330,460,620";
        internal const string WORLD_BUILDING_TEXT = "\n\nService key for the Ship's terminal which allows {0} to legally use the Ship's 'Discombobulator' module." +
            " Comes with a list of opt-in maintenance procedures that promise to optimze the discharge and refractory of the system." +
            " Said document contains no mention of whatever it might be that it was included in the Ship's design to discombobulate.\n\n";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            instance = this;
        }

        void Update()
        {
            if (flashCooldown > 0f)
            {
                flashCooldown -= Time.deltaTime;
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
            Terminal terminal = UpgradeBus.Instance.GetTerminal();
            terminal.terminalAudio.maxDistance = 100f;
            terminal.terminalAudio.PlayOneShot(UpgradeBus.Instance.flashNoise);
            StartCoroutine(ResetRange(terminal));
            flashCooldown = UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_COOLDOWN.Value;
            Collider[] array = Physics.OverlapSphere(terminal.transform.position, UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_RADIUS.Value, 524288);
            if (array.Length <= 0) return;
            for (int i = 0; i < array.Length; i++)
            {
                EnemyAICollisionDetect component = array[i].GetComponent<EnemyAICollisionDetect>();
                if (component == null) continue;
                EnemyAI enemy = component.mainScript;
                if (CanDealDamage())
                {
                    int forceValue = UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_INITIAL_DAMAGE.Value + UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_DAMAGE_INCREASE.Value * (GetUpgradeLevel(UPGRADE_NAME) - UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_DAMAGE_LEVEL.Value);
                    enemy.HitEnemy(forceValue);
                }
                if (!enemy.isEnemyDead) enemy.SetEnemyStunned(true, UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_STUN_DURATION.Value + UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_INCREMENT.Value * GetUpgradeLevel(UPGRADE_NAME), null);
            }
        }

        private bool CanDealDamage()
        {
            return UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_DAMAGE_LEVEL.Value > 0 && GetUpgradeLevel(UPGRADE_NAME) + 1 >= UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_DAMAGE_LEVEL.Value;
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
            System.Func<int, float> infoFunction = level => UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_STUN_DURATION.Value + (level * UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_INCREMENT.Value);
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
    }
}
