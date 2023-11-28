﻿using HarmonyLib;
using MoreShipUpgrades.Managers;
using System;
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
            if(UpgradeBus.instance.flashCooldown > 0f)
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
                if(!UpgradeBus.instance.terminalFlash)
                {
                    TerminalNode failNode = new TerminalNode();
                    failNode.displayText = "You don't have access to this command yet. Purchase the 'Discombobulator'.";
                    failNode.clearPreviousText = true;
                    __result = failNode;
                    return;
                }
                if(UpgradeBus.instance.flashCooldown > 0f)
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
                    if(array.Length > 0)
                    {
                        TerminalNode node = new TerminalNode();
                        node.clearPreviousText = true;

                        float stunDuration = 0f;
                        for (int i = 0; i < array.Length; i++)
                        {
                            EnemyAICollisionDetect component = array[i].GetComponent<EnemyAICollisionDetect>();
                            if (!(component == null))
                            {
                                // Get stun duration from SetEnemyStunned method
                                component.mainScript.SetEnemyStunned(true, 7.5f, null);
                                stunDuration = Mathf.Max(stunDuration, component.mainScript.stunDuration);
                            }
                        }

                        // Display stun duration with countdown in the terminal
                        CoroutineStunDurationCountdown coroutine = new CoroutineStunDurationCountdown(stunDuration, node, __instance);
                        __instance.StartCoroutine(coroutine.Run());

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
                if(!UpgradeBus.instance.terminalFlash)
                {
                    TerminalNode failNode = new TerminalNode();
                    failNode.displayText = "You don't have access to this command yet. Purchase 'Discombobulator'.";
                    failNode.clearPreviousText = true;
                    __result = failNode;
                    return;
                }
                if(UpgradeBus.instance.flashCooldown > 0f)
                {
                    TerminalNode node = new TerminalNode();
                    node.displayText = $"You can discombobulate again in {Mathf.Round(UpgradeBus.instance.flashCooldown)} seconds.";
                    node.clearPreviousText= true;
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
            else if(text.ToLower() == "unlocks")
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
            else if(text.ToLower() == "lategame")
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
    }

    internal class CoroutineStunDurationCountdown
    {
        private float duration;
        private TerminalNode node;
        private Terminal terminal;

        public CoroutineStunDurationCountdown(float duration, TerminalNode node, Terminal terminal)
        {
            this.duration = duration;
            this.node = node;
            this.terminal = terminal;
        }

        public IEnumerator Run()
        {
            float timer = duration;

            while (timer > 0f)
            {
                node.displayText = $"Stun grenade hit enemies. {Mathf.Round(timer)} seconds remaining.";
                terminal.UpdateTerminalDisplay();
                yield return null;
                timer -= Time.deltaTime;
            }

            // Reset the message after the countdown
            node.displayText = "Enemies are no longer stunned.";
            terminal.UpdateTerminalDisplay();

            // Display remaining cooldown
            if (UpgradeBus.instance.flashCooldown > 0f)
            {
                TerminalNode cooldownNode = new TerminalNode();
                cooldownNode.displayText = $"You can discombobulate again in {Mathf.Round(UpgradeBus.instance.flashCooldown)} seconds.";
                cooldownNode.clearPreviousText = true;
                terminal.screenText.Add(cooldownNode);
            }
        }
    }
}