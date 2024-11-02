using System;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    public class CurrencyManager : NetworkBehaviour
    {
        public static CurrencyManager Instance;
        int currencyAmount;

        void Awake()
        {
            Instance = this;
        }

        public static bool Enabled
        {
            get
            {
                return UpgradeBus.Instance.PluginConfiguration.ALTERNATIVE_CURRENCY_ENABLED;
            }
        }

        public static string GetNewCreditFormat(string format)
        {
            if (!Enabled) return format;
            return format + $" - {Instance.GetCurrencyAmount()}";
        }

        public float GetCreditRatio()
        {
            return 1f / UpgradeBus.Instance.PluginConfiguration.ALTERNATIVE_CURRENCY_CREDITS_TO_CURRENCY_RATIO;
        }

        public int GetCurrencyAmountFromQuota(int quotaFullfilled)
        {
            return Mathf.RoundToInt(GetCreditRatio() *  quotaFullfilled);
        }

        public int GetCurrencyAmountFromCredits(int credits)
        {
            return Mathf.CeilToInt(GetCreditRatio() * credits);
        }

        public void SetCurrencyAmount(int amount)
        {
            currencyAmount = amount;
        }

        public void AddCurrencyAmountFromQuota(int quota)
        {
            AddCurrencyAmount(GetCurrencyAmountFromQuota(quota));

            HUDManager.Instance.DisplayTip("Player Credits", $"You currently have {GetCurrencyAmount()} player credits to use in the upgrade shop.");
        }

        public void AddCurrencyAmount(int amount)
        {
            currencyAmount += amount;
        }

        public void RemoveCurrencyAmount(int amount)
        {
            currencyAmount -= amount;
        }

        public int GetCurrencyAmount()
        {
            return currencyAmount;
        }
    }
}
