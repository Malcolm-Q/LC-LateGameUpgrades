using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents
{
    public class radarFlashScript : BaseUpgrade
    {
        // The radar booster we wish to stun enemies around of
        public RadarBoosterItem selectedRadar;
        public static string UPGRADE_NAME = "Booster Shock";

        // Configuration
        public static string ENABLED_CONFIGURATION = string.Format("Enable {0} Upgrade", UPGRADE_NAME);
        public static string ENABLED_DESCRIPTION = "Stun enemies near specified radar booster while pinging.";
        public static bool ENABLED_DEFAULT = true;

        public static string PRICE_CONFIGURATION = string.Format("Price of {0} Upgrade", UPGRADE_NAME);
        public static int PRICE_DEFAULT = 500;

        public static string COOLDOWN_CONFIGURATION = string.Format("{0} Cooldown", UPGRADE_NAME);
        public static float COOLDOWN_DEFAULT = 60f;

        public static string RADIUS_CONFIGURATION = string.Format("{0} Effect Radius", UPGRADE_NAME);
        public static float RADIUS_DEFAULT = 10f;

        public static string STUN_DURATION_CONFIGURATION = string.Format("{0} Stun Duration", UPGRADE_NAME);
        public static float STUN_DURATION_DEFAULT = 4.0f;

        public static string NOTIFY_CHAT_CONFIGURATION = "Notify Local Chat of Enemy Stun Duration";
        public static bool NOTIFY_CHAT_DEFAULT = true;

        public static string INCREMENT_CONFIGURATION = string.Format("{0} Increment", UPGRADE_NAME);
        public static string INCREMENT_DESCRIPTION = "The amount added to stun duration on upgrade.";
        public static float INCREMENT_DEFAULT = 1f;

        public static string PRICES_DEFAULT = "250,300,450,500";
        public static bool INDIVIDUAL_DEFAULT = true;

        // Chat notifications
        private static string LOAD_OUTPUT = "\n<color={0}>{1} is active!</color>";
        private static string LOAD_COLOUR = "#FF0000";

        private static string UNLOAD_OUTPUT = "\n<color={0}>{1} has been disabled.</color>";
        private static string UNLOAD_COLOUR = "#FF0000";

        // Terminal Responses
        public static string ON_COOLDOWN = "Pinged radar booster.\n{0} is on cooldown for {1} seconds.\n";
        public static string STUNNED_ENEMIES = "Pinged radar booster and stunned nearby enemies.\n";

        // Info Messages
        // It's different from infoJson because the user should have some idea of the range of the stun.
        public static string INITIAL_LEVEL_INFO = "LVL {0} - ${1}: Stuns enemies around the pinged radar booster for {2} seconds at {3} meter radius.\n";

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            UpgradeBus.instance.UpgradeObjects.Add(UPGRADE_NAME, gameObject);
        }

        public override void load()
        {
            UpgradeBus.instance.radarFlashScript = (radarFlashScript)this;
            UpgradeBus.instance.radarFlash = true;

            HUDManager.Instance.chatText.text += string.Format(LOAD_OUTPUT, LOAD_COLOUR, UPGRADE_NAME);
        }
        public override void Register()
        {
            if (!UpgradeBus.instance.UpgradeObjects.ContainsKey(UPGRADE_NAME)) { UpgradeBus.instance.UpgradeObjects.Add(UPGRADE_NAME, gameObject); }
        }
        public override void Increment()
        {
            UpgradeBus.instance.radarFlashLevel++;
        }
        public override void Unwind()
        {
            UpgradeBus.instance.radarFlash = false;
            UpgradeBus.instance.radarFlashLevel = 0;
            HUDManager.Instance.chatText.text += string.Format(UNLOAD_OUTPUT, UNLOAD_COLOUR, UPGRADE_NAME);
        }

        [ServerRpc(RequireOwnership = false)]
        public void PlayAudioAndUpdateRadarCooldownServerRpc()
        {
            PlayAudioAndUpdateRadarCooldownClientRpc();
        }

        [ClientRpc]
        private void PlayAudioAndUpdateRadarCooldownClientRpc()
        {
            selectedRadar.pingAudio.maxDistance = 100f;
            selectedRadar.pingAudio.PlayOneShot(UpgradeBus.instance.radarFlashNoise);
            StartCoroutine(ResetRange(selectedRadar));
            UpgradeBus.instance.radarFlashCooldown = UpgradeBus.instance.cfg.RADAR_BOOSTER_SHOCKWAVE_COOLDOWN;
            Collider[] array = Physics.OverlapSphere(selectedRadar.transform.position, UpgradeBus.instance.cfg.RADAR_BOOSTER_SHOCKWAVE_RADIUS, 524288);
            if (array.Length > 0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    EnemyAICollisionDetect component = array[i].GetComponent<EnemyAICollisionDetect>();
                    if (component != null)
                    {
                        component.mainScript.SetEnemyStunned(true, UpgradeBus.instance.cfg.RADAR_BOOSTER_SHOCKWAVE_STUN_DURATION + (UpgradeBus.instance.cfg.RADAR_BOOSTER_SHOCKWAVE_INCREMENT * UpgradeBus.instance.radarFlashLevel));
                    }
                }
            }
            selectedRadar = null;
        }
        private IEnumerator ResetRange(RadarBoosterItem radar)
        {
            yield return new WaitForSeconds(2f);
            radar.pingAudio.maxDistance = 17f;

        }
    }
}
