using GameNetcodeStuff;
using LethalLib.Modules;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.Items.Contracts.Exorcism;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    internal class CommandParser
    {
        private static LGULogger logger = new LGULogger(nameof(CommandParser));
        private static bool attemptCancelContract = false;
        private static string attemptSpecifyContract = null;
        private static TerminalKeyword routeKeyword;

        const string LOAD_LGU_COMMAND = "load lgu";
        public static List<string> contracts = new List<string> { "data", "exterminator", "extraction","exorcism","defusal" };
        public static List<string> contractInfos = new List<string> {
            "\n\nOur systems have detected an active PC somewhere in the facility.\nFind it, use the bruteforce command on the ship terminal with the devices IP to get login credentials, then use the cd, ls, and mv commands to find the .db file (enter `mv survey.db` in the containing folder).\n\n",
            "\n\nIt's been reported that the population of hoarder bugs on this moon have skyrocketed and become aggressive. You must destroy their nest at all costs.\n\n",
            "\n\nCrew number 5339 have reported that one of their operatives was lost on this moon and left behind. You will have to find or bring a medkit to heal and extract the lost operative.\n\n" ,
            "\n\nUnusual activity in the spirit world has been reported at this facility.\nFind the ritual site to determine what type of demon it is, enter `demon DemonName` in the terminal to get the ritual instructions. Find ritual items and banish the demon.\n\n" ,
            "\n\nAn unknown party has planted a bomb at an integral point in this facility.\nYou must find the bomb and work together to defuse it.\nUse the `Lookup` command with the bombs serial number to get defusal instructions.\n\n" 
        };


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
            return DisplayTerminalMessage($"Discombobulator hit {array.Length} enemies.\n\n");
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
            displayText += "\n\nIn the case of credit desync to force an amount of credits type `forceCredits 123`";
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
                return DisplayTerminalMessage("LGU save has been wiped.\n\n");
            }
            else
            {
                logger.LogError("LGU SAVE NOT FOUND in ExecuteResetLGUSave()!");
                return DisplayTerminalMessage("LGU save was not found!\n\n");
            }
        }
        private static TerminalNode ExecuteForceCredits(string creditAmount, ref Terminal __instance)
        {
            if (int.TryParse(creditAmount, out int value))
            {
                if (__instance.IsHost || __instance.IsServer)
                {
                    LGUStore.instance.SyncCreditsClientRpc(value);
                    return DisplayTerminalMessage($"All clients should now have ${value}\n\n");
                }
                else return DisplayTerminalMessage("Only the host can do this");
            }

            return DisplayTerminalMessage($"Failed to parse value {creditAmount}.\n\n");
        }

        private static TerminalNode ExecuteInternsCommand(ref Terminal terminal)
        {
            if (terminal.groupCredits < UpgradeBus.instance.cfg.INTERN_PRICE) return DisplayTerminalMessage($"Interns cost {UpgradeBus.instance.cfg.INTERN_PRICE} credits and you have {terminal.groupCredits} credits.\n");

            PlayerControllerB player = StartOfRound.Instance.mapScreen.targetedPlayer;
            if (!player.isPlayerDead) return DisplayTerminalMessage($"{player.playerUsername} is still alive, they can't be replaced with an intern.\n\n");
            logger.LogDebug($"Player {player.playerUsername} is dead and we have enough credits, executing revive command...");
            terminal.groupCredits -= UpgradeBus.instance.cfg.INTERN_PRICE;
            LGUStore.instance.SyncCreditsServerRpc(terminal.groupCredits);
            UpgradeBus.instance.internScript.ReviveTargetedPlayerServerRpc();
            string name = UpgradeBus.instance.internNames[UnityEngine.Random.Range(0, UpgradeBus.instance.internNames.Length)];
            string interest = UpgradeBus.instance.internInterests[UnityEngine.Random.Range(0, UpgradeBus.instance.internInterests.Length)];
            logger.LogInfo($"Successfully executed intern command for {player.playerUsername}!");
            return DisplayTerminalMessage($"{player.playerUsername} has been replaced with:\n\nNAME: {name}\nAGE: {UnityEngine.Random.Range(19, 76)}\nIQ: {UnityEngine.Random.Range(2, 160)}\nINTERESTS: {interest}\n\n{name} HAS BEEN TELEPORTED INSIDE THE FACILITY, PLEASE ACQUAINTANCE YOURSELF ACCORDINGLY");
        }
        private static TerminalNode ExecuteLoadLGUCommand(string text, ref Terminal terminal)
        {
            if (text.ToLower() == LOAD_LGU_COMMAND) return DisplayTerminalMessage("Enter the name of the user whos upgrades/save you want to copy. Ex: `load lgu steve`\n");

            PlayerControllerB[] players = UnityEngine.Object.FindObjectsOfType<PlayerControllerB>();
            List<string> playerNames = new List<string>();
            string playerNameToSearch = text.Substring(text.IndexOf(LOAD_LGU_COMMAND) + LOAD_LGU_COMMAND.Length).Trim();
            logger.LogDebug($"Starting to look for players with same name as {playerNameToSearch}");
            foreach (PlayerControllerB player in players)
            {
                if (player == null) continue;
                string playerName = player.playerUsername;
                ulong playerSteamID = player.playerSteamId;
                if (playerName == null) continue;
                playerNames.Add(playerName);
                logger.LogDebug($"Comparing {playerName} with {playerNameToSearch} case insensitive...");
                if (playerName.ToLower() != playerNameToSearch.ToLower()) continue;

                LGUStore.instance.ShareSaveServerRpc();
                terminal.StartCoroutine(WaitForSync(playerSteamID));
                logger.LogInfo($"Attempting to overwrite local save data with {playerName}'s save data.");
                return DisplayTerminalMessage($"Attempting to overwrite local save data with {playerName}'s save data\nYou should see a popup in 5 seconds...\n.\n");
            }
            string csvNames = string.Join(", ", playerNames);
            logger.LogInfo($"{playerNameToSearch} was not found among: {csvNames}");
            return DisplayTerminalMessage($"The name {playerNameToSearch} was not found. The following names were found:\n{csvNames}\n");
        }

        private static TerminalNode ExecuteTransmitMessage(string message, ref TerminalNode __result)
        {
            if (UnityEngine.Object.FindObjectOfType<SignalTranslator>() == null)
            {
                logger.LogInfo("User tried to use signar translator without owning one.");
                return DisplayTerminalMessage("You have to buy a Signal Translator to use this command\n\n");
            }

            if (!UpgradeBus.instance.pager)
            {
                logger.LogInfo("User used signal translator successfully without Pager upgrade");
                return __result;
            }

            if (message == "")
            {
                logger.LogInfo("User entered an invalid message for pager");
                return DisplayTerminalMessage("You have to enter a message to broadcast\nEX: `page get back to the ship!`\n\n");
            }

            logger.LogInfo($"Broadcasting {message} with Pager upgrade...");
            UpgradeBus.instance.pageScript.ReqBroadcastChatServerRpc(message);
            return DisplayTerminalMessage($"Broadcasted message: '{message}'\n\n");
        }

        private static TerminalNode ExecuteUpgradeCommand(string text, ref Terminal terminal, ref TerminalNode outputNode)
        {
            foreach (CustomTerminalNode customNode in UpgradeBus.instance.terminalNodes)
            {
                if (text.ToLower() == customNode.Name.ToLower() || text.ToLower() == $"buy {customNode.Name.ToLower()}" || text.ToLower() == $"purchase {customNode.Name.ToLower()}") return ExecuteBuyUpgrade(customNode, ref terminal);

                if (text.ToLower() == $"info {customNode.Name.ToLower()}" || text.ToLower() == $"{customNode.Name.ToLower()} info") return DisplayTerminalMessage(customNode.Description + "\n\n");

                if (text.ToLower() == $"unload {customNode.Name.ToLower()}" || text.ToLower() == $"{customNode.Name.ToLower()} unload") return ExecuteUnloadUpgrade(customNode);
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
            logger.LogInfo($"Unload executed, unwinging {customNode.Name} on local client!");
            UpgradeBus.instance.UpgradeObjects[customNode.Name].GetComponent<BaseUpgrade>().Unwind();
            LGUStore.instance.UpdateLGUSaveServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, JsonConvert.SerializeObject(new SaveInfo()));
            customNode.Unlocked = false;
            customNode.CurrentUpgrade = 0;
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
            logger.LogInfo($"Scan Hives command found {filteredHives.Length} hives.");
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
            logger.LogInfo($"Scan scrap command found {filteredScrap.Length} valid scrap items.");
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
            logger.LogInfo($"Scan players command found {alivePlayers.Length} alive players and {deadPlayers.Length} dead players.");
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
                logger.LogInfo("Scan Enemies: Verbose mode = true");
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
                logger.LogInfo("Scan Enemies: Verbose mode = false");
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
            logger.LogInfo($"Scan Enemies found {enemies.Length} alive enemies.");
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
                logger.LogInfo($"Scan Doors, player is inside factory. Found {fireEscape.Count} doors.");
            }
            else
            {
                displayText = $"Entrances for {player.playerUsername} (X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}):\n";
                foreach (EntranceTeleport door in mainDoors)
                {
                    displayText += $"\nX:{Mathf.RoundToInt(door.transform.position.x)},Y:{Mathf.RoundToInt(door.transform.position.y)},Z:{Mathf.RoundToInt(door.transform.position.z)} - {Mathf.RoundToInt(Vector3.Distance(door.transform.position, player.transform.position))} units away.";
                }
                logger.LogInfo($"Scan Doors, player is outside factory. Found {mainDoors.Count} doors.");
            }
            displayText += "\n";
            return DisplayTerminalMessage(displayText);
        }
        private static TerminalNode ExecuteContractCommands(string secondWord, ref Terminal terminal)
        {
            switch(secondWord)
            {
                case "cancel": return ExecuteContractCancelCommand(ref terminal);
                case "info": return DisplayTerminalMessage(string.Format(AssetBundleHandler.GetInfoFromJSON("Contract"), UpgradeBus.instance.cfg.CONTRACT_PRICE));
                default: return TryGetContract(secondWord, ref terminal);
            }
        }
        private static TerminalNode ExecuteContractCancelCommand(ref Terminal terminal)
        {
            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            if (UpgradeBus.instance.contractLevel == "None")
            {
                node.displayText = "You must have accepted a contract to execute this command...\n\n";
                node.clearPreviousText = true;
                return node;
            }
            attemptCancelContract = true;
            node.clearPreviousText = true;
            node.displayText = "Type CONFIRM to cancel your current contract. There will be no refunds.\n\n";
            return node;
        }
        static TerminalNode TryGetMoonContract(string possibleMoon, ref Terminal terminal)
        {
            logger.LogDebug($"Trying to assign a contract on moon called {possibleMoon}");
            string txt = null;
            if (UpgradeBus.instance.contractLevel != "None")
            {
                txt = $"You currently have a {UpgradeBus.instance.contractType} contract on {UpgradeBus.instance.contractLevel}!\n\n";
                txt += string.Format(AssetBundleHandler.GetInfoFromJSON("Contract"), UpgradeBus.instance.cfg.CONTRACT_SPECIFY_PRICE);
                logger.LogInfo($"User tried starting a new specified moon contract while they still have a {UpgradeBus.instance.contractType} contract on {UpgradeBus.instance.contractLevel}!");
                return DisplayTerminalMessage(txt);
            }
            if (terminal.groupCredits < UpgradeBus.instance.cfg.CONTRACT_SPECIFY_PRICE)
            {
                txt = $"Specified Moon contracts cost ${UpgradeBus.instance.cfg.CONTRACT_SPECIFY_PRICE} and you have ${terminal.groupCredits}\n\n";
                return DisplayTerminalMessage(txt);
            }
            string moon = GetSpecifiedLevel(possibleMoon);
            if (moon == "None")
            {
                txt = $"Wasn't possible to find any moons whose name contains {possibleMoon} to provide a contract on it...\n\n";
                return DisplayTerminalMessage(txt);
            }
            attemptSpecifyContract = moon;
            txt = $"Type CONFIRM if you wish to have a contract on {moon} for the cost of {UpgradeBus.instance.cfg.CONTRACT_SPECIFY_PRICE} Company credits.\n\n";
            return DisplayTerminalMessage(txt);
        }
        static TerminalNode TryGetContract(string possibleMoon, ref Terminal terminal)
        {
            if (possibleMoon != "") return TryGetMoonContract(possibleMoon, ref terminal);
            string txt = null;
            if(UpgradeBus.instance.contractLevel != "None")
            {
                txt = $"You currently have a {UpgradeBus.instance.contractType} contract on {UpgradeBus.instance.contractLevel}!\n\n";
                txt += string.Format(AssetBundleHandler.GetInfoFromJSON("Contract"), UpgradeBus.instance.cfg.CONTRACT_PRICE);
                logger.LogInfo($"User tried starting a new contract while they still have a {UpgradeBus.instance.contractType} contract on {UpgradeBus.instance.contractLevel}!");
                return DisplayTerminalMessage(txt);
            }
            if(terminal.groupCredits < UpgradeBus.instance.cfg.CONTRACT_PRICE)
            {
                txt = $"Contracts costs ${UpgradeBus.instance.cfg.CONTRACT_PRICE} and you have ${terminal.groupCredits}\n\n";
                return DisplayTerminalMessage(txt);
            }
            LGUStore.instance.SyncCreditsServerRpc(terminal.groupCredits - UpgradeBus.instance.cfg.CONTRACT_PRICE);
            int i = Random.Range(0,contracts.Count);
            if( contracts.Count > 1)
            {
                while (i == ContractScript.lastContractIndex)
                {
                    i = Random.Range(0, contracts.Count);
                }
            }
            UpgradeBus.instance.contractType = contracts[i];
            txt = $"A {contracts[i]} contract has been accepted for {RandomLevel()}!{contractInfos[i]}";

            if (terminal.IsHost || terminal.IsServer) LGUStore.instance.SyncContractDetailsClientRpc(UpgradeBus.instance.contractLevel, i);
            else LGUStore.instance.ReqSyncContractDetailsServerRpc(UpgradeBus.instance.contractLevel, i);

            logger.LogInfo($"User accepted a {UpgradeBus.instance.contractType} contract on {UpgradeBus.instance.contractLevel}");

            return DisplayTerminalMessage(txt);
        }
        static string GetSpecifiedLevel(string moon)
        {
            string lvl = UpgradeBus.instance.contractLevel;
            SelectableLevel[] availableLevels = StartOfRound.Instance.levels;
            for (int i = 0; i < availableLevels.Length; i++) 
            {
                logger.LogDebug(availableLevels[i].PlanetName.ToLower());
                if (!availableLevels[i].PlanetName.ToLower().Contains(moon)) continue;
                lvl = availableLevels[i].PlanetName;
                break;
            }
            return lvl;
        }
        static string RandomLevel()
        {
            string lvl = UpgradeBus.instance.contractLevel;
            string lastLevel = null;
            SelectableLevel[] availableLevels = StartOfRound.Instance.levels;
            bool[] usedLevels = new bool[availableLevels.Length];
            for(int i = 0; i < usedLevels.Length; i++)
            {
                usedLevels[i] = false;
            }
            bool allUsed = true;
            while (lvl == UpgradeBus.instance.contractLevel)
            {
                int levelIndex = UnityEngine.Random.Range(0, availableLevels.Length);
                if (usedLevels[levelIndex]) continue;
                usedLevels[levelIndex] = true;
                SelectableLevel level = availableLevels[levelIndex];
                lastLevel = level.PlanetName;
                if (level.PlanetName.Contains("Gordion")) continue;
                logger.LogDebug($"Picked {level.PlanetName} as possible moon for contract...");
                if (routeKeyword == null) routeKeyword = UpgradeBus.instance.GetTerminal().terminalNodes.allKeywords.First(k => k.word == "route");
                for(int i = 0; i < routeKeyword.compatibleNouns.Length && lvl == UpgradeBus.instance.contractLevel; i++)
                {
                    TerminalNode routeMoon = routeKeyword.compatibleNouns[i].result;
                    int itemCost = routeMoon.itemCost;
                    if (UpgradeBus.instance.cfg.CONTRACT_FREE_MOONS_ONLY && itemCost != 0)
                    {
                        logger.LogDebug($"Criteria algorithm skipped a choice due to configuration only allowing free moons (Choice: {level.PlanetName})");
                        break;
                    }
                    CompatibleNoun[] additionalNodes = routeMoon.terminalOptions;
                    for(int j = 0; j < additionalNodes.Length && lvl == UpgradeBus.instance.contractLevel; j++)
                    {
                        TerminalNode confirmNode = additionalNodes[j].result;
                        if (confirmNode == null) continue;
                        if (confirmNode.buyRerouteToMoon != levelIndex) continue;

                        logger.LogDebug($"Criteria algorithm made a choice and decided to assign contract on {level.PlanetName}");
                        lvl = level.PlanetName;
                    }
                }
                if (lvl != UpgradeBus.instance.contractLevel) break;
                allUsed = true;
                for (int i = 0; i < usedLevels.Length; i++)
                {
                    allUsed &= usedLevels[i];
                }
                if (allUsed) break;
            }
            if (lvl == UpgradeBus.instance.contractLevel && allUsed)
            {
                logger.LogDebug($"Criteria algorithm did not make a choice, we will use the last selected moon ({lastLevel})");
                lvl = lastLevel;
            }
            logger.LogDebug($"{lvl} will be the moon for the random contract...");
            UpgradeBus.instance.contractLevel = lvl;
            return lvl;
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
        private static TerminalNode ExecuteExtendDeadlineCommand(string daysString, ref Terminal terminal, ref TerminalNode outputNode)
        {
            if (!UpgradeBus.instance.cfg.EXTEND_DEADLINE_ENABLED) return outputNode;

            if (daysString == "")
                return DisplayTerminalMessage($"You need to specify how many days you wish to extend the deadline for: \"extend deadline <days>\"\n\n");

            if (!(int.TryParse(daysString, out int days) && days > 0)) 
                return DisplayTerminalMessage($"Invalid value ({daysString}) inserted to extend the deadline.\n\n");

            if (terminal.groupCredits < days * UpgradeBus.instance.cfg.EXTEND_DEADLINE_PRICE) 
                return DisplayTerminalMessage($"Not enough credits to purchase the proposed deadline extension.\n Total price: {days * UpgradeBus.instance.cfg.EXTEND_DEADLINE_PRICE}\n Current credits: {terminal.groupCredits}\n\n");

            terminal.groupCredits -= days * UpgradeBus.instance.cfg.EXTEND_DEADLINE_PRICE;
            LGUStore.instance.SyncCreditsServerRpc(terminal.groupCredits);
            LGUStore.instance.ExtendDeadlineServerRpc(days);

            return DisplayTerminalMessage($"Extended the deadline by {days} day{(days == 1 ? "" : "s")}.\n\n");
        }
        private static TerminalNode ExecuteExtendCommands(string secondWord, string thirdWord, ref Terminal terminal, ref TerminalNode outputNode)
        {
            switch(secondWord)
            {
                case "deadline": return ExecuteExtendDeadlineCommand(thirdWord, ref terminal, ref outputNode);
                default: return outputNode;
            }
        }
        private static TerminalNode ExecuteToggleCommands(string secondWord, ref TerminalNode outputNode)
        {
            switch (secondWord)
            {
                case "lightning": return ExecuteToggleLightning();
                default: return outputNode;
            }
        }
        private static TerminalNode ExecuteBruteForce(string secondWord)
        {
            switch (secondWord)
            {
                case "": return DisplayTerminalMessage($"Enter a valid address for a device to connect to!\n\n");
                default: return HandleBruteForce(secondWord);
            }
        }
        private static TerminalNode CheckConfirmForCancel(string word, ref Terminal terminal)
        {
            attemptCancelContract = false;
            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            if (word.ToLower() != "confirm")
            {
                node.displayText = "Failed to confirm user's input. Invalidated cancel contract request.\n\n";
                node.clearPreviousText = true;
                return node;
            }
            if (terminal.IsHost || terminal.IsServer) LGUStore.instance.SyncContractDetailsClientRpc("None", -1);
            else LGUStore.instance.ReqSyncContractDetailsServerRpc("None", -1);
            node.displayText = "Cancelling contract...\n\n";
            node.clearPreviousText = true;
            return node;

        }
        static TerminalNode CheckConfirmForSpecify(string word, ref Terminal terminal)
        {
            string moon = attemptSpecifyContract;
            attemptSpecifyContract = null;
            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            if (word.ToLower() != "confirm")
            {
                node.displayText = "Failed to confirm user's input. Invalidated specified moon contract request.\n\n";
                node.clearPreviousText = true;
                return node;
            }
            
            LGUStore.instance.SyncCreditsServerRpc(terminal.groupCredits - UpgradeBus.instance.cfg.CONTRACT_SPECIFY_PRICE);
            int i = Random.Range(0, contracts.Count);
            UpgradeBus.instance.contractType = contracts[i];
            UpgradeBus.instance.contractLevel = moon;
            if (terminal.IsHost || terminal.IsServer) LGUStore.instance.SyncContractDetailsClientRpc(UpgradeBus.instance.contractLevel, i);
            else LGUStore.instance.ReqSyncContractDetailsServerRpc(UpgradeBus.instance.contractLevel, i);
            logger.LogInfo($"User accepted a {UpgradeBus.instance.contractType} contract on {UpgradeBus.instance.contractLevel}");
            return DisplayTerminalMessage($"A {contracts[i]} contract has been accepted for {moon}!{contractInfos[i]}");
        }
        public static void ParseLGUCommands(string fullText, ref Terminal terminal, ref TerminalNode outputNode)
        {
            string[] textArray = fullText.Split();
            string firstWord = textArray[0].ToLower();
            string secondWord = textArray.Length > 1 ? textArray[1].ToLower() : "";
            string thirdWord = textArray.Length > 2 ? textArray[2].ToLower() : "";
            if (attemptSpecifyContract != null)
            {
                outputNode = CheckConfirmForSpecify(firstWord, ref terminal);
                return;
            }
            if (attemptCancelContract)
            {
                outputNode = CheckConfirmForCancel(firstWord, ref terminal);
                return;
            }
            switch(firstWord)
            {
                case "demon": outputNode = LookupDemon(secondWord, thirdWord); return;
                case "lookup": outputNode = DefuseBombCommand(secondWord); return;
                case "toggle": outputNode = ExecuteToggleCommands(secondWord, ref outputNode); return;
                case "contract": outputNode = ExecuteContractCommands(secondWord, ref terminal); return;
                case "bruteforce": outputNode= ExecuteBruteForce(secondWord); return;
                case "initattack":
                case "atk": outputNode = ExecuteDiscombobulatorAttack(ref terminal); return;
                case "cd":
                case "cooldown": outputNode = ExecuteDiscombobulatorCooldown(); return;
                case "lategame": outputNode = ExecuteLategameCommands(secondWord); return;
                case "lgu": outputNode = UpgradeBus.instance.ConstructNode(); return;
                case "reset": outputNode = ExecuteResetCommands(secondWord, ref outputNode); return;
                case "forcecredits": outputNode = ExecuteForceCredits(secondWord, ref terminal); return;
                case "intern":
                case "interns": outputNode = ExecuteInternsCommand(ref terminal); return;
                case "extend": outputNode = ExecuteExtendCommands(secondWord, thirdWord, ref terminal, ref outputNode); return;
                case "load": outputNode = ExecuteLoadCommands(secondWord, fullText, ref terminal, ref outputNode); return;
                case "scan": outputNode = ExecuteScanCommands(secondWord, ref outputNode); return;
                case "transmit": outputNode = ExecuteTransmitMessage(fullText.Substring(firstWord.Length+1), ref outputNode); return;
                case "scrap": outputNode = ExecuteScrapCommands(secondWord, ref terminal, ref outputNode); return;
                default: outputNode = ExecuteUpgradeCommand(fullText, ref terminal, ref outputNode); return;
            }
        }
        private static TerminalNode ExecuteScrapInsuranceCommand(ref Terminal terminal, ref TerminalNode outputNode)
        {
            if (!UpgradeBus.instance.cfg.SCRAP_INSURANCE_ENABLED) return outputNode;

            if (ScrapInsurance.ScrapInsuranceStatus())
                return DisplayTerminalMessage($"You already purchased insurance to protect your scrap belongings.\n\n");

            if (!StartOfRound.Instance.inShipPhase)
                return DisplayTerminalMessage($"You can only acquire insurance while in orbit.\n\n");

            if (terminal.groupCredits < UpgradeBus.instance.cfg.SCRAP_INSURANCE_PRICE)
                return DisplayTerminalMessage($"Not enough credits to purchase Scrap Insurance.\nPrice: {UpgradeBus.instance.cfg.SCRAP_INSURANCE_PRICE}\nCurrent credits: {terminal.groupCredits}\n\n");

            terminal.groupCredits -= UpgradeBus.instance.cfg.SCRAP_INSURANCE_PRICE;
            LGUStore.instance.SyncCreditsServerRpc(terminal.groupCredits);
            ScrapInsurance.TurnOnScrapInsurance();
            return DisplayTerminalMessage($"Scrap Insurance has been activated.\nIn case of a team wipe in your next trip, your scrap will be preserved.\n\n");
        }
        private static TerminalNode ExecuteScrapCommands(string secondWord, ref Terminal terminal, ref TerminalNode outputNode)
        {
            switch(secondWord)
            {
                case "insurance": return ExecuteScrapInsuranceCommand(ref terminal, ref outputNode);
                default: return outputNode;
            }
        }
        private static TerminalNode LookupDemon(string secondWord, string thirdWord)
        {
            string demon = secondWord.ToUpper();
            if (demon == "DE" && thirdWord.ToUpper() == "OGEN") demon = demon + " " + thirdWord.ToUpper();
            if (PentagramScript.DemonInstructions.ContainsKey(demon))
            {
                string[] items = PentagramScript.DemonInstructions[demon];
                return DisplayTerminalMessage($"To banish this spirit you must assemble the following items:\n\n{items[0]}\n{items[1]}\n{items[2]}\n\n");
            }
            string ghostList = string.Join("\n", PentagramScript.DemonInstructions.Keys.ToArray());
            return DisplayTerminalMessage($"There is no known data on {secondWord}. Known spirits are:\n\n{ghostList}\n\n");
        }

        private static TerminalNode DefuseBombCommand(string secondWord)
        {
            if(UpgradeBus.instance.contractLevel != StartOfRound.Instance.currentLevel.PlanetName || UpgradeBus.instance.contractType != "defusal")
            {
                return DisplayTerminalMessage("YOU MUST BE IN A DEFUSAL CONTRACT TO USE THIS COMMAND!\n\n");
            }
            if (secondWord == "") return DisplayTerminalMessage("YOU MUST ENTER A SERIAL NUMBER TO LOOK UP!\n\n");
            if (secondWord.ToLower() == UpgradeBus.instance.SerialNumber.ToLower() || secondWord.ToLower() == UpgradeBus.instance.SerialNumber.Replace("-","").ToLower())
            {
                logger.LogInfo("DEFUSAL: user entered correct serial number!");
                return DisplayTerminalMessage("CUT THE WIRES IN THE FOLLOWING ORDER:\n\n" + string.Join("\n\n", UpgradeBus.instance.bombOrder) +"\n\n");
            }
            else
            {
                logger.LogInfo($"DEFUSAL: user entered incorrect serial number! Entered: {secondWord}, Expected: {UpgradeBus.instance.SerialNumber} (case and hyphen insensitive)");
                if (UpgradeBus.instance.fakeBombOrders.ContainsKey(secondWord))
                {
                    logger.LogInfo("DEFUSAL: Reusing previously generated fake defusal under this key.");
                    return DisplayTerminalMessage("CUT THE WIRES IN THE FOLLOWING ORDER:\n\n" + string.Join("\n\n", UpgradeBus.instance.fakeBombOrders[secondWord]) +"\n\n");
                }
                logger.LogInfo("DEFUSAL: Generating new fake defusal under this key.");
                List<string> falseOrder = new List<string> { "red","green","blue" };
                Tools.ShuffleList(falseOrder);
                UpgradeBus.instance.fakeBombOrders.Add(secondWord, falseOrder);
                return DisplayTerminalMessage("CUT THE WIRES IN THE FOLLOWING ORDER:\n\n" + string.Join("\n\n", falseOrder) +"\n\n");
            }
        }

        private static TerminalNode HandleBruteForce(string secondWord)
        {
            string txt = null;
            string ip = UpgradeBus.instance.DataMinigameKey;
            if(secondWord == ip)
            {
                logger.LogInfo($"USER CORRECTLY ENTERED IP ADDRESS, user: {UpgradeBus.instance.DataMinigameUser}, pass: {UpgradeBus.instance.DataMinigamePass}");
                txt = $"PING {ip} ({ip}): 56 data bytes\r\n64 bytes from {ip}: icmp_seq=0 ttl=64 time=1.234 ms\r\n64 bytes from {ip}: icmp_seq=1 ttl=64 time=1.345 ms\r\n64 bytes from {ip}: icmp_seq=2 ttl=64 time=1.123 ms\r\n64 bytes from {ip}: icmp_seq=3 ttl=64 time=1.456 ms\r\n\r\n--- {ip} ping statistics ---\r\n4 packets transmitted, 4 packets received, 0.0% packet loss\r\nround-trip min/avg/max/stddev = 1.123/1.289/1.456/0.123 ms\n\n";
                txt += $"CONNECTION ESTABLISHED --- RETRIEVING CREDENTIALS...\n\nUSER: {UpgradeBus.instance.DataMinigameUser}\nPASSWORD: {UpgradeBus.instance.DataMinigamePass}\n";
            }
            else
            {
                logger.LogInfo($"USER INCORRECTLY ENTERED IP ADDRESS! submitted: {secondWord}, expected: {ip}");
                txt = $"PING {secondWord} ({secondWord}): 56 data bytes\r\nRequest timeout for icmp_seq 0\r\nRequest timeout for icmp_seq 1\r\nRequest timeout for icmp_seq 2\r\nRequest timeout for icmp_seq 3\r\n\r\n--- {secondWord} ping statistics ---\r\n4 packets transmitted, 0 packets received, 100.0% packet loss\n\n";
                txt += $"CONNECTION FAILED -- INVALID ADDRESS?\n\n";
            }
            return DisplayTerminalMessage(txt);
        }

        private static IEnumerator WaitForSync(ulong id)
        {
            yield return new WaitForSeconds(3f);
            HUDManager.Instance.DisplayTip("LOADING SAVE DATA", $"Overwiting local save data with the save under player id: {id}");
            LGUStore.instance.saveInfo = LGUStore.instance.lguSave.playerSaves[id];
            LGUStore.instance.UpdateUpgradeBus(false);
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
