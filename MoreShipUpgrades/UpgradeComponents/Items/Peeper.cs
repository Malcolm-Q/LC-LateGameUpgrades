using MoreShipUpgrades.Managers;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items
{
    public class Peeper : GrabbableObject
    {
        /// <summary>
        /// Wether the instance of the class can stop coil-heads from moving or not
        /// </summary>
        private bool Active;
        /// <summary>
        /// The Animator component of the instance of the class
        /// </summary>
        private Animator anim;

        public override void Start()
        {
            base.Start();
            anim = GetComponent<Animator>();
            UpgradeBus.instance.coilHeadItems.Add(this);
        }
        public override void Update()
        {
            base.Update();
            SetActive(!isHeld && !isHeldByEnemy);
        }
        
        private void SetActive(bool enable)
        {
            Active = enable;
            anim.SetBool("Grounded", enable);
        }

        public bool HasLineOfSightToPosition(Vector3 pos, int range = 60)
        {
            if (!Active) return false;
            float num = Vector3.Distance(transform.position, pos);
            bool result = num < range && !Physics.Linecast(transform.position, pos, StartOfRound.Instance.collidersRoomDefaultAndFoliage, QueryTriggerInteraction.Ignore);
            return result;
        }

        public static bool HasLineOfSightToPeepers(Vector3 springPosition)
        {
            foreach (Peeper peeper in UpgradeBus.instance.coilHeadItems)
            {
                if (peeper == null)
                {
                    UpgradeBus.instance.coilHeadItems.Remove(peeper);
                    continue;
                }
                if (peeper.HasLineOfSightToPosition(springPosition)) return true;
            }
            return false;
        }
    }
}
