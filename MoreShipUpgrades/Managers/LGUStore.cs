﻿using MoreShipUpgrades.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections;
using MoreShipUpgrades.UpgradeComponents;

namespace MoreShipUpgrades.Managers
{
    public class LGUStore : NetworkBehaviour
    {
        public static LGUStore instance;
        public SaveInfo saveInfo;
        public LGUSave lguSave;
        private ulong playerID = 0;

        private static Dictionary<string, Func<SaveInfo, bool>> conditions = new Dictionary<string, Func<SaveInfo, bool>>
        {
            { "Malware Broadcaster", saveInfo => saveInfo.DestroyTraps },
            { "Light Footed", saveInfo => saveInfo.softSteps },
            {"Discombobulator", SaveInfo => SaveInfo.terminalFlash },
            {"Bigger Lungs", SaveInfo => SaveInfo.biggerLungs },
            {"Running Shoes", SaveInfo => SaveInfo.runningShoes },
            {"NV Headset Batteries", SaveInfo => SaveInfo.nightVision },
            {"Strong Legs", SaveInfo => SaveInfo.strongLegs },
            {"Better Scanner", SaveInfo => SaveInfo.scannerUpgrade },
            {"Beekeeper", SaveInfo => SaveInfo.beekeeper },
            {"Back Muscles", SaveInfo => SaveInfo.exoskeleton },
            {"Locksmith", SaveInfo => SaveInfo.lockSmith },
            {"Walkie GPS", SaveInfo => SaveInfo.walkies },
            {radarFlashScript.UPGRADE_NAME, SaveInfo => SaveInfo.radarFlash },
        };

        private static Dictionary<string, Func<SaveInfo, int>> levelConditions = new Dictionary<string, Func<SaveInfo, int>>
        {
            { "Malware Broadcaster", saveInfo => 0 },
            { "Light Footed", saveInfo => saveInfo.lightLevel },
            { "Discombobulator", saveInfo => saveInfo.discoLevel },
            { "Bigger Lungs", saveInfo => saveInfo.lungLevel },
            { "Running Shoes", saveInfo => saveInfo.runningLevel },
            { "NV Headset Batteries", saveInfo => saveInfo.nightVisionLevel },
            { "Strong Legs", saveInfo => saveInfo.legLevel },
            { "Better Scanner", saveInfo => 0 },
            { "Beekeeper", saveInfo => saveInfo.beeLevel },
            { "Back Muscles", saveInfo => saveInfo.backLevel },
            { "Locksmith", saveInfo => 0 },
            { "Walkie GPS", saveInfo => 0 },
            {radarFlashScript.UPGRADE_NAME, SaveInfo => SaveInfo.radarFlashLevel },
        };

