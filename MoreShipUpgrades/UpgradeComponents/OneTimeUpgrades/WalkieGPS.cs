using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Upgrades;
using MoreShipUpgrades.UpgradeComponents.Interfaces;
using System;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

namespace MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades
{
    class WalkieGPS : OneTimeUpgrade
    {
        public static string UPGRADE_NAME = "Walkie GPS";
        public static WalkieGPS instance;

        private GameObject canvas;
        private Text x, y, z, time;
        internal override void Start()
        {
            upgradeName = UPGRADE_NAME;
            base.Start();
            canvas = transform.GetChild(0).gameObject;
            x = canvas.transform.GetChild(0).GetComponent<Text>();
            y = canvas.transform.GetChild(1).GetComponent<Text>();
            z = canvas.transform.GetChild(2).GetComponent<Text>();
            time = canvas.transform.GetChild(3).GetComponent<Text>();
        }

        public override void Load()
        {
            base.Load();

            UpgradeBus.instance.walkies = true;
            instance = this;
        }

        public override void Unwind()
        {
            base.Unwind();

            UpgradeBus.instance.walkies = false;
        }

        public void Update()
        {
            if (!UpgradeBus.instance.walkieUIActive) return;

            Vector3 pos = GameNetworkManager.Instance.localPlayerController.transform.position;
            x.text = $"X: {pos.x.ToString("F1")}";
            y.text = $"Y: {pos.y.ToString("F1")}";
            z.text = $"Z: {pos.z.ToString("F1")}";

            int num = (int)(TimeOfDay.Instance.normalizedTimeOfDay * (60f * TimeOfDay.Instance.numberOfHours)) + 360;
            int num2 = (int)Mathf.Floor(num / 60);
            string amPM = "AM";
            string text = "";
            if (num2 >= 24)
            {
                text = "12:00 AM";
            }
            if (num2 > 12)
            {
                amPM = "PM";
            }
            if (num2 > 12)
            {
                num2 %= 12;
            }
            int num3 = num % 60;
            text = string.Format("{0:00}:{1:00}", num2, num3).TrimStart('0') + amPM;
            time.text = text;
        }

        public void WalkieActive()
        {
            if (canvas.activeInHierarchy) return;

            UpgradeBus.instance.walkieUIActive = true;
            canvas.SetActive(true);
        }

        public void WalkieDeactivate()
        {
            UpgradeBus.instance.walkieUIActive = false;
            canvas.SetActive(false);
        }

        public override string GetDisplayInfo(int price = -1)
        {
            return "Displays your location and time when holding a walkie talkie.\nEspecially useful for fog.";
        }
    }
}
