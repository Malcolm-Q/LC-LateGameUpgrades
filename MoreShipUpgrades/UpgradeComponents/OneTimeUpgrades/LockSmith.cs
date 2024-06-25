using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Misc.TerminalNodes;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    class LockSmith : OneTimeUpgrade, IUpgradeWorldBuilding
    {
        internal const string UPGRADE_NAME = "Locksmith";
        internal const string WORLD_BUILDING_TEXT = "\n\nOn-the-job training package that supplies {0} with proprietary knowledge of the 'Ram, Scan, Bump' technique" +
            " for bypassing The Company's proprietary Low-Tech Manual Security Doors' security system. Comes with an 'all-nines-notched' key, a rubber gasket, and a plastic handle on a metal rod {1}.\n\n";
        public static LockSmith instance;

        List<GameObject> pins;
        readonly List<int> order = [0, 1, 2, 3, 4];
        int currentPin;
        public DoorLock currentDoor;
        bool canPick;
        public int timesStruck;
        public override bool CanInitializeOnStart => UpgradeBus.Instance.PluginConfiguration.LOCKSMITH_PRICE.Value <= 0;
        void Awake()
        {
            upgradeName = UPGRADE_NAME;
            overridenUpgradeName = UpgradeBus.Instance.PluginConfiguration.LOCKSMITH_OVERRIDE_NAME;
            instance = this;
        }
        internal override void Start()
        {
            base.Start();
            Transform tumbler = transform.GetChild(0).GetChild(0).GetChild(0);
            GameObject pin1, pin2, pin3, pin4, pin5;
            pin1 = tumbler.GetChild(0).gameObject;
            pin2 = tumbler.GetChild(1).gameObject;
            pin3 = tumbler.GetChild(2).gameObject;
            pin4 = tumbler.GetChild(3).gameObject;
            pin5 = tumbler.GetChild(4).gameObject;

            pin1.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => StrikePin(0));
            pin2.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => StrikePin(1));
            pin3.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => StrikePin(2));
            pin4.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => StrikePin(3));
            pin5.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => StrikePin(4));

            pins = [ pin1, pin2, pin3, pin4, pin5 ];
        }

        void Update()
        {
            if (!Keyboard.current[Key.Escape].wasPressedThisFrame) return;
            if (!transform.GetChild(0).gameObject.activeInHierarchy) return;
            ToggleLocksmithUI(false);
        }

        public void BeginLockPick(DoorLock door)
        {
            currentDoor = door;
            timesStruck = 0;
            ToggleLocksmithUI(true);

            SelectMinigame();
        }
        void ToggleLocksmithUI(bool toggle)
        {
            if (toggle)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            Cursor.visible = toggle;
            transform.GetChild(0).gameObject.SetActive(toggle);
            GameNetworkManager.Instance.localPlayerController.quickMenuManager.isMenuOpen = toggle;
        }
        void SelectMinigame()
        {
            canPick = false;
            currentPin = 0;
            for (int i = 0; i < pins.Count; i++)
            {
                float offset = Random.Range(40f, 90f);
                pins[i].transform.localPosition = new Vector3(pins[i].transform.localPosition.x, offset, pins[i].transform.localPosition.z);
            }
            RandomizeListOrder(order);
            StartCoroutine(CommunicateOrder(order));
        }
        public void StrikePin(int i)
        {
            if (!canPick) { return; }
            timesStruck++;
            if (i != order[currentPin])
            {
                SelectMinigame();
                RoundManager.Instance.PlayAudibleNoise(currentDoor.transform.position, 30f, 0.65f, timesStruck, false, 0);
                return;
            }
            currentPin++;
            pins[i].transform.localPosition = new Vector3(pins[i].transform.localPosition.x, 35f, pins[i].transform.localPosition.z);
            if (currentPin == 5)
            {
                ToggleLocksmithUI(false);
                currentDoor.UnlockDoorSyncWithServer();
            }
            RoundManager.Instance.PlayAudibleNoise(currentDoor.transform.position, 10f, 0.65f, timesStruck, false, 0);
        }
        void RandomizeListOrder<T>(List<T> list)
        {
            int n = list.Count;
            System.Random rng = new();

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }

        private IEnumerator CommunicateOrder(List<int> lst)
        {
            yield return new WaitForSeconds(0.75f);

            for (int i = 0; i < lst.Count; i++)
            {
                pins[lst[i]].GetComponent<Image>().color = Color.green;
                yield return new WaitForSeconds(0.25f);
                pins[lst[i]].GetComponent<Image>().color = Color.white;
            }
            canPick = true;
        }

        public string GetWorldBuildingText(bool shareStatus = false)
        {
            return string.Format(WORLD_BUILDING_TEXT, shareStatus ? "your crew" : "you", shareStatus ? "for each of your coworkers" : "");
        }

        public override string GetDisplayInfo(int price = -1)
        {
            return $"${price} - Allows you to pick door locks by completing a minigame.";
        }
        public new static (string, string[]) RegisterScrapToUpgrade()
        {
            return (UPGRADE_NAME, UpgradeBus.Instance.PluginConfiguration.LOCKSMITH_ITEM_PROGRESSION_ITEMS.Value.Split(","));
        }
        public new static void RegisterUpgrade()
        {
            SetupGenericPerk<LockSmith>(UPGRADE_NAME);
        }
        public new static CustomTerminalNode RegisterTerminalNode()
        {
            LategameConfiguration configuration = UpgradeBus.Instance.PluginConfiguration;

            return UpgradeBus.Instance.SetupOneTimeTerminalNode(UPGRADE_NAME,
                                    configuration.SHARED_UPGRADES.Value || !configuration.LOCKSMITH_INDIVIDUAL.Value,
                                    configuration.LOCKSMITH_ENABLED.Value,
                                    configuration.LOCKSMITH_PRICE.Value,
                                    configuration.OVERRIDE_UPGRADE_NAMES ? configuration.LOCKSMITH_OVERRIDE_NAME : "");
        }
    }
}
