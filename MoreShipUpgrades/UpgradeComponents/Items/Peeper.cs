using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Linq;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Items
{
    public class Peeper : LategameItem, IDisplayInfo
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
        bool KeepScanNode
        {
            get
            {
                return UpgradeBus.Instance.PluginConfiguration.PEEPER_SCAN_NODE;
            }
        }

        public override void Start()
        {
            base.Start();
            anim = GetComponent<Animator>();
            UpgradeBus.Instance.coilHeadItems.Add(this);
            if (!KeepScanNode) LguScanNodeProperties.RemoveScanNode(gameObject);
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

        public static new void LoadItem()
        {
            Item Peeper = AssetBundleHandler.GetItemObject("Peeper");
            if (Peeper == null) return;

            Peeper.creditsWorth = UpgradeBus.Instance.PluginConfiguration.PEEPER_PRICE.Value;
            Peeper.twoHanded = false;
            Peeper.itemId = 492017;
            Peeper.twoHandedAnimation = false;
            Peeper.spawnPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Peeper peepScript = Peeper.spawnPrefab.AddComponent<Peeper>();
            peepScript.itemProperties = Peeper;
            peepScript.grabbable = true;
            peepScript.grabbableToEnemies = true;
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(Peeper.spawnPrefab);

            UpgradeBus.Instance.ItemsToSync.Add("Peeper", Peeper);

            ItemManager.SetupStoreItem(Peeper);
        }
    }
}
