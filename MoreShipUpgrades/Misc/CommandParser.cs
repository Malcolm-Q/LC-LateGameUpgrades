using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.UI;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.Items.Contracts.Exorcism;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    internal static class CommandParser
    {
        private static LguLogger logger = new LguLogger(nameof(CommandParser));
        private static bool attemptCancelContract = false;
        private static string attemptSpecifyContract = null;
        static (string, LevelWeatherType) attemptWeatherProbe = (null, LevelWeatherType.None);

        const string LOAD_LGU_COMMAND = "load lgu";
        public static readonly List<string> contracts = new List<string> { "data", "exterminator", "extraction","exorcism","defusal" };
        public static readonly List<string> contractInfos = new List<string> {
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
        private static TerminalNode ExecuteDiscombobulatorAttack(ref Terminal terminal)
        {
            if (!BaseUpgrade.GetActiveUpgrade(Discombobulator.UPGRADE_NAME)) return DisplayTerminalMessage(LGUConstants.DISCOMBOBULATOR_NOT_ACTIVE);

            if (Discombobulator.instance.flashCooldown > 0f) return DisplayTerminalMessage(string.Format(LGUConstants.DISCOMBOBULATOR_ON_COOLDOWN_FORMAT, Mathf.Round(Discombobulator.instance.flashCooldown)));

            RoundManager.Instance.PlayAudibleNoise(terminal.transform.position, 60f, 0.8f, 0, false, 14155);
            Discombobulator.instance.PlayAudioAndUpdateCooldownServerRpc();

            Collider[] array = Physics.OverlapSphere(terminal.transform.position, UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_RADIUS.Value, 524288);
            if (array.Length <= 0) return DisplayTerminalMessage(LGUConstants.DISCOMBOBULATOR_NO_ENEMIES);

            if (UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_NOTIFY_CHAT.Value)
            {
                terminal.StartCoroutine(CountDownChat(UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_STUN_DURATION.Value + (UpgradeBus.Instance.PluginConfiguration.DISCOMBOBULATOR_INCREMENT.Value * BaseUpgrade.GetUpgradeLevel(Discombobulator.UPGRADE_NAME))));
            }
            return DisplayTerminalMessage(string.Format(LGUConstants.DISCOMBULATOR_HIT_ENEMIES, array.Length));
        }

        private static TerminalNode ExecuteDiscombobulatorCooldown()
        {
            if (!BaseUpgrade.GetActiveUpgrade(Discombobulator.UPGRADE_NAME)) return DisplayTerminalMessage(LGUConstants.DISCOMBOBULATOR_NOT_ACTIVE);

            if (Discombobulator.instance.flashCooldown > 0f) return DisplayTerminalMessage(string.Format(LGUConstants.DISCOMBOBULATOR_DISPLAY_COOLDOWN, Mathf.Round(Discombobulator.instance.flashCooldown)));

            return DisplayTerminalMessage(LGUConstants.DISCOMBOBULATOR_READY);
        }

        private static TerminalNode ExecuteModInformation()
        {
            string displayText = "Late Game Upgrades\n\nType `lategame store` or `lgu` to view upgrades.\n\nMost of the mod is configurable via the config file in `BepInEx/config/`.";
            displayText += "\n\nType `lategame commands` to see all commands related to upgrades";
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
            if (LguStore.Instance.LguSave.playerSaves.ContainsKey(GameNetworkManager.Instance.localPlayerController.playerSteamId))
            {
                UpgradeBus.Instance.ResetAllValues(false);
                SaveInfo saveInfo = new SaveInfo();
                ulong id = GameNetworkManager.Instance.localPlayerController.playerSteamId;
                LguStore.Instance.SaveInfo = saveInfo;
                LguStore.Instance.UpdateLGUSaveServerRpc(id, JsonConvert.SerializeObject(saveInfo));
                return DisplayTerminalMessage(LGUConstants.LGU_SAVE_WIPED);
            }
            else
            {
                logger.LogError("LGU SAVE NOT FOUND in ExecuteResetLGUSave()!");
                return DisplayTerminalMessage(LGUConstants.LGU_SAVE_NOT_FOUND);
            }
        }
        private static TerminalNode ExecuteForceCredits(string creditAmount, ref Terminal __instance)
        {
            if (int.TryParse(creditAmount, out int value))
            {
                if (__instance.IsHost || __instance.IsServer)
                {
                    LguStore.Instance.SyncCreditsClientRpc(value);
                    return DisplayTerminalMessage(string.Format(LGUConstants.FORCE_CREDITS_SUCCESS_FORMAT, value));
                }
                else return DisplayTerminalMessage(LGUConstants.FORCE_CREDITS_HOST_ONLY);
            }

            return DisplayTerminalMessage(string.Format(LGUConstants.FORCE_CREDITS_PARSED_FAIL_FORMAT, creditAmount));
        }

        private static TerminalNode ExecuteInternsCommand(ref Terminal terminal)
        {
            if (terminal.groupCredits < UpgradeBus.Instance.PluginConfiguration.INTERN_PRICE.Value)
                return DisplayTerminalMessage(string.Format(LGUConstants.INTERNS_NOT_ENOUGH_CREDITS_FORMAT, UpgradeBus.Instance.PluginConfiguration.INTERN_PRICE.Value, terminal.groupCredits));

            PlayerControllerB player = StartOfRound.Instance.mapScreen.targetedPlayer;
            if (!player.isPlayerDead) return DisplayTerminalMessage(string.Format(LGUConstants.INTERNS_PLAYER_ALREADY_ALIVE_FORMAT, player.playerUsername));
            terminal.groupCredits -= UpgradeBus.Instance.PluginConfiguration.INTERN_PRICE.Value;
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits);
            Interns.instance.ReviveTargetedPlayerServerRpc();
            string name = Interns.instance.internNames[UnityEngine.Random.Range(0, Interns.instance.internNames.Length)];
            string interest = Interns.instance.internInterests[UnityEngine.Random.Range(0, Interns.instance.internInterests.Length)];
            logger.LogInfo($"Successfully executed intern command for {player.playerUsername}!");
            return DisplayTerminalMessage($"{player.playerUsername} has been replaced with:\n\nNAME: {name}\nAGE: {UnityEngine.Random.Range(19, 76)}\nIQ: {UnityEngine.Random.Range(2, 160)}\nINTERESTS: {interest}\n\n{name} HAS BEEN TELEPORTED INSIDE THE FACILITY, PLEASE ACQUAINTANCE YOURSELF ACCORDINGLY");
        }
        private static TerminalNode ExecuteLoadLGUCommand(string text, ref Terminal terminal)
        {
            if (text.ToLower() == LOAD_LGU_COMMAND) return DisplayTerminalMessage(LGUConstants.LOAD_LGU_NO_NAME);

            PlayerControllerB[] players = UnityEngine.Object.FindObjectsOfType<PlayerControllerB>();
            List<string> playerNames = new List<string>();
            string playerNameToSearch = text.Substring(text.IndexOf(LOAD_LGU_COMMAND) + LOAD_LGU_COMMAND.Length).Trim();
            foreach (PlayerControllerB player in players)
            {
                if (player == null) continue;
                string playerName = player.playerUsername;
                ulong playerSteamID = player.playerSteamId;
                if (playerName == null) continue;
                playerNames.Add(playerName);
                if (!playerName.ToLower().Contains(playerNameToSearch.ToLower())) continue;

                LguStore.Instance.ShareSaveServerRpc();
                terminal.StartCoroutine(WaitForSync(playerSteamID));
                logger.LogInfo($"Attempting to overwrite local save data with {playerName}'s save data.");
                return DisplayTerminalMessage(string.Format(LGUConstants.LOAD_LGU_SUCCESS_FORMAT, playerName));
            }
            string csvNames = string.Join(", ", playerNames);
            logger.LogInfo($"{playerNameToSearch} was not found among: {csvNames}");
            return DisplayTerminalMessage(string.Format(LGUConstants.LOAD_LGU_FAILURE_FORMAT, playerNameToSearch, csvNames));
        }

        private static TerminalNode ExecuteUpgradeCommand(string text, ref Terminal terminal, ref TerminalNode outputNode)
        {
            foreach (CustomTerminalNode customNode in UpgradeBus.Instance.terminalNodes)
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
                LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits - price);
                if (!customNode.Unlocked)
                {
                    LguStore.Instance.HandleUpgrade(customNode.Name);
                    if (customNode.MaxUpgrade != 0) { displayText = $"You Upgraded {customNode.Name} to level {customNode.CurrentUpgrade + 1}  \n\n"; }
                    else { displayText = $"You Purchased {customNode.Name}  \n\n"; }
                }
                else if (customNode.Unlocked && customNode.MaxUpgrade > customNode.CurrentUpgrade)
                {
                    LguStore.Instance.HandleUpgrade(customNode.Name, true);
                    displayText = $"You Upgraded {customNode.Name} to level {customNode.CurrentUpgrade + 1} \n\n";
                }
                if (customNode.salePerc != 1f && UpgradeBus.Instance.PluginConfiguration.SALE_APPLY_ONCE.Value) customNode.salePerc = 1f;
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
            UpgradeBus.Instance.UpgradeObjects[customNode.Name].GetComponent<BaseUpgrade>().Unwind();
            LguStore.Instance.UpdateLGUSaveServerRpc(GameNetworkManager.Instance.localPlayerController.playerSteamId, JsonConvert.SerializeObject(new SaveInfo()));
            customNode.Unlocked = false;
            customNode.CurrentUpgrade = 0;
            return DisplayTerminalMessage(string.Format(LGUConstants.UNLOAD_LGU_SUCCESS_FORMAT, customNode.Name));
        }

        private static TerminalNode ExecuteScanHivesCommand()
        {
            if (BaseUpgrade.GetUpgradeLevel(BetterScanner.UPGRADE_NAME) < 1) return DisplayTerminalMessage(LGUConstants.SCANNER_LEVEL_REQUIRED);

            GrabbableObject[] scrapItems = UnityEngine.Object.FindObjectsOfType<GrabbableObject>().ToArray();
            GrabbableObject[] filteredHives = scrapItems.Where(scrap => scrap.itemProperties.itemName == "Hive").ToArray();
            GrabbableObject[] bestHives = filteredHives.OrderByDescending(v => v.scrapValue).ToArray();
            StringBuilder stringBuilder = new();
            stringBuilder.Append($"Found {bestHives.Length} Hives:").AppendLine();
            foreach (GrabbableObject scrap in bestHives)
            {
                stringBuilder.Append($"{scrap.scrapValue} // X: {scrap.gameObject.transform.position.x.ToString("F1")}, Y: {scrap.gameObject.transform.position.y.ToString("F1")}, Z: {scrap.gameObject.transform.position.z.ToString("F1")}").AppendLine();
            }
            stringBuilder.Append("Don't forget your GPS!").AppendLine().AppendLine();
            logger.LogInfo($"Scan Hives command found {filteredHives.Length} hives.");
            return DisplayTerminalMessage(stringBuilder.ToString());
        }

        private static TerminalNode ExecuteScanScrapCommand()
        {
            if (BaseUpgrade.GetUpgradeLevel(BetterScanner.UPGRADE_NAME) < 1) return DisplayTerminalMessage(LGUConstants.SCANNER_LEVEL_REQUIRED);

            GrabbableObject[] scrapItems = UnityEngine.Object.FindObjectsOfType<GrabbableObject>().ToArray();
            GrabbableObject[] filteredScrap = scrapItems.Where(scrap => scrap.isInFactory && scrap.itemProperties.isScrap).ToArray();
            GrabbableObject[] bestScrap = filteredScrap.OrderByDescending(v => v.scrapValue).Take(5).ToArray();
            StringBuilder stringBuilder = new();
            stringBuilder.Append("Most valuable items:").AppendLine();
            foreach (GrabbableObject scrap in bestScrap)
            {
                stringBuilder.Append($"{scrap.itemProperties.itemName}: ${scrap.scrapValue}\nX: {Mathf.RoundToInt(scrap.gameObject.transform.position.x)}, Y: {Mathf.RoundToInt(scrap.gameObject.transform.position.y)}, Z: {Mathf.RoundToInt(scrap.gameObject.transform.position.z)}").AppendLine();
            }
            stringBuilder.Append("Don't forget your GPS!").AppendLine().AppendLine();
            logger.LogInfo($"Scan scrap command found {filteredScrap.Length} valid scrap items.");
            return DisplayTerminalMessage(stringBuilder.ToString());
        }

        private static TerminalNode ExecuteScanPlayerCommand()
        {
            if (BaseUpgrade.GetUpgradeLevel(BetterScanner.UPGRADE_NAME) < 1) return DisplayTerminalMessage(LGUConstants.SCANNER_LEVEL_REQUIRED);

            PlayerControllerB[] players = UnityEngine.Object.FindObjectsOfType<PlayerControllerB>().ToArray();
            PlayerControllerB[] filteredPlayers = players.Where(player => player.playerSteamId != 0).ToArray();
            PlayerControllerB[] alivePlayers = filteredPlayers.Where(player => !player.isPlayerDead).ToArray();
            PlayerControllerB[] deadPlayers = filteredPlayers.Where(player => player.isPlayerDead).ToArray();
            StringBuilder stringBuilder = new();

            stringBuilder.Append("Alive Players:").AppendLine();
            foreach (PlayerControllerB player in alivePlayers)
            {
                stringBuilder.Append($"\n{player.playerUsername} - X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}").AppendLine();
            }
            stringBuilder.Append("Dead Players:").AppendLine();
            foreach (PlayerControllerB player in deadPlayers)
            {
                stringBuilder.Append($"\n{player.playerUsername} - X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}").AppendLine();
            }
            stringBuilder.AppendLine();
            logger.LogInfo($"Scan players command found {alivePlayers.Length} alive players and {deadPlayers.Length} dead players.");
            return DisplayTerminalMessage(stringBuilder.ToString());
        }

        private static TerminalNode ExecuteScanEnemiesCommand()
        {
            if (BaseUpgrade.GetUpgradeLevel(BetterScanner.UPGRADE_NAME) < 1) return DisplayTerminalMessage(LGUConstants.SCANNER_LEVEL_REQUIRED);

            EnemyAI[] enemies = UnityEngine.Object.FindObjectsOfType<EnemyAI>().Where(enem => !enem.isEnemyDead).ToArray();
            if (enemies.Length <= 0) return DisplayTerminalMessage("0 enemies detected\n\n");

            Dictionary<string, int> enemyCount = new Dictionary<string, int>();
            if (!UpgradeBus.Instance.PluginConfiguration.VERBOSE_ENEMIES.Value)
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
                    else realName = "Unknown";
                    if (enemyCount.ContainsKey(realName)) { enemyCount[realName]++; }
                    else { enemyCount.Add(realName, 1); }
                }

            }
            logger.LogInfo($"Scan Enemies found {enemies.Length} alive enemies.");
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Alive Enemies: {enemies.Length}").AppendLine();
            foreach (KeyValuePair<string, int> count in enemyCount)
            {
                stringBuilder.Append($"{count.Key} - {count.Value}").AppendLine();
            }
            stringBuilder.AppendLine();
            return DisplayTerminalMessage(stringBuilder.ToString());
        }

        private static TerminalNode ExecuteScanDoorsCommand()
        {
            if (BaseUpgrade.GetUpgradeLevel(BetterScanner.UPGRADE_NAME) < 1) return DisplayTerminalMessage(LGUConstants.SCANNER_LEVEL_REQUIRED);

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

            StringBuilder stringBuilder = new StringBuilder();
            if (player.isInsideFactory)
            {
                stringBuilder.Append($"Closest exits to {player.playerUsername} " +
                                    $"(X:{Mathf.RoundToInt(player.transform.position.x)}," +
                                    $"Y:{Mathf.RoundToInt(player.transform.position.y)}," +
                                    $"Z:{Mathf.RoundToInt(player.transform.position.z)}):")
                            .AppendLine();
                GameObject[] Closest3 = fireEscape.OrderBy(door => Vector3.Distance(door.transform.position, player.transform.position)).Take(3).ToArray();
                foreach (Vector3 doorPosition in Closest3.Select(door => door.transform.position))
                {
                    stringBuilder.Append($"X:{Mathf.RoundToInt(doorPosition.x)}," +
                                        $"Y:{Mathf.RoundToInt(doorPosition.y)}," +
                                        $"Z:{Mathf.RoundToInt(doorPosition.z)} - " +
                                        $"{Mathf.RoundToInt(Vector3.Distance(doorPosition, player.transform.position))} units away.")
                                .AppendLine();
                }
                logger.LogInfo($"Scan Doors, player is inside factory. Found {fireEscape.Count} doors.");
            }
            else
            {
                stringBuilder.Append($"Entrances for {player.playerUsername} " +
                                    $"(X:{Mathf.RoundToInt(player.transform.position.x)}," +
                                    $"Y:{Mathf.RoundToInt(player.transform.position.y)}," +
                                    $"Z:{Mathf.RoundToInt(player.transform.position.z)}):")
                            .AppendLine();
                foreach (Vector3 doorPosition in mainDoors.Select(door => door.transform.position))
                {
                    stringBuilder.Append($"X:{Mathf.RoundToInt(doorPosition.x)},Y:{Mathf.RoundToInt(doorPosition.y)},Z:{Mathf.RoundToInt(doorPosition.z)} - {Mathf.RoundToInt(Vector3.Distance(doorPosition, player.transform.position))} units away.").AppendLine();
                }
                logger.LogInfo($"Scan Doors, player is outside factory. Found {mainDoors.Count} doors.");
            }
            stringBuilder.AppendLine();
            return DisplayTerminalMessage(stringBuilder.ToString());
        }
        private static TerminalNode ExecuteContractCommands(string secondWord, ref Terminal terminal)
        {
            switch(secondWord)
            {
                case "cancel": return ExecuteContractCancelCommand();
                case "info": return DisplayTerminalMessage(string.Format(AssetBundleHandler.GetInfoFromJSON("Contract"), UpgradeBus.Instance.PluginConfiguration.CONTRACT_PRICE.Value));
                default: return TryGetContract(secondWord, ref terminal);
            }
        }
        private static TerminalNode ExecuteContractCancelCommand()
        {
            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            if (ContractManager.Instance.contractLevel == "None")
            {
                node.displayText = LGUConstants.CONTRACT_CANCEL_FAIL;
                node.clearPreviousText = true;
                return node;
            }
            attemptCancelContract = true;
            node.clearPreviousText = true;
            node.displayText = LGUConstants.CONTRACT_CANCEL_CONFIRM_PROMPT;
            return node;
        }
        static TerminalNode TryGetMoonContract(string possibleMoon, ref Terminal terminal)
        {
            logger.LogDebug($"Trying to assign a contract on moon called {possibleMoon}");
            string txt = null;
            if (ContractManager.Instance.contractLevel != "None")
            {
                txt = $"You currently have a {ContractManager.Instance.contractType} contract on {ContractManager.Instance.contractLevel}!\n\n";
                txt += string.Format(AssetBundleHandler.GetInfoFromJSON("Contract"), UpgradeBus.Instance.PluginConfiguration.CONTRACT_SPECIFY_PRICE.Value);
                logger.LogInfo($"User tried starting a new specified moon contract while they still have a {ContractManager.Instance.contractType} contract on {ContractManager.Instance.contractLevel}!");
                return DisplayTerminalMessage(txt);
            }
            if (terminal.groupCredits < UpgradeBus.Instance.PluginConfiguration.CONTRACT_SPECIFY_PRICE.Value)
            {
                txt = $"Specified Moon contracts cost ${UpgradeBus.Instance.PluginConfiguration.CONTRACT_SPECIFY_PRICE.Value} and you have ${terminal.groupCredits}\n\n";
                return DisplayTerminalMessage(txt);
            }
            string moon = ContractManager.GetSpecifiedLevel(possibleMoon);
            if (moon == "None")
            {
                txt = $"Wasn't possible to find any moons whose name contains {possibleMoon} to provide a contract on it...\n\n";
                return DisplayTerminalMessage(txt);
            }
            attemptSpecifyContract = moon;
            txt = $"Type CONFIRM if you wish to have a contract on {moon} for the cost of {UpgradeBus.Instance.PluginConfiguration.CONTRACT_SPECIFY_PRICE.Value} Company credits.\n\n";
            return DisplayTerminalMessage(txt);
        }
        static TerminalNode TryGetContract(string possibleMoon, ref Terminal terminal)
        {
            if (contracts.Count == 0) return DisplayTerminalMessage(LGUConstants.CONTRACT_FAIL);
            if (possibleMoon != "") return TryGetMoonContract(possibleMoon, ref terminal);
            string txt = null;
            if(ContractManager.Instance.contractLevel != "None")
            {
                txt = $"You currently have a {ContractManager.Instance.contractType} contract on {ContractManager.Instance.contractLevel}!\n\n";
                txt += string.Format(AssetBundleHandler.GetInfoFromJSON("Contract"), UpgradeBus.Instance.PluginConfiguration.CONTRACT_PRICE.Value);
                logger.LogInfo($"User tried starting a new contract while they still have a {ContractManager.Instance.contractType} contract on {ContractManager.Instance.contractLevel}!");
                return DisplayTerminalMessage(txt);
            }
            if(terminal.groupCredits < UpgradeBus.Instance.PluginConfiguration.CONTRACT_PRICE.Value)
            {
                txt = $"Contracts costs ${UpgradeBus.Instance.PluginConfiguration.CONTRACT_PRICE.Value} and you have ${terminal.groupCredits}\n\n";
                return DisplayTerminalMessage(txt);
            }
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits - UpgradeBus.Instance.PluginConfiguration.CONTRACT_PRICE.Value);
            int i = Random.Range(0,contracts.Count);
            if( contracts.Count > 1)
            {
                while (i == ContractManager.Instance.lastContractIndex)
                {
                    i = Random.Range(0, contracts.Count);
                }
            }
            ContractManager.Instance.contractType = contracts[i];
            txt = $"A {contracts[i]} contract has been accepted for {ContractManager.RandomLevel()}!{contractInfos[i]}";

            if (terminal.IsHost || terminal.IsServer) ContractManager.Instance.SyncContractDetailsClientRpc(ContractManager.Instance.contractLevel, i);
            else ContractManager.Instance.ReqSyncContractDetailsServerRpc(ContractManager.Instance.contractLevel, i);

            logger.LogInfo($"User accepted a {ContractManager.Instance.contractType} contract on {ContractManager.Instance.contractLevel}");

            return DisplayTerminalMessage(txt);
        }

        private static TerminalNode ExecuteLategameCommands(string secondWord)
        {
            switch (secondWord)
            {
                case "store": return UpgradeBus.Instance.ConstructNode();
                case "commands": return ExecuteLGUCommands();
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
            if (!UpgradeBus.Instance.PluginConfiguration.EXTEND_DEADLINE_ENABLED.Value) return outputNode;

            if (daysString == "")
                return DisplayTerminalMessage(LGUConstants.EXTEND_DEADLINE_USAGE);

            if (!(int.TryParse(daysString, out int days) && days > 0)) 
                return DisplayTerminalMessage(string.Format(LGUConstants.EXTEND_DEADLINE_PARSING_ERROR_FORMAT, daysString));

            if (terminal.groupCredits < days * UpgradeBus.Instance.PluginConfiguration.EXTEND_DEADLINE_PRICE.Value) 
                return DisplayTerminalMessage(string.Format(LGUConstants.EXTEND_DEADLINE_NOT_ENOUGH_CREDITS_FORMAT, days * UpgradeBus.Instance.PluginConfiguration.EXTEND_DEADLINE_PRICE.Value, terminal.groupCredits));

            terminal.groupCredits -= days * UpgradeBus.Instance.PluginConfiguration.EXTEND_DEADLINE_PRICE.Value;
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits);
            ExtendDeadlineScript.instance.ExtendDeadlineServerRpc(days);

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
                default: return outputNode;
            }
        }
        private static TerminalNode ExecuteBruteForce(string secondWord)
        {
            switch (secondWord)
            {
                case "": return DisplayTerminalMessage(LGUConstants.BRUTEFORCE_USAGE);
                default: return HandleBruteForce(secondWord);
            }
        }
        private static TerminalNode CheckConfirmForCancel(string word, ref Terminal terminal)
        {
            attemptCancelContract = false;
            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            if (word.ToLower() != "confirm")
            {
                node.displayText = LGUConstants.CONTRACT_CANCEL_CONFIRM_PROMPT_FAIL;
                node.clearPreviousText = true;
                return node;
            }
            if (terminal.IsHost || terminal.IsServer) ContractManager.Instance.SyncContractDetailsClientRpc("None", -1);
            else ContractManager.Instance.ReqSyncContractDetailsServerRpc("None", -1);
            node.displayText = LGUConstants.CONTRACT_CANCEL_CONFIRM_PROMPT_SUCCESS;
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
                node.displayText = LGUConstants.CONTRACT_SPECIFY_CONFIRM_PROMPT_FAIL;
                node.clearPreviousText = true;
                return node;
            }
            
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits - UpgradeBus.Instance.PluginConfiguration.CONTRACT_SPECIFY_PRICE.Value);
            int i = Random.Range(0, contracts.Count);
            ContractManager.Instance.contractType = contracts[i];
            ContractManager.Instance.contractLevel = moon;
            if (terminal.IsHost || terminal.IsServer) ContractManager.Instance.SyncContractDetailsClientRpc(ContractManager.Instance.contractLevel, i);
            else ContractManager.Instance.ReqSyncContractDetailsServerRpc(ContractManager.Instance.contractLevel, i);
            logger.LogInfo($"User accepted a {ContractManager.Instance.contractType} contract on {ContractManager.Instance.contractLevel}");
            return DisplayTerminalMessage(string.Format(LGUConstants.CONTRACT_SPECIFY_CONFIRM_PROMPT_SUCCESS_FORMAT, contracts[i], moon, contractInfos[i]));
        }
        static TerminalNode CheckConfirmForWeatherProbe(string word, ref Terminal terminal)
        {
            (string, LevelWeatherType) selectedWeather = attemptWeatherProbe;
            attemptWeatherProbe = (null, LevelWeatherType.None);
            if (word.ToLower() != "confirm") return DisplayTerminalMessage(LGUConstants.WEATHER_SPECIFY_CONFIRM_PROMPT_FAIL);
            terminal.groupCredits -= UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PICKED_WEATHER_PRICE.Value;
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits);
            LguStore.Instance.SyncWeatherServerRpc(selectedWeather.Item1, selectedWeather.Item2);
            return DisplayTerminalMessage(string.Format(LGUConstants.CONTRACT_SPECIFY_CONFIRM_PROMPT_SUCCESS_FORMAT, selectedWeather.Item1, selectedWeather.Item2, UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PICKED_WEATHER_PRICE.Value));
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
            if (attemptWeatherProbe != (null,LevelWeatherType.None))
            {
                outputNode = CheckConfirmForWeatherProbe(firstWord, ref terminal);
                return;
            }
            switch (firstWord)
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
                case "lgu":
                    {
                        GameObject store = Object.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube));
                        store.name = "UpgradesStore";
                        Object.Destroy(store.GetComponent<MeshRenderer>());
                        Object.Destroy(store.GetComponent<MeshFilter>());
                        store.AddComponent<UpgradesStore>();
                        outputNode = LguTerminalNode.GetHelpTerminalNode();
                        return;
                    }
                case "reset": outputNode = ExecuteResetCommands(secondWord, ref outputNode); return;
                case "forcecredits": outputNode = ExecuteForceCredits(secondWord, ref terminal); return;
                case "intern":
                case "interns": outputNode = ExecuteInternsCommand(ref terminal); return;
                case "extend": outputNode = ExecuteExtendCommands(secondWord, thirdWord, ref terminal, ref outputNode); return;
                case "load": outputNode = ExecuteLoadCommands(secondWord, fullText, ref terminal, ref outputNode); return;
                case "scan": outputNode = ExecuteScanCommands(secondWord, ref outputNode); return;
                case "scrap": outputNode = ExecuteScrapCommands(secondWord, ref terminal, ref outputNode); return;
                case "probe": outputNode = ExecuteProbeCommands(secondWord, thirdWord, ref terminal, ref outputNode); return;
                default: outputNode = ExecuteUpgradeCommand(fullText, ref terminal, ref outputNode); return;
            }
        }
        static TerminalNode ExecuteProbeCommands(string secondWord, string thirdWord, ref Terminal terminal, ref TerminalNode outputNode)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_ENABLED.Value) return outputNode;
            if (secondWord == "") return DisplayTerminalMessage(LGUConstants.WEATHER_PROBE_USAGE);
            if (!StartOfRound.Instance.inShipPhase) return DisplayTerminalMessage(LGUConstants.WEATHER_PROBE_ONLY_IN_ORBIT);
            if (thirdWord != "") return ExecuteSpecifiedProbeCommand(secondWord, thirdWord, terminal);
            if (terminal.groupCredits < UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PRICE.Value) return DisplayTerminalMessage(string.Format(LGUConstants.WEATHER_PROBE_NOT_ENOUGH_CREDITS_FORMAT, UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PRICE.Value, terminal.groupCredits));

            (string, LevelWeatherType) selectedWeather = WeatherManager.PickWeather(secondWord);
            if (selectedWeather.Item1 == null) return DisplayTerminalMessage(string.Format(LGUConstants.WEATHER_PROBE_MOON_NOT_FOUND_FORMAT, secondWord));
            if (StartOfRound.Instance.levels.First(x => x.PlanetName == selectedWeather.Item1).currentWeather == selectedWeather.Item2) return DisplayTerminalMessage(string.Format(LGUConstants.WEATHER_PROBE_SAME_WEATHER_FORMAT, selectedWeather.Item1, selectedWeather.Item2));
            terminal.groupCredits -= UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PRICE.Value;
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits);
            LguStore.Instance.SyncWeatherServerRpc(selectedWeather.Item1, selectedWeather.Item2);
            return DisplayTerminalMessage(string.Format(LGUConstants.WEATHER_PROBE_SUCCESS_FORMAT, selectedWeather.Item1, selectedWeather.Item2, UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PRICE.Value));
        }
        static TerminalNode ExecuteSpecifiedProbeCommand(string secondWord, string thirdWord, Terminal terminal)
        {
            if (terminal.groupCredits < UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PICKED_WEATHER_PRICE.Value)
                return DisplayTerminalMessage(string.Format(LGUConstants.WEATHER_PROBE_SPECIFY_NOT_ENOUGH_CREDITS_FORMAT, UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PICKED_WEATHER_PRICE.Value, terminal.groupCredits));

            (string, LevelWeatherType) selectedWeather = WeatherManager.PickWeather(secondWord, thirdWord);
            if (selectedWeather.Item1 == null) return DisplayTerminalMessage(string.Format(LGUConstants.WEATHER_PROBE_MOON_NOT_FOUND_FORMAT, secondWord));
            if (selectedWeather.Item2 == LevelWeatherType.DustClouds) return DisplayTerminalMessage(LGUConstants.WEATHER_PROBE_SPECIFY_INVALID_WEATHER);
            if (StartOfRound.Instance.levels.First(x => x.PlanetName == selectedWeather.Item1).currentWeather == selectedWeather.Item2) return DisplayTerminalMessage(string.Format(LGUConstants.WEATHER_PROBE_SAME_WEATHER_FORMAT, selectedWeather.Item1, selectedWeather.Item2));
            attemptWeatherProbe = selectedWeather;
            return DisplayTerminalMessage(string.Format(LGUConstants.WEATHER_PROBE_SPECIFY_SUCCESS_FORMAT, selectedWeather.Item1, (selectedWeather.Item2 == LevelWeatherType.None ? "no weather" : $"a {selectedWeather.Item2} weather"), UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PICKED_WEATHER_PRICE.Value));
        }
        private static TerminalNode ExecuteScrapInsuranceCommand(ref Terminal terminal, ref TerminalNode outputNode)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.SCRAP_INSURANCE_ENABLED.Value) return outputNode;

            if (ScrapInsurance.GetScrapInsuranceStatus())
                return DisplayTerminalMessage(LGUConstants.SCRAP_INSURANCE_ALREADY_PURCHASED);

            if (!StartOfRound.Instance.inShipPhase)
                return DisplayTerminalMessage(LGUConstants.SCRAP_INSURANCE_ONLY_IN_ORBIT);

            if (terminal.groupCredits < UpgradeBus.Instance.PluginConfiguration.SCRAP_INSURANCE_PRICE.Value)
                return DisplayTerminalMessage(string.Format(LGUConstants.SCRAP_INSURANCE_NOT_ENOUGH_CREDITS_FORMAT, UpgradeBus.Instance.PluginConfiguration.SCRAP_INSURANCE_PRICE.Value, terminal.groupCredits));

            terminal.groupCredits -= UpgradeBus.Instance.PluginConfiguration.SCRAP_INSURANCE_PRICE.Value;
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits);
            ScrapInsurance.TurnOnScrapInsurance();
            return DisplayTerminalMessage(LGUConstants.SCRAP_INSURANCE_SUCCESS);
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
            if(ContractManager.Instance.contractLevel != StartOfRound.Instance.currentLevel.PlanetName || ContractManager.Instance.contractType != "defusal")
            {
                return DisplayTerminalMessage(LGUConstants.LOOKUP_NOT_IN_CONTRACT);
            }
            if (secondWord == "") return DisplayTerminalMessage(LGUConstants.LOOKUP_USAGE);
            if (secondWord.ToLower() == ContractManager.Instance.SerialNumber.ToLower() || secondWord.ToLower() == ContractManager.Instance.SerialNumber.Replace("-","").ToLower())
            {
                logger.LogInfo("DEFUSAL: user entered correct serial number!");
                return DisplayTerminalMessage(string.Format(LGUConstants.LOOKUP_CUT_WIRES_FORMAT, string.Join("\n\n", ContractManager.Instance.bombOrder)));
            }
            else
            {
                logger.LogInfo($"DEFUSAL: user entered incorrect serial number! Entered: {secondWord}, Expected: {ContractManager.Instance.SerialNumber} (case and hyphen insensitive)");
                if (ContractManager.Instance.fakeBombOrders.ContainsKey(secondWord))
                {
                    logger.LogInfo("DEFUSAL: Reusing previously generated fake defusal under this key.");
                    return DisplayTerminalMessage(string.Format(LGUConstants.LOOKUP_CUT_WIRES_FORMAT, string.Join("\n\n", ContractManager.Instance.fakeBombOrders[secondWord])));
                }
                logger.LogInfo("DEFUSAL: Generating new fake defusal under this key.");
                List<string> falseOrder = new List<string> { "red","green","blue" };
                Tools.ShuffleList(falseOrder);
                ContractManager.Instance.fakeBombOrders.Add(secondWord, falseOrder);
                return DisplayTerminalMessage(string.Format(LGUConstants.LOOKUP_CUT_WIRES_FORMAT, string.Join("\n\n", falseOrder)));
            }
        }

        private static TerminalNode HandleBruteForce(string secondWord)
        {
            string txt = null;
            string ip = ContractManager.Instance.DataMinigameKey;
            if(secondWord == ip)
            {
                logger.LogInfo($"USER CORRECTLY ENTERED IP ADDRESS, user: {ContractManager.Instance.DataMinigameUser}, pass: {ContractManager.Instance.DataMinigamePass}");
                txt = $"PING {ip} ({ip}): 56 data bytes\r\n64 bytes from {ip}: icmp_seq=0 ttl=64 time=1.234 ms\r\n64 bytes from {ip}: icmp_seq=1 ttl=64 time=1.345 ms\r\n64 bytes from {ip}: icmp_seq=2 ttl=64 time=1.123 ms\r\n64 bytes from {ip}: icmp_seq=3 ttl=64 time=1.456 ms\r\n\r\n--- {ip} ping statistics ---\r\n4 packets transmitted, 4 packets received, 0.0% packet loss\r\nround-trip min/avg/max/stddev = 1.123/1.289/1.456/0.123 ms\n\n";
                txt += $"CONNECTION ESTABLISHED --- RETRIEVING CREDENTIALS...\n\nUSER: {ContractManager.Instance.DataMinigameUser}\nPASSWORD: {ContractManager.Instance.DataMinigamePass}\n";
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
            LguStore.Instance.SaveInfo = LguStore.Instance.LguSave.playerSaves[id];
            LguStore.Instance.UpdateUpgradeBus(false);
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

        private static TerminalNode ExecuteLGUCommands()
        {
            string displayText = "Late Game Commands\n\n";
            displayText += HelpTerminalNode.HandleHelpExtendDeadline();
            displayText += HelpTerminalNode.HandleHelpInterns();
            displayText += HelpTerminalNode.HandleHelpContract();
            displayText += HelpTerminalNode.HandleHelpDiscombobulator();
            displayText += HelpTerminalNode.HandleHelpScrapInsurance();
            displayText += HelpTerminalNode.HandleHelpWeatherProbe();
            displayText += "\n\n";
            return DisplayTerminalMessage(displayText);
        }
    }
}
