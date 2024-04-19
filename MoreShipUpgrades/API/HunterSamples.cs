using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents.Items;
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
        /// <param name="registerNetworkPrefab">Wether to register the prefab to the Network Manager or not</param>
        /// <param name="grabbableToEnemies">Wether the enemies will be capable of holding this item or not</param>
        public static void RegisterSample(Item sampleItem, EnemyAI monster, int hunterLevel, bool registerNetworkPrefab = false, bool grabbableToEnemies = true)
        {
            if (monster == null)
            {
                Plugin.mls.LogError($"Script of the enemy was not provided to pull the name of its type.");
                return;
            }
            RegisterSample(sampleItem, monster.enemyType, hunterLevel, registerNetworkPrefab, grabbableToEnemies);
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
        /// <param name="registerNetworkPrefab">Wether to register the prefab to the Network Manager or not</param>
        /// <param name="grabbableToEnemies">Wether the enemies will be capable of holding this item or not</param>
        public static void RegisterSample(Item sampleItem, EnemyType monsterType, int hunterLevel, bool registerNetworkPrefab = false, bool grabbableToEnemies = true)
        {
            if (monsterType == null)
            {
                Plugin.mls.LogError($"An enemy type must be provided to know what monster will spawn the sample when killed.");
                return;
            }
            RegisterSample(sampleItem, monsterType.enemyName, hunterLevel, registerNetworkPrefab, grabbableToEnemies);
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
        /// <param name="registerNetworkPrefab">Wether to register the prefab to the Network Manager or not</param>
        /// <param name="grabbableToEnemies">Wether the enemies will be capable of holding this item or not</param>
        public static void RegisterSample(Item sampleItem, string monsterName, int hunterLevel, bool registerNetworkPrefab = false, bool grabbableToEnemies = true)
        {
            if (monsterName == "" || monsterName == null)
            {
                Plugin.mls.LogError($"A name must be provided in order to spawn the sample. The name must be the one set on the EnemyAI.enemyType.name.");
                return;
            }
            if (SpawnItemManager.Instance.samplePrefabs.ContainsKey(monsterName))
            {
                Plugin.mls.LogError($"An enemy was already registered as {monsterName} for its sample generation.");
                return;
            }
            if (sampleItem == null)
            {
                Plugin.mls.LogError($"Item properties scriptable object was not provided when registering a custom sample for enemy registered as {monsterName}.");
                return;
            }
            if (sampleItem.spawnPrefab == null)
            {
                Plugin.mls.LogError($"Item properties scriptable object does contain a spawn prefab when registering a custom sample for enemy registered as {monsterName}.");
                return;
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
            RegisterSampleItem(sampleItem, monsterName, registerNetworkPrefab, grabbableToEnemies);
            moddedLevels.Add(monsterName, hunterLevel-1);
        }

        internal static void RegisterSampleItem(Item sampleItem, string monsterName, bool registerNetworkPrefab = false, bool grabbableToEnemies = true)
        {
            MonsterSample sampleScript = sampleItem.spawnPrefab.AddComponent<MonsterSample>();
            sampleScript.grabbable = true;
            sampleScript.grabbableToEnemies = grabbableToEnemies;
            sampleScript.itemProperties = sampleItem;
            sampleItem.spawnPrefab.AddComponent<ScrapValueSyncer>();
            if (registerNetworkPrefab) LethalLib.Modules.NetworkPrefabs.RegisterNetworkPrefab(sampleItem.spawnPrefab);
            SpawnItemManager.Instance.samplePrefabs.Add(monsterName.ToLower(), sampleItem.spawnPrefab);
            LethalLib.Modules.Items.RegisterItem(sampleItem);
            Plugin.mls.LogInfo($"Registed sample for the enemy \"{monsterName}\"...");
        }
    }
}