        private void Start()
        {
            instance = this;
            if (NetworkManager.IsHost)
            {
                string saveNum = GameNetworkManager.Instance.saveFileNum.ToString();
                string filePath = Path.Combine(Application.persistentDataPath, $"LGU_{saveNum}.json");
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    lguSave = JsonConvert.DeserializeObject<LGUSave>(json);
                    HandleSpawns();
                    UpdateUpgradeBus();
                }
                else
                {
                    lguSave = new LGUSave();
                    HandleSpawns();
                    UpdateUpgradeBus();
                }
            }
            else
            {
                ShareSaveServerRpc();
            }
            string path = Path.Combine(Application.persistentDataPath, "IntroLGU");
            if(File.Exists(path))
            {
                if(File.ReadAllText(path) != UpgradeBus.instance.version)
                {
                    Instantiate(UpgradeBus.instance.introScreen);
                    File.WriteAllText(path, UpgradeBus.instance.version);
                }    
            }
            else
            {
                Instantiate(UpgradeBus.instance.introScreen);
                File.WriteAllText(path, UpgradeBus.instance.version);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void ServerSaveFileServerRpc()
        {
            string saveNum = GameNetworkManager.Instance.saveFileNum.ToString();
            string filePath = Path.Combine(Application.persistentDataPath, $"LGU_{saveNum}.json");
            string json = JsonConvert.SerializeObject(lguSave);
            File.WriteAllText(filePath, json);
        }

        [ServerRpc(RequireOwnership = false)]
        public void UpdateLGUSaveServerRpc(ulong id, string json)
        {
            lguSave.playerSaves[id] = JsonConvert.DeserializeObject<SaveInfo>(json);
        }

        public void HandleSpawns()
        {
            foreach (CustomTerminalNode node in UpgradeBus.instance.terminalNodes)
            {
                GameObject go = Instantiate(node.Prefab, Vector3.zero, Quaternion.identity);
                go.GetComponent<NetworkObject>().Spawn();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void PlayersFiredServerRpc()
        {
            ResetUpgradeBusClientRpc();
            UpgradeBus.instance.ResetAllValues();
            string saveNum = GameNetworkManager.Instance.saveFileNum.ToString();
            string filePath = Path.Combine(Application.persistentDataPath, $"LGU_{saveNum}.json");
            lguSave = new LGUSave();
            string json = JsonConvert.SerializeObject(lguSave);
            File.WriteAllText(filePath, json);
        }

        [ClientRpc]
        private void ResetUpgradeBusClientRpc()
        {
            UpgradeBus.instance.ResetAllValues();
            saveInfo = new SaveInfo();
        }

        [ServerRpc(RequireOwnership = false)]
        public void ShareSaveServerRpc()
        {
            string json = JsonConvert.SerializeObject(lguSave);
            ShareSaveClientRpc(json);
        }

        [ClientRpc]
        public void ShareSaveClientRpc(string json)
        {
            foreach(BaseUpgrade upgrade in FindObjectsOfType<BaseUpgrade>())
            {
                upgrade.Register();
            }
            lguSave = JsonConvert.DeserializeObject<LGUSave>(json);
            UpdateUpgradeBus();
        }

        [ServerRpc(RequireOwnership =false)]
        public void UpdateBeePercsServerRpc(ulong id, int lvl)
        {
            UpdateBeePercsClientRpc(id, lvl);
        }

        [ClientRpc]
        private void UpdateBeePercsClientRpc(ulong id, int lvl)
        {
            if(UpgradeBus.instance.beePercs.ContainsKey(id))
            {
                UpgradeBus.instance.beePercs[id] = lvl;
            }
            else
            {
                UpgradeBus.instance.beePercs.Add(id, lvl);
            }
        }

        [ServerRpc(RequireOwnership =false)]
        public void UpdateForceMultsServerRpc(ulong id, int lvl)
        {
            UpdateForceMultsClientRpc(id, lvl);
        }

        [ClientRpc]
        private void UpdateForceMultsClientRpc(ulong id, int lvl)
        {
            if(UpgradeBus.instance.forceMults.ContainsKey(id))
            {
                UpgradeBus.instance.forceMults[id] = lvl;
            }
            else
            {
                UpgradeBus.instance.forceMults.Add(id, lvl);
            }
        }


        [ServerRpc(RequireOwnership = false)]
        public void DeleteUpgradesServerRpc()
        {
            BaseUpgrade[] upgradeObjects = GameObject.FindObjectsOfType<BaseUpgrade>();
            foreach (BaseUpgrade upgrade in upgradeObjects)
            {
                GameNetworkManager.Destroy(upgrade.gameObject);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SyncCreditsServerRpc(int newCredits)
        {
            SyncCreditsClientRpc(newCredits);
        }

        [ClientRpc]
        private void SyncCreditsClientRpc(int newCredits)
        {
            Terminal terminal = GameObject.Find("TerminalScript").GetComponent<Terminal>();
            terminal.groupCredits = newCredits;
        }

        [ServerRpc(RequireOwnership =false)]
        private void RegisterNewPlayerServerRpc(ulong id, string json)
        {
            lguSave.playerSaves.Add(id,JsonConvert.DeserializeObject<SaveInfo>(json));
        }

        public void UpdateUpgradeBus(bool UseLocalSteamID = true)
        {
            if(UseLocalSteamID)
            {
                if(playerID == 0)
                {
                    StartCoroutine(WaitForSteamID());
                    return;
                }
                if(!lguSave.playerSaves.ContainsKey(playerID))
                {
                    saveInfo = new SaveInfo();
                    RegisterNewPlayerServerRpc(playerID, JsonConvert.SerializeObject(saveInfo));
                    return;
                }
                saveInfo = lguSave.playerSaves[playerID];
            }
            UpgradeBus.instance.DestroyTraps = saveInfo.DestroyTraps;
            UpgradeBus.instance.softSteps = saveInfo.softSteps;
            UpgradeBus.instance.scannerUpgrade = saveInfo.scannerUpgrade;
            UpgradeBus.instance.nightVision = saveInfo.nightVision;
            UpgradeBus.instance.nightVisionActive = saveInfo.nightVisionActive;
            UpgradeBus.instance.exoskeleton = saveInfo.exoskeleton;
            UpgradeBus.instance.TPButtonPressed = saveInfo.TPButtonPressed;
            UpgradeBus.instance.beekeeper = saveInfo.beekeeper;
            UpgradeBus.instance.terminalFlash = saveInfo.terminalFlash;
            UpgradeBus.instance.radarFlash = saveInfo.radarFlash;
            UpgradeBus.instance.strongLegs = saveInfo.strongLegs;
            UpgradeBus.instance.runningShoes = saveInfo.runningShoes;
            UpgradeBus.instance.biggerLungs = saveInfo.biggerLungs;
            UpgradeBus.instance.proteinPowder = saveInfo.proteinPowder;

            UpgradeBus.instance.beeLevel = saveInfo.beeLevel;
            UpgradeBus.instance.proteinLevel = saveInfo.proteinLevel;
            UpgradeBus.instance.lungLevel = saveInfo.lungLevel;
            UpgradeBus.instance.walkies = saveInfo.walkies;
            UpgradeBus.instance.backLevel = saveInfo.backLevel;
            UpgradeBus.instance.runningLevel = saveInfo.runningLevel;
            UpgradeBus.instance.lightLevel = saveInfo.lightLevel;
            UpgradeBus.instance.discoLevel = saveInfo.discoLevel;
            UpgradeBus.instance.radarFlashLevel = saveInfo.radarFlashLevel;
            UpgradeBus.instance.legLevel = saveInfo.legLevel;
            UpgradeBus.instance.nightVisionLevel = saveInfo.nightVisionLevel;

            StartCoroutine(WaitForUpgradeObject());
        }

        private IEnumerator WaitForSteamID()
        {
            yield return new WaitForSeconds(1f);
            while(playerID == 0)
            {
                playerID = GameNetworkManager.Instance.localPlayerController.playerSteamId;
                Debug.Log(playerID);
                yield return new WaitForSeconds(0.5f);
            }
            UpdateUpgradeBus();
        }

        private IEnumerator WaitForUpgradeObject()
        {
            yield return new WaitForSeconds(1f);
            foreach (CustomTerminalNode customNode in UpgradeBus.instance.terminalNodes)
            {
                if (conditions.TryGetValue(customNode.Name, out var condition) && condition(saveInfo))
                {
                    customNode.Unlocked = true;
                    levelConditions.TryGetValue(customNode.Name, out var level);
                    customNode.CurrentUpgrade = level.Invoke(saveInfo);
                    UpgradeBus.instance.UpgradeObjects[customNode.Name].GetComponent<BaseUpgrade>().load();
                }
            }
        }

        public void HandleUpgrade(string name, bool increment = false)
        {
            if (!UpgradeBus.instance.IndividualUpgrades[name])
            {
                HandleUpgradeServerRpc(name, increment);
                Debug.Log("Shared Upgrade");
                return;
            }
            foreach (CustomTerminalNode node in UpgradeBus.instance.terminalNodes)
            {
                if (node.Name == name)
                {
                    node.Unlocked = true;
                    if(increment) { node.CurrentUpgrade++; }
                    break;
                }
            }
            if(!increment){ UpgradeBus.instance.UpgradeObjects[name].GetComponent<BaseUpgrade>().load(); }
            else { UpgradeBus.instance.UpgradeObjects[name].GetComponent<BaseUpgrade>().Increment(); }
            saveInfo = new SaveInfo();
            UpdateLGUSaveServerRpc(playerID, JsonConvert.SerializeObject(saveInfo));
        }

        [ServerRpc(RequireOwnership = false)]
        public void EnableNightVisionServerRpc()
        {
            EnableNightVisionClientRpc();
        }

        [ClientRpc]
        private void EnableNightVisionClientRpc()
        {
            UpgradeBus.instance.UpgradeObjects["NV Headset Batteries"].GetComponent<nightVisionScript>().EnableOnClient();
        }

        [ServerRpc(RequireOwnership = false)]
        private void HandleUpgradeServerRpc(string name, bool increment)
        {
            HandleUpgradeClientRpc(name, increment);
        }

        [ClientRpc]
        private void HandleUpgradeClientRpc(string name, bool increment)
        {
            foreach (CustomTerminalNode node in UpgradeBus.instance.terminalNodes)
            {
                if (node.Name == name)
                {
                    node.Unlocked = true;
                    if(increment) { node.CurrentUpgrade++; }
                }
            }
            if(!increment){ UpgradeBus.instance.UpgradeObjects[name].GetComponent<BaseUpgrade>().load(); }
            else { UpgradeBus.instance.UpgradeObjects[name].GetComponent<BaseUpgrade>().Increment(); }
            saveInfo = new SaveInfo();
            UpdateLGUSaveServerRpc(playerID, JsonConvert.SerializeObject(saveInfo));
        }
    }

    [Serializable]
    public class SaveInfo
    {
        public bool DestroyTraps = UpgradeBus.instance.DestroyTraps;
        public bool softSteps = UpgradeBus.instance.softSteps;
        public bool scannerUpgrade = UpgradeBus.instance.scannerUpgrade;
        public bool nightVision = UpgradeBus.instance.nightVision;
        public bool nightVisionActive = UpgradeBus.instance.nightVisionActive;
        public bool exoskeleton = UpgradeBus.instance.exoskeleton;
        public bool TPButtonPressed = UpgradeBus.instance.TPButtonPressed;
        public bool beekeeper = UpgradeBus.instance.beekeeper;
        public bool terminalFlash = UpgradeBus.instance.terminalFlash;
        public bool radarFlash = UpgradeBus.instance.radarFlash;
        public bool strongLegs = UpgradeBus.instance.strongLegs;
        public bool proteinPowder = UpgradeBus.instance.proteinPowder;
        public bool runningShoes = UpgradeBus.instance.runningShoes;
        public bool biggerLungs = UpgradeBus.instance.biggerLungs;
        public bool lockSmith = UpgradeBus.instance.lockSmith;
        public bool walkies = UpgradeBus.instance.walkies;

        public int beeLevel = UpgradeBus.instance.beeLevel;
        public int proteinLevel = UpgradeBus.instance.proteinLevel;
        public int lungLevel = UpgradeBus.instance.lungLevel;
        public int backLevel = UpgradeBus.instance.backLevel;
        public int runningLevel = UpgradeBus.instance.runningLevel;
        public int lightLevel = UpgradeBus.instance.lightLevel;
        public int discoLevel = UpgradeBus.instance.discoLevel;
        public int radarFlashLevel = UpgradeBus.instance.radarFlashLevel;
        public int legLevel = UpgradeBus.instance.legLevel;
        public int nightVisionLevel = UpgradeBus.instance.nightVisionLevel;
    }

    [Serializable]
    public class LGUSave
    {
        public Dictionary<ulong,SaveInfo> playerSaves = new Dictionary<ulong,SaveInfo>();
    }
}
