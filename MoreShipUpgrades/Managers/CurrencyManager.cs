using GameNetcodeStuff;
using MoreShipUpgrades.Configuration;
using MoreShipUpgrades.Misc.Util;
using System;
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
        int spentCurrencyAmount;
        public int SpentCurrencyAmount
        {
            get
            {
                return spentCurrencyAmount;
            }
            set
            {
                spentCurrencyAmount = value;
            }
        }

        void Awake()
        {
            if (Instance == null) Instance = this;
            showCurrencyAmount = false;
		}
		public static AlternativeCurrencyConfiguration Config
		{
			get
			{
				return UpgradeBus.Instance.PluginConfiguration.AlternativeCurrencyConfiguration;
			}
		}

		public static bool Enabled
        {
            get
            {
                return Config.Enabled;
            }
        }

        public static bool BlockGainOperations
        {
            get
            {
                return Config.BlockGainOperations;
            }
        }

        public static int MaximumAmountPerPlayer
        {
            get
            {
                return Config.MaximumAmountPerPlayer;
            }
        }


        public static string GetNewCreditFormat(string format)
        {
            if (!Enabled) return format;
            if (!Instance.ShowCurrentAmount) return format;
            if (MaximumAmountPerPlayer > 0)
			{
				return $"{Instance.CurrencyAmount} / {Mathf.Clamp(Instance.CurrencyAmount + Instance.SpentCurrencyAmount, 0, MaximumAmountPerPlayer)} {LguConstants.ALTERNATIVE_CURRENCY_ALIAS}";
			}
			return $"{Instance.CurrencyAmount} / {(Instance.CurrencyAmount + Instance.SpentCurrencyAmount)} {LguConstants.ALTERNATIVE_CURRENCY_ALIAS}";
        }

        public float GetCreditRatio()
        {
            return 1f / Config.CreditsToCurrencyRatio;
        }

        public float GetQuotaRatio()
        {
            return 1f / Config.QuotaToCurrencyRatio;
        }

        public float GetCreditToCurrencyConversionRatio()
        {
            return 1f / Config.CreditsToCurrencyConversionRatio;
        }

        public float GetCurrencyToCreditConversionRatio()
        {
            return 1f / Config.CurrencyToCreditsConversionRatio;
        }

        public int GetCurrencyAmountFromQuota(int quotaFullfilled)
        {
            return Mathf.RoundToInt(GetQuotaRatio() *  quotaFullfilled);
        }

        public int GetCurrencyAmountFromCredits(int credits)
        {
            return Mathf.CeilToInt(GetCreditRatio() * credits);
		}
		public int GetRequiredCreditsFromCurrencyConversion(int currencyAmount)
		{
			return Mathf.CeilToInt(currencyAmount / GetCreditToCurrencyConversionRatio());
		}

		public int GetCurrencyAmountFromCreditsConversion(int convertedCredits)
        {
            return Mathf.CeilToInt(GetCreditToCurrencyConversionRatio() * convertedCredits);
        }

        public int GetCreditsFromCurrencyAmountConversion(int convertedCurrencyAmount)
        {
            return Mathf.CeilToInt(convertedCurrencyAmount / GetCurrencyToCreditConversionRatio());
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

        public void AddCurrencyAmount(int amount, bool trackSpent = false)
        {
            CurrencyAmount = Mathf.Clamp(CurrencyAmount + amount, 0, MaximumAmountPerPlayer > 0 ? MaximumAmountPerPlayer : int.MaxValue);
            if (trackSpent)
                SpentCurrencyAmount -= amount;
        }

        public void RemoveCurrencyAmount(int amount, bool trackSpent = false)
        {
            CurrencyAmount -= amount;
            if (trackSpent) SpentCurrencyAmount += amount;
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

		internal bool BlockExceedOperations(int amount)
		{
            if (MaximumAmountPerPlayer <= 0 || !BlockGainOperations) return false;
            return (Config.IncludeSpentInMaximum ? SpentCurrencyAmount : 0) + CurrencyAmount + amount > MaximumAmountPerPlayer;
		}

		internal void ResetAllValues()
		{
            currencyAmount = 0;
            spentCurrencyAmount = 0;
		}
	}
}
