using MoreShipUpgrades.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Netcode;
using UnityEngine;
using Newtonsoft.Json;
using GameNetcodeStuff;
using System.Collections;

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
            {"Night Vision", SaveInfo => SaveInfo.nightVision },
            {"Strong Legs", SaveInfo => SaveInfo.strongLegs },
            {"Better Scanner", SaveInfo => SaveInfo.scannerUpgrade },
            {"Beekeeper", SaveInfo => SaveInfo.beekeeper },
            {"Back Muscles", SaveInfo => SaveInfo.exoskeleton },
            {"Pager", SaveInfo => SaveInfo.pager },
            {"Locksmith", SaveInfo => SaveInfo.lockSmith },
        };

        private static Dictionary<string, Func<SaveInfo, int>> levelConditions = new Dictionary<string, Func<SaveInfo, int>>
        {
            { "Malware Broadcaster", saveInfo => 0 },
            { "Light Footed", saveInfo => saveInfo.lightLevel },
            { "Discombobulator", saveInfo => saveInfo.discoLevel },
            { "Bigger Lungs", saveInfo => saveInfo.lungLevel },
            { "Running Shoes", saveInfo => saveInfo.runningLevel },
            { "Night Vision", saveInfo => 0 },
            { "Strong Legs", saveInfo => saveInfo.legLevel },
            { "Better Scanner", saveInfo => 0 },
            { "Beekeeper", saveInfo => saveInfo.beeLevel },
            { "Back Muscles", saveInfo => saveInfo.backLevel },
            { "Pager", saveInfo => 0 },
            { "Locksmith", saveInfo => 0 }
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
                    lguSave.playerSaves.Add(playerID, new SaveInfo());
                    string json = JsonConvert.SerializeObject(saveInfo);
                    File.WriteAllText(filePath, json);
                    HandleSpawns();
                }
            }
            else
            {
                ShareSaveServerRpc();
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
            PlayerControllerB[] players = GameObject.FindObjectsOfType<PlayerControllerB>();
            foreach (PlayerControllerB player in players)
            {
                player.movementSpeed = 4.6f;
                player.sprintTime = 11;
                player.jumpForce = 13;
            }
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

        public void UpdateUpgradeBus()
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
            UpgradeBus.instance.DestroyTraps = saveInfo.DestroyTraps;
            UpgradeBus.instance.softSteps = saveInfo.softSteps;
            UpgradeBus.instance.scannerUpgrade = saveInfo.scannerUpgrade;
            UpgradeBus.instance.nightVision = saveInfo.nightVision;
            UpgradeBus.instance.nightVisionActive = saveInfo.nightVisionActive;
            UpgradeBus.instance.exoskeleton = saveInfo.exoskeleton;
            UpgradeBus.instance.TPButtonPressed = saveInfo.TPButtonPressed;
            UpgradeBus.instance.beekeeper = saveInfo.beekeeper;
            UpgradeBus.instance.terminalFlash = saveInfo.terminalFlash;
            UpgradeBus.instance.strongLegs = saveInfo.strongLegs;
            UpgradeBus.instance.runningShoes = saveInfo.runningShoes;
            UpgradeBus.instance.biggerLungs = saveInfo.biggerLungs;
            UpgradeBus.instance.beeLevel = saveInfo.beeLevel;
            UpgradeBus.instance.lungLevel = saveInfo.lungLevel;
            UpgradeBus.instance.backLevel = saveInfo.backLevel;
            UpgradeBus.instance.runningLevel = saveInfo.runningLevel;
            UpgradeBus.instance.lightLevel = saveInfo.lightLevel;
            UpgradeBus.instance.discoLevel = saveInfo.discoLevel;
            UpgradeBus.instance.legLevel = saveInfo.legLevel;
            UpgradeBus.instance.pager = saveInfo.pager;

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
        public bool strongLegs = UpgradeBus.instance.strongLegs;
        public bool runningShoes = UpgradeBus.instance.runningShoes;
        public bool biggerLungs = UpgradeBus.instance.biggerLungs;
        public bool lockSmith = UpgradeBus.instance.lockSmith;
        public bool pager = UpgradeBus.instance.pager;
        public int beeLevel = UpgradeBus.instance.beeLevel;
        public int lungLevel = UpgradeBus.instance.lungLevel;
        public int backLevel = UpgradeBus.instance.backLevel;
        public int runningLevel = UpgradeBus.instance.runningLevel;
        public int lightLevel = UpgradeBus.instance.lightLevel;
        public int discoLevel = UpgradeBus.instance.discoLevel;
        public int legLevel = UpgradeBus.instance.legLevel;
    }

    [Serializable]
    public class LGUSave
    {
        public Dictionary<ulong,SaveInfo> playerSaves = new Dictionary<ulong,SaveInfo>();
    }
}
