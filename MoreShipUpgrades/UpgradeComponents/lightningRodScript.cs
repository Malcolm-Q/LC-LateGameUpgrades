using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class lightningRodScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Lightning Rod";
        public static lightningRodScript instance;

        // Configuration
        public static string ENABLED_SECTION = string.Format("Enable {0} Upgrade", UPGRADE_NAME);
        public static bool ENABLED_DEFAULT = true;
        public static string ENABLED_DESCRIPTION = "A device which redirects lightning bolts to the ship.";

        public static string PRICE_SECTION = string.Format("{0} Price", UPGRADE_NAME);
        public static int PRICE_DEFAULT = 1000;

        public static string ACTIVE_SECTION = "Active on Purchase";
        public static bool ACTIVE_DEFAULT = true;
        public static string ACTIVE_DESCRIPTION = string.Format("If true: {0} will be active on purchase.", UPGRADE_NAME);

        // Chat Messages
        private static string LOAD_COLOUR = "#FF0000";
        private static string LOAD_MESSAGE = string.Format("\n<color={0}>{1} is active!</color>", LOAD_COLOUR, UPGRADE_NAME);

        private static string UNLOAD_COLOUR = LOAD_COLOUR;
        private static string UNLOAD_MESSAGE = string.Format("\n<color={0}>{1} has been disabled</color>", UNLOAD_COLOUR, UPGRADE_NAME);

        // Toggle
        public static string ACCESS_DENIED_MESSAGE = string.Format("You don't have access to this command yet. Purchase the '{0}'.\n", UPGRADE_NAME);
        public static string TOGGLE_ON_MESSAGE = string.Format("{0} has been enabled. Lightning bolts will now be redirected to the ship.\n", UPGRADE_NAME);
        public static string TOGGLE_OFF_MESSAGE = string.Format("{0} has been disabled. Lightning bolts will no longer be redirected to the ship.\n", UPGRADE_NAME);

        // distance
        public static string DIST_SECTION = "Effective Distance of lightning rod.";
        public static float DIST_DEFAULT = 175f;
        public static string DIST_DESCRIPTION = string.Format("The closer you are the more likely the rod will reroute lightning.", UPGRADE_NAME);

        public bool CanTryInterceptLightning { get; internal set; }
        public bool LightningIntercepted { get; internal set; }

        void Awake()
        {
            instance = this;
        }

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
            UpgradeBus.instance.lightningRodActive = false;
            HUDManager.Instance.chatText.text += UNLOAD_MESSAGE;
        }

        public static void TryInterceptLightning(ref StormyWeather __instance, ref GrabbableObject ___targetingMetalObject)
        {
            if (!instance.CanTryInterceptLightning) return;
            instance.CanTryInterceptLightning = false;

            Terminal terminal = UpgradeBus.instance.GetTerminal();
            float dist = Vector3.Distance(___targetingMetalObject.transform.position, terminal.transform.position);
            Plugin.mls.LogInfo(string.Format("[{0}] Distance from ship: {1}\n", UPGRADE_NAME, dist));
            Plugin.mls.LogInfo(string.Format("[{0}] Effective distance of the lightning rod: {1}", UPGRADE_NAME, UpgradeBus.instance.cfg.LIGHTNING_ROD_DIST));

            if (dist > UpgradeBus.instance.cfg.LIGHTNING_ROD_DIST) return;

            dist /= UpgradeBus.instance.cfg.LIGHTNING_ROD_DIST;
            float prob = 1 - dist;
            float rand = Random.value;

            Plugin.mls.LogInfo(string.Format("[{0}] Number to beat: {1}",UPGRADE_NAME, prob));
            Plugin.mls.LogInfo(string.Format("[{0}] Number: {1}",UPGRADE_NAME, rand));
            if (rand < prob)
            {
                Plugin.mls.LogInfo(string.Format("[{0}] Planning interception...", UPGRADE_NAME));
                __instance.staticElectricityParticle.Stop();
                instance.LightningIntercepted = true;
                LGUStore.instance.CoordinateInterceptionClientRpc();
            }
        }

        public static void RerouteLightningBolt(ref Vector3 strikePosition, ref StormyWeather __instance)
        {
            Plugin.mls.LogInfo(string.Format("[{0}] Intercepted Lightning Strike...", UPGRADE_NAME));
            Terminal terminal = UpgradeBus.instance.GetTerminal();
            strikePosition = terminal.transform.position;
            instance.LightningIntercepted = false;
            __instance.staticElectricityParticle.gameObject.SetActive(true);
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
