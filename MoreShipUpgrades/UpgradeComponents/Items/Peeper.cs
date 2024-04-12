using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Linq;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items
{
    public class Peeper : GrabbableObject, IDisplayInfo
    {
        internal const string ITEM_NAME = "Peeper";
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
            UpgradeBus.Instance.coilHeadItems.Add(this);
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
            foreach (Peeper peeper in UpgradeBus.Instance.coilHeadItems.ToList())
            {
                if (peeper == null)
                {
                    UpgradeBus.Instance.coilHeadItems.Remove(peeper);
                    continue;
                }
                if (peeper.HasLineOfSightToPosition(springPosition)) return true;
            }
            return false;
        }

        public string GetDisplayInfo()
        {
            return "Looks at coil heads, don't lose it";
        }
    }
}
