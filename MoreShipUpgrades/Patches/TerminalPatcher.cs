using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
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
        private static void CustomParser(ref Terminal __instance, ref TerminalNode __result)
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
                    RoundManager.Instance.PlayAudibleNoise(__instance.transform.position, 60f, 0.8f, 0, false, 14155);
                    UpgradeBus.instance.flashScript.PlayAudioAndUpdateCooldownServerRpc();

                    TerminalNode node = new TerminalNode();
                    Collider[] array = Physics.OverlapSphere(__instance.transform.position, UpgradeBus.instance.cfg.DISCOMBOBULATOR_RADIUS, 524288);
                    if (array.Length > 0)
                    {
                        node.displayText = $"Stun grenade hit {array.Length} enemies.";
                        node.clearPreviousText = true;
                        __result = node;
                        if (UpgradeBus.instance.cfg.DISCOMBOBULATOR_NOTIFY_CHAT)
                        {
                            __instance.StartCoroutine(CountDownChat(UpgradeBus.instance.cfg.DISCOMBOBULATOR_STUN_DURATION + (UpgradeBus.instance.cfg.DISCOMBOBULATOR_INCREMENT * UpgradeBus.instance.discoLevel)));
                        }
                    }
                    else
                    {
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
                    TerminalNode node = new TerminalNode();
                    node.displayText = $"You can discombobulate again in {Mathf.Round(UpgradeBus.instance.flashCooldown)} seconds.";
                    node.clearPreviousText = true;
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
            else if (text.Split()[0].ToLower() == "page")
            {
                string[] splits = text.Split();
                if (UpgradeBus.instance.pager)
                {
                    if (splits.Length == 1)
                    {
                        TerminalNode node = new TerminalNode();
                        node.displayText = "You have to enter a message to broadcast\nEX: `page get back to the ship!`";
                        node.clearPreviousText = true;
                        __result = node;
                    }
                    else
                    {
                        string msg = string.Join(" ", splits.Skip(1));
                        TerminalNode node = new TerminalNode();
                        node.clearPreviousText = true;
                        if(UpgradeBus.instance.pageScript.isOnCooldown)
                        {
                            node.displayText = $"Pager is on cooldown for {UpgradeBus.instance.pageScript.remainingCooldownTime} seconds!";
                        }
                        else
                        {
                            UpgradeBus.instance.pageScript.ReqBroadcastChatServerRpc(msg);
                            node.displayText = $"Broadcasted message: '{msg}'\n\nPager is now on cooldown for {UpgradeBus.instance.pageScript.cooldownDuration}";
                        }
                        __result = node;
                    }
                }
                else
                {
                    TerminalNode node = new TerminalNode();
                    node.displayText = "You don't have access to this command.\nPurchase the pager from lategame store.";
                    node.clearPreviousText = true;
                    __result = node;
                }
            }
            else if (text.ToLower() == "lategame")
            {
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                node.displayText = "Late Game Upgrades\n\nType `lategame store` or `lgu` to view upgrades.\n\nMost of the mod is configurable via the config file in `BepInEx/config/`.";
                node.displayText += "\n\nUse the info command to get info about an item. EX: `info beekeeper`.";
                node.displayText += "\n\nYou must type the exact name of the upgrade (case insensitve).";
                node.displayText += "\n\nTo force wipe an lgu save file type `reset lgu`.";
                node.displayText += "\n\nIf using latecompany and your client doesn't have the upgrades, type `load lgu`.";
                __result = node;
            }
            else if (text.ToLower() == "lategame store" || text.ToLower() == "lgu")
            {
                __result = UpgradeBus.instance.ConstructNode();
            }
            else if (text.ToLower() == "reset lgu")
            {
                TerminalNode node = new TerminalNode();
                node.displayText = "LGU save has been wiped.";
                node.clearPreviousText = true;
                __result = node;
                LGUStore.instance.PlayersFiredServerRpc();
            }
            else if (text.ToLower() == "load lgu")
            {
                BaseUpgrade[] upgrades = GameObject.FindObjectsOfType<BaseUpgrade>();
                foreach (BaseUpgrade upgrade in upgrades)
                {
                    upgrade.load();
                }
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                node.displayText = "Reapplied upgrade effects to this client. Only run this once.";
                __result = node;
            }
            else
            {
                foreach(CustomTerminalNode customNode in UpgradeBus.instance.terminalNodes)
                {
                    if(text.ToLower() == customNode.Name.ToLower())
                    {
                        TerminalNode node = new TerminalNode();
                        node.clearPreviousText = true;

                        bool canAfford = __instance.groupCredits >= customNode.Price;

                        if(canAfford && (!customNode.Unlocked || customNode.MaxUpgrade > customNode.CurrentUpgrade))
                        {
                            LGUStore.instance.SyncCreditsServerRpc(__instance.groupCredits - customNode.Price);
                            if (!customNode.Unlocked)
                            {
                                LGUStore.instance.HandleUpgrade(customNode.Name);
                                if (customNode.MaxUpgrade != 0) { node.displayText = $"You Upgraded {customNode.Name} to level {customNode.CurrentUpgrade + 1}  \n"; }
                                else { node.displayText = $"You Purchased {customNode.Name}  \n"; }
                            }
                            else if(customNode.Unlocked && customNode.MaxUpgrade > customNode.CurrentUpgrade)
                            {
                                LGUStore.instance.HandleUpgrade(customNode.Name, true);
                                node.displayText = $"You Upgraded {customNode.Name} to level {customNode.CurrentUpgrade + 1} \n";
                            }
                        }
                        else if(customNode.Unlocked && canAfford)
                        {
                            if (customNode.MaxUpgrade == 0) { node.displayText = "You already unlocked this upgrade.  \n"; }
                            else { node.displayText = "This upgrade is already max level  \n"; }
                        }
                        else
                        {
                            node.displayText = "You can't afford this item.  \n";
                        }
                        __result = node;
                    }
                    else if(text.ToLower() == $"info {customNode.Name.ToLower()}")
                    {
                        TerminalNode node = new TerminalNode();
                        node.displayText = customNode.Description + "\n\n";
                        node.clearPreviousText = true;
                        __result = node;
                    }
                }
            }
        }

        private static IEnumerator CountDownChat(float count)
        {
            HUDManager.Instance.chatText.text = "";
            HUDManager.Instance.chatText.text += $"<color=#FFFFFF>Stun Duration: {count.ToString("F1")} seconds.</color>";
            while(count > 0f)
            {
                yield return new WaitForSeconds(1f);
                count -= 1f;
                HUDManager.Instance.chatText.text = "";
                HUDManager.Instance.chatText.text += $"<color=#FFFFFF>Stun Duration: {count.ToString("F1")} seconds.</color>";
            }
            HUDManager.Instance.chatText.text = "";
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Effected enemies are no longer stunned!</color>";
        }
    }
}
