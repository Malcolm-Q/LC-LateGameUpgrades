using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class exoskeletonScript : BaseUpgrade
    {
        
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.backLevel++;
            UpdatePlayerWeight();
        }

        public override void load()
        {
            UpgradeBus.instance.exoskeleton = true;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Back Muscles is active!</color>";
            UpdatePlayerWeight();
        }
        public override void Unwind()
        {
            UpgradeBus.instance.exoskeleton = false;
            UpgradeBus.instance.backLevel = 0;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Back Muscles has been disabled.</color>";
            UpdatePlayerWeight();
        }
        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey("Back Muscles")) { UpgradeBus.instance.UpgradeObjects.Add("Back Muscles", gameObject); }
        }

        public static float CalculateWeight(float desiredValue, float minimumValue, float maximumValue)
        {
            float defaultWeight = Mathf.Clamp(desiredValue, minimumValue, maximumValue);
            if (!UpgradeBus.instance.exoskeleton) return defaultWeight;
            return defaultWeight * (UpgradeBus.instance.cfg.CARRY_WEIGHT_REDUCTION - (UpgradeBus.instance.backLevel * UpgradeBus.instance.cfg.CARRY_WEIGHT_INCREMENT));
        }
        public static void UpdatePlayerWeight()
        {
            PlayerControllerB player = GameNetworkManager.Instance.localPlayerController;
            if (UpgradeBus.instance.exoskeleton && player.ItemSlots.Length > 0)
            {
                UpgradeBus.instance.alteredWeight = 1f;
                for (int i = 0; i < player.ItemSlots.Length; i++)
                {
                    GrabbableObject obj = player.ItemSlots[i];
                    if (obj != null)
                    {
                        UpgradeBus.instance.alteredWeight += CalculateWeight(obj.itemProperties.weight - 1f, 0f, 10f);
                    }
                }
                player.carryWeight = UpgradeBus.instance.alteredWeight;
                if (player.carryWeight < 1f) { player.carryWeight = 1f; }
            }
        }
    }   

}
