using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using GameNetcodeStuff;

namespace MoreShipUpgrades.UpgradeComponents.Items.Contracts.DataRetrieval
{
    internal class DataPCScript : NetworkBehaviour
    {
        static LGULogger logger = new LGULogger(nameof(DataPCScript));

        GameObject root;
        public GameObject loot;

        public AudioClip error, startup;
        AudioSource audio;
        InteractTrigger trig;

        Text IPText;
        InputField userField, passField, gameField;
        string user, pass, ip;
        string defaultText;
        string dir = "C:\\WINDOWS";
        static string rootDir = "C:\\WINDOWS";
        List<string> dirs = new List<string>() {
            rootDir+"\\Documents",
            rootDir+"\\Downloads",
            rootDir+"\\TopSecret",
            rootDir+"\\ImportantFiles",
            rootDir+"\\CatPhotos",
            rootDir+"\\ReallyImportantFiles",
            rootDir+"\\NotImportantFiles",
            rootDir+"\\Data",
            rootDir+"\\Fortnite",
            rootDir+"\\Goobers",
            rootDir+"\\Games",
            rootDir+"\\Recipes",
            rootDir+"\\PicsThatGoHard",
            rootDir+"\\Files",
            rootDir+"\\Surveys",
        };

        List<string> fileDirs = new List<string>();
        string[] files = {
            "minecraft.exe",
            "MoreRam.dll",
            "why_mustard_is_better_than_ketchup.txt",
            "cursed_cat_photo.png",
            "slightly_less_cursed_cat_photo.jpg",
            "singles_in_your_area.exe",
            "silly_goofy_file.dll",
            "orange.png",
            "apple.png",
            "free_robux.exe",
            "free_vbux.exe",
            "unlock_every_fortnite_skin.exe",
            "how_to_exorcize_ghosts.txt",
            "benefits_of_eating_microplastics.txt",
            "me_eating_microplastics.mp4",
            "how_to_season_microplastics.mp4",
            "amogus.dll",
            "best_frogs_compilation.mp4",
            "scariest_frogs_compilation.mp4",
            "minecraft_mobs_irl.mp4",
            "drinking_and_driving_tutorial.mp4",
        };
        private bool interactable = true;
        private PlayerControllerB interactingPlayer;

        void Start()
        {
            root = transform.GetChild(2).gameObject;
            root.SetActive(false);
            IPText = root.transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
            userField = root.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<InputField>();
            passField = root.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<InputField>();
            gameField = root.transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<InputField>();
            root.transform.GetChild(0).GetChild(1).GetChild(2).GetComponent<Button>().onClick.AddListener(Login);
            GetComponent<InteractTrigger>().onInteract.AddListener(Interact);
            defaultText = gameField.text;
            gameField.onValueChanged.AddListener(OnChange);
            trig = GetComponent<InteractTrigger>();

            audio = GetComponent<AudioSource>();



            if (IsHost || IsServer)
            {
                ip = $"{Random.Range(0, 213)}.{Random.Range(0, 99)}.{Random.Range(0, 99)}.{Random.Range(0, 255)}";
                IPText.text = IPText.text.Replace("[IP]", ip);
                user = RandomString();
                pass = RandomString();
                SyncGameDetailsClientRpc(ip, user, pass);
            }
            AddFiles();
        }

        void OnChange(string text)
        {
            StartCoroutine(PreserveDefaultText(text));
        }


