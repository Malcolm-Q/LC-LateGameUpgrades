using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using UnityEngine;

namespace MoreShipUpgrades.API
{
    internal static class UpgradeApi
    {
        public static GameObject RegisterOneTimeUpgrade<T>() where T : OneTimeUpgrade
        {
            return RegisterUpgrade<T>();
        }
        public static void RegisterOneTimeUpgrade<T>(GameObject upgradePrefab, bool registerNetworkPrefab = true) where T : OneTimeUpgrade
        {
            RegisterUpgrade<T>(upgradePrefab, registerNetworkPrefab);
        }
        public static GameObject RegisterTierUpgrade<T>() where T : TierUpgrade
        {
            return RegisterUpgrade<T>();
        }
        public static void RegisterTierUpgrade<T>(GameObject upgradePrefab, bool registerNetworkPrefab = true) where T : TierUpgrade
        {
            RegisterUpgrade<T>(upgradePrefab, registerNetworkPrefab);
        }
        public static GameObject RegisterUpgrade<T>() where T : BaseUpgrade
        {
            GameObject prefab = LethalLib.Modules.NetworkPrefabs.CreateNetworkPrefab(typeof(T).Name);
            prefab.hideFlags = HideFlags.HideAndDontSave;
            prefab.AddComponent<T>();
            UpgradeBus.Instance.upgradeTypes.Add(typeof(T));
            return prefab;
        }
        public static void RegisterUpgrade<T>(GameObject upgradePrefab, bool registerNetworkPrefab = true) where T : BaseUpgrade
        {
            upgradePrefab.AddComponent<T>();
            RegisterUpgrade(upgradePrefab, registerNetworkPrefab);
        }
        public static void RegisterUpgrade(GameObject upgradePrefab, bool registerNetworkPrefab = true)
        {
            upgradePrefab.hideFlags = HideFlags.HideAndDontSave;
            if (registerNetworkPrefab) LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(upgradePrefab);
            BaseUpgrade upgrade = upgradePrefab.GetComponent<BaseUpgrade>();
            UpgradeBus.Instance.upgradeTypes.Add(upgrade.GetType());
        }
    }
}
