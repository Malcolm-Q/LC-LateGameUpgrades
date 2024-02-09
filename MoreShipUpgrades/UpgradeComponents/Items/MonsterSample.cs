using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;
using System;

namespace MoreShipUpgrades.UpgradeComponents.Items
{
    internal class MonsterSample : PhysicsProp
    {
        /// <summary>
        /// The ParticleSystem component linked to the sample
        /// </summary>
        private ParticleSystem particles;
        /// <summary>
        /// Logger of the class
        /// </summary>
        private static LGULogger logger = new LGULogger(nameof(MonsterSample));
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
            particles = GetComponentInChildren<ParticleSystem>();
            if (particles == null) logger.LogError($"Couldn't find {nameof(ParticleSystem)} component in the sample...");
            if (scrapValue > 0) return;
            GetComponent<ScrapValueSyncer>().SetScrapValue(random.Next(minValue: itemProperties.minValue, maxValue: itemProperties.maxValue));
        }
        public override void EquipItem()
        {
            base.EquipItem();
            particles?.Play();
        }

        public override void DiscardItem()
        {
            base.DiscardItem();
            particles?.Stop();
            particles?.Clear();
        }

        public override void PocketItem()
        {
            base.PocketItem();
            particles?.Stop();
            particles?.Clear();
        }
    }
}
