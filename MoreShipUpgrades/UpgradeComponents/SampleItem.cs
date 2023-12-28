using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class SampleItem : PhysicsProp
    {
        public override void EquipItem()
        {
            ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
            if (particles != null)
            {
                particles.Play();
            }
            base.EquipItem();
        }

        public override void DiscardItem()
        {
            ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
            if (particles != null)
            {
                particles.Play();
            }
            base.DiscardItem();
        }

        public override void PocketItem()
        {
            ParticleSystem particles = GetComponentInChildren<ParticleSystem>();
            if (particles != null)
            {
                particles.Stop();
                particles.Clear();
            }
            base.PocketItem();
        }
    }
}
