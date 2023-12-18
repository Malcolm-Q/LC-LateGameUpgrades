using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class lightningRodScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Lightning Rod";

        // Configuration
        public static string ENABLED_SECTION = string.Format("Enable {0} Upgrade", UPGRADE_NAME);
        public static bool ENABLED_DEFAULT = true;
        public static string ENABLED_DESCRIPTION = "A device which redirects lightning bolts to the ship.";

        public static string PRICE_SECTION = string.Format("{0} Price", UPGRADE_NAME);
        public static int PRICE_DEFAULT = 1000;

        public static string PROBABILITY_SECTION = string.Format("Probability that {0} will redirect a lightning bolt to the ship.", UPGRADE_NAME);
        public static float PROBABILITY_DEFAULT = 1.0f;
        public static string PROBABILITY_DESCRIPTION = "Values from [0,1], being 0 equivalent to zero chance and 1 being guaranteed to redirect.";

        public static string ACTIVE_SECTION = "Active on Purchase";
        public static bool ACTIVE_DEFAULT = true;
        public static string ACTIVE_DESCRIPTION = string.Format("If true: {0} will be active on purchase.", UPGRADE_NAME);

        // Chat Messages
        private static string LOAD_COLOUR = "#FF0000";
        private static string LOAD_MESSAGE = string.Format("\n<color={0}>{1} is active!</color>", LOAD_COLOUR, UPGRADE_NAME);

        private static string UNLOAD_COLOUR = LOAD_COLOUR;
        private static string UNLOAD_MESSAGE = string.Format("\n<color={0}>{1} has been disabled</color>", UNLOAD_COLOUR, UPGRADE_NAME);

        // Toggle
        private static string ACCESS_DENIED_MESSAGE = string.Format("You don't have access to this command yet. Purchase the '{0}'.\n", lightningRodScript.UPGRADE_NAME);
        private static string TOGGLE_ON_MESSAGE = string.Format("{0} has been enabled. Lightning bolts will now be redirected to the ship.\n", UPGRADE_NAME);
        private static string TOGGLE_OFF_MESSAGE = string.Format("{0} has been disabled. Lightning bolts will no longer be redirected to the ship.\n", UPGRADE_NAME);

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.UpgradeObjects.Add(UPGRADE_NAME, gameObject);
        }

        public override void Increment()
        {

        }

        public override void load()
        {
            UpgradeBus.instance.lightningRod = true;
            UpgradeBus.instance.lightningRodProbability = UpgradeBus.instance.cfg.LIGHTNING_ROD_PROBABILITY;
            UpgradeBus.instance.lightningRodActive = UpgradeBus.instance.cfg.LIGHTNING_ROD_ACTIVE;
            HUDManager.Instance.chatText.text += LOAD_MESSAGE;
        }

        public override void Register()
        {
            if (!UpgradeBus.instance.UpgradeObjects.ContainsKey(UPGRADE_NAME)) { UpgradeBus.instance.UpgradeObjects.Add(UPGRADE_NAME, gameObject); }
        }

        public override void Unwind()
        {
            UpgradeBus.instance.lightningRod = false;
            UpgradeBus.instance.lightningRodProbability = 0f;
            UpgradeBus.instance.lightningRodActive = false;
            HUDManager.Instance.chatText.text += UNLOAD_MESSAGE;
        }

        public static void TryCatchLightningBolt(ref Vector3 explosionPosition)
        {
            if (UpgradeBus.instance.lightningRodActive && Random.Range(0, 1) <= UpgradeBus.instance.lightningRodProbability)
            {
                Terminal terminal = GameObject.Find("TerminalScript").GetComponent<Terminal>();
                explosionPosition = terminal.transform.position;
            }
        }

        public static void ToggleLightningRod(ref TerminalNode __result)
        {
            UpgradeBus.instance.lightningRodActive = !UpgradeBus.instance.lightningRodActive;
            TerminalNode infoNode = new TerminalNode();
            infoNode.displayText = UpgradeBus.instance.lightningRodActive ? TOGGLE_ON_MESSAGE: TOGGLE_OFF_MESSAGE;
            infoNode.clearPreviousText = true;
            __result = infoNode;
        }

        public static void AccessDeniedMessage(ref TerminalNode __result)
        {
            TerminalNode failNode = new TerminalNode();
            failNode.displayText = ACCESS_DENIED_MESSAGE;
            failNode.clearPreviousText = true;
            __result = failNode;
        }
    }
}
