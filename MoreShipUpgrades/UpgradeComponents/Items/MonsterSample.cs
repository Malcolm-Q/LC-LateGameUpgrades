using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items
{
    internal class MonsterSample : PhysicsProp
    {
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static LguLogger logger = new LguLogger(nameof(MonsterSample));
        private static int usedMapSeed = -1;
        private static System.Random random = null;
        public override void Start()
        {
            base.Start();
            if (usedMapSeed < 0 || random == null || usedMapSeed != StartOfRound.Instance.randomMapSeed)
            {
                usedMapSeed = StartOfRound.Instance.randomMapSeed;
                random = new System.Random(usedMapSeed + 105);
            }
            GetComponent<ScrapValueSyncer>().SetScrapValue(random.Next(minValue: itemProperties.minValue, maxValue: itemProperties.maxValue));
        }
        public override void EquipItem()
        {
            base.EquipItem();
            ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
            particles.Play();
        }

        public override void DiscardItem()
        {
            base.DiscardItem();
            ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
            particles.Stop();
            particles.Clear();
        }

        public override void PocketItem()
        {
            base.PocketItem();
            ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
            particles.Stop();
            particles.Clear();
        }
    }
}
