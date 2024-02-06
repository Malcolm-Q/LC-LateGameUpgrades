using MoreShipUpgrades.Managers;
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
        public static string UPGRADE_NAME = "Locksmith";
        internal const string WORLD_BUILDING_TEXT = "\n\nOn-the-job training package that supplies {0} with proprietary knowledge of the 'Ram, Scan, Bump' technique" +
            " for bypassing The Company's proprietary Low-Tech Manual Security Doors' security system. Comes with an 'all-nines-notched' key, a rubber gasket, and a plastic handle on a metal rod {1}.\n\n";
        public static LockSmith instance;

        private GameObject pin1, pin2, pin3, pin4, pin5;
        private List<GameObject> pins;
        private List<int> order = new List<int> { 0, 1, 2, 3, 4 };
        private int currentPin = 0;
        public DoorLock currentDoor = null;
        private bool canPick = false;
        public int timesStruck;

        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            base.Start();
            instance = this;
            Transform tumbler = transform.GetChild(0).GetChild(0).GetChild(0);
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

            pins = new List<GameObject> { pin1, pin2, pin3, pin4, pin5 };
        }

        public override void Load()
        {
            base.Load();

            UpgradeBus.instance.lockSmith = true;
        }

        public override void Register()
        {
            base.Register();
        }
        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.lockSmith = false;
        }

        void Update()
        {
            if (!Keyboard.current[Key.Escape].wasPressedThisFrame) return;
            if (!transform.GetChild(0).gameObject.activeInHierarchy) return;
            transform.GetChild(0).gameObject.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void BeginLockPick()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

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
                BeginLockPick();
                RoundManager.Instance.PlayAudibleNoise(currentDoor.transform.position, 30f, 0.65f, timesStruck, false, 0);
                return;
            }
            currentPin++;
            pins[i].transform.localPosition = new Vector3(pins[i].transform.localPosition.x, 35f, pins[i].transform.localPosition.z);
            if (currentPin == 5)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                transform.GetChild(0).gameObject.SetActive(false);
                currentDoor.UnlockDoorSyncWithServer();
            }
            RoundManager.Instance.PlayAudibleNoise(currentDoor.transform.position, 10f, 0.65f, timesStruck, false, 0);
        }
        void RandomizeListOrder<T>(List<T> list)
        {
            int n = list.Count;
            System.Random rng = new System.Random();

            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
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
            return "Allows you to pick door locks by completing a minigame.";
        }
    }
}
