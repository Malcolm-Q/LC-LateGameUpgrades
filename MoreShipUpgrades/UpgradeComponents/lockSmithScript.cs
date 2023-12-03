using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace MoreShipUpgrades.UpgradeComponents
{
    public class lockSmithScript : BaseUpgrade
    {
        private GameObject pin1, pin2, pin3, pin4, pin5;
        private List<GameObject> pins;
        private List<float> offsets = new List<float> { 0.0f, 0.0f, 0.0f, 0.0f, 0.0f };
        private List<int> order = new List<int> { 0, 1, 2, 3, 4 };
        private int currentPin = 0;
        public DoorLock currentDoor = null;
        void Start()
        {
            StartCoroutine(lateApply());
            Transform tumbler = transform.GetChild(0).GetChild(0).GetChild(0);
            pin1 = tumbler.GetChild(0).gameObject;
            pin2 = tumbler.GetChild(1).gameObject;
            pin3 = tumbler.GetChild(2).gameObject;
            pin4 = tumbler.GetChild(3).gameObject;
            pin5 = tumbler.GetChild(4).gameObject;

            pin1.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => StrikePin(0));
            pin2.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => StrikePin(1));
            pin3.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => StrikePin(2));
            pin4.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => StrikePin(3));
            pin5.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => StrikePin(4));

            pins = new List<GameObject> { pin1, pin2, pin3, pin4, pin5 };
        }

        private IEnumerator lateApply()
        {
            yield return new WaitForSeconds(1);
            UpgradeBus.instance.lockSmith = true;
            UpgradeBus.instance.lockScript = this;
            HUDManager.Instance.chatText.text += "\n<color=#FF0000>Locksmith is active!</color>";
            UpgradeBus.instance.UpgradeObjects.Add("Locksmith", gameObject);
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            if (Keyboard.current[Key.Escape].wasPressedThisFrame)
            {
                if (!transform.GetChild(0).gameObject.activeInHierarchy) { return; }
                transform.GetChild(0).gameObject.SetActive(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void BeginLockPick()
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            currentPin = 0;
            for (int i = 0; i < pins.Count; i++)
            {
                float offset = UnityEngine.Random.Range(50f, 70f);
                offsets[i] = offset;
                pins[i].transform.localPosition = new Vector3(pins[i].transform.localPosition.x, offset, pins[i].transform.localPosition.z);
            }
            RandomizeListOrder(order);
            StartCoroutine(CommunicateOrder(order));
        }

        public void StrikePin(int i)
        {
            if (i == order[currentPin])
            {
                currentPin++;
                pins[i].transform.localPosition = new Vector3(pins[i].transform.localPosition.x, 80f, pins[i].transform.localPosition.z);
                if (currentPin == 5)
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    transform.GetChild(0).gameObject.SetActive(false);
                    currentDoor.UnlockDoorSyncWithServer();
                }
            }
            else
            {
                BeginLockPick();
            }
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
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < lst.Count; i++)
            {
                pins[lst[i]].transform.GetChild(0).GetComponent<Image>().color = Color.white;
                yield return new WaitForSeconds(0.5f);
                pins[lst[i]].transform.GetChild(0).GetComponent<Image>().color = Color.blue;
            }
        }
    }
}
