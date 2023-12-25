using MoreShipUpgrades.Managers;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    public class coilHeadItem : GrabbableObject
    {
        private bool Active, audioInit;
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
            if (isHeld || isHeldByEnemy) 
            {
                Active = false;
                anim.SetBool("Grounded", false);
            }
            else 
            {
                Active = true; 
                anim.SetBool("Grounded", true);
            }
        }

        public bool HasLineOfSightToPosition(Vector3 pos, int range = 60)
        {
            if(!Active) return false;
            float num = Vector3.Distance(base.transform.position, pos);
            bool result = num < (float)range && !Physics.Linecast(transform.position, pos, StartOfRound.Instance.collidersRoomDefaultAndFoliage, QueryTriggerInteraction.Ignore);
            return result;
        }

        public static bool HasLineOfSightToPeepers(Vector3 springPosition)
        {
            foreach (coilHeadItem peeper in UpgradeBus.instance.coilHeadItems)
            {
                if (peeper == null)
                {
                    UpgradeBus.instance.coilHeadItems.Remove(peeper);
                    continue;
                }
                if (peeper.HasLineOfSightToPosition(springPosition))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
