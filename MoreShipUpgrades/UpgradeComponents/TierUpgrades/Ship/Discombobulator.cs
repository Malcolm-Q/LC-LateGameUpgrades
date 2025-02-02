using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Configuration.Custom;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
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
        public const string PRICES_DEFAULT = "450,330,460,620";
        internal const string WORLD_BUILDING_TEXT = "\n\nService key for the Ship's terminal which allows {0} to legally use the Ship's 'Discombobulator' module." +
            " Comes with a list of opt-in maintenance procedures that promise to optimze the discharge and refractory of the system." +
            " Said document contains no mention of whatever it might be that it was included in the Ship's design to discombobulate.\n\n";
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = GetConfiguration().DiscombobulatorUpgradeConfiguration.OverrideName;
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
        internal void UseDiscombobulatorClientRpc()
        {
            Terminal terminal = UpgradeBus.Instance.GetTerminal();
            PlayAudio(ref terminal);
            flashCooldown = GetConfiguration().DiscombobulatorUpgradeConfiguration.InitialEffect.Value;
            StunNearbyEnemies(ref terminal);
        }

        void StunNearbyEnemies(ref Terminal terminal)
        {
            DiscombobulatorUpgradeConfiguration config = GetConfiguration().DiscombobulatorUpgradeConfiguration;
            Collider[] array = Physics.OverlapSphere(terminal.transform.position, config.Radius.Value, 524288);
            if (array.Length == 0) return;
            for (int i = 0; i < array.Length; i++)
            {
                EnemyAICollisionDetect component = array[i].GetComponent<EnemyAICollisionDetect>();
                if (component == null) continue;
                EnemyAI enemy = component.mainScript;
                if (IsEnemyBlacklisted(enemy)) continue;
                if (CanDealDamage())
                {
                    int forceValue = config.InitialDamage.Value + (config.IncrementalDamage.Value * (GetUpgradeLevel(UPGRADE_NAME) - config.DamageLevel.Value));
                    enemy.HitEnemy(forceValue);
                }
                float actualDuration = config.InitialEffect.Value + (config.IncrementalEffect.Value * GetUpgradeLevel(UPGRADE_NAME));
                if (config.Absolute) actualDuration /= enemy.enemyType.stunTimeMultiplier;

                if (!enemy.isEnemyDead) enemy.SetEnemyStunned(true, actualDuration, null);
            }
        }

        bool IsEnemyBlacklisted(EnemyAI enemy)
        {
            string enemyName = enemy.enemyType.enemyName;
            string[] blacklistedEnemies = GetConfiguration().DiscombobulatorUpgradeConfiguration.BlacklistEnemies.Value.Split(",");
            if (ContainsEnemyName(enemyName, blacklistedEnemies)) return true;
            ScanNodeProperties scanNode = enemy.gameObject.GetComponentInChildren<ScanNodeProperties>();
            if (scanNode == null) return false;
            enemyName = scanNode.headerText;
            return ContainsEnemyName(enemyName, blacklistedEnemies);
        }

        bool ContainsEnemyName(string enemyName, string[] blacklistedEnemies)
        {
            for (int i = 0; i < blacklistedEnemies.Length; i++)
            {
                string blacklistedEnemy = blacklistedEnemies[i];
                if (enemyName.Equals(blacklistedEnemy, System.StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        void PlayAudio(ref Terminal terminal)
        {
            terminal.terminalAudio.maxDistance = 100f;
            terminal.terminalAudio.PlayOneShot(UpgradeBus.Instance.flashNoise);
            StartCoroutine(ResetRange(terminal));
        }

        private bool CanDealDamage()
        {
            DiscombobulatorUpgradeConfiguration config = GetConfiguration().DiscombobulatorUpgradeConfiguration;
            return config.DamageLevel.Value > 0 && GetUpgradeLevel(UPGRADE_NAME) + 1 >= config.DamageLevel.Value;
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
            static float infoFunction(int level)
            {
                DiscombobulatorUpgradeConfiguration config = GetConfiguration().DiscombobulatorUpgradeConfiguration;
                return config.InitialEffect.Value + (level * config.IncrementalDamage.Value);
            }
            string infoFormat = AssetBundleHandler.GetInfoFromJSON(UPGRADE_NAME);
            return Tools.GenerateInfoForUpgrade(infoFormat, initialPrice, incrementalPrices, infoFunction);
        }
        public override bool CanInitializeOnStart
        {
            get
            {
                DiscombobulatorUpgradeConfiguration config = GetConfiguration().DiscombobulatorUpgradeConfiguration;
                string[] prices = config.Prices.Value.Split(',');
                return prices.Length == 0 || (prices.Length == 1 && (prices[0].Length == 0 || prices[0] == "0"));
            }
        }
        [ServerRpc(RequireOwnership = false)]
        public void SetCooldownServerRpc(float cooldown)
        {
            SetCooldownClientRpc(cooldown);
        }
        [ClientRpc]
        public void SetCooldownClientRpc(float cooldown)
        {
            flashCooldown = Mathf.Clamp(cooldown, 0f, GetConfiguration().DiscombobulatorUpgradeConfiguration.Cooldown);
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, GetConfiguration().DiscombobulatorUpgradeConfiguration.ItemProgressionItems.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            UpgradeBus.Instance.flashNoise = AssetBundleHandler.GetAudioClip("Flashbang");

            SetupGenericPerk<Discombobulator>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            AudioClip flashSFX = AssetBundleHandler.GetAudioClip("Flashbang");
            if (!flashSFX) return null;

            UpgradeBus.Instance.flashNoise = flashSFX;
            return UpgradeBus.Instance.SetupMultiplePurchaseableTerminalNode(UPGRADE_NAME, GetConfiguration().DiscombobulatorUpgradeConfiguration);

        }
    }
}
