using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(DeleteFileButton))]
    internal class DeleteButtonPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("DeleteFile")]
        private static void deleteLGUFile(DeleteFileButton __instance)
        {
            string filePath = Path.Combine(Application.persistentDataPath, $"LGU_{__instance.fileToDelete}.json");
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

}
