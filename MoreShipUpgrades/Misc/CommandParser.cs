using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    internal class CommandParser
    {
        const string LOAD_LGU_COMMAND = "load lgu";
        private static TerminalNode DisplayTerminalMessage(string message, bool clearPreviousText = true)
        {
            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            node.displayText = message;
            node.clearPreviousText = clearPreviousText;
            return node;
        }

        private static TerminalNode ExecuteToggleLightning()
        {
            if (!UpgradeBus.instance.lightningRod) return DisplayTerminalMessage(lightningRodScript.ACCESS_DENIED_MESSAGE);

            return DisplayTerminalMessage(UpgradeBus.instance.lightningRodActive ? lightningRodScript.TOGGLE_ON_MESSAGE : lightningRodScript.TOGGLE_OFF_MESSAGE);
        }
        private static TerminalNode ExecuteDiscombobulatorAttack(ref Terminal terminal)
        {
            if (!UpgradeBus.instance.terminalFlash) return DisplayTerminalMessage("You don't have access to this command yet. Purchase the 'Discombobulator'.\n\n");

            if (UpgradeBus.instance.flashCooldown > 0f) return DisplayTerminalMessage($"You can discombobulate again in {Mathf.Round(UpgradeBus.instance.flashCooldown)} seconds.\nType 'cooldown' or 'cd' to check discombobulation cooldown.\n\n");

            RoundManager.Instance.PlayAudibleNoise(terminal.transform.position, 60f, 0.8f, 0, false, 14155);
            UpgradeBus.instance.flashScript.PlayAudioAndUpdateCooldownServerRpc();

            Collider[] array = Physics.OverlapSphere(terminal.transform.position, UpgradeBus.instance.cfg.DISCOMBOBULATOR_RADIUS, 524288);
            if (array.Length <= 0) return DisplayTerminalMessage("No stunned enemies detected.\n\n");

            if (UpgradeBus.instance.cfg.DISCOMBOBULATOR_NOTIFY_CHAT)
            {
                terminal.StartCoroutine(CountDownChat(UpgradeBus.instance.cfg.DISCOMBOBULATOR_STUN_DURATION + (UpgradeBus.instance.cfg.DISCOMBOBULATOR_INCREMENT * UpgradeBus.instance.discoLevel)));
            }
            return DisplayTerminalMessage($"Stun grenade hit {array.Length} enemies.\n\n");
        }

        private static TerminalNode ExecuteDiscombobulatorCooldown()
        {
            if (!UpgradeBus.instance.terminalFlash) return DisplayTerminalMessage("You don't have access to this command yet. Purchase the 'Discombobulator'.\n\n");

            if (UpgradeBus.instance.flashCooldown > 0f) return DisplayTerminalMessage($"You can discombobulate again in {Mathf.Round(UpgradeBus.instance.flashCooldown)} seconds.\n\n");

            return DisplayTerminalMessage("Discombobulate is ready, Type 'initattack' or 'atk' to execute.\n\n");
        }

        private static TerminalNode ExecuteModInformation()
        {
            string displayText = "Late Game Upgrades\n\nType `lategame store` or `lgu` to view upgrades.\n\nMost of the mod is configurable via the config file in `BepInEx/config/`.";
            displayText += "\n\nUse the info command to get info about an item. EX: `info beekeeper`.";
            displayText += "\n\nYou must type the exact name of the upgrade (case insensitve).";
            displayText += "\n\nTo force wipe an lgu save file type `reset lgu`. (will only wipe the clients save).";
            displayText += "\n\nTo reapply any upgrades that failed to apply type `load lgu`.";
            displayText += "\n\nIn the case of credit desync to force an amount of credits type `forceCredits 123`, to attempt to sync credits type `syncCredits`";
            displayText += "\n\n";
            return DisplayTerminalMessage(displayText);
        }

        private static TerminalNode ExecuteResetLGUSave()
        {
            if (LGUStore.instance.lguSave.playerSaves.ContainsKey(GameNetworkManager.Instance.localPlayerController.playerSteamId))
            {
                UpgradeBus.instance.ResetAllValues(false);
                SaveInfo saveInfo = new SaveInfo();
                ulong id = GameNetworkManager.Instance.localPlayerController.playerSteamId;
                LGUStore.instance.saveInfo = saveInfo;
                LGUStore.instance.UpdateLGUSaveServerRpc(id, JsonConvert.SerializeObject(saveInfo));
            }
            return DisplayTerminalMessage("LGU save has been wiped.\n\n");
        }
        private static TerminalNode ExecuteForceCredits(string creditAmount, ref Terminal __instance)
        {
            if (int.TryParse(creditAmount, out int value))
            {
                __instance.groupCredits = value;
                return DisplayTerminalMessage($"This client now has {value} credits.  \n\nThis was intended to be used when credit desync occurs due to Bigger Lobby or More Company.\n\n");
            }

            return DisplayTerminalMessage($"Failed to parse value {creditAmount}.\n\n");
        }

        private static TerminalNode ExecuteSyncCredits(ref Terminal terminal)
        {
            LGUStore.instance.SyncCreditsServerRpc(terminal.groupCredits);
            return DisplayTerminalMessage($"Sending an RPC to sync all clients credits with your credits. ({terminal.groupCredits})\n\n");
        }

        private static TerminalNode ExecuteInternsCommand(ref Terminal terminal)
        {
            if (terminal.groupCredits < UpgradeBus.instance.cfg.INTERN_PRICE) return DisplayTerminalMessage($"Interns cost {UpgradeBus.instance.cfg.INTERN_PRICE} credits and you have {terminal.groupCredits} credits.\n");

            PlayerControllerB player = StartOfRound.Instance.mapScreen.targetedPlayer;
            if (!player.isPlayerDead) return DisplayTerminalMessage($"{player.playerUsername} is still alive, they can't be replaced with an intern.\n\n");

            terminal.groupCredits -= UpgradeBus.instance.cfg.INTERN_PRICE;
            LGUStore.instance.SyncCreditsServerRpc(terminal.groupCredits);
            UpgradeBus.instance.internScript.ReviveTargetedPlayerServerRpc();
            string name = UpgradeBus.instance.internNames[UnityEngine.Random.Range(0, UpgradeBus.instance.internNames.Length)];
            string interest = UpgradeBus.instance.internInterests[UnityEngine.Random.Range(0, UpgradeBus.instance.internInterests.Length)];
            return DisplayTerminalMessage($"{player.playerUsername} has been replaced with:\n\nNAME: {name}\nAGE: {UnityEngine.Random.Range(19, 76)}\nIQ: {UnityEngine.Random.Range(2, 160)}\nINTERESTS: {interest}\n\n{name} HAS BEEN TELEPORTED INSIDE THE FACILITY, PLEASE ACQUAINTANCE YOURSELF ACCORDINGLY");
        }
        private static TerminalNode ExecuteLoadLGUCommand(string text, ref Terminal terminal)
        {
            if (text.ToLower() == LOAD_LGU_COMMAND) return DisplayTerminalMessage("Enter the name of the user whos upgrades/save you want to copy. Ex: `load lgu steve`\n");

            PlayerControllerB[] players = UnityEngine.Object.FindObjectsOfType<PlayerControllerB>();
            List<string> playerNames = new List<string>();
            var playerNameToSearch = text.Substring(text.IndexOf(LOAD_LGU_COMMAND) + LOAD_LGU_COMMAND.Length).Trim();
            foreach (PlayerControllerB player in players)
            {
                playerNames.Add(player.playerUsername);
                if (player.playerUsername.ToLower() != playerNameToSearch.ToLower()) continue;

                LGUStore.instance.ShareSaveServerRpc();
                terminal.StartCoroutine(WaitForSync(player.playerSteamId));
                return DisplayTerminalMessage($"Syncing with {player.playerUsername}\nThis should take 5 seconds\nPulling data...\n");
            }
            return DisplayTerminalMessage($"The name {playerNameToSearch} was not found. The following names were found:\n{string.Join(", ", playerNames)}\n");
        }

        private static TerminalNode ExecuteTransmitMessage(string message, ref TerminalNode __result)
        {
            if (UnityEngine.Object.FindObjectOfType<SignalTranslator>() == null) return DisplayTerminalMessage("You have to buy a Signal Translator to use this command\n\n");

            if (!UpgradeBus.instance.pager) return __result;

            if (message == "") return DisplayTerminalMessage("You have to enter a message to broadcast\nEX: `page get back to the ship!`\n\n");

            UpgradeBus.instance.pageScript.ReqBroadcastChatServerRpc(message);
            return DisplayTerminalMessage($"Broadcasted message: '{message}'\n\n");
        }

        private static TerminalNode ExecuteUpgradeCommand(string text, ref Terminal terminal, ref TerminalNode outputNode)
        {
            foreach (CustomTerminalNode customNode in UpgradeBus.instance.terminalNodes)
            {
                if (text.ToLower() == customNode.Name.ToLower()) return ExecuteBuyUpgrade(customNode, ref terminal);

                if (text.ToLower() == $"info {customNode.Name.ToLower()}") return DisplayTerminalMessage(customNode.Description + "\n\n");

                if (text.ToLower() == $"unload {customNode.Name.ToLower()}") return ExecuteUnloadUpgrade(customNode);
            }
            return outputNode;
        }

        private static TerminalNode ExecuteBuyUpgrade(CustomTerminalNode customNode, ref Terminal terminal)
        {
            string displayText = null;
            int price = 0;
            if (!customNode.Unlocked) { price = (int)(customNode.UnlockPrice * customNode.salePerc); }
            else if (customNode.MaxUpgrade > customNode.CurrentUpgrade) { price = (int)(customNode.Prices[customNode.CurrentUpgrade] * customNode.salePerc); }

            bool canAfford = terminal.groupCredits >= price;
            if (canAfford && (!customNode.Unlocked || customNode.MaxUpgrade > customNode.CurrentUpgrade))
            {
                LGUStore.instance.SyncCreditsServerRpc(terminal.groupCredits - price);
                if (!customNode.Unlocked)
                {
                    LGUStore.instance.HandleUpgrade(customNode.Name);
                    if (customNode.MaxUpgrade != 0) { displayText = $"You Upgraded {customNode.Name} to level {customNode.CurrentUpgrade + 1}  \n\n"; }
                    else { displayText = $"You Purchased {customNode.Name}  \n\n"; }
                }
                else if (customNode.Unlocked && customNode.MaxUpgrade > customNode.CurrentUpgrade)
                {
                    LGUStore.instance.HandleUpgrade(customNode.Name, true);
                    displayText = $"You Upgraded {customNode.Name} to level {customNode.CurrentUpgrade + 1} \n\n";
                }
            }
            else if (customNode.Unlocked && canAfford)
            {
                if (customNode.MaxUpgrade == 0) { displayText = "You already unlocked this upgrade.  \n\n"; }
                else { displayText = "This upgrade is already max level  \n\n"; }
            }
            else
            {
                displayText = "You can't afford this item.  \n\n";
            }
            return DisplayTerminalMessage(displayText);
        }

        private static TerminalNode ExecuteUnloadUpgrade(CustomTerminalNode customNode)
        {
            UpgradeBus.instance.UpgradeObjects[customNode.Name].GetComponent<BaseUpgrade>().Unwind();
            LGUStore.instance.UpdateLGUSaveServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, JsonConvert.SerializeObject(new SaveInfo()));
            customNode.Unlocked = false;
            return DisplayTerminalMessage($"Unwinding {customNode.Name.ToLower()}\n\n");
        }

        private static TerminalNode ExecuteScanHivesCommand()
        {
            if (UpgradeBus.instance.scanLevel < 1) return DisplayTerminalMessage("\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n");

            GrabbableObject[] scrapItems = UnityEngine.Object.FindObjectsOfType<GrabbableObject>().ToArray();
            GrabbableObject[] filteredHives = scrapItems.Where(scrap => scrap.itemProperties.itemName == "Hive").ToArray();
            GrabbableObject[] bestHives = filteredHives.OrderByDescending(v => v.scrapValue).ToArray();
            string displayText = $"Found {bestHives.Length} Hives:";
            foreach (GrabbableObject scrap in bestHives)
            {
                displayText += $"\n${scrap.scrapValue} // X: {scrap.gameObject.transform.position.x.ToString("F1")}, Y: {scrap.gameObject.transform.position.y.ToString("F1")}, Z: {scrap.gameObject.transform.position.z.ToString("F1")}";
            }
            displayText += "\nDon't forget your GPS!\n\n";
            return DisplayTerminalMessage(displayText);
        }

        private static TerminalNode ExecuteScanScrapCommand()
        {
            if (UpgradeBus.instance.scanLevel < 1) return DisplayTerminalMessage("\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n");

            GrabbableObject[] scrapItems = UnityEngine.Object.FindObjectsOfType<GrabbableObject>().ToArray();
            GrabbableObject[] filteredScrap = scrapItems.Where(scrap => scrap.isInFactory && scrap.itemProperties.isScrap).ToArray();
            GrabbableObject[] bestScrap = filteredScrap.OrderByDescending(v => v.scrapValue).Take(5).ToArray();
            string displayText = "Most valuable items:\n";
            foreach (GrabbableObject scrap in bestScrap)
            {
                displayText += $"\n{scrap.itemProperties.itemName}: ${scrap.scrapValue}\nX: {Mathf.RoundToInt(scrap.gameObject.transform.position.x)}, Y: {Mathf.RoundToInt(scrap.gameObject.transform.position.y)}, Z: {Mathf.RoundToInt(scrap.gameObject.transform.position.z)}\n";
            }
            displayText += "\n\nDon't forget your GPS!\n\n";
            return DisplayTerminalMessage(displayText);
        }

        private static TerminalNode ExecuteScanPlayerCommand()
        {
            if (UpgradeBus.instance.scanLevel < 1) return DisplayTerminalMessage("\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n");

            PlayerControllerB[] players = UnityEngine.Object.FindObjectsOfType<PlayerControllerB>().ToArray();
            PlayerControllerB[] filteredPlayers = players.Where(player => player.playerSteamId != 0).ToArray();
            PlayerControllerB[] alivePlayers = filteredPlayers.Where(player => !player.isPlayerDead).ToArray();
            PlayerControllerB[] deadPlayers = filteredPlayers.Where(player => player.isPlayerDead).ToArray();
            string displayText = "Alive Players:\n";
            foreach (PlayerControllerB player in alivePlayers)
            {
                displayText += $"\n{player.playerUsername} - X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}";
            }
            displayText += "\nDead Players:\n";
            foreach (PlayerControllerB player in deadPlayers)
            {
                displayText += $"\n{player.playerUsername} - X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}";
            }
            displayText += "\n\n";
            return DisplayTerminalMessage(displayText);
        }

        private static TerminalNode ExecuteScanEnemiesCommand()
        {
            if (UpgradeBus.instance.scanLevel < 1) return DisplayTerminalMessage("\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n");

            EnemyAI[] enemies = UnityEngine.Object.FindObjectsOfType<EnemyAI>().Where(enem => !enem.isEnemyDead).ToArray();
            string displayText = null;
            if (enemies.Length <= 0) return DisplayTerminalMessage("0 enemies detected\n\n");

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

            }
            displayText = $"Alive Enemies: {enemies.Length}\n";
            foreach (KeyValuePair<string, int> count in enemyCount)
            {
                displayText += $"\n{count.Key} - {count.Value}";
            }
            displayText += "\n\n";
            return DisplayTerminalMessage(displayText);
        }

        private static TerminalNode ExecuteScanDoorsCommand()
        {
            if (UpgradeBus.instance.scanLevel < 1) return DisplayTerminalMessage("\nUpgrade Better Scanner to level 2 to use this command\nEnter `info better scanner` to check upgrades.\n\n");

            List<GameObject> fireEscape = UnityEngine.Object.FindObjectsOfType<GameObject>().Where(obj => obj.name == "SpawnEntranceBTrigger").ToList();
            List<EntranceTeleport> mainDoors = UnityEngine.Object.FindObjectsOfType<EntranceTeleport>().ToList();
            List<EntranceTeleport> doorsToRemove = new List<EntranceTeleport>();

            foreach (EntranceTeleport door in mainDoors)
            {
                if (door.gameObject.transform.position.y >= -170) continue;
                doorsToRemove.Add(door);
            }
            foreach (EntranceTeleport doorToRemove in doorsToRemove)
            {
                mainDoors.Remove(doorToRemove);
                if (!fireEscape.Contains(doorToRemove.gameObject)) fireEscape.Add(doorToRemove.gameObject);
            }
            PlayerControllerB player = StartOfRound.Instance.mapScreen.targetedPlayer;

            string displayText = null;
            if (player.isInsideFactory)
            {
                displayText = $"Closest exits to {player.playerUsername} (X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}):\n";
                GameObject[] Closest3 = fireEscape.OrderBy(door => Vector3.Distance(door.transform.position, player.transform.position)).Take(3).ToArray();
                foreach (GameObject door in fireEscape)
                {
                    displayText += $"\nX:{Mathf.RoundToInt(door.transform.position.x)},Y:{Mathf.RoundToInt(door.transform.position.y)},Z:{Mathf.RoundToInt(door.transform.position.z)} - {Mathf.RoundToInt(Vector3.Distance(door.transform.position, player.transform.position))} units away.";
                }
            }
            else
            {
                displayText = $"Entrances for {player.playerUsername} (X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}):\n";
                foreach (EntranceTeleport door in mainDoors)
                {
                    displayText += $"\nX:{Mathf.RoundToInt(door.transform.position.x)},Y:{Mathf.RoundToInt(door.transform.position.y)},Z:{Mathf.RoundToInt(door.transform.position.z)} - {Mathf.RoundToInt(Vector3.Distance(door.transform.position, player.transform.position))} units away.";
                }
            }
            displayText += "\n";
            return DisplayTerminalMessage(displayText);
        }
        private static TerminalNode ExecuteToggleCommands(string secondWord, ref TerminalNode outputNode)
        {
            switch (secondWord)
            {
                case "lightning": return ExecuteToggleLightning();
                default: return outputNode;
            }
        }
        private static TerminalNode ExecuteLategameCommands(string secondWord)
        {
            switch (secondWord)
            {
                case "store": return UpgradeBus.instance.ConstructNode();
                default: return ExecuteModInformation();
            }
        }
        private static TerminalNode ExecuteResetCommands(string secondWord, ref TerminalNode outputNode)
        {
            switch (secondWord)
            {
                case "lgu": return ExecuteResetLGUSave();
                default: return outputNode;
            }
        }
        private static TerminalNode ExecuteLoadCommands(string secondWord, string fullText, ref Terminal terminal, ref TerminalNode outputNode)
        {
            switch (secondWord)
            {
                case "lgu": return ExecuteLoadLGUCommand(fullText, ref terminal);
                default: return outputNode;
            }
        }
        private static TerminalNode ExecuteScanCommands(string secondWord, ref TerminalNode outputNode)
        {
            switch (secondWord)
            {
                case "hives": return ExecuteScanHivesCommand();
                case "scrap": return ExecuteScanScrapCommand();
                case "player": return ExecuteScanPlayerCommand();
                case "enemies": return ExecuteScanEnemiesCommand();
                case "doors": return ExecuteScanDoorsCommand();
                default: return outputNode;
            }
        }
        private static TerminalNode ExecuteExtendDeadlineCommand(string daysString, ref Terminal terminal)
        {
            if (daysString == "")
                return DisplayTerminalMessage($"You need to specify how many days you wish to extend the deadline for: \"extend deadline <days>\"");
            if (!(int.TryParse(daysString, out int days) && days > 0)) 
                return DisplayTerminalMessage($"Invalid value ({daysString}) inserted to extend the deadline.\n");

            if (terminal.groupCredits < days * UpgradeBus.instance.cfg.EXTEND_DEADLINE_PRICE) 
                return DisplayTerminalMessage($"Not enough credits to purchase the proposed deadline extension.\n Total price: {days * UpgradeBus.instance.cfg.EXTEND_DEADLINE_PRICE}\n Current credits: {terminal.groupCredits}\n");

            terminal.groupCredits -= days * UpgradeBus.instance.cfg.EXTEND_DEADLINE_PRICE;
            LGUStore.instance.SyncCreditsServerRpc(terminal.groupCredits);
            UpgradeBus.instance.extendScript.ExtendDeadlineClientRpc(days);

            return DisplayTerminalMessage($"Extended the deadline by {days} day{(days == 1 ? "" : "s")}");
        }
        private static TerminalNode ExecuteExtendCommands(string secondWord, string thirdWord, ref Terminal terminal, ref TerminalNode outputNode)
        {
            switch(secondWord)
            {
                case "deadline": return ExecuteExtendDeadlineCommand(thirdWord, ref terminal);
                default: return outputNode;
            }
        }
        public static void ParseLGUCommands(string fullText, ref Terminal terminal, ref TerminalNode outputNode)
        {
            string[] textArray = fullText.Split();
            string firstWord = textArray[0].ToLower();
            string secondWord = textArray.Length > 1 ? textArray[1].ToLower() : "";
            string thirdWord = textArray.Length > 2 ? textArray[2].ToLower() : "";
            switch(firstWord)
            {
                case "help": if (!outputNode.displayText.Contains(">LATEGAME\nDisplays information related with Lategame-Upgrades mod\n")) outputNode.displayText += ">LATEGAME\nDisplays information related with Lategame-Upgrades mod\n"; return;
                case "toggle": outputNode = ExecuteToggleCommands(secondWord, ref outputNode); return;
                case "initattack":
                case "atk": outputNode = ExecuteDiscombobulatorAttack(ref terminal); return;
                case "cd":
                case "cooldown": outputNode = ExecuteDiscombobulatorCooldown(); return;
                case "lategame": outputNode = ExecuteLategameCommands(secondWord); return;
                case "lgu": outputNode = UpgradeBus.instance.ConstructNode(); return;
                case "reset": outputNode = ExecuteResetCommands(secondWord, ref outputNode); return;
                case "forcecredits": outputNode = ExecuteForceCredits(secondWord, ref terminal); return;
                case "synccredits": outputNode = ExecuteSyncCredits(ref terminal); return;
                case "intern":
                case "interns": outputNode = ExecuteInternsCommand(ref terminal); return;
                case "extend": outputNode = ExecuteExtendCommands(secondWord, thirdWord, ref terminal, ref outputNode); return;
                case "load": outputNode = ExecuteLoadCommands(secondWord, fullText, ref terminal, ref outputNode); return;
                case "scan": outputNode = ExecuteScanCommands(secondWord, ref outputNode); return;
                case "transmit": outputNode = ExecuteTransmitMessage(fullText.Substring(firstWord.Length+1), ref outputNode); return;
                default: outputNode = ExecuteUpgradeCommand(fullText, ref terminal, ref outputNode); return;
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
            while (count > 0f)
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
