using HarmonyLib;
using MoreShipUpgrades.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void Counter()
        {
            if (UpgradeBus.instance.flashCooldown > 0f)
            {
                UpgradeBus.instance.flashCooldown -= Time.deltaTime;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch("ParsePlayerSentence")]
        private static void DestroyObject(ref Terminal __instance, ref TerminalNode __result)
        {
            string text = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);
            if (text.ToLower() == "initattack" || text.ToLower() == "atk")
            {
                if (!UpgradeBus.instance.terminalFlash)
                {
                    TerminalNode failNode = new TerminalNode();
                    failNode.displayText = "You don't have access to this command yet. Purchase the 'Discombobulator'.";
                    failNode.clearPreviousText = true;
                    __result = failNode;
                    return;
                }
                if (UpgradeBus.instance.flashCooldown > 0f)
                {
                    TerminalNode failNode = new TerminalNode();
                    failNode.displayText = $"You can discombobulate again in {Mathf.Round(UpgradeBus.instance.flashCooldown)} seconds.\nType 'cooldown' or 'cd' to check discombobulation cooldown.";
                    failNode.clearPreviousText = true;
                    __result = failNode;
                }
                else
                {
                    __instance.terminalAudio.PlayOneShot(UpgradeBus.instance.flashNoise);
                    RoundManager.Instance.PlayAudibleNoise(__instance.transform.position, 60f, 0.8f, 0, false, 14155);
                    UpgradeBus.instance.flashCooldown = 120f;
                    Collider[] array = Physics.OverlapSphere(__instance.transform.position, 40f, 524288);
                    if (array.Length > 0)
                    {
                        float maxStunDuration = 0f;
                        foreach (Collider collider in array)
                        {
                            EnemyAICollisionDetect component = collider.GetComponent<EnemyAICollisionDetect>();
                            if (component != null)
                            {
                                float stunDuration = 7.5f; // Set the stun duration as needed
                                component.mainScript.SetEnemyStunned(true, stunDuration, null);
                                maxStunDuration = Mathf.Max(maxStunDuration, stunDuration);
                            }
                        }

                        // Display stun duration with countdown in the terminal
                        CoroutineStunDurationCountdown coroutine = new CoroutineStunDurationCountdown(maxStunDuration, __instance);
                        __instance.StartCoroutine(coroutine.Run());

                        CoroutineStunCountdownChat coroutineChat = new CoroutineStunCountdownChat(maxStunDuration, __instance);
                        __instance.StartCoroutine(coroutineChat.Run());

                        TerminalNode node = new TerminalNode();
                        node.clearPreviousText = true;
                        node.displayText = $"Stun grenade hit {array.Length} enemies.";
                        __result = node;
                    }
                    else
                    {
                        TerminalNode node = new TerminalNode();
                        node.displayText = "No stunned enemies detected.";
                        node.clearPreviousText = true;
                        __result = node;
                    }
                }
            }
            else if (text.ToLower() == "cooldown" || text.ToLower() == "cd")
            {
                if (!UpgradeBus.instance.terminalFlash)
                {
                    TerminalNode failNode = new TerminalNode();
                    failNode.displayText = "You don't have access to this command yet. Purchase 'Discombobulator'.";
                    failNode.clearPreviousText = true;
                    __result = failNode;
                    return;
                }
                if (UpgradeBus.instance.flashCooldown > 0f)
                {
                    CoroutineCooldownCountdown coroutine = new CoroutineCooldownCountdown(UpgradeBus.instance.flashCooldown, __instance);
                    __instance.StartCoroutine(coroutine.Run());

                    TerminalNode node = new TerminalNode();
                    node.clearPreviousText = true;
                    node.displayText = $"You can discombobulate again in {Mathf.Round(UpgradeBus.instance.flashCooldown)} seconds.";
                    __result = node;
                }
                else
                {
                    TerminalNode node = new TerminalNode();
                    node.displayText = "Discombobulate is ready, Type 'initattack' or 'atk' to execute.";
                    node.clearPreviousText = true;
                    __result = node;
                }
            }
            else if (text.ToLower() == "unlocks")
            {
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                node.displayText = "Purchased Upgrades:";
                if (UpgradeBus.instance.beekeeper) { node.displayText += "\n\nBeekeeper"; }
                if (UpgradeBus.instance.biggerLungs) { node.displayText += "\n\nBigger Lungs"; }
                if (UpgradeBus.instance.exoskeleton) { node.displayText += "\n\nBack Muscles"; }
                if (UpgradeBus.instance.softSteps) { node.displayText += "\n\nLight Footed"; }
                if (UpgradeBus.instance.nightVision) { node.displayText += "\n\nNight Vision"; }
                if (UpgradeBus.instance.runningShoes) { node.displayText += "\n\nRunning Shoes"; }
                if (UpgradeBus.instance.scannerUpgrade) { node.displayText += "\n\nBetter Scanner"; }
                if (UpgradeBus.instance.strongLegs) { node.displayText += "\n\nStrong Legs"; }
                if (UpgradeBus.instance.DestroyTraps) { node.displayText += "\n\nMalware Broadcaster"; }
                if (UpgradeBus.instance.terminalFlash) { node.displayText += "\n\nDiscombobulator"; }
                __result = node;
            }
            else if (text.ToLower() == "lategame")
            {
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                node.displayText = "Late Game Upgrades\n\nType unlocks to see which upgrades you've already purchased.\n\nThis mod patches and changes quite a bit and may conflict with problematic mods like LateCompany";
                node.displayText += "\n\nUpgrades arrive via the dropship but are not physical. You will see a red chat message when they are applied.";
                node.displayText += "\n\nUse the info command to get info about an item. EX: `info beekeeper`";
                node.displayText += "\n\nHave fun and please report bugs to the Lethal Company modding discord";
                __result = node;
            }
        }

        internal class CoroutineCooldownCountdown
        {
            private readonly float cooldown;
            private readonly Terminal terminal;

            public CoroutineCooldownCountdown(float cooldown, Terminal terminal)
            {
                this.cooldown = cooldown;
                this.terminal = terminal;
            }

            public IEnumerator Run()
            {
                float remainingCooldown = cooldown;

                while (remainingCooldown > 0f)
                {
                    TerminalNode countdownNode = new TerminalNode();
                    countdownNode.displayText = $"Remaining cooldown: {Mathf.Round(remainingCooldown)} seconds";
                    terminal.screenText.text += $"\n{countdownNode.displayText}";

                    // Append to chat
                    HUDManager.Instance.chatText.text += $"\n<color=#FF0000>Remaining cooldown: {Mathf.Round(remainingCooldown)} seconds";

                    yield return new WaitForSeconds(1f);
                    remainingCooldown -= 1f;
                }
            }
        }

        internal class CoroutineStunDurationCountdown
        {
            private float remainingTime;
            private Terminal terminal;

            public CoroutineStunDurationCountdown(float duration, Terminal terminal)
            {
                remainingTime = duration;
                this.terminal = terminal;
            }

            public IEnumerator Run()
            {
                while (remainingTime > 0f)
                {
                    TerminalNode countdownNode = new TerminalNode();
                    countdownNode.displayText = $"Remaining Stun Duration: {Mathf.Round(remainingTime)} seconds";
                    terminal.screenText.text += $"\n{countdownNode.displayText}";

                    yield return new WaitForSeconds(1f);
                    remainingTime -= 1f;
                }
            }
        }

        internal class CoroutineStunCountdownChat
        {
            private float remainingTime;
            private float lastDisplayedTime;
            private Terminal terminal;

            public CoroutineStunCountdownChat(float duration, Terminal terminal)
            {
                remainingTime = duration;
                lastDisplayedTime = Mathf.Round(duration);
                this.terminal = terminal;
            }

            public IEnumerator Run()
            {
                float startTime = Time.time;

                while (remainingTime > 0f)
                {
                    float roundedTime = Mathf.Round(remainingTime);

                    // Display in chat only when the rounded time changes
                    if (roundedTime != lastDisplayedTime)
                    {
                        HUDManager.Instance.chatText.text += $"\n<color=#FF0000>Stun Countdown: {roundedTime} seconds";
                        lastDisplayedTime = roundedTime;
                    }

                    yield return null;

                    // Calculate time elapsed since the start of the coroutine
                    float elapsed = Time.time - startTime;

                    // Adjust remaining time based on elapsed time
                    remainingTime = Mathf.Max(0f, remainingTime - elapsed);

                    // Update the start time for the next iteration
                    startTime = Time.time;
                }
            }
        }
    }
}