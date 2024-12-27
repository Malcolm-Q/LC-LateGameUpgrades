using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.Commands;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.UpgradeComponents.Commands
{
    public class Interns : BaseCommand
    {
        internal const string NAME = "Interns";
        public static Interns instance;

        List<PlayerControllerB> recentlyInterned;
        internal string[] internNames, internInterests;
        public enum TeleportRestriction
        {
            None,
            ExitBuilding,
            EnterShip,
        }
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            recentlyInterned = new();
            internNames = AssetBundleHandler.GetInfoFromJSON("InternNames").Split(",");
            internInterests = AssetBundleHandler.GetInfoFromJSON("InternInterests").Split(",");
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
            int health = 100;
            if (UpgradeBus.Instance.PluginConfiguration.StimpackConfiguration.Enabled)
            {
                bool playerRegistered = Stimpack.Instance.playerHealthLevels.ContainsKey(player.playerSteamId);
                health = playerRegistered ? Stimpack.GetHealthFromPlayer(100, player.playerSteamId) : 100;
            }
            player.ResetPlayerBloodObjects(player.isPlayerDead);

            if (player.isPlayerDead || player.isPlayerControlled)
            {
                player.isClimbingLadder = false;
                player.inVehicleAnimation = false;
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
            if (StartOfRound.Instance.currentLevel.spawnEnemiesAndScrap) recentlyInterned.Add(player);
        }
        internal void RemoveRecentlyInterned(PlayerControllerB player)
        {
            if (ContainsRecentlyInterned(player))
                recentlyInterned.Remove(player);
        }

        [ServerRpc(RequireOwnership = false)]
        internal void RemoveRecentlyInternedServerRpc(NetworkBehaviourReference player)
        {
            RemoveRecentlyInternedClientRpc(player);
        }

        [ClientRpc]
        internal void RemoveRecentlyInternedClientRpc(NetworkBehaviourReference player)
        {
            if (player.TryGet(out  PlayerControllerB playerController))
            {
                RemoveRecentlyInterned(playerController);
            }
        }

        internal void ResetRecentlyInterned()
        {
            recentlyInterned.Clear();
        }

        internal bool ContainsRecentlyInterned(PlayerControllerB player)
        {
            return recentlyInterned.Contains(player);
        }
        internal void AddRecentlyInterned(PlayerControllerB player)
        {

            if (!ContainsRecentlyInterned(player))
                recentlyInterned.Remove(player);
        }
        public static new void RegisterCommand()
        {
            SetupGenericCommand<Interns>(NAME);
        }

    }
}
