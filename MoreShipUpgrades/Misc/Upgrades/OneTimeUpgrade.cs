using BepInEx.Logging;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.Misc.Upgrades
{
    /// <summary>
    /// Class which represents upgrades that are only purchased only once to gain the full effect of it
    /// </summary>
    abstract class OneTimeUpgrade : BaseUpgrade, IOneTimeUpgradeDisplayInfo
    {
        #region Overriden Methods
        internal override void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }
        public override void Load()
        {
            string loadColour = "#FF0000";
            string loadMessage = $"\n<color={loadColour}>{upgradeName} is active!</color>";
            HUDManager.Instance.chatText.text += loadMessage;
        }

        public override void Register()
        {
            UnityEngine.Debug.Log($"{upgradeName}");
            if (!UpgradeBus.instance.UpgradeObjects.ContainsKey(upgradeName)) { UpgradeBus.instance.UpgradeObjects.Add(upgradeName, gameObject); }
        }

        public override void Unwind()
        {
            string unloadColour = "#FF0000";
            string unloadMessage = $"\n<color={unloadColour}>{upgradeName} has been disabled!</color>";
            HUDManager.Instance.chatText.text += unloadMessage;
        }
        #endregion
        #region Interface Methods
        public abstract string GetDisplayInfo(int price = -1);
        #endregion
    }
}
