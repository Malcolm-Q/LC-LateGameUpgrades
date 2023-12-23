
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class DivingKitScript : GrabbableObject
    {
        public override void Update()
        {
            if(playerHeldBy != null && playerHeldBy == GameNetworkManager.Instance.localPlayerController)
            {
                StartOfRound.Instance.drowningTimer = 1f;
            }
            base.Update();
        }
    }
}
