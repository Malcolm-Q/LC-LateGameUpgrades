using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System.Collections.Generic;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items
{
    internal class MonsterSample : LategameItem
    {
        private static int usedMapSeed = -1;
        private static System.Random random = null;
        ParticleSystem particles;

        protected override bool KeepScanNode
        {
            get
            {
                return true;
            }
        }

        public override void Start()
        {
            base.Start();
            if (usedMapSeed < 0 || random == null || usedMapSeed != StartOfRound.Instance.randomMapSeed)
            {
                usedMapSeed = StartOfRound.Instance.randomMapSeed;
                random = new System.Random(usedMapSeed + 105);
            }
            GetComponent<ScrapValueSyncer>().SetScrapValue(random.Next(minValue: itemProperties.minValue, maxValue: itemProperties.maxValue));
            particles = GetComponentInChildren<ParticleSystem>();
        }
        public override void EquipItem()
        {
            base.EquipItem();
            ToggleParticles(toggle: true);
        }

        public override void DiscardItem()
        {
            base.DiscardItem();
            ToggleParticles(toggle: false);
        }

        public override void PocketItem()
        {
            base.PocketItem();
            ToggleParticles(toggle: false);
        }

        void ToggleParticles(bool toggle)
        {
            if (toggle)
            {
                particles.Play(true);
            }
            else
            {
                particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        public static new void LoadItem()
        {
            Dictionary<string, int> MINIMUM_VALUES = new Dictionary<string, int>()
            {
                { "centipede", UpgradeBus.Instance.PluginConfiguration.SNARE_FLEA_SAMPLE_MINIMUM_VALUE.Value },
                { "bunker spider", UpgradeBus.Instance.PluginConfiguration.BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE.Value },
                { "hoarding bug", UpgradeBus.Instance.PluginConfiguration.HOARDING_BUG_SAMPLE_MINIMUM_VALUE.Value },
                { "flowerman", UpgradeBus.Instance.PluginConfiguration.BRACKEN_SAMPLE_MINIMUM_VALUE.Value },
                { "mouthdog", UpgradeBus.Instance.PluginConfiguration.EYELESS_DOG_SAMPLE_MINIMUM_VALUE.Value },
                { "baboon hawk", UpgradeBus.Instance.PluginConfiguration.BABOON_HAWK_SAMPLE_MINIMUM_VALUE.Value },
                { "crawler", UpgradeBus.Instance.PluginConfiguration.THUMPER_SAMPLE_MINIMUM_VALUE.Value },
                { "forestgiant", UpgradeBus.Instance.PluginConfiguration.FOREST_KEEPER_SAMPLE_MINIMUM_VALUE.Value },
                { "manticoil", UpgradeBus.Instance.PluginConfiguration.MANTICOIL_SAMPLE_MINIMUM_VALUE.Value },
                { "tulip snake", UpgradeBus.Instance.PluginConfiguration.TULIP_SNAKE_SAMPLE_MINIMUM_VALUE.Value },
                { "bush wolf", UpgradeBus.Instance.PluginConfiguration.KIDNAPPER_FOX_SAMPLE_MINIMUM_VALUE.Value },
                { "puffer", UpgradeBus.Instance.PluginConfiguration.SPORE_LIZARD_SAMPLE_MINIMUM_VALUE.Value },
                { "maneater", UpgradeBus.Instance.PluginConfiguration.MANEATER_SAMPLE_MINIMUM_VALUE },
            };
            Dictionary<string, int> MAXIMUM_VALUES = new Dictionary<string, int>()
            {
                { "centipede", UpgradeBus.Instance.PluginConfiguration.SNARE_FLEA_SAMPLE_MAXIMUM_VALUE.Value },
                { "bunker spider", UpgradeBus.Instance.PluginConfiguration.BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE.Value },
                { "hoarding bug", UpgradeBus.Instance.PluginConfiguration.HOARDING_BUG_SAMPLE_MAXIMUM_VALUE.Value },
                { "flowerman", UpgradeBus.Instance.PluginConfiguration.BRACKEN_SAMPLE_MAXIMUM_VALUE.Value },
                { "mouthdog", UpgradeBus.Instance.PluginConfiguration.EYELESS_DOG_SAMPLE_MAXIMUM_VALUE.Value },
                { "baboon hawk", UpgradeBus.Instance.PluginConfiguration.BABOON_HAWK_SAMPLE_MAXIMUM_VALUE.Value },
                { "crawler", UpgradeBus.Instance.PluginConfiguration.THUMPER_SAMPLE_MAXIMUM_VALUE.Value },
                { "forestgiant", UpgradeBus.Instance.PluginConfiguration.FOREST_KEEPER_SAMPLE_MAXIMUM_VALUE.Value },
                { "manticoil", UpgradeBus.Instance.PluginConfiguration.MANTICOIL_SAMPLE_MAXIMUM_VALUE.Value },
                { "tulip snake", UpgradeBus.Instance.PluginConfiguration.TULIP_SNAKE_SAMPLE_MAXIMUM_VALUE.Value },
                { "bush wolf", UpgradeBus.Instance.PluginConfiguration.KIDNAPPER_FOX_SAMPLE_MAXIMUM_VALUE.Value },
                { "puffer", UpgradeBus.Instance.PluginConfiguration.SPORE_LIZARD_SAMPLE_MAXIMUM_VALUE.Value },
                { "maneater", UpgradeBus.Instance.PluginConfiguration.MANEATER_SAMPLE_MAXIMUM_VALUE },
            };
            foreach (string creatureName in AssetBundleHandler.samplePaths.Keys)
            {
                Item sample = AssetBundleHandler.GetItemObject(creatureName);
                sample.minValue = MINIMUM_VALUES[creatureName];
                sample.maxValue = MAXIMUM_VALUES[creatureName];
                API.HunterSamples.RegisterSampleItem(sample, creatureName, registerNetworkPrefab: true, grabbableToEnemies: true);
            }
        }
    }
}
