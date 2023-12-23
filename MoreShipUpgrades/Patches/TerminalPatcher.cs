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
        

        [HarmonyPostfix]
        [HarmonyPatch("ParsePlayerSentence")]
        private static void CustomParser(ref Terminal __instance, ref TerminalNode __result)
        {
            string text = __instance.screenText.text.Substring(__instance.screenText.text.Length - __instance.textAdded);
            if (text.Split()[0].ToLower() == "toggle" && text.Split()[1].ToLower() == "lightning")
            {
                if (!UpgradeBus.instance.lightningRod)
                {
                    lightningRodScript.AccessDeniedMessage(ref __result);
                    return;
                }
                lightningRodScript.ToggleLightningRod(ref __result);
            }
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
            else if (text.ToLower() == "lategame")
            {
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                node.displayText = "Late Game Upgrades\n\nType `lategame store` or `lgu` to view upgrades.\n\nMost of the mod is configurable via the config file in `BepInEx/config/`.";
                node.displayText += "\n\nUse the info command to get info about an item. EX: `info beekeeper`.";
                node.displayText += "\n\nYou must type the exact name of the upgrade (case insensitve).";
                node.displayText += "\n\nTo force wipe an lgu save file type `reset lgu`. (will only wipe the clients save).";
                node.displayText += "\n\nTo reapply any upgrades that failed to apply type `load lgu`.";
                node.displayText += "\n\nIn the case of credit desync to force an amount of credits type `forceCredits 123`, to attempt to sync credits type `syncCredits`";
                __result = node;
            }
            else if (text.ToLower() == "lategame store" || text.ToLower() == "lgu")
            {
                __result = UpgradeBus.instance.ConstructNode();
            }
            else if (text.ToLower() == "reset lgu")
            {
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                node.displayText = "LGU save has been wiped.";
                __result = node;
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
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                if (int.TryParse(text.Split()[1], out int value))
                {
                    __instance.groupCredits = value;
                    node.displayText = $"This client now has {value} credits.  \n\nThis was intended to be used when credit desync occurs due to Bigger Lobby or More Company.";
                }
                else
                {
                    node.displayText = $"Failed to parse value {text.Split()[1]}.";
                }
                __result = node;
            }
            else if (text.Split()[0].ToLower() == "synccredits")
            {
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                node.displayText = $"Sending an RPC to sync all clients credits with your credits. ({__instance.groupCredits})";
                __result = node;
                LGUStore.instance.SyncCreditsServerRpc(__instance.groupCredits);
            }
            else if (text.ToLower() == "intern" || text.ToLower() == "interns")
            {
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;

                if(__instance.groupCredits < UpgradeBus.instance.cfg.INTERN_PRICE)
                {
                    node.displayText = $"Interns cost {UpgradeBus.instance.cfg.INTERN_PRICE} credits and you have {__instance.groupCredits} credits.\n";
                    __result = node;
                    return;
                }
                PlayerControllerB player = StartOfRound.Instance.mapScreen.targetedPlayer;
                if(!player.isPlayerDead)
                {
                    node.displayText = $"{player.playerUsername} is still alive, they can't be replaced with an intern.\n\n";
                    __result = node;
                    return;
                }
                __instance.groupCredits -= UpgradeBus.instance.cfg.INTERN_PRICE;
                LGUStore.instance.SyncCreditsServerRpc( __instance.groupCredits );
                UpgradeBus.instance.internScript.ReviveTargetedPlayerServerRpc();
                string name = UpgradeBus.instance.internNames[UnityEngine.Random.Range(0, UpgradeBus.instance.internNames.Length)];
                string interest = UpgradeBus.instance.internInterests[UnityEngine.Random.Range(0, UpgradeBus.instance.internInterests.Length)];
                node.displayText = $"{player.playerUsername} has been replaced with:\n\nNAME: {name}\nAGE: {UnityEngine.Random.Range(19,76)}\nIQ: {UnityEngine.Random.Range(2,160)}\nINTERESTS: {interest}\n\n{name} HAS BEEN TELEPORTED INSIDE THE FACILITY, PLEASE ACQUAINTANCE YOURSELF ACCORDINGLY";
                __result = node;
            }
            else if (text.ToLower().Contains(LOAD_LGU_COMMAND))
            {
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                if(text.ToLower() == LOAD_LGU_COMMAND)
                {
                    node.displayText = "Enter the name of the user whos upgrades/save you want to copy. Ex: `load lgu steve`\n";
                    __result = node;
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
                        node.displayText = $"Syncing with {player.playerUsername}\nThis should take 5 seconds\nPulling data...\n";
                        LGUStore.instance.ShareSaveServerRpc();
                        __instance.StartCoroutine(WaitForSync(player.playerSteamId));
                        __result = node;
                        return;
                    }
                }
                node.displayText = $"The name {playerNameToSearch} was not found. The following names were found:\n{string.Join(", ",playerNames)}\n";
                __result = node;
                return;
            }
            else if (text.ToLower().Contains("scan hives"))
            {
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                if(UpgradeBus.instance.scanLevel < 1)
                {
                    node.displayText = "\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n";
                    __result = node;
                    return;
                }
                GrabbableObject[] scrapItems = GameObject.FindObjectsOfType<GrabbableObject>().ToArray();
                GrabbableObject[] filteredHives = scrapItems.Where(scrap => scrap.itemProperties.itemName == "Hive").ToArray();
                GrabbableObject[] bestHives = filteredHives.OrderByDescending(v => v.scrapValue).ToArray();
                node.displayText = $"Found {bestHives.Length} Hives:";
                foreach(GrabbableObject scrap in bestHives)
                {
                    node.displayText += $"\n${scrap.scrapValue} // X: {scrap.gameObject.transform.position.x.ToString("F1")}, Y: {scrap.gameObject.transform.position.y.ToString("F1")}, Z: {scrap.gameObject.transform.position.z.ToString("F1")}";
                }
                node.displayText += "\nDon't forget your GPS!\n\n";
                __result = node;
            }
            else if (text.ToLower().Contains("scan scrap"))
            {
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                if(UpgradeBus.instance.scanLevel < 1)
                {
                    node.displayText = "\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n";
                    __result = node;
                    return;
                }
                GrabbableObject[] scrapItems = GameObject.FindObjectsOfType<GrabbableObject>().ToArray();
                GrabbableObject[] filteredScrap = scrapItems.Where(scrap => scrap.isInFactory && scrap.itemProperties.isScrap).ToArray();
                GrabbableObject[] bestScrap = filteredScrap.OrderByDescending(v => v.scrapValue).Take(5).ToArray();
                node.displayText = "Most valuable items:\n";
                foreach(GrabbableObject scrap in bestScrap)
                {
                    node.displayText += $"\n{scrap.itemProperties.itemName}: ${scrap.scrapValue}\nX: {Mathf.RoundToInt(scrap.gameObject.transform.position.x)}, Y: {Mathf.RoundToInt(scrap.gameObject.transform.position.y)}, Z: {Mathf.RoundToInt(scrap.gameObject.transform.position.z)}\n";
                }
                node.displayText += "\n\nDon't forget your GPS!\n\n";
                __result = node;
            }
            else if (text.ToLower().Contains("scan player"))
            {
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                if(UpgradeBus.instance.scanLevel < 1)
                {
                    node.displayText = "\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n";
                    __result = node;
                    return;
                }
                PlayerControllerB[] players = GameObject.FindObjectsOfType<PlayerControllerB>().ToArray();
                PlayerControllerB[] filteredPlayers = players.Where(player => player.playerSteamId != 0).ToArray();
                PlayerControllerB[] alivePlayers = filteredPlayers.Where(player => !player.isPlayerDead).ToArray();
                PlayerControllerB[] deadPlayers = filteredPlayers.Where(player => player.isPlayerDead).ToArray();
                node.displayText = "Alive Players:\n";
                foreach(PlayerControllerB player in alivePlayers)
                {
                    node.displayText += $"\n{player.playerUsername} - X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}";
                }
                node.displayText += "\nDead Players:\n";
                foreach(PlayerControllerB player in deadPlayers)
                {
                    node.displayText += $"\n{player.playerUsername} - X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}";
                }
                __result = node;
            }
            else if (text.ToLower().Contains("scan enemies"))
            {
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                if(UpgradeBus.instance.scanLevel < 1)
                {
                    node.displayText = "\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n";
                    __result = node;
                    return;
                }
                EnemyAI[] enemies = GameObject.FindObjectsOfType<EnemyAI>().Where(enem => !enem.isEnemyDead).ToArray();
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
                        node.displayText = $"Alive Enemies: {enemies.Length}\n";
                        foreach (KeyValuePair<string, int> count in enemyCount)
                        {
                            node.displayText += $"\n{count.Key} - {count.Value}";
                        }
                    }
                }
                else
                {
                    node.displayText = "0 enemies detected\n\n";
                }
                __result = node;
            }
            else if (text.ToLower().Contains("scan doors"))
            {
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                if(UpgradeBus.instance.scanLevel < 1)
                {
                    node.displayText = "\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n";
                    __result = node;
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

                if(player.isInsideFactory)
                {
                    node.displayText = $"Closest exits to {player.playerUsername} (X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}):\n";
                    GameObject[] Closest3 = fireEscape.OrderBy(door => Vector3.Distance(door.transform.position, player.transform.position)).Take(3).ToArray();
                    foreach(GameObject door in fireEscape)
                    {
                        node.displayText += $"\nX:{Mathf.RoundToInt(door.transform.position.x)},Y:{Mathf.RoundToInt(door.transform.position.y)},Z:{Mathf.RoundToInt(door.transform.position.z)} - {Mathf.RoundToInt(Vector3.Distance(door.transform.position, player.transform.position))} units away.";
                    }
                }
                else
                {
                    node.displayText = $"Entrances for {player.playerUsername} (X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}):\n";
                    foreach(EntranceTeleport door in mainDoors)
                    {
                        node.displayText += $"\nX:{Mathf.RoundToInt(door.transform.position.x)},Y:{Mathf.RoundToInt(door.transform.position.y)},Z:{Mathf.RoundToInt(door.transform.position.z)} - {Mathf.RoundToInt(Vector3.Distance(door.transform.position, player.transform.position))} units away.";
                    }               
                }
                __result = node;
            }
            else if (text.ToLower().Split()[0] == "transmit")
            {
                string[] splits = text.Split();
                TerminalNode node = new TerminalNode();
                node.clearPreviousText = true;
                if(GameObject.FindObjectOfType<SignalTranslator>() == null)
                {
                    node.displayText = "You have to buy a Signal Translator to use this command\n\n";
                    __result = node;
                }
                else if (UpgradeBus.instance.pager)
                {
                    if (splits.Length == 1)
                    {
                        node.displayText = "You have to enter a message to broadcast\nEX: `page get back to the ship!`";
                        __result = node;
                    }
                    else
                    {
                        string msg = string.Join(" ", splits.Skip(1));
                        UpgradeBus.instance.pageScript.ReqBroadcastChatServerRpc(msg);
                        node.displayText = $"Broadcasted message: '{msg}'";
                        __result = node;
                    }
                }
            }
            else
            {
                foreach(CustomTerminalNode customNode in UpgradeBus.instance.terminalNodes)
                {
                    if(text.ToLower() == customNode.Name.ToLower())
                    {
                        TerminalNode node = new TerminalNode();
                        node.clearPreviousText = true;
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
                    else if (text.ToLower() == $"unload {customNode.Name.ToLower()}")
                    {
                        UpgradeBus.instance.UpgradeObjects[customNode.Name].GetComponent<BaseUpgrade>().Unwind();
                        LGUStore.instance.UpdateLGUSaveServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, JsonConvert.SerializeObject(new SaveInfo()));
                        TerminalNode node = new TerminalNode();
                        customNode.Unlocked = false;
                        node.displayText = $"Unwinding {customNode.Name.ToLower()}";
                        node.clearPreviousText = true;
                        __result = node;
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
