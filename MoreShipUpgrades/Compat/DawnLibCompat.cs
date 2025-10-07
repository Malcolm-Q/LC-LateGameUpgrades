using Dawn;

namespace MoreShipUpgrades.Compat
{
	internal class DawnLibCompat
	{
		public static bool Enabled =>
			BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(DawnLib.PLUGIN_GUID);
	}
}
