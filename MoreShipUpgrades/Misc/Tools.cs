using LethalLib.Modules;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    internal class Tools
    {
        public static void ShuffleList<T>(List<T> list)
        {
            if(list == null) throw new ArgumentNullException("list");

            System.Random random = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static bool SpawnMob(string mob, Vector3 position, int numToSpawn) // this could be moved to tools
        {
            for (int i = 0; i < RoundManager.Instance.currentLevel.Enemies.Count; i++)
            {
                if (RoundManager.Instance.currentLevel.Enemies[i].enemyType.enemyName == mob)
                {
                    for (int j = 0; j < numToSpawn; j++)
                    {
                        RoundManager.Instance.SpawnEnemyOnServer(position, 0f, i);
                    }
                    return true;
                }
            }
            return false;
        }

    }
}
