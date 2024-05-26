using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoreShipUpgrades.API
{
    internal static class HunterSamples
    {
        internal static readonly Dictionary<string, int> moddedLevels = [];
        const float DEFAULT_ITEM_WEIGHT = 1.0f;
        /// <summary>
        /// Registers the provided item properties as a custom sample dropped by a monster named by its type when Hunter upgrade is active.
        /// <para></para>
        /// Two components will be added to the provided item:
        /// <para></para>
        /// A "MonsterSample" component which simply calculates the actual value of the item when spawned from a monster kill (and play any particle system located in the prefab when holding)
        /// <para></para>
        /// A "ScrapValueSyncer" component which ensures that all clients will see the same scrap value on the item
        /// </summary>
        /// <param name="sampleItem">Item properties of a sample to be registered unto the Hunter upgrade</param>
        /// <param name="monster">Script of the enemy that spawns the sample when killed.</param>
        /// <param name="hunterLevel">Level of Hunter upgrade for the sample to s</param>
        /// <param name="registerNetworkPrefab">Wether to register the prefab to the Network Manager or not</param>
        /// <param name="grabbableToEnemies">Wether the enemies will be capable of holding this item or not</param>
        /// <param name="weight">Weight value associated with the sample to influence the chance of spawning it on enemy kill</param>
        public static void RegisterSample(Item sampleItem, EnemyAI monster, int hunterLevel, bool registerNetworkPrefab = false, bool grabbableToEnemies = true, double weight = 50)
        {
            if (!CheckConditionsForRegister(monster)) return;

            RegisterSample(sampleItem, monster.enemyType, hunterLevel, registerNetworkPrefab, grabbableToEnemies, weight);
        }
        /// <summary>
        /// Registers the provided item properties as a custom sample dropped by a monster named by its type when Hunter upgrade is active.
        /// <para></para>
        /// Two components will be added to the provided item:
        /// <para></para>
        /// A "MonsterSample" component which simply calculates the actual value of the item when spawned from a monster kill (and play any particle system located in the prefab when holding)
        /// <para></para>
        /// A "ScrapValueSyncer" component which ensures that all clients will see the same scrap value on the item
        /// </summary>
        /// <param name="sampleItem">Item properties of a sample to be registered unto the Hunter upgrade</param>
        /// <param name="monsterType">Type of the enemy to retrieve its name.</param>
        /// <param name="hunterLevel">Level of Hunter upgrade for the sample to s</param>
        /// <param name="registerNetworkPrefab">Wether to register the prefab to the Network Manager or not</param>
        /// <param name="grabbableToEnemies">Wether the enemies will be capable of holding this item or not</param>
        /// <param name="weight">Weight value associated with the sample to influence the chance of spawning it on enemy kill</param>
        public static void RegisterSample(Item sampleItem, EnemyType monsterType, int hunterLevel, bool registerNetworkPrefab = false, bool grabbableToEnemies = true, double weight = 50)
        {
            if (!CheckConditionsForRegister(monsterType)) return;

            RegisterSample(sampleItem, monsterType.enemyName, hunterLevel, registerNetworkPrefab, grabbableToEnemies, weight);
        }
        /// <summary>
        /// Registers the provided item properties as a custom sample dropped by a monster named by its type when Hunter upgrade is active.
        /// <para></para>
        /// Two components will be added to the provided item:
        /// <para></para>
        /// A "MonsterSample" component which simply calculates the actual value of the item when spawned from a monster kill (and play any particle system located in the prefab when holding)
        /// <para></para>
        /// A "ScrapValueSyncer" component which ensures that all clients will see the same scrap value on the item
        /// </summary>
        /// <param name="sampleItem">Item properties of a sample to be registered unto the Hunter upgrade</param>
        /// <param name="monsterName">Name of the enemy that spawns the sample when killed.</param>
        /// <param name="hunterLevel">Level of Hunter upgrade for the sample to s</param>
        /// <param name="registerNetworkPrefab">Wether to register the prefab to the Network Manager or not</param>
        /// <param name="grabbableToEnemies">Wether the enemies will be capable of holding this item or not</param>
        /// <param name="weight">Weight value associated with the sample to influence the chance of spawning it on enemy kill</param>
        public static void RegisterSample(Item sampleItem, string monsterName, int hunterLevel, bool registerNetworkPrefab = false, bool grabbableToEnemies = true, double weight = 50)
        {
            if (!CheckConditionsForRegister(ref sampleItem, ref monsterName, ref hunterLevel, ref weight)) return;

            RegisterSampleItem(sampleItem, monsterName, registerNetworkPrefab, grabbableToEnemies, weight);
            if (!moddedLevels.ContainsKey(monsterName)) moddedLevels.Add(monsterName, hunterLevel-1);
        }
        /// <summary>
        /// Generic register where you can specify what grabbableObject component you wish to add to the item registered as a sample and will use the provided arguments into its script behaviour.
        /// <para></para>
        /// Three components will be added to the provided item:
        /// <para></para>
        /// A "SampleComponent" component which focuses on picking a scrap value between your item properties' minimum and maximum value.
        /// <para></para>
        /// A "ScrapValueSyncer" component which ensures that all clients will see the same scrap value on the item
        /// <para></para>
        /// Your "GrabbableObject" derived component so that you do your own logic on the item
        /// </summary>
        /// <typeparam name="T">Type of "GrabbableObject" component to be inserted into the registered item</typeparam>
        /// <param name="sampleItem">Item properties of a sample to be registered unto the Hunter upgrade</param>
        /// <param name="monster">Script of the enemy that spawns the sample when killed.</param>
        /// <param name="hunterLevel">Level of Hunter upgrade for the sample to s</param>
        /// <param name="registerNetworkPrefab">Wether to register the prefab to the Network Manager or not</param>
        /// <param name="weight">Weight value associated with the sample to influence the chance of spawning it on enemy kill</param>
        public static void RegisterSample<T>(Item sampleItem, EnemyAI monster, int hunterLevel, bool registerNetworkPrefab = false, double weight = 50) where T : GrabbableObject
        {
            if (!CheckConditionsForRegister(monster)) return;

            RegisterSample<T>(sampleItem, monster.enemyType, hunterLevel, registerNetworkPrefab, weight);
        }

        /// <summary>
        /// Generic register where you can specify what grabbableObject component you wish to add to the item registered as a sample and will use the provided arguments into its script behaviour.
        /// <para></para>
        /// Three components will be added to the provided item:
        /// <para></para>
        /// A "SampleComponent" component which focuses on picking a scrap value between your item properties' minimum and maximum value.
        /// <para></para>
        /// A "ScrapValueSyncer" component which ensures that all clients will see the same scrap value on the item
        /// <para></para>
        /// Your "GrabbableObject" derived component so that you do your own logic on the item
        /// </summary>
        /// <typeparam name="T">Type of "GrabbableObject" component to be inserted into the registered item</typeparam>
        /// <param name="sampleItem">Item properties of a sample to be registered unto the Hunter upgrade</param>
        /// <param name="monsterType">Type of the enemy to retrieve its name.</param>
        /// <param name="hunterLevel">Level of Hunter upgrade for the sample to s</param>
        /// <param name="registerNetworkPrefab">Wether to register the prefab to the Network Manager or not</param>
        /// <param name="weight">Weight value associated with the sample to influence the chance of spawning it on enemy kill</param>
        public static void RegisterSample<T>(Item sampleItem, EnemyType monsterType, int hunterLevel, bool registerNetworkPrefab = false, double weight = 50) where T : GrabbableObject
        {
            if (!CheckConditionsForRegister(monsterType)) return;

            RegisterSample<T>(sampleItem, monsterType.enemyName, hunterLevel, registerNetworkPrefab, weight);
        }

        /// <summary>
        /// Generic register where you can specify what grabbableObject component you wish to add to the item registered as a sample and will use the provided arguments into its script behaviour.
        /// <para></para>
        /// Three components will be added to the provided item:
        /// <para></para>
        /// A "SampleComponent" component which focuses on picking a scrap value between your item properties' minimum and maximum value.
        /// <para></para>
        /// A "ScrapValueSyncer" component which ensures that all clients will see the same scrap value on the item
        /// <para></para>
        /// Your "GrabbableObject" derived component so that you do your own logic on the item
        /// </summary>
        /// <typeparam name="T">Type of "GrabbableObject" component to be inserted into the registered item</typeparam>
        /// <param name="sampleItem">Item properties of a sample to be registered unto the Hunter upgrade</param>
        /// <param name="monsterName">Name of the enemy that spawns the sample when killed.</param>
        /// <param name="hunterLevel">Level of Hunter upgrade for the sample to s</param>
        /// <param name="registerNetworkPrefab">Wether to register the prefab to the Network Manager or not</param>
        /// <param name="weight">Weight value associated with the sample to influence the chance of spawning it on enemy kill</param>
        public static void RegisterSample<T>(Item sampleItem, string monsterName, int hunterLevel, bool registerNetworkPrefab = false, double weight = 50) where T : GrabbableObject
        {
            if (!CheckConditionsForRegister(ref sampleItem, ref monsterName, ref hunterLevel, ref weight)) return;

            RegisterSampleItem<T>(sampleItem, monsterName, registerNetworkPrefab, weight);
            if (!moddedLevels.ContainsKey(monsterName)) moddedLevels.Add(monsterName, hunterLevel - 1);
        }

        public static bool IsHunterEnabled()
        {
            return UpgradeBus.Instance.PluginConfiguration.HUNTER_ENABLED.Value;
        }

        internal static void RegisterSampleItem<T>(Item sampleItem, string monsterName, bool registerNetworkPrefab = false, double weight = 50) where T : GrabbableObject
        {
            GrabbableObject sampleScript = sampleItem.spawnPrefab.AddComponent<T>();
            sampleScript.grabbable = true;
            sampleScript.itemProperties = sampleItem;
            sampleItem.spawnPrefab.AddComponent<SampleComponent>();
            sampleItem.spawnPrefab.AddComponent<ScrapValueSyncer>();
            if (registerNetworkPrefab) LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(sampleItem.spawnPrefab);
            if (!ItemManager.Instance.samplePrefabs.ContainsKey(monsterName.ToLower())) ItemManager.Instance.samplePrefabs.Add(monsterName.ToLower(), new WeightingGroup<GameObject>());
            ItemManager.Instance.samplePrefabs[monsterName.ToLower()].Add(sampleItem.spawnPrefab, weight);
            LethalLib.Modules.Items.RegisterItem(sampleItem);
            Plugin.mls.LogInfo($"Registed sample for the enemy \"{monsterName}\"...");
        }
        internal static void RegisterSampleItem(Item sampleItem, string monsterName, bool registerNetworkPrefab = false, bool grabbableToEnemies = true, double weight = 50)
        {
            GrabbableObject sampleScript = sampleItem.spawnPrefab.GetComponent<GrabbableObject>();
            if (sampleScript == null) sampleScript = sampleItem.spawnPrefab.AddComponent<MonsterSample>();
            else sampleItem.spawnPrefab.AddComponent<SampleComponent>();
            sampleScript.grabbable = true;
            sampleScript.grabbableToEnemies = grabbableToEnemies;
            sampleScript.itemProperties = sampleItem;
            sampleItem.spawnPrefab.AddComponent<ScrapValueSyncer>();
            if (registerNetworkPrefab) LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(sampleItem.spawnPrefab);
            if (!ItemManager.Instance.samplePrefabs.ContainsKey(monsterName.ToLower())) ItemManager.Instance.samplePrefabs.Add(monsterName.ToLower(), new WeightingGroup<GameObject>());
            ItemManager.Instance.samplePrefabs[monsterName.ToLower()].Add(sampleItem.spawnPrefab, weight);
            LethalLib.Modules.Items.RegisterItem(sampleItem);
            Plugin.mls.LogInfo($"Registed sample for the enemy \"{monsterName}\"...");
        }
        static bool CheckConditionsForRegister(EnemyAI monster)
        {
            if (monster == null)
            {
                Plugin.mls.LogError($"Script of the enemy was not provided to pull the name of its type.");
                return false;
            }
            return true;
        }
        static bool CheckConditionsForRegister(EnemyType monsterType)
        {
            if (monsterType == null)
            {
                Plugin.mls.LogError($"An enemy type must be provided to know what monster will spawn the sample when killed.");
                return false;
            }
            return true;
        }
        static bool CheckConditionsForRegister(ref Item sampleItem, ref string monsterName, ref int hunterLevel, ref double weight)
        {
            if (monsterName == "" || monsterName == null)
            {
                Plugin.mls.LogError($"A name must be provided in order to spawn the sample. The name must be the one set on the EnemyAI.enemyType.name.");
                return false;
            }
            if (sampleItem == null)
            {
                Plugin.mls.LogError($"Item properties scriptable object was not provided when registering a custom sample for enemy registered as {monsterName}.");
                return false;
            }
            if (sampleItem.spawnPrefab == null)
            {
                Plugin.mls.LogError($"Item properties scriptable object does not contain a spawn prefab when registering a custom sample for enemy registered as {monsterName}.");
                return false;
            }
            if (sampleItem.itemIcon == null)
            {
                Plugin.mls.LogWarning($"Provided item properties scriptable object does not have an icon sprite for when it's shown in the inventory.");
                Plugin.mls.LogWarning($"It will be shown as blank white square when picked up the sample for the enemy \"{monsterName}\"...");
            }
            if (sampleItem.grabSFX == null || sampleItem.dropSFX == null || sampleItem.pocketSFX == null || sampleItem.throwSFX == null)
            {
                Plugin.mls.LogWarning($"One or more sounds have been found missing in the provided item properties.");
                Plugin.mls.LogWarning($"Errors/Warnings related to sounds might be caused by this sample item for the enemy \"{monsterName}\".");
            }
            if (sampleItem.weight < DEFAULT_ITEM_WEIGHT)
            {
                Plugin.mls.LogWarning($"Provided item properties scriptable object has a weight value inferior to default, leading to losing weight when grabbed by the player.");
                Plugin.mls.LogWarning($"Defaulting to default value of 1.0 on the sample item for the enemy \"{monsterName}\"...");
                sampleItem.weight = DEFAULT_ITEM_WEIGHT;
            }
            if (sampleItem.minValue > sampleItem.maxValue)
            {
                Plugin.mls.LogWarning($"The minimum scrap value of the registering sample for enemy registered as {monsterName} can not surpass the maximum scrap value.");
                Plugin.mls.LogWarning($"Defaulting to equal values...");
                sampleItem.minValue = sampleItem.maxValue;
            }
            if (!sampleItem.isScrap)
            {
                Plugin.mls.LogWarning($"Provided item properties scriptable object is not considered as a scrap item.");
                Plugin.mls.LogWarning($"Defaulting the check to being considered scrap on the sample item for the enemy \"{monsterName}\"...");
                sampleItem.isScrap = true;
            }
            if (!sampleItem.itemSpawnsOnGround)
            {
                Plugin.mls.LogWarning($"Provided item properties scriptable object is considered to not spawn on the ground.");
                Plugin.mls.LogWarning($"Defaulting the check to spawn on the ground on the sample item for the enemy \"{monsterName}\"...");
                sampleItem.itemSpawnsOnGround = true;
            }
            if (hunterLevel <= 0)
            {
                Plugin.mls.LogWarning($"Provided hunter level is not valid for sample registration.");
                Plugin.mls.LogWarning($"Defaulting the hunter level to 1 for the sample item for the enemy \"{monsterName}\"...");
                hunterLevel = 1;
            }
            if (hunterLevel > UpgradeBus.Instance.PluginConfiguration.HUNTER_UPGRADE_PRICES.Value.Split(",").Length)
            {
                Plugin.mls.LogWarning($"Provided hunter level is too high for sample registration.");
                Plugin.mls.LogWarning($"Defaulting the hunter level to the maximum allowed level for the sample item for the enemy \"{monsterName}\"...");
                Plugin.mls.LogWarning($"This is a configuration error, not a developer error as they cannot predict what is the maximum level Hunter upgrade can be without messing with internals (for now anyways).");
                hunterLevel = UpgradeBus.Instance.PluginConfiguration.HUNTER_UPGRADE_PRICES.Value.Split(",").Length;
            }
            if (weight <= 0)
            {
                Plugin.mls.LogWarning($"Provided spawn weight is not valid for sample registration.");
                Plugin.mls.LogWarning($"Defaulting the spawn weight value to 50 for the sample item for the enemy \"{monsterName}\"...");
                weight = 50;
            }
            return true;
        }
    }
}
