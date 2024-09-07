using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship
{
    class Discombobulator : TierUpgrade, IUpgradeWorldBuilding
    {
        internal float flashCooldown = 0f;
        public static Discombobulator instance;

        public const string UPGRADE_NAME = "Discombobulator";
        public const string PRICES_DEFAULT = "330,460,620";
        internal const string WORLD_BUILDING_TEXT = "\n\nService key for the Ship's terminal which allows {0} to legally use the Ship's 'Discombobulator' module." +
            " Comes with a list of opt-in maintenance procedures that promise to optimze the discharge and refractory of the system." +
            " Said document contains no mention of whatever it might be that it was included in the Ship's design to discombobulate.\n\n";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_OVERRIDE_NAME;
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
        public void UseDiscombobulatorServerRpc()
        {
            UseDiscombobulatorClientRpc();
        }

        [ClientRpc]
        private void UseDiscombobulatorClientRpc()
        {
            Terminal terminal = UpgradeBus.Instance.GetTerminal();
            PlayAudio(ref terminal);
            flashCooldown = UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_COOLDOWN.Value;
            StunNearbyEnemies(ref terminal);
        }

        void StunNearbyEnemies(ref Terminal terminal)
        {
            Collider[] array = Physics.OverlapSphere(terminal.transform.position, UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_RADIUS.Value, 524288);
            if (array.Length == 0) return;
            for (int i = 0; i < array.Length; i++)
            {
                EnemyAICollisionDetect component = array[i].GetComponent<EnemyAICollisionDetect>();
                if (component == null) continue;
                EnemyAI enemy = component.mainScript;
                if (CanDealDamage())
                {
                    int forceValue = UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_INITIAL_DAMAGE.Value + (UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_DAMAGE_INCREASE.Value * (GetUpgradeLevel(UPGRADE_NAME) - UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_DAMAGE_LEVEL.Value));
                    enemy.HitEnemy(forceValue);
                }
                if (!enemy.isEnemyDead) enemy.SetEnemyStunned(true, UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_STUN_DURATION.Value + (UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_INCREMENT.Value * GetUpgradeLevel(UPGRADE_NAME)), null);
            }
        }

        void PlayAudio(ref Terminal terminal)
        {
            terminal.terminalAudio.maxDistance = 100f;
            terminal.terminalAudio.PlayOneShot(UpgradeBus.Instance.flashNoise);
            StartCoroutine(ResetRange(terminal));
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
            static float infoFunction(int level) => UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_STUN_DURATION.Value + (level * UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_INCREMENT.Value);
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                string[] prices = UpgradeBus.Instance.PluginConfiguration.DISCO_UPGRADE_PRICES.Value.Split(',');
                return UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_PRICE.Value <= 0 && prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0");
            }
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            UpgradeBus.Instance.flashNoise = AssetBundleHandler.GetAudioClip("Flashbang");

            SetupGenericPerk<Discombobulator>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;
            AudioClip flashSFX = AssetBundleHandler.GetAudioClip("Flashbang");
            if (!flashSFX) return null;

            UpgradeBus.Instance.flashNoise = flashSFX;
            return UpgradeBus.Instance.SetupMultiplePurchasableTerminalNode(UPGRADE_NAME,
                                                configuration.SHARED_UPGRADES.Value || !configuration.DISCOMBOBULATOR_INDIVIDUAL.Value,
                                                configuration.DISCOMBOBULATOR_ENABLED.Value,
                                                configuration.DISCOMBOBULATOR_PRICE.Value,
                                                UpgradeBus.ParseUpgradePrices(configuration.DISCO_UPGRADE_PRICES.Value),
                                                configuration.OVERRIDE_UPGRADE_NAMES ? configuration.DISCOMBOBULATOR_OVERRIDE_NAME : "");
        }
    }
}
