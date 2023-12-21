using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    internal class proteinPowderScript : BaseUpgrade
    {
        public static string UPGRADE_NAME = "Protein Powder";

        private static int CRIT_DAMAGE_VALUE = 100;

        // Configuration
        public static string ENABLED_SECTION = string.Format("Enable {0} Upgrade", UPGRADE_NAME);
        public static bool ENABLED_DEFAULT = true;
        public static string ENABLED_DESCRIPTION = "Do more damage with shovels";

        public static string PRICE_SECTION = string.Format("Price of {0} Upgrade", UPGRADE_NAME);
        public static int PRICE_DEFAULT = 1000;

        public static string UNLOCK_FORCE_SECTION = "Initial additional hit force";
        public static int UNLOCK_FORCE_DEFAULT = 1;
        public static string UNLOCK_FORCE_DESCRIPTION = "The value added to hit force on initial unlock.";

        public static string INCREMENT_FORCE_SECTION = "Additional hit force per level";
        public static int INCREMENT_FORCE_DEFAULT = 1;
        public static string INCREMENT_FORCE_DESCRIPTION = "Every time protein powder is upgraded this value will be added to the value above.";

        public static string INDIVIDUAL_SECTION = "Individual Purchase";
        public static bool INDIVIDUAL_DEFAULT = true;
        public static string INDIVIDUAL_DESCRIPTION = "If true: upgrade will apply only to the client that purchased it.";

        public static string PRICES_SECTION = "Price of each additional upgrade";
        public static string PRICES_DEFAULT = "700";
        public static string PRICES_DESCRIPTION = "Value must be seperated by commas EX: '123,321,222'";

        public static string CRIT_CHANCE_SECTION = "Chance of dealing a crit which will instakill the enemy.";
        public static float CRIT_CHANCE_DEFAULT = 0.01f;
        public static string CRIT_CHANCE_DESCRIPTION = "This value is only valid when maxed out Protein Powder. Any previous levels will not apply crit.";

        // Chat Messages
        private static string LOAD_COLOUR = "#FF0000";
        private static string LOAD_MESSAGE = string.Format("\n<color={0}>{1} is active!</color>", LOAD_COLOUR, UPGRADE_NAME);

        private static string UNLOAD_COLOUR = LOAD_COLOUR;
        private static string UNLOAD_MESSAGE = string.Format("\n<color={0}>{1} has been disabled</color>", UNLOAD_COLOUR, UPGRADE_NAME);

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            Register();
        }

        public override void Increment()
        {
            UpgradeBus.instance.proteinLevel++;
            LGUStore.instance.UpdateForceMultsServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, (UpgradeBus.instance.cfg.PROTEIN_INCREMENT * UpgradeBus.instance.proteinLevel)+UpgradeBus.instance.cfg.PROTEIN_UNLOCK_FORCE);
        }

        public override void load()
        {
            UpgradeBus.instance.proteinPowder = true;
            HUDManager.Instance.chatText.text += LOAD_MESSAGE;
            LGUStore.instance.UpdateForceMultsServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, UpgradeBus.instance.cfg.PROTEIN_UNLOCK_FORCE);
        }

        public override void Register()
        {
            if(!UpgradeBus.instance.UpgradeObjects.ContainsKey(UPGRADE_NAME)) { UpgradeBus.instance.UpgradeObjects.Add(UPGRADE_NAME, gameObject); }
        }

        public override void Unwind()
        {
            UpgradeBus.instance.proteinLevel = 0;
            UpgradeBus.instance.proteinPowder = false;
            HUDManager.Instance.chatText.text += UNLOAD_MESSAGE;
            LGUStore.instance.UpdateForceMultsServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, 0);
        }

        public static void DamageNearbyEnemies(ref Shovel __instance, ref List<RaycastHit> ___objectsHitByShovelList)
        {
            PlayerControllerB player = __instance.playerHeldBy;
            UnityEngine.Vector3 start = player.gameplayCamera.transform.position;
            for (int i = 0; i < ___objectsHitByShovelList.Count; i++)
            {
                IHittable hittable;
                RaycastHit hit;

                bool hitEnemy = ___objectsHitByShovelList[i].transform.TryGetComponent<IHittable>(out hittable) &&
                            !(___objectsHitByShovelList[i].transform == player.transform) &&
                            (___objectsHitByShovelList[i].point == UnityEngine.Vector3.zero ||
                            !Physics.Linecast(start, ___objectsHitByShovelList[i].point, out hit, StartOfRound.Instance.collidersAndRoomMaskAndDefault));
                if (hitEnemy)
                {
                    try
                    {
                        bool canCrit = TryToCritEnemy(ref player);
                        int damageValue = canCrit ? CRIT_DAMAGE_VALUE : UpgradeBus.instance.forceMults[player.playerSteamId];
                        hittable.Hit(damageValue, player.gameplayCamera.transform.forward, player, true);
                        Plugin.mls.LogInfo(string.Format("[{0}] Hitting the enemy with a force value of {1}", UPGRADE_NAME, damageValue));
                    }
                    catch (Exception e)
                    {
                        Plugin.mls.LogError(string.Format("[{0}] An error occurred when hitting the enemy: {1}", UPGRADE_NAME, e));
                    }
                }
                else if (___objectsHitByShovelList[i].transform.gameObject.layer == 8 || ___objectsHitByShovelList[i].transform.gameObject.layer == 11)
                {
                    break;
                }
            }
        }

        private static bool TryToCritEnemy(ref PlayerControllerB player)
        {
            Plugin.mls.LogInfo(string.Format("Levels purchaseable for Protein Powder: {0}",UpgradeBus.instance.cfg.PROTEIN_UPGRADE_PRICES.Split(',').Length));
            Plugin.mls.LogInfo("Current level on Protein Powder: " + UpgradeBus.instance.proteinLevel);

            bool reachedLastUpgrade = UpgradeBus.instance.proteinLevel == UpgradeBus.instance.cfg.PROTEIN_UPGRADE_PRICES.Split(',').Length;
            if (!reachedLastUpgrade) return false;

            return UnityEngine.Random.value < UpgradeBus.instance.cfg.PROTEIN_CRIT_CHANCE;
        }
    }
}
