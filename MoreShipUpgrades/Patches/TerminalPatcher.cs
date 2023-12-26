using GameNetcodeStuff;
using HarmonyLib;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.UpgradeComponents;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

namespace MoreShipUpgrades.Patches
{
    [HarmonyPatch(typeof(Terminal))]
    internal class TerminalPatcher
    {
        const string LOAD_LGU_COMMAND = "load lgu";

        [HarmonyPostfix]
        [HarmonyPatch("Update")]
        private static void Counter()
        {
            if(UpgradeBus.instance.flashCooldown > 0f)
            {
                UpgradeBus.instance.flashCooldown -= Time.deltaTime;
            }
        }
        
        private static TerminalNode DisplayTerminalMessage(string message, bool clearPreviousText = true)
        {
            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            node.displayText = message;
            node.clearPreviousText = clearPreviousText;
            return node;
        }

        [HarmonyPostfix]
        [HarmonyPatch("ParsePlayerSentence")]
        private static void CustomParser(ref Terminal __instance, ref TerminalNode __result)
        {
            string text = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);
            if (text.Split()[0].ToLower() == "toggle" && text.Split()[1].ToLower() == "lightning")
            {
                if (!UpgradeBus.instance.lightningRod)
                {
                    __result = DisplayTerminalMessage(lightningRodScript.ACCESS_DENIED_MESSAGE);
                    return;
                }
                __result = DisplayTerminalMessage(UpgradeBus.instance.lightningRodActive ? lightningRodScript.TOGGLE_ON_MESSAGE : lightningRodScript.TOGGLE_OFF_MESSAGE);
                return;
            }
            if (text.ToLower() == "initattack" || text.ToLower() == "atk")
            {
                if (!UpgradeBus.instance.terminalFlash)
                {
                    __result = DisplayTerminalMessage("You don't have access to this command yet. Purchase the 'Discombobulator'.");
                    return;
                }
                if (UpgradeBus.instance.flashCooldown > 0f)
                {
                    __result = DisplayTerminalMessage($"You can discombobulate again in {Mathf.Round(UpgradeBus.instance.flashCooldown)} seconds.\nType 'cooldown' or 'cd' to check discombobulation cooldown.");
                }
                else
                {
                    RoundManager.Instance.PlayAudibleNoise(__instance.transform.position, 60f, 0.8f, 0, false, 14155);
                    UpgradeBus.instance.flashScript.PlayAudioAndUpdateCooldownServerRpc();

                    TerminalNode node = new TerminalNode();
                    Collider[] array = Physics.OverlapSphere(__instance.transform.position, UpgradeBus.instance.cfg.DISCOMBOBULATOR_RADIUS, 524288);
                    if (array.Length > 0)
                    {
                        __result = DisplayTerminalMessage($"Stun grenade hit {array.Length} enemies.");
                        if (UpgradeBus.instance.cfg.DISCOMBOBULATOR_NOTIFY_CHAT)
                        {
                            __instance.StartCoroutine(CountDownChat(UpgradeBus.instance.cfg.DISCOMBOBULATOR_STUN_DURATION + (UpgradeBus.instance.cfg.DISCOMBOBULATOR_INCREMENT * UpgradeBus.instance.discoLevel)));
                        }
                    }
                    else
                    {
                        __result = DisplayTerminalMessage("No stunned enemies detected.");
                    }
                }
            }
            else if (text.ToLower() == "cooldown" || text.ToLower() == "cd")
            {
                if (!UpgradeBus.instance.terminalFlash)
                {
                    __result = DisplayTerminalMessage("You don't have access to this command yet. Purchase 'Discombobulator'.");
                    return;
                }
                if (UpgradeBus.instance.flashCooldown > 0f)
                {
                    __result = DisplayTerminalMessage($"You can discombobulate again in {Mathf.Round(UpgradeBus.instance.flashCooldown)} seconds.");
                }
                else
                {
                    __result = DisplayTerminalMessage("Discombobulate is ready, Type 'initattack' or 'atk' to execute.");
                }
            }
            else if (text.ToLower() == "lategame")
            {
                string displayText = "Late Game Upgrades\n\nType `lategame store` or `lgu` to view upgrades.\n\nMost of the mod is configurable via the config file in `BepInEx/config/`.";
                displayText += "\n\nUse the info command to get info about an item. EX: `info beekeeper`.";
                displayText += "\n\nYou must type the exact name of the upgrade (case insensitve).";
                displayText += "\n\nTo force wipe an lgu save file type `reset lgu`. (will only wipe the clients save).";
                displayText += "\n\nTo reapply any upgrades that failed to apply type `load lgu`.";
                displayText += "\n\nIn the case of credit desync to force an amount of credits type `forceCredits 123`, to attempt to sync credits type `syncCredits`";
                __result = DisplayTerminalMessage(displayText);
            }
            else if (text.ToLower() == "lategame store" || text.ToLower() == "lgu")
            {
                __result = UpgradeBus.instance.ConstructNode();
            }
            else if (text.ToLower() == "reset lgu")
            {
                __result = DisplayTerminalMessage("LGU save has been wiped.");
                if(LGUStore.instance.lguSave.playerSaves.ContainsKey(GameNetworkManager.Instance.localPlayerController.playerSteamId))
                {
                    UpgradeBus.instance.ResetAllValues(false);
                    SaveInfo saveInfo = new SaveInfo();
                    ulong id = GameNetworkManager.Instance.localPlayerController.playerSteamId;
                    LGUStore.instance.saveInfo = saveInfo;
                    LGUStore.instance.UpdateLGUSaveServerRpc(id, JsonConvert.SerializeObject(saveInfo));
                }
            }
            else if (text.Split()[0].ToLower() == "forcecredits")
            {
                if (int.TryParse(text.Split()[1], out int value))
                {
                    __instance.groupCredits = value;
                    __result = DisplayTerminalMessage($"This client now has {value} credits.  \n\nThis was intended to be used when credit desync occurs due to Bigger Lobby or More Company.\n");
                }
                else
                {
                    __result = DisplayTerminalMessage($"Failed to parse value {text.Split()[1]}.");
                }
                return;
            }
            else if (text.Split()[0].ToLower() == "synccredits")
            {
                __result = DisplayTerminalMessage($"Sending an RPC to sync all clients credits with your credits. ({__instance.groupCredits})");
                LGUStore.instance.SyncCreditsServerRpc(__instance.groupCredits);
            }
            else if (text.ToLower() == "intern" || text.ToLower() == "interns")
            {
                if(__instance.groupCredits < UpgradeBus.instance.cfg.INTERN_PRICE)
                {
                    __result = DisplayTerminalMessage($"Interns cost {UpgradeBus.instance.cfg.INTERN_PRICE} credits and you have {__instance.groupCredits} credits.\n");
                    return;
                }
                PlayerControllerB player = StartOfRound.Instance.mapScreen.targetedPlayer;
                if(!player.isPlayerDead)
                {
                    __result = DisplayTerminalMessage($"{player.playerUsername} is still alive, they can't be replaced with an intern.\n\n");
                    return;
                }
                __instance.groupCredits -= UpgradeBus.instance.cfg.INTERN_PRICE;
                LGUStore.instance.SyncCreditsServerRpc( __instance.groupCredits );
                UpgradeBus.instance.internScript.ReviveTargetedPlayerServerRpc();
                string name = UpgradeBus.instance.internNames[UnityEngine.Random.Range(0, UpgradeBus.instance.internNames.Length)];
                string interest = UpgradeBus.instance.internInterests[UnityEngine.Random.Range(0, UpgradeBus.instance.internInterests.Length)];
                __result = DisplayTerminalMessage($"{player.playerUsername} has been replaced with:\n\nNAME: {name}\nAGE: {UnityEngine.Random.Range(19, 76)}\nIQ: {UnityEngine.Random.Range(2, 160)}\nINTERESTS: {interest}\n\n{name} HAS BEEN TELEPORTED INSIDE THE FACILITY, PLEASE ACQUAINTANCE YOURSELF ACCORDINGLY");
            }
            else if (text.ToLower().Contains(LOAD_LGU_COMMAND))
            {
                if(text.ToLower() == LOAD_LGU_COMMAND)
                {
                    __result = DisplayTerminalMessage("Enter the name of the user whos upgrades/save you want to copy. Ex: `load lgu steve`\n");
                    return;
                }
                PlayerControllerB[] players = GameObject.FindObjectsOfType<PlayerControllerB>();
                List<string> playerNames = new List<string>();
                var playerNameToSearch = text.Substring(text.IndexOf(LOAD_LGU_COMMAND) + LOAD_LGU_COMMAND.Length).Trim();
                foreach (PlayerControllerB player in players)
                {
                    playerNames.Add(player.playerUsername);
                    if (player.playerUsername.ToLower() == playerNameToSearch.ToLower())
                    {
                        LGUStore.instance.ShareSaveServerRpc();
                        __instance.StartCoroutine(WaitForSync(player.playerSteamId));
                        __result = DisplayTerminalMessage($"Syncing with {player.playerUsername}\nThis should take 5 seconds\nPulling data...\n");
                        return;
                    }
                }
                __result = DisplayTerminalMessage($"The name {playerNameToSearch} was not found. The following names were found:\n{string.Join(", ", playerNames)}\n");
                return;
            }
            else if (text.ToLower().Contains("scan hives"))
            {
                if(UpgradeBus.instance.scanLevel < 1)
                {
                    __result = DisplayTerminalMessage("\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n");
                    return;
                }
                GrabbableObject[] scrapItems = GameObject.FindObjectsOfType<GrabbableObject>().ToArray();
                GrabbableObject[] filteredHives = scrapItems.Where(scrap => scrap.itemProperties.itemName == "Hive").ToArray();
                GrabbableObject[] bestHives = filteredHives.OrderByDescending(v => v.scrapValue).ToArray();
                string displayText = $"Found {bestHives.Length} Hives:";
                foreach(GrabbableObject scrap in bestHives)
                {
                    displayText += $"\n${scrap.scrapValue} // X: {scrap.gameObject.transform.position.x.ToString("F1")}, Y: {scrap.gameObject.transform.position.y.ToString("F1")}, Z: {scrap.gameObject.transform.position.z.ToString("F1")}";
                }
                displayText += "\nDon't forget your GPS!\n\n";
                __result = DisplayTerminalMessage(displayText);
            }
            else if (text.ToLower().Contains("scan scrap"))
            {
                if(UpgradeBus.instance.scanLevel < 1)
                {
                    __result = DisplayTerminalMessage("\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n");
                    return;
                }
                GrabbableObject[] scrapItems = GameObject.FindObjectsOfType<GrabbableObject>().ToArray();
                GrabbableObject[] filteredScrap = scrapItems.Where(scrap => scrap.isInFactory && scrap.itemProperties.isScrap).ToArray();
                GrabbableObject[] bestScrap = filteredScrap.OrderByDescending(v => v.scrapValue).Take(5).ToArray();
                string displayText = "Most valuable items:\n";
                foreach(GrabbableObject scrap in bestScrap)
                {
                    displayText += $"\n{scrap.itemProperties.itemName}: ${scrap.scrapValue}\nX: {Mathf.RoundToInt(scrap.gameObject.transform.position.x)}, Y: {Mathf.RoundToInt(scrap.gameObject.transform.position.y)}, Z: {Mathf.RoundToInt(scrap.gameObject.transform.position.z)}\n";
                }
                displayText += "\n\nDon't forget your GPS!\n\n";
                __result = DisplayTerminalMessage(displayText);
            }
            else if (text.ToLower().Contains("scan player"))
            {
                if(UpgradeBus.instance.scanLevel < 1)
                {
                    __result = DisplayTerminalMessage("\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n");
                    return;
                }
                PlayerControllerB[] players = GameObject.FindObjectsOfType<PlayerControllerB>().ToArray();
                PlayerControllerB[] filteredPlayers = players.Where(player => player.playerSteamId != 0).ToArray();
                PlayerControllerB[] alivePlayers = filteredPlayers.Where(player => !player.isPlayerDead).ToArray();
                PlayerControllerB[] deadPlayers = filteredPlayers.Where(player => player.isPlayerDead).ToArray();
                string displayText = "Alive Players:\n";
                foreach(PlayerControllerB player in alivePlayers)
                {
                    displayText += $"\n{player.playerUsername} - X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}";
                }
                displayText += "\nDead Players:\n";
                foreach(PlayerControllerB player in deadPlayers)
                {
                    displayText += $"\n{player.playerUsername} - X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}";
                }
                __result = DisplayTerminalMessage(displayText);
            }
            else if (text.ToLower().Contains("scan enemies"))
            {
                if(UpgradeBus.instance.scanLevel < 1)
                {
                    __result = DisplayTerminalMessage("\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n");
                    return;
                }
                EnemyAI[] enemies = GameObject.FindObjectsOfType<EnemyAI>().Where(enem => !enem.isEnemyDead).ToArray();
                string displayText = null;
                if(enemies.Length > 0)
                {
                    Dictionary<string, int> enemyCount = new Dictionary<string, int>();
                    if (!UpgradeBus.instance.cfg.VERBOSE_ENEMIES)
                    {
                        enemyCount.Add("Outside Enemies", 0);
                        enemyCount.Add("Inside Enemies", 0);
                        foreach (EnemyAI enemy in enemies)
                        {
                            if (enemy.isOutside) enemyCount["Outside Enemies"]++;
                            else enemyCount["Inside Enemies"]++;
                        }
                    }
                    else
                    {
                        foreach (EnemyAI enemy in enemies)
                        {
                            ScanNodeProperties scanNode = enemy.GetComponentInChildren<ScanNodeProperties>();
                            string realName = "";
                            if (scanNode != null) realName = scanNode.headerText; // this should resolve the issue with this command
                            else realName = "Unkown";
                            if (enemyCount.ContainsKey(realName)) { enemyCount[realName]++; }
                            else { enemyCount.Add(realName, 1); }
                        }
                        displayText = $"Alive Enemies: {enemies.Length}\n";
                        foreach (KeyValuePair<string, int> count in enemyCount)
                        {
                            displayText += $"\n{count.Key} - {count.Value}";
                        }
                    }
                }
                else
                {
                    displayText = "0 enemies detected\n\n";
                }
                __result = DisplayTerminalMessage(displayText);
            }
            else if (text.ToLower().Contains("scan doors"))
            {
                if(UpgradeBus.instance.scanLevel < 1)
                {
                    __result = DisplayTerminalMessage("\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n");
                    return;
                }
                List<GameObject> fireEscape = GameObject.FindObjectsOfType<GameObject>().Where(obj => obj.name == "SpawnEntranceBTrigger").ToList();
                List<EntranceTeleport> mainDoors = GameObject.FindObjectsOfType<EntranceTeleport>().ToList();
                List<EntranceTeleport> doorsToRemove = new List<EntranceTeleport>();

                foreach (EntranceTeleport door in mainDoors)
                {
                    if (door.gameObject.transform.position.y < -170)
                    {
                        doorsToRemove.Add(door);
                    }
                }
                foreach (EntranceTeleport doorToRemove in doorsToRemove)
                {
                    mainDoors.Remove(doorToRemove);
                    if (!fireEscape.Contains(doorToRemove.gameObject)) fireEscape.Add(doorToRemove.gameObject);
                }
                PlayerControllerB player = StartOfRound.Instance.mapScreen.targetedPlayer;

                string displayText = null;
                if(player.isInsideFactory)
                {
                    displayText = $"Closest exits to {player.playerUsername} (X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}):\n";
                    GameObject[] Closest3 = fireEscape.OrderBy(door => Vector3.Distance(door.transform.position, player.transform.position)).Take(3).ToArray();
                    foreach(GameObject door in fireEscape)
                    {
                        displayText += $"\nX:{Mathf.RoundToInt(door.transform.position.x)},Y:{Mathf.RoundToInt(door.transform.position.y)},Z:{Mathf.RoundToInt(door.transform.position.z)} - {Mathf.RoundToInt(Vector3.Distance(door.transform.position, player.transform.position))} units away.";
                    }
                }
                else
                {
                    displayText = $"Entrances for {player.playerUsername} (X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}):\n";
                    foreach(EntranceTeleport door in mainDoors)
                    {
                        displayText += $"\nX:{Mathf.RoundToInt(door.transform.position.x)},Y:{Mathf.RoundToInt(door.transform.position.y)},Z:{Mathf.RoundToInt(door.transform.position.z)} - {Mathf.RoundToInt(Vector3.Distance(door.transform.position, player.transform.position))} units away.";
                    }               
                }
                __result = DisplayTerminalMessage(displayText);
            }
            else if (text.ToLower().Split()[0] == "transmit")
            {
                string[] splits = text.Split();
                if(GameObject.FindObjectOfType<SignalTranslator>() == null)
                {
                    __result = DisplayTerminalMessage("You have to buy a Signal Translator to use this command\n\n");
                }
                else if (UpgradeBus.instance.pager)
                {
                    if (splits.Length == 1)
                    {
                        __result = DisplayTerminalMessage("You have to enter a message to broadcast\nEX: `page get back to the ship!`");
                    }
                    else
                    {
                        string msg = string.Join(" ", splits.Skip(1));
                        UpgradeBus.instance.pageScript.ReqBroadcastChatServerRpc(msg);
                        __result = DisplayTerminalMessage($"Broadcasted message: '{msg}'");
                    }
                }
            }
            else
            {
                foreach(CustomTerminalNode customNode in UpgradeBus.instance.terminalNodes)
                {
                    if(text.ToLower() == customNode.Name.ToLower())
                    {
                        string displayText = null;
                        int price = 0;
                        if (!customNode.Unlocked) { price = (int)(customNode.UnlockPrice * customNode.salePerc); }
                        else if(customNode.MaxUpgrade> customNode.CurrentUpgrade) { price = (int)(customNode.Prices[customNode.CurrentUpgrade] * customNode.salePerc); }
                       
                        bool canAfford = __instance.groupCredits >= price;
                        if(canAfford && (!customNode.Unlocked || customNode.MaxUpgrade > customNode.CurrentUpgrade))
                        {
                            LGUStore.instance.SyncCreditsServerRpc(__instance.groupCredits - price);
                            if (!customNode.Unlocked)
                            {
                                LGUStore.instance.HandleUpgrade(customNode.Name);
                                if (customNode.MaxUpgrade != 0) { displayText = $"You Upgraded {customNode.Name} to level {customNode.CurrentUpgrade + 1}  \n"; }
                                else { displayText = $"You Purchased {customNode.Name}  \n"; }
                            }
                            else if(customNode.Unlocked && customNode.MaxUpgrade > customNode.CurrentUpgrade)
                            {
                                LGUStore.instance.HandleUpgrade(customNode.Name, true);
                                displayText = $"You Upgraded {customNode.Name} to level {customNode.CurrentUpgrade + 1} \n";
                            }
                        }
                        else if(customNode.Unlocked && canAfford)
                        {
                            if (customNode.MaxUpgrade == 0) { displayText = "You already unlocked this upgrade.  \n"; }
                            else { displayText = "This upgrade is already max level  \n"; }
                        }
                        else
                        {
                            displayText = "You can't afford this item.  \n";
                        }
                        __result = DisplayTerminalMessage(displayText);
                    }
                    else if(text.ToLower() == $"info {customNode.Name.ToLower()}")
                    {
                        __result = DisplayTerminalMessage(customNode.Description + "\n\n");
                    }
                    else if (text.ToLower() == $"unload {customNode.Name.ToLower()}")
                    {
                        UpgradeBus.instance.UpgradeObjects[customNode.Name].GetComponent<BaseUpgrade>().Unwind();
                        LGUStore.instance.UpdateLGUSaveServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, JsonConvert.SerializeObject(new SaveInfo()));
                        customNode.Unlocked = false;
                        __result = DisplayTerminalMessage($"Unwinding {customNode.Name.ToLower()}");
                    }
                }
            }
        }

        private static IEnumerator WaitForSync(ulong id)
        {
            yield return new WaitForSeconds(3f);
            LGUStore.instance.saveInfo = LGUStore.instance.lguSave.playerSaves[id];
            LGUStore.instance.UpdateUpgradeBus(false);
            HUDManager.Instance.chatText.text += "\nAPPLYING RETRIEVED SAVE";
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
