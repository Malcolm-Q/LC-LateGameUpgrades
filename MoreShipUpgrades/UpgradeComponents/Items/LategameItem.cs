using MoreShipUpgrades.Misc;
using System;

namespace MoreShipUpgrades.UpgradeComponents.Items
{
    public abstract class LategameItem : GrabbableObject
    {
        public static void LoadItem()
        {
            throw new NotImplementedException();
        }

        protected abstract bool KeepScanNode { get; }

        public override void Start()
        {
            base.Start();
            if (!KeepScanNode) LguScanNodeProperties.RemoveScanNode(gameObject);
        }
    }
}
