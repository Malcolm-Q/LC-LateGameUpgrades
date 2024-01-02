using HarmonyLib;
using System.IO;
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
            if (!File.Exists(filePath)) return;

            File.Delete(filePath);
        }
    }

}
