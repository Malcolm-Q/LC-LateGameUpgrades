using BepInEx;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using UnityEngine;
using MoreShipUpgrades.Managers;
using System.IO;
using System.Reflection;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Items;
using System.Linq;
using MoreShipUpgrades.Compat;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Commands;
using MoreShipUpgrades.Misc.UI.Application;
using InteractiveTerminalAPI.UI;

namespace MoreShipUpgrades
{
    [BepInEx.BepInPlugin(Metadata.GUID,Metadata.NAME,Metadata.VERSION)]
    [BepInDependency("evaisa.lethallib","0.13.0")]
    [BepInDependency("com.sigurd.csync")]
    [BepInDependency("com.rune580.LethalCompanyInputUtils")]
    [BepInDependency("WhiteSpike.InteractiveTerminalAPI")]
    public class Plugin : BaseUnityPlugin
    {
        internal static readonly ManualLogSource mls = BepInEx.Logging.Logger.CreateLogSource(Metadata.NAME);

        public new static LategameConfiguration Config;

        void Awake()
        {
            Config = new LategameConfiguration(base.Config);

            // netcode patching stuff
            IEnumerable<Type> types;
            try
            {
                types = Assembly.GetExecutingAssembly().GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                types = e.Types.Where(t => t != null);
            }

            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }

            string assetDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "shipupgrades");
            AssetBundle UpgradeAssets = AssetBundle.LoadFromFile(assetDir);

            GameObject gameObject = new GameObject("UpgradeBus");
            gameObject.hideFlags = HideFlags.HideAndDontSave;
            gameObject.AddComponent<UpgradeBus>();
            gameObject = new GameObject("SpawnItemManager");
            gameObject.AddComponent<ItemManager>();

            UpgradeBus.Instance.UpgradeAssets = UpgradeAssets;
            SetupModStore(ref UpgradeAssets);

            SetupItems(ref types);
            SetupCommands(ref types);
            SetupPerks(ref types);

            ContractManager.SetupContractMapObjects(ref UpgradeAssets);
            InputUtilsCompat.Init();
            PatchManager.PatchMainVersion();

            InteractiveTerminalManager.RegisterApplication<UpgradeStoreApplication>(["lgu", "lategame store"]);
            InteractiveTerminalManager.RegisterApplication<WeatherProbeApplication>("probe");
            InteractiveTerminalManager.RegisterApplication<ExtendDeadlineApplication>("extend deadline");
            InteractiveTerminalManager.RegisterApplication<ContractApplication>("contracts");

            mls.LogInfo($"{Metadata.NAME} {Metadata.VERSION} has been loaded successfully.");
        }

        private void SetupModStore(ref AssetBundle bundle)
        {
            GameObject modStore = AssetBundleHandler.TryLoadGameObjectAsset(ref bundle, "Assets/ShipUpgrades/LguStore.prefab");
            if (modStore == null) return;

            modStore.AddComponent<ContractManager>();
            modStore.AddComponent<LguStore>();
            LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(modStore);
            UpgradeBus.Instance.modStorePrefab = modStore;
        }
        private void SetupItems(ref IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof(LategameItem))) continue;
                if (type == typeof(LategameItem)) continue;
                UpgradeBus.Instance.itemTypes.Add(type);

                MethodInfo method = type.GetMethod(nameof(LategameItem.LoadItem), BindingFlags.Static | BindingFlags.Public);
                if (method == null) continue;
                method.Invoke(null, null);
            }
            mls.LogInfo("Items have been setup");
        }
        private void SetupPerks(ref IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof(BaseUpgrade))) continue;
                if (type == typeof(OneTimeUpgrade) || type == typeof(TierUpgrade) || type == typeof(GameAttributeTierUpgrade)) continue;
                UpgradeBus.Instance.upgradeTypes.Add(type);

                MethodInfo method = type.GetMethod(nameof(BaseUpgrade.RegisterUpgrade), BindingFlags.Static | BindingFlags.Public);
                method.Invoke(null, null);
            }
            mls.LogInfo("Upgrades have been setup");
        }

        private void SetupCommands(ref IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof(BaseCommand))) continue;
                UpgradeBus.Instance.commandTypes.Add(type);

                MethodInfo method = type.GetMethod(nameof(BaseCommand.RegisterCommand), BindingFlags.Static | BindingFlags.Public);
                method.Invoke(null, null);
            }
            mls.LogInfo("Commands have been setup");
        }
    }   
}
