using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class lightningRodScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Lightning Rod";
        public static lightningRodScript instance;
        private static LGULogger logger;

        // Configuration
        public static string ENABLED_SECTION = $"Enable {UPGRADE_NAME} Upgrade";
        public static bool ENABLED_DEFAULT = true;
        public static string ENABLED_DESCRIPTION = "A device which redirects lightning bolts to the ship.";

        public static string PRICE_SECTION = $"{UPGRADE_NAME} Price";
        public static int PRICE_DEFAULT = 1000;

        public static string ACTIVE_SECTION = "Active on Purchase";
        public static bool ACTIVE_DEFAULT = true;
        public static string ACTIVE_DESCRIPTION = $"If true: {UPGRADE_NAME} will be active on purchase.";

        // Chat Messages
        private static string LOAD_COLOUR = "#FF0000";
        private static string LOAD_MESSAGE = $"\n<color={LOAD_COLOUR}>{UPGRADE_NAME} is active!</color>";

        private static string UNLOAD_COLOUR = LOAD_COLOUR;
        private static string UNLOAD_MESSAGE = $"\n<color={UNLOAD_COLOUR}>{UPGRADE_NAME} has been disabled</color>";

        // Toggle
        private static string ACCESS_DENIED_MESSAGE = $"You don't have access to this command yet. Purchase the '{UPGRADE_NAME}'.\n";
        private static string TOGGLE_ON_MESSAGE = $"{UPGRADE_NAME} has been enabled. Lightning bolts will now be redirected to the ship.\n";
        private static string TOGGLE_OFF_MESSAGE = $"{UPGRADE_NAME} has been disabled. Lightning bolts will no longer be redirected to the ship.\n";

        // distance
        public static string DIST_SECTION = $"Effective Distance of {UPGRADE_NAME}.";
        public static float DIST_DEFAULT = 175f;
        public static string DIST_DESCRIPTION = $"The closer you are the more likely the rod will reroute lightning.";

        public bool CanTryInterceptLightning { get; internal set; }
        public bool LightningIntercepted { get; internal set; }

        void Awake()
        {
            instance = this;
            logger = new LGULogger(UPGRADE_NAME);
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
            logger.LogInfo($"Distance from ship: {dist}");
            logger.LogInfo($"Effective distance of the lightning rod: {UpgradeBus.instance.cfg.LIGHTNING_ROD_DIST}");

            if (dist > UpgradeBus.instance.cfg.LIGHTNING_ROD_DIST) return;

            dist /= UpgradeBus.instance.cfg.LIGHTNING_ROD_DIST;
            float prob = 1 - dist;
            float rand = Random.value;

            logger.LogInfo($"Number to beat: {prob}");
            logger.LogInfo($"Number: {rand}");
            if (rand < prob)
            {
                logger.LogInfo("Planning interception...");
                __instance.staticElectricityParticle.Stop();
                instance.LightningIntercepted = true;
                LGUStore.instance.CoordinateInterceptionClientRpc();
            }
        }

        public static void RerouteLightningBolt(ref Vector3 strikePosition, ref StormyWeather __instance)
        {
            logger.LogInfo($"Intercepted Lightning Strike...");
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
