using MoreShipUpgrades.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using Newtonsoft.Json;
using System.Xml.Linq;
using System.Linq;
using GameNetcodeStuff;

namespace MoreShipUpgrades.Managers
{
    public class LGUStore : NetworkBehaviour
    {
        public static LGUStore instance;
        public SaveInfo saveInfo;

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
        private bool hasRun = false;
        private void Start()
        {
            instance = this;
            if(NetworkManager.IsHost && !hasRun)
            {
                hasRun = true;
                string saveNum = GameNetworkManager.Instance.saveFileNum.ToString();
                string filePath = Path.Combine(Application.persistentDataPath, $"LGU_{saveNum}.json");
                if(File.Exists(filePath) )
                {
                    string json = File.ReadAllText(filePath);
                    saveInfo = JsonConvert.DeserializeObject<SaveInfo>(json);
                    UpdateUpgradeBus();
                    HandleSpawns();
                }
                else
                {
                    saveInfo = new SaveInfo();
                    string json = JsonConvert.SerializeObject( saveInfo );
                    File.WriteAllText(filePath, json );
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
            saveInfo = new SaveInfo();
            string json = JsonConvert.SerializeObject(saveInfo);
            File.WriteAllText(filePath, json);
        }

        public void HandleSpawns()
        {
            foreach(CustomTerminalNode node in UpgradeBus.instance.terminalNodes)
            {
                if(node.Unlocked)
                {
                    GameObject go = Instantiate(node.Prefab, Vector3.zero, Quaternion.identity);
                    go.GetComponent<NetworkObject>().Spawn();
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void PlayersFiredServerRpc()
        {
            ResetUpgradeBusClientRpc();
            UpgradeBus.instance.ResetAllValues();
            string saveNum = GameNetworkManager.Instance.saveFileNum.ToString();
            string filePath = Path.Combine(Application.persistentDataPath, $"LGU_{saveNum}.json");
            saveInfo = new SaveInfo();
            string json = JsonConvert.SerializeObject(saveInfo);
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

        [ServerRpc(RequireOwnership =false)]
        public void ShareSaveServerRpc()
        {
            string json = JsonConvert.SerializeObject(saveInfo);
            ShareSaveClientRpc(json);
        }

        [ClientRpc]
        public void ShareSaveClientRpc(string json)
        {
            saveInfo = JsonConvert.DeserializeObject<SaveInfo>(json);
            UpdateUpgradeBus();
        }

        [ServerRpc(RequireOwnership = false)]
        public void SaveLGUDataServerRpc()
        {
            string saveNum = GameNetworkManager.Instance.saveFileNum.ToString();
            string filePath = Path.Combine(Application.persistentDataPath, $"LGU_{saveNum}.json");
            string json = JsonConvert.SerializeObject( new SaveInfo() );
            File.WriteAllText(filePath, json );
        }


        [ServerRpc(RequireOwnership = false)]
        public void ReqSpawnServerRpc(string goName, bool increment = false)
        {
            if(!increment)
            {
                foreach(CustomTerminalNode customNode in UpgradeBus.instance.terminalNodes)
                {
                    if(customNode.Name == goName)
                    {
                        GameObject go = Instantiate(customNode.Prefab, Vector3.zero, Quaternion.identity);
                        go.GetComponent<NetworkObject>().Spawn();
                        UpdateNodesClientRpc(goName);
                        break;
                    }
                }
            }
            else
            {
                UpdateNodesClientRpc(goName, increment);
            }
        }

        [ClientRpc]
        private void UpdateNodesClientRpc(string goName, bool increment = false)
        {
            foreach(CustomTerminalNode customNode in UpgradeBus.instance.terminalNodes)
            {
                if(customNode.Name == goName)
                {
                    customNode.Unlocked = true;
                    if(increment)
                    {
                        customNode.CurrentUpgrade++;
                        UpgradeBus.instance.UpgradeObjects[goName].GetComponent<BaseUpgrade>().Increment();
                    }
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void DeleteUpgradesServerRpc()
        {
            BaseUpgrade[] upgradeObjects = GameObject.FindObjectsOfType<BaseUpgrade>();
            foreach(BaseUpgrade upgrade in upgradeObjects)
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

        public void UpdateUpgradeBus()
        {
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

            foreach(CustomTerminalNode customNode in UpgradeBus.instance.terminalNodes)
            {
                if(conditions.TryGetValue(customNode.Name, out var condition) && condition(saveInfo))
                {
                    customNode.Unlocked = true;
                    levelConditions.TryGetValue(customNode.Name, out var level);
                    customNode.CurrentUpgrade = level.Invoke(saveInfo);
                }
            }
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
}
