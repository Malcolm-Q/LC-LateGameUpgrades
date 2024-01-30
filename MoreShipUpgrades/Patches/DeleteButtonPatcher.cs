using HarmonyLib;
using MoreShipUpgrades.Misc;
using System.IO;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(DeleteFileButton))]
    internal class DeleteButtonPatcher
    {
        private static LGULogger logger = new LGULogger(nameof(DeleteButtonPatcher));
        [HarmonyPostfix]
        [HarmonyPatch(nameof(DeleteFileButton.DeleteFile))]
        private static void deleteLGUFile(DeleteFileButton __instance)
        {
            string filePath = Path.Combine(Application.persistentDataPath, $"LGU_{__instance.fileToDelete}.json");
            if (!File.Exists(filePath)) return;
            logger.LogDebug($"Deleting LGU file located at {filePath}");
            File.Delete(filePath);
        }
    }

}
