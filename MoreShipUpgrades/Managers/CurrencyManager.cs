using GameNetcodeStuff;
using MoreShipUpgrades.Misc.Util;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Managers
{
    public class CurrencyManager : NetworkBehaviour
    {
        public static CurrencyManager Instance;

        bool showCurrencyAmount;
        public bool ShowCurrentAmount
        {
            get
            {
                return showCurrencyAmount;
            }
            set
            {
                showCurrencyAmount = value;
            }
        }

        int currencyAmount;
        public int CurrencyAmount
        {
            get
            {
                return currencyAmount;
            }
            set
            {
                currencyAmount = value;
            }
        }

        void Awake()
        {
            Instance = this;
            showCurrencyAmount = false;
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
            if (!Instance.ShowCurrentAmount) return format;
            return $"{Instance.CurrencyAmount} {LguConstants.ALTERNATIVE_CURRENCY_ALIAS}";
        }

        public float GetCreditRatio()
        {
            return 1f / UpgradeBus.Instance.PluginConfiguration.ALTERNATIVE_CURRENCY_CREDITS_TO_CURRENCY_RATIO;
        }

        public float GetQuotaRatio()
        {
            return 1f / UpgradeBus.Instance.PluginConfiguration.ALTERNATIVE_CURRENCY_QUOTA_TO_CURRENCY_RATIO;
        }

        public float GetCreditConversionRatio()
        {
            return 1f / UpgradeBus.Instance.PluginConfiguration.ALTERNATIVE_CURRENCY_CONVERSION_CREDITS_TO_CURRENCY_RATIO;
        }

        public int GetCurrencyAmountFromQuota(int quotaFullfilled)
        {
            return Mathf.RoundToInt(GetQuotaRatio() *  quotaFullfilled);
        }

        public int GetCurrencyAmountFromCredits(int credits)
        {
            return Mathf.CeilToInt(GetCreditRatio() * credits);
        }

        public int GetCurrencyAmountFromCreditsConversion(int convertedCredits)
        {
            return Mathf.CeilToInt(GetCreditConversionRatio() * convertedCredits);
        }

        public int GetCreditsFromCurrencyAmountConversion(int convertedCurrencyAmount)
        {
            return Mathf.CeilToInt(convertedCurrencyAmount / GetCreditConversionRatio());
        }

        [ClientRpc]
        public void AddCurrencyAmountFromQuotaClientRpc(int quotaFullfilled)
        {
            AddCurrencyAmountFromQuota(quotaFullfilled);
        }

        public void AddCurrencyAmountFromQuota(int quota)
        {
            AddCurrencyAmount(GetCurrencyAmountFromQuota(quota));

            HUDManager.Instance.DisplayTip("Player Credits", $"You currently have {CurrencyAmount} {LguConstants.ALTERNATIVE_CURRENCY_ALIAS}s to use in the upgrade shop.");
        }

        public void AddCurrencyAmount(int amount)
        {
            CurrencyAmount += amount;
        }

        public void RemoveCurrencyAmount(int amount)
        {
            CurrencyAmount -= amount;
        }

        [ServerRpc(RequireOwnership = false)]
        public void TradePlayerCreditsServerRpc(ulong tradingClientId, int playerCreditAmount, ServerRpcParams serverRpcParams = default)
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams()
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = [tradingClientId]
                }
            };
            TradePlayerCreditsClientRpc(serverRpcParams.Receive.SenderClientId, playerCreditAmount, clientRpcParams);
        }

        [ClientRpc]
        public void TradePlayerCreditsClientRpc(ulong traderClientId, int playerCreditAmount, ClientRpcParams clientRpcParams = default)
        {
            AddCurrencyAmount(playerCreditAmount);
            PlayerControllerB traderPlayer = StartOfRound.Instance.allPlayerScripts[traderClientId];

            HUDManager.Instance.DisplayTip("Player Credits", $"You have received {playerCreditAmount} {LguConstants.ALTERNATIVE_CURRENCY_ALIAS}s from {traderPlayer.playerUsername}. You currently have {CurrencyAmount} {LguConstants.ALTERNATIVE_CURRENCY_ALIAS}s to use in the upgrade shop.");
        }
    }
}