        void Interact(PlayerControllerB player)
        {
            if (interactable)
            {
                interactingPlayer = player;
                root.SetActive(true);
                if (IsHost || IsServer)
                {
                    InitiateGameClientRpc(new NetworkBehaviourReference(this));
                }
                else
                {
                    InitiateGameServerRpc(new NetworkBehaviourReference(this));
                }
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        void InitiateGameServerRpc(NetworkBehaviourReference netRef)
        {
            InitiateGameClientRpc(netRef);
        }

        [ClientRpc]
        void SyncGameDetailsClientRpc(string Key, string user, string pass)
        {
            logger.LogInfo($"Received Broadcasted minigame info!\nKey: {Key}\nuser: {user}\npassword: {pass}");
            UpgradeBus.instance.DataMinigameKey = Key;
            UpgradeBus.instance.DataMinigameUser = user;
            UpgradeBus.instance.DataMinigamePass = pass;
            this.user = user;
            this.pass = pass;
            if (IPText == null) IPText = transform.GetChild(2).GetChild(0).GetChild(2).GetChild(0).GetComponent<Text>();
            IPText.text = IPText.text.Replace("[IP]", Key);
        }

        [ClientRpc]
        void InitiateGameClientRpc(NetworkBehaviourReference netRef)
        {
            netRef.TryGet(out DataPCScript pcScript);
            if (pcScript != null)
            {
                pcScript.interactable = false;
                pcScript.trig.interactable = false;
                pcScript.audio.PlayOneShot(startup);
            }
            else logger.LogError("Unable to resolve netRef!");
        }

        [ServerRpc(RequireOwnership = false)]
        void ExitGameServerRpc(NetworkBehaviourReference netRef, bool succeeded)
        {
            ExitGameClientRpc(netRef, succeeded);
        }

        [ClientRpc]
        void ExitGameClientRpc(NetworkBehaviourReference netRef, bool succeeded)
        {
            if ((IsHost || IsServer) && succeeded)
            {
                GameObject go = Instantiate(loot, transform.position + Vector3.up, Quaternion.identity);
                go.GetComponent<ScrapValueSyncer>().SetScrapValue(UpgradeBus.instance.cfg.CONTRACT_DATA_REWARD.Value);
                go.GetComponent<NetworkObject>().Spawn();
                logger.LogInfo("Loot successfully spawned");
            }
            netRef.TryGet(out DataPCScript pcScript);
            if (pcScript == null)
            {
                logger.LogError("Unable to resolve netRef!");
                root.SetActive(false);
                return;
            }
            if (succeeded)
            {
                Destroy(pcScript.GetComponent<BoxCollider>());
                //pcScript.trig.disabledHoverTip = "Data has been retrieved!";
            }
            else
            {
                pcScript.trig.interactable = true;
                interactable = true;
            }
            root.SetActive(false);
        }

        public void HandleInput(string input)
        {
            string sub = dir + ">";
            int index = input.IndexOf(sub);

            string result = index != -1 ? input.Substring(index + sub.Length) : "";
            string[] words = result.Trim().Split(' ');
            string firstWord = words[0].ToLower();
            string secondWord = "";
            if (words.Length > 1)
            {
                secondWord = words[1];
            }
            if (firstWord == "ls")
            {
                if (dir == rootDir)
                {
                    gameField.text = $"{string.Join("\n", dirs)}\n\n{dir}> ";
                    defaultText = gameField.text;
                    return;
                }
                gameField.text = $"{string.Join("\n", fileDirs.Where(x => x.Contains(dir)))}\n\n{dir}> ";
                defaultText = gameField.text;
                return;
            }
            else if (firstWord == "cd")
            {
                if (secondWord == "")
                {
                    gameField.text = $"YOU MUST PROVIDE A VALID DIRECTORY TO SWITCH TO\n\n{dir}";
                    defaultText = gameField.text;
                    return;
                }
                if (secondWord == ".." || secondWord == "~")
                {
                    if (dir == rootDir)
                    {
                        gameField.text = $"YOU ARE ALREADY IN THE ROOT DIRECTORY\n\n{dir}> ";
                        defaultText = gameField.text;
                        return;
                    }
                    dir = rootDir;
                    gameField.text = $"{dir}> ";
                    defaultText = gameField.text;
                    return;
                }
                if (dirs.Contains($"{dir}\\{secondWord}"))
                {
                    dir = $"{dir}\\{secondWord}";
                    gameField.text = $"{dir}> ";
                    defaultText = gameField.text;
                    return;
                }
                gameField.text = $"{dir}\\{secondWord} IS NOT A VALID DIRECTORY\n\nENTER LS TO VIEW DIRECTORIES\n\n{dir}> ";
                defaultText = gameField.text;
                return;
            }
            else if (firstWord == "mv")
            {
                if (secondWord == "")
                {
                    gameField.text = $"YOU MUST PROVIDE A VALID FILE TO MOVE\n\n{dir}";
                    defaultText = gameField.text;
                    return;
                }
                if (secondWord == "survey.db")
                {
                    if (fileDirs.Contains($"{dir}\\survey.db"))
                    {
                        logger.LogInfo("Minigame completed, spawning loot and exiting...");
                        GameNetworkManager.Instance.localPlayerController.playerActions.Enable();
                        if (IsHost || IsServer)
                        {
                            ExitGameClientRpc(new NetworkBehaviourReference(this), true);
                        }
                        else
                        {

                            ExitGameServerRpc(new NetworkBehaviourReference(this), true);
                        }
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                        return;
                    }
                    else
                    {
                        gameField.text = $"{dir}\\survey.db does not exist\n\n{dir}> ";
                        defaultText = gameField.text;
                        return;
                    }
                }
                if (fileDirs.Contains($"{dir}\\{secondWord}"))
                {
                    gameField.text = $"{dir}\\{secondWord} IS NOT A FILE OF INTEREST\n\n{dir}> ";
                    defaultText = gameField.text;
                    return;
                }
                gameField.text = $"{dir}\\{secondWord} IS NOT A VALID FILE\n\nENTER LS TO VIEW FILES\n\n{dir}> ";
                defaultText = gameField.text;
                return;
            }
            gameField.text = $"{firstWord} {secondWord} WAS NOT RECOGNIZED AS A COMMAND\n\nCOMMANDS ARE :\nLS\nMV\nMV\n\n{dir}> ";
            defaultText = gameField.text;
        }

        public void Login()
        {
            logger.LogInfo($"Submitted user: {userField.text}\nTrue user: {user}\nSubmitted password: {passField.text}\nTrue password: {pass}");
            if (userField.text == user && passField.text == pass)
            {
                gameField.transform.parent.gameObject.SetActive(true);
                userField.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                audio.PlayOneShot(error);
                userField.text = "";
                passField.text = "";
            }
        }

        static string RandomString()
        {
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            int length = Random.Range(5, 8 + 1);

            char[] randomChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                randomChars[i] = allowedChars[Random.Range(0, allowedChars.Length)];
            }

            string randomString = new string(randomChars);
            return randomString;
        }

        void AddFiles()
        {
            int targCount = Random.Range(3, 7);
            while (dirs.Count > targCount)
            {
                dirs.Remove(dirs[Random.Range(0, dirs.Count)]);
            }
            foreach (string dir in dirs)
            {
                int numFiles = Random.Range(1, 4);
                for (int i = 0; i < numFiles; i++)
                {
                    fileDirs.Add($"{dir}\\{files[Random.Range(0, files.Length)]}");
                }
            }
            fileDirs.Add($"{dirs[Random.Range(0, dirs.Count)]}\\survey.db");
        }

        void Update() // don't yell at me
        {
            if (GameNetworkManager.Instance.localPlayerController != interactingPlayer) return;
            if (Keyboard.current.enterKey.wasReleasedThisFrame)
            {
                HandleInput(gameField.text);
                StartCoroutine(MoveCaret());
            }
            else if (Keyboard.current.escapeKey.wasReleasedThisFrame)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                interactable = true;
                if (IsHost || IsServer)
                {
                    ExitGameClientRpc(new NetworkBehaviourReference(this), false);
                }
                else
                {
                    ExitGameServerRpc(new NetworkBehaviourReference(this), false);
                }
            }
        }

        private IEnumerator MoveCaret()
        {
            yield return null;
            gameField.caretPosition = gameField.text.Length;
        }

        private IEnumerator PreserveDefaultText(string text)
        {
            yield return null;
            if (!text.Contains(defaultText))
            {
                gameField.text = defaultText;
                gameField.caretPosition = defaultText.Length;
            }
        }
    }
}
