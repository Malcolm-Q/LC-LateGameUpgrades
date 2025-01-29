

using MissileTurret;

namespace MoreShipUpgrades.Compat
{
    internal static class MissileTurretsCompat
    {
        public static bool Enabled =>
            BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("Finnerex.MissileTurret");

        internal static bool IsMissileTurret(ref TerminalAccessibleObject possibleHazard)
        {
            return possibleHazard.GetComponent<MissileTurretAI>() != null;
        }
    }
}
