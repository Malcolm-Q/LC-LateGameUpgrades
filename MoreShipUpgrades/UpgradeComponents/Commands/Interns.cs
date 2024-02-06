using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace MoreShipUpgrades.UpgradeComponents.Commands
{
    public class Interns : NetworkBehaviour
    {
        public static string UPGRADE_NAME = "Interns";
        public static Interns instance;
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        [ServerRpc(RequireOwnership = false)]
        public void ReviveTargetedPlayerServerRpc()
        {
            ReviveTargetedPlayerClientRpc();

            Vector3 vector = RoundManager.Instance.insideAINodes[Random.Range(0, RoundManager.Instance.insideAINodes.Length)].transform.position;
            vector = RoundManager.Instance.GetRandomNavMeshPositionInRadiusSpherical(vector, 10f, default);
            NetworkBehaviourReference netRef = new NetworkBehaviourReference(StartOfRound.Instance.mapScreen.targetedPlayer);

            TelePlayerClientRpc(vector, netRef);
        }

        [ClientRpc]
        private void TelePlayerClientRpc(Vector3 vector, NetworkBehaviourReference netRef)
        {
            netRef.TryGet(out NetworkBehaviour player);
            if (player != null)
            {
                player.transform.GetComponent<PlayerControllerB>().TeleportPlayer(vector);
            }
        }

        [ClientRpc]
        private void ReviveTargetedPlayerClientRpc()
        {
            PlayerControllerB player = StartOfRound.Instance.mapScreen.targetedPlayer;
            bool playerRegistered = UpgradeBus.instance.playerHealthLevels.ContainsKey(player.playerSteamId);
            int health = playerRegistered ? Stimpack.GetHealthFromPlayer(100, player.playerSteamId) : 100;
            player.ResetPlayerBloodObjects(player.isPlayerDead);

            if (player.isPlayerDead || player.isPlayerControlled)
            {
                player.isClimbingLadder = false;
                player.ResetZAndXRotation();
                player.thisController.enabled = true;
                player.health = health;
                player.disableLookInput = false;
                if (player.isPlayerDead)
                {
                    player.isPlayerDead = false;
                    player.isPlayerControlled = true;
                    player.isInElevator = false;
                    player.isInHangarShipRoom = false;
                    player.isInsideFactory = true;
                    player.wasInElevatorLastFrame = false;
                    StartOfRound.Instance.SetPlayerObjectExtrapolate(false);
                    player.setPositionOfDeadPlayer = false;
                    player.helmetLight.enabled = false;
                    player.Crouch(false);
                    player.criticallyInjured = false;
                    if (player.playerBodyAnimator != null)
                    {
                        player.playerBodyAnimator.SetBool("Limp", false);
                    }
                    player.bleedingHeavily = false;
                    player.activatingItem = false;
                    player.twoHanded = false;
                    player.inSpecialInteractAnimation = false;
                    player.disableSyncInAnimation = false;
                    player.inAnimationWithEnemy = null;
                    player.holdingWalkieTalkie = false;
                    player.speakingToWalkieTalkie = false;
                    player.isSinking = false;
                    player.isUnderwater = false;
                    player.sinkingValue = 0f;
                    player.statusEffectAudio.Stop();
                    player.DisableJetpackControlsLocally();
                    player.health = health;
                    player.mapRadarDotAnimator.SetBool("dead", false);
                    player.deadBody = null;
                    if (player == GameNetworkManager.Instance.localPlayerController)
                    {
                        HUDManager.Instance.gasHelmetAnimator.SetBool("gasEmitting", false);
                        player.hasBegunSpectating = false;
                        HUDManager.Instance.RemoveSpectateUI();
                        HUDManager.Instance.gameOverAnimator.SetTrigger("revive");
                        player.hinderedMultiplier = 1f;
                        player.isMovementHindered = 0;
                        player.sourcesCausingSinking = 0;
                        HUDManager.Instance.HideHUD(false);
                    }
                }
                SoundManager.Instance.earsRingingTimer = 0f;
                player.voiceMuffledByEnemy = false;
                if (player.currentVoiceChatIngameSettings == null)
                {
                    StartOfRound.Instance.RefreshPlayerVoicePlaybackObjects();
                }
                if (player.currentVoiceChatIngameSettings != null)
                {
                    if (player.currentVoiceChatIngameSettings.voiceAudio == null)
                    {
                        player.currentVoiceChatIngameSettings.InitializeComponents();
                    }
                    if (player.currentVoiceChatIngameSettings.voiceAudio == null)
                    {
                        return;
                    }
                    player.currentVoiceChatIngameSettings.voiceAudio.GetComponent<OccludeAudio>().overridingLowPass = false;
                }
            }
            StartOfRound.Instance.livingPlayers++;
            if (GameNetworkManager.Instance.localPlayerController == player)
            {
                player.bleedingHeavily = false;
                player.criticallyInjured = false;
                player.playerBodyAnimator.SetBool("Limp", false);
                player.health = health;
                HUDManager.Instance.UpdateHealthUI(health, false);
                player.spectatedPlayerScript = null;
                HUDManager.Instance.audioListenerLowPass.enabled = false;
                StartOfRound.Instance.SetSpectateCameraToGameOverMode(false, player);
                TimeOfDay.Instance.DisableAllWeather(false);
                StartOfRound.Instance.UpdatePlayerVoiceEffects();
                player.thisPlayerModel.enabled = true;
            }
            else
            {
                player.thisPlayerModel.enabled = true;
                player.thisPlayerModelLOD1.enabled = true;
                player.thisPlayerModelLOD2.enabled = true;
            }
        }


    }
}
