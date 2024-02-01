using MoreShipUpgrades.Misc;
using UnityEngine;

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
        public override void Start()
        {
            base.Start();
            particles = GetComponentInChildren<ParticleSystem>();
            if (particles == null) logger.LogError($"Couldn't find {nameof(ParticleSystem)} component in the sample...");
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
