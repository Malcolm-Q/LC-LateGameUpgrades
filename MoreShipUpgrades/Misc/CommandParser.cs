using GameNetcodeStuff;
using MoreShipUpgrades.Configuration.Upgrades.Custom;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UI.TerminalNodes;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.Items.Contracts.Exorcism;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship;
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
        static readonly LguLogger logger = new(nameof(CommandParser));

        const string LOAD_LGU_COMMAND = "load lgu";
        public static readonly List<string> contracts = [LguConstants.DATA_CONTRACT_NAME, LguConstants.EXTERMINATOR_CONTRACT_NAME, LguConstants.EXTRACTION_CONTRACT_NAME,LguConstants.EXORCISM_CONTRACT_NAME,LguConstants.DEFUSAL_CONTRACT_NAME];
        public static readonly List<string> contractInfos = [
            "\n\nOur systems have detected an active PC somewhere in the facility.\nFind it, use the bruteforce command on the ship terminal with the devices IP to get login credentials, then use the cd, ls, and mv commands to find the .db file (enter `mv survey.db` in the containing folder).\n\n",
            "\n\nIt's been reported that the population of hoarder bugs on this moon have skyrocketed and become aggressive. You must destroy their nest at all costs.\n\n",
            "\n\nCrew number 5339 have reported that one of their operatives was lost on this moon and left behind. You will have to find or bring a medkit to heal and extract the lost operative.\n\n" ,
            "\n\nUnusual activity in the spirit world has been reported at this facility.\nFind the ritual site to determine what type of demon it is, enter `demon DemonName` in the terminal to get the ritual instructions. Find ritual items and banish the demon.\n\n" ,
            "\n\nAn unknown party has planted a bomb at an integral point in this facility.\nYou must find the bomb and work together to defuse it.\nUse the `Lookup` command with the bombs serial number to get defusal instructions.\n\n"
        ];

        private static TerminalNode DisplayTerminalMessage(string message, bool clearPreviousText = true)
        {
            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            node.displayText = message;
            node.clearPreviousText = clearPreviousText;
            return node;
        }
        private static TerminalNode ExecuteDiscombobulatorAttack(ref Terminal terminal)
        {
            if (!BaseUpgrade.GetActiveUpgrade(Discombobulator.UPGRADE_NAME)) return DisplayTerminalMessage(LguConstants.DISCOMBOBULATOR_NOT_ACTIVE);

            if (Discombobulator.instance.flashCooldown > 0f) return DisplayTerminalMessage(string.Format(LguConstants.DISCOMBOBULATOR_ON_COOLDOWN_FORMAT, Mathf.Round(Discombobulator.instance.flashCooldown)));

            RoundManager.Instance.PlayAudibleNoise(terminal.transform.position, 60f, 0.8f, 0, false, 14155);
            Discombobulator.instance.UseDiscombobulatorServerRpc();

            DiscombobulatorUpgradeConfiguration config = UpgradeBus.Instance.PluginConfiguration.DiscombobulatorUpgradeConfiguration;
            Collider[] array = Physics.OverlapSphere(terminal.transform.position, config.Radius.Value, 524288);
            if (array.Length == 0) return DisplayTerminalMessage(LguConstants.DISCOMBOBULATOR_NO_ENEMIES);

            if (config.NotifyChat.Value)
            {
                terminal.StartCoroutine(CountDownChat(config.InitialEffect.Value + (config.IncrementalEffect.Value * BaseUpgrade.GetUpgradeLevel(Discombobulator.UPGRADE_NAME))));
            }
            return DisplayTerminalMessage(string.Format(LguConstants.DISCOMBULATOR_HIT_ENEMIES, array.Length));
        }

        private static TerminalNode ExecuteDiscombobulatorCooldown()
        {
            if (!BaseUpgrade.GetActiveUpgrade(Discombobulator.UPGRADE_NAME)) return DisplayTerminalMessage(LguConstants.DISCOMBOBULATOR_NOT_ACTIVE);

            if (Discombobulator.instance.flashCooldown > 0f) return DisplayTerminalMessage(string.Format(LguConstants.DISCOMBOBULATOR_DISPLAY_COOLDOWN, Mathf.Round(Discombobulator.instance.flashCooldown)));

            return DisplayTerminalMessage(LguConstants.DISCOMBOBULATOR_READY);
        }

        private static TerminalNode ExecuteModInformation()
        {
            string displayText = "Late Game Upgrades\n\nType `lategame store` or `lgu` to view upgrades.\n\nMost of the mod is configurable via the config file in `BepInEx/config/com.malco.lethalcompany.moreshipupgrades.cfg`.";
            displayText += "\n\nType `lategame commands` or 'lgc' to see all commands related to upgrades";
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
                SaveInfo saveInfo = new();
                ulong id = GameNetworkManager.Instance.localPlayerController.playerSteamId;
                LguStore.Instance.SaveInfo = saveInfo;
                LguStore.Instance.UpdateLGUSaveServerRpc(id, Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(saveInfo)));
                return DisplayTerminalMessage(LguConstants.LGU_SAVE_WIPED);
            }
            else
            {
                logger.LogError("LGU SAVE NOT FOUND in ExecuteResetLGUSave()!");
                return DisplayTerminalMessage(LguConstants.LGU_SAVE_NOT_FOUND);
            }
        }
        private static TerminalNode ExecuteForceCredits(string creditAmount, ref Terminal terminal)
        {
            if (int.TryParse(creditAmount, out int value))
            {
                if (terminal.IsHost || terminal.IsServer)
                {
                    terminal.SyncGroupCreditsClientRpc(value, terminal.numberOfItemsInDropship);
                    return DisplayTerminalMessage(string.Format(LguConstants.FORCE_CREDITS_SUCCESS_FORMAT, value));
                }
                else
                {
                    return DisplayTerminalMessage(LguConstants.FORCE_CREDITS_HOST_ONLY);
                }
            }

            return DisplayTerminalMessage(string.Format(LguConstants.FORCE_CREDITS_PARSED_FAIL_FORMAT, creditAmount));
        }

        private static TerminalNode ExecuteInternsCommand(ref Terminal terminal, TerminalNode outputNode)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.INTERN_ENABLED) return outputNode;

            if (terminal.groupCredits < UpgradeBus.Instance.PluginConfiguration.INTERN_PRICE.Value)
                return DisplayTerminalMessage(string.Format(LguConstants.INTERNS_NOT_ENOUGH_CREDITS_FORMAT, UpgradeBus.Instance.PluginConfiguration.INTERN_PRICE.Value, terminal.groupCredits));

            if (Interns.instance.revivalTimer > 0f)
                return DisplayTerminalMessage(string.Format("We currently cannot provide you with a new intern. Please try again later. ({0} seconds)\n\n", Interns.instance.revivalTimer.ToString("F2")));

            if (UpgradeBus.Instance.PluginConfiguration.INTERNS_USAGES_PER_LANDING != -1 && Interns.instance.currentUsages >= UpgradeBus.Instance.PluginConfiguration.INTERNS_USAGES_PER_LANDING)
                return DisplayTerminalMessage(string.Format("You have depleted your interns for the current moon landing ({0} usages). They will be replenished on the next moon landing\n\n", Interns.instance.currentUsages));

            if (Interns.instance.delayReviveTimer > 0f)
                return DisplayTerminalMessage(string.Format($"An intern is being dispatched to replace {Interns.instance.delayedRevivePlayer.playerUsername} in {Interns.instance.delayReviveTimer.ToString("F2")} seconds.\n\n"));

            PlayerControllerB player = StartOfRound.Instance.mapScreen.targetedPlayer;
            if (!player.isPlayerDead) return DisplayTerminalMessage(string.Format(LguConstants.INTERNS_PLAYER_ALREADY_ALIVE_FORMAT, player.playerUsername));
            terminal.BuyItemsServerRpc([], terminal.groupCredits - UpgradeBus.Instance.PluginConfiguration.INTERN_PRICE.Value, terminal.numberOfItemsInDropship);
            Interns.instance.ReviveTargetedPlayerServerRpc();
            string name = Interns.instance.internNames[UnityEngine.Random.Range(0, Interns.instance.internNames.Length)];
            string interest = Interns.instance.internInterests[UnityEngine.Random.Range(0, Interns.instance.internInterests.Length)];
            logger.LogInfo($"Successfully executed intern command for {player.playerUsername}!");
            return DisplayTerminalMessage($"{player.playerUsername} has been replaced with:\n\nNAME: {name}\n" +
                $"AGE: {UnityEngine.Random.Range(19, 76)}\nIQ: {UnityEngine.Random.Range(2, 160)}\n" +
                $"INTERESTS: {interest}\n\n{name} HAS BEEN DISPATCHED TO YOUR LOCATION, PLEASE ACQUAINTANCE YOURSELF ACCORDINGLY\n\n");
        }
        private static TerminalNode ExecuteLoadLGUCommand(string text, ref Terminal terminal)
        {
            if (text.ToLower() == LOAD_LGU_COMMAND) return DisplayTerminalMessage(LguConstants.LOAD_LGU_NO_NAME);

            PlayerControllerB[] players = UnityEngine.Object.FindObjectsOfType<PlayerControllerB>();
            List<string> playerNames = [];
            string playerNameToSearch = text[(text.IndexOf(LOAD_LGU_COMMAND) + LOAD_LGU_COMMAND.Length)..].Trim();
            foreach (PlayerControllerB player in players)
            {
                if (player == null) continue;
                string playerName = player.playerUsername;
                ulong playerSteamID = player.playerSteamId;
                if (playerName == null) continue;
                playerNames.Add(playerName);
                if (!playerName.Contains(playerNameToSearch, System.StringComparison.OrdinalIgnoreCase)) continue;

                LguStore.Instance.ShareSaveServerRpc();
                terminal.StartCoroutine(WaitForSync(playerSteamID));
                logger.LogInfo($"Attempting to overwrite local save data with {playerName}'s save data.");
                return DisplayTerminalMessage(string.Format(LguConstants.LOAD_LGU_SUCCESS_FORMAT, playerName));
            }
            string csvNames = string.Join(", ", playerNames);
            logger.LogInfo($"{playerNameToSearch} was not found among: {csvNames}");
            return DisplayTerminalMessage(string.Format(LguConstants.LOAD_LGU_FAILURE_FORMAT, playerNameToSearch, csvNames));
        }

        private static TerminalNode ExecuteScanHivesCommand()
        {
            if (BaseUpgrade.GetUpgradeLevel(BetterScanner.UPGRADE_NAME) < 1) return DisplayTerminalMessage(LguConstants.SCANNER_LEVEL_REQUIRED);

            GrabbableObject[] scrapItems = Object.FindObjectsOfType<GrabbableObject>();
            GrabbableObject[] filteredHives = scrapItems.Where(scrap => scrap.itemProperties.itemName == "Hive").ToArray();
            GrabbableObject[] bestHives = [.. filteredHives.OrderByDescending(v => v.scrapValue)];
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine($"Found {bestHives.Length} Hives:");
            foreach (GrabbableObject scrap in bestHives)
            {
                stringBuilder.AppendLine($"{scrap.scrapValue} // X: {scrap.gameObject.transform.position.x:F1}, Y: {scrap.gameObject.transform.position.y:F1}, Z: {scrap.gameObject.transform.position.z:F1}");
            }
            stringBuilder.AppendLine("Don't forget your GPS!").AppendLine();
            logger.LogInfo($"Scan Hives command found {filteredHives.Length} hives.");
            return DisplayTerminalMessage(stringBuilder.ToString());
        }

        private static TerminalNode ExecuteScanScrapCommand()
        {
            if (BaseUpgrade.GetUpgradeLevel(BetterScanner.UPGRADE_NAME) < 1) return DisplayTerminalMessage(LguConstants.SCANNER_LEVEL_REQUIRED);

            GrabbableObject[] scrapItems = [.. Object.FindObjectsOfType<GrabbableObject>()];
            GrabbableObject[] filteredScrap = [.. scrapItems.Where(scrap => scrap.isInFactory && scrap.itemProperties.isScrap)];
            GrabbableObject[] bestScrap = [.. filteredScrap.OrderByDescending(v => v.scrapValue).Take(5)];
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine("Most valuable items:");
            foreach (GrabbableObject scrap in bestScrap)
            {
                stringBuilder.AppendLine($"{scrap.itemProperties.itemName}: ${scrap.scrapValue}\nX: {Mathf.RoundToInt(scrap.gameObject.transform.position.x)}, Y: {Mathf.RoundToInt(scrap.gameObject.transform.position.y)}, Z: {Mathf.RoundToInt(scrap.gameObject.transform.position.z)}");
            }
            stringBuilder.AppendLine("Don't forget your GPS!").AppendLine();
            logger.LogInfo($"Scan scrap command found {filteredScrap.Length} valid scrap items.");
            return DisplayTerminalMessage(stringBuilder.ToString());
        }

        private static TerminalNode ExecuteScanPlayerCommand()
        {
            if (BaseUpgrade.GetUpgradeLevel(BetterScanner.UPGRADE_NAME) < 1) return DisplayTerminalMessage(LguConstants.SCANNER_LEVEL_REQUIRED);

            PlayerControllerB[] players = [.. Object.FindObjectsOfType<PlayerControllerB>()];
            PlayerControllerB[] filteredPlayers = [.. players.Where(player => player.playerSteamId != 0)];
            PlayerControllerB[] alivePlayers = [.. filteredPlayers.Where(player => !player.isPlayerDead)];
            PlayerControllerB[] deadPlayers = [.. filteredPlayers.Where(player => player.isPlayerDead)];
            StringBuilder stringBuilder = new();

            stringBuilder.AppendLine("Alive Players:");
            foreach (PlayerControllerB player in alivePlayers)
            {
                stringBuilder.AppendLine($"\n{player.playerUsername} - X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}");
            }
            stringBuilder.AppendLine("Dead Players:");
            foreach (PlayerControllerB player in deadPlayers)
            {
                stringBuilder.AppendLine($"\n{player.playerUsername} - X:{Mathf.RoundToInt(player.transform.position.x)},Y:{Mathf.RoundToInt(player.transform.position.y)},Z:{Mathf.RoundToInt(player.transform.position.z)}");
            }
            stringBuilder.AppendLine();
            logger.LogInfo($"Scan players command found {alivePlayers.Length} alive players and {deadPlayers.Length} dead players.");
            return DisplayTerminalMessage(stringBuilder.ToString());
        }
        private static TerminalNode ExecuteScanEnemiesCommand()
        {
            if (BaseUpgrade.GetUpgradeLevel(BetterScanner.UPGRADE_NAME) < 1) return DisplayTerminalMessage(LguConstants.SCANNER_LEVEL_REQUIRED);

            EnemyAI[] enemies = Object.FindObjectsOfType<EnemyAI>().Where(enem => !enem.isEnemyDead).ToArray();
            if (enemies.Length == 0) return DisplayTerminalMessage("0 enemies detected\n\n");

            Dictionary<string, int> enemyCount = [];
            if (!UpgradeBus.Instance.PluginConfiguration.BetterScannerUpgradeConfiguration.VerboseEnemies.Value)
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
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine($"Alive Enemies: {enemies.Length}");
            foreach (KeyValuePair<string, int> count in enemyCount)
            {
                stringBuilder.AppendLine($"{count.Key} - {count.Value}");
            }
            stringBuilder.AppendLine();
            return DisplayTerminalMessage(stringBuilder.ToString());
        }

        private static TerminalNode ExecuteScanDoorsCommand()
        {
            if (BaseUpgrade.GetUpgradeLevel(BetterScanner.UPGRADE_NAME) < 1) return DisplayTerminalMessage(LguConstants.SCANNER_LEVEL_REQUIRED);

            List<GameObject> fireEscape = [.. Object.FindObjectsOfType<GameObject>().Where(obj => obj.name == "SpawnEntranceBTrigger")];
            List<EntranceTeleport> mainDoors = [.. Object.FindObjectsOfType<EntranceTeleport>()];
            List<EntranceTeleport> doorsToRemove = [];

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

            StringBuilder stringBuilder = new();
            if (player.isInsideFactory)
            {
                stringBuilder.AppendLine($"Closest exits to {player.playerUsername} " +
                                    $"(X:{Mathf.RoundToInt(player.transform.position.x)}," +
                                    $"Y:{Mathf.RoundToInt(player.transform.position.y)}," +
                                    $"Z:{Mathf.RoundToInt(player.transform.position.z)}):");
                GameObject[] Closest3 = fireEscape.OrderBy(door => Vector3.Distance(door.transform.position, player.transform.position)).Take(3).ToArray();
                foreach (Vector3 doorPosition in Closest3.Select(door => door.transform.position))
                {
                    stringBuilder.AppendLine($"X:{Mathf.RoundToInt(doorPosition.x)}," +
                                        $"Y:{Mathf.RoundToInt(doorPosition.y)}," +
                                        $"Z:{Mathf.RoundToInt(doorPosition.z)} - " +
                                        $"{Mathf.RoundToInt(Vector3.Distance(doorPosition, player.transform.position))} units away.");
                }
                logger.LogInfo($"Scan Doors, player is inside factory. Found {fireEscape.Count} doors.");
            }
            else
            {
                stringBuilder.AppendLine($"Entrances for {player.playerUsername} " +
                                    $"(X:{Mathf.RoundToInt(player.transform.position.x)}," +
                                    $"Y:{Mathf.RoundToInt(player.transform.position.y)}," +
                                    $"Z:{Mathf.RoundToInt(player.transform.position.z)}):");
                foreach (Vector3 doorPosition in mainDoors.Select(door => door.transform.position))
                {
                    stringBuilder.AppendLine($"X:{Mathf.RoundToInt(doorPosition.x)},Y:{Mathf.RoundToInt(doorPosition.y)},Z:{Mathf.RoundToInt(doorPosition.z)} - {Mathf.RoundToInt(Vector3.Distance(doorPosition, player.transform.position))} units away.");
                }
                logger.LogInfo($"Scan Doors, player is outside factory. Found {mainDoors.Count} doors.");
            }
            stringBuilder.AppendLine();
            return DisplayTerminalMessage(stringBuilder.ToString());
        }

        private static TerminalNode ExecuteLategameCommands(string secondWord)
        {
            return secondWord switch
            {
                "commands" => ExecuteLGUCommands(),
                _ => ExecuteModInformation(),
            };
        }
        private static TerminalNode ExecuteResetCommands(string secondWord, ref TerminalNode outputNode)
        {
            return secondWord switch
            {
                "lgu" => ExecuteResetLGUSave(),
                _ => outputNode,
            };
        }
        private static TerminalNode ExecuteLoadCommands(string secondWord, string fullText, ref Terminal terminal, ref TerminalNode outputNode)
        {
            return secondWord switch
            {
                "lgu" => ExecuteLoadLGUCommand(fullText, ref terminal),
                _ => outputNode,
            };
        }
        private static TerminalNode ExecuteScanCommands(string secondWord, ref TerminalNode outputNode)
        {
            return secondWord switch
            {
                "hives" => ExecuteScanHivesCommand(),
                "scrap" => ExecuteScanScrapCommand(),
                "player" => ExecuteScanPlayerCommand(),
                "enemies" => ExecuteScanEnemiesCommand(),
                "doors" => ExecuteScanDoorsCommand(),
                _ => outputNode,
            };
        }
        private static TerminalNode ExecuteBruteForce(string secondWord)
        {
            return secondWord switch
            {
                "" => DisplayTerminalMessage(LguConstants.BRUTEFORCE_USAGE),
                _ => HandleBruteForce(secondWord),
            };
        }
        public static void ParseLGUCommands(string fullText, ref Terminal terminal, ref TerminalNode outputNode)
        {
            string[] textArray = fullText.Split();
            string firstWord = textArray[0].ToLower();
            string secondWord = textArray.Length > 1 ? textArray[1].ToLower() : "";
            string thirdWord = textArray.Length > 2 ? textArray[2].ToLower() : "";
            switch (firstWord)
            {
                case "demon": outputNode = LookupDemon(secondWord, thirdWord); return;
                case "lookup": outputNode = DefuseBombCommand(secondWord); return;
                case "bruteforce": outputNode= ExecuteBruteForce(secondWord); return;
                case "initattack":
                case "atk": outputNode = ExecuteDiscombobulatorAttack(ref terminal); return;
                case "cd":
                case "cooldown": outputNode = ExecuteDiscombobulatorCooldown(); return;
                case "lategame": outputNode = ExecuteLategameCommands(secondWord); return;
                case "lgc": outputNode = ExecuteLGUCommands(); return;
                case "reset": outputNode = ExecuteResetCommands(secondWord, ref outputNode); return;
                case "forcecredits": outputNode = ExecuteForceCredits(secondWord, ref terminal); return;
                case "intern":
                case "interns": outputNode = ExecuteInternsCommand(ref terminal, outputNode); return;
                case "load": outputNode = ExecuteLoadCommands(secondWord, fullText, ref terminal, ref outputNode); return;
                case "scan": outputNode = ExecuteScanCommands(secondWord, ref outputNode); return;
                case "quantum": ExecuteQuantumCommands(ref terminal, ref outputNode); return;
                case "contract": ExecuteContractCommands(ref terminal, ref outputNode); return;
                default: return;
            }
        }
        static void ExecuteContractCommands(ref Terminal terminal, ref TerminalNode outputNode)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.CONTRACT_PROVIDE_RANDOM_ONLY) return;
            outputNode = TryGetContract(ref terminal);
        }
        static TerminalNode TryGetContract(ref Terminal terminal)
        {
            if (contracts.Count == 0) return DisplayTerminalMessage(LguConstants.CONTRACT_FAIL);
            string txt;
            if (ContractManager.Instance.contractLevel != "None")
            {
                txt = $"You currently have a {ContractManager.Instance.contractType} contract on {ContractManager.Instance.contractLevel}!\n\n";
                txt += string.Format(AssetBundleHandler.GetInfoFromJSON("Contract"), UpgradeBus.Instance.PluginConfiguration.CONTRACT_PRICE.Value);
                logger.LogInfo($"User tried starting a new contract while they still have a {ContractManager.Instance.contractType} contract on {ContractManager.Instance.contractLevel}!");
                return DisplayTerminalMessage(txt);
            }
            if (terminal.groupCredits < UpgradeBus.Instance.PluginConfiguration.CONTRACT_PRICE.Value)
            {
                txt = $"Contracts costs ${UpgradeBus.Instance.PluginConfiguration.CONTRACT_PRICE.Value} and you have ${terminal.groupCredits}\n\n";
                return DisplayTerminalMessage(txt);
            }
            terminal.BuyItemsServerRpc([], terminal.groupCredits - UpgradeBus.Instance.PluginConfiguration.CONTRACT_PRICE.Value, terminal.numberOfItemsInDropship);
            int i = Random.Range(0, contracts.Count);
            if (contracts.Count > 1)
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
        private static void ExecuteQuantumCommands(ref Terminal terminal, ref TerminalNode outputNode)
        {
            if (!UpgradeBus.Instance.PluginConfiguration.QuantumDisruptorConfiguration.Enabled) return;
            if (!BaseUpgrade.GetActiveUpgrade(QuantumDisruptor.UPGRADE_NAME))
            {
                outputNode = DisplayTerminalMessage("You need \'Quantum Disruptor\' upgrade active to use this command.\n");
                return;
            }
            (bool, string) canRevert = QuantumDisruptor.Instance.CanRevertTime();
            if (!canRevert.Item1)
            {
                outputNode = DisplayTerminalMessage(canRevert.Item2);
                return;
            }
            if (terminal.IsHost || terminal.IsServer) QuantumDisruptor.Instance.RevertTimeClientRpc();
            else QuantumDisruptor.Instance.RevertTimeServerRpc();
            outputNode = DisplayTerminalMessage($"Successfully reverted back current moon's time by {QuantumDisruptor.Instance.hoursToReduce}. You currently have {QuantumDisruptor.Instance.currentUsages} out of {QuantumDisruptor.Instance.availableUsages} usages.\n");
        }
        private static TerminalNode LookupDemon(string secondWord, string thirdWord)
        {
            string demon = secondWord.ToUpper();
            if (demon == "DE" && string.Equals(thirdWord, "OGEN", System.StringComparison.OrdinalIgnoreCase)) demon = demon + " " + thirdWord.ToUpper();
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
            if(ContractManager.Instance.contractLevel != StartOfRound.Instance.currentLevel.PlanetName || ContractManager.Instance.contractType != LguConstants.DEFUSAL_CONTRACT_NAME)
            {
                return DisplayTerminalMessage(LguConstants.LOOKUP_NOT_IN_CONTRACT);
            }
            if (secondWord.Length == 0) return DisplayTerminalMessage(LguConstants.LOOKUP_USAGE);
            if (string.Equals(secondWord, ContractManager.Instance.SerialNumber, System.StringComparison.OrdinalIgnoreCase) || string.Equals(secondWord, ContractManager.Instance.SerialNumber.Replace("-", ""), System.StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInfo("DEFUSAL: user entered correct serial number!");
                return DisplayTerminalMessage(string.Format(LguConstants.LOOKUP_CUT_WIRES_FORMAT, string.Join("\n\n", ContractManager.Instance.bombOrder)));
            }
            else
            {
                logger.LogInfo($"DEFUSAL: user entered incorrect serial number! Entered: {secondWord}, Expected: {ContractManager.Instance.SerialNumber} (case and hyphen insensitive)");
                if (ContractManager.Instance.fakeBombOrders.ContainsKey(secondWord))
                {
                    logger.LogInfo("DEFUSAL: Reusing previously generated fake defusal under this key.");
                    return DisplayTerminalMessage(string.Format(LguConstants.LOOKUP_CUT_WIRES_FORMAT, string.Join("\n\n", ContractManager.Instance.fakeBombOrders[secondWord])));
                }
                logger.LogInfo("DEFUSAL: Generating new fake defusal under this key.");
                List<string> falseOrder = ["red","green","blue"];
                Tools.ShuffleList(falseOrder);
                ContractManager.Instance.fakeBombOrders.Add(secondWord, falseOrder);
                return DisplayTerminalMessage(string.Format(LguConstants.LOOKUP_CUT_WIRES_FORMAT, string.Join("\n\n", falseOrder)));
            }
        }

        private static TerminalNode HandleBruteForce(string secondWord)
        {
            string txt;
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
                txt += "CONNECTION FAILED -- INVALID ADDRESS?\n\n";
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
            HUDManager.Instance.chatText.text += $"<color=#FFFFFF>Stun Duration: {count:F1} seconds.</color>";
            while (count > 0f)
            {
                yield return new WaitForSeconds(1f);
                count--;
                HUDManager.Instance.chatText.text = "";
                HUDManager.Instance.chatText.text += $"<color=#FFFFFF>Stun Duration: {count:F1} seconds.</color>";
            }
            HUDManager.Instance.chatText.text = "";
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Effected enemies are no longer stunned!</color>";
        }

        private static TerminalNode ExecuteLGUCommands()
        {
            string displayText = "Late Game Commands\n\n";
            displayText += HelpTerminalNode.HandleHelpInterns();
            displayText += HelpTerminalNode.HandleHelpContract();
            displayText += HelpTerminalNode.HandleHelpDiscombobulator();
            displayText += HelpTerminalNode.HandleAlternateCurrency();
            displayText += "\n\n";
            return DisplayTerminalMessage(displayText);
        }
    }
}
