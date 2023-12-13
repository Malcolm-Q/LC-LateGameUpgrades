using MoreShipUpgrades.Managers;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    public class coilHeadItem : GrabbableObject
    {
        private bool Active;

        public override void Start()
        {
            base.Start();
            UpgradeBus.instance.coilHeadItems.Add(this);
        }
        public override void Update()
        {
            base.Update();
            if (isHeld || isHeldByEnemy) { Active = false; }
            else { Active = true; }
        }
        public bool HasLineOfSightToPosition(Vector3 pos, int range = 60)
        {
            if(!Active) return false;
            float num = Vector3.Distance(base.transform.position, pos);
            return num < (float)range && !Physics.Linecast(transform.position, pos, StartOfRound.Instance.collidersRoomDefaultAndFoliage, QueryTriggerInteraction.Ignore);
        }
    }
}
