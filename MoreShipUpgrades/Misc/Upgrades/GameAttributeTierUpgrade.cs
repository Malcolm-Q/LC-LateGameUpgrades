using GameNetcodeStuff;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.UpgradeComponents.Interfaces;

namespace MoreShipUpgrades.Misc.Upgrades
{
    /// <summary>
    /// Class responsible to handle game attributes such as health, jump force, movement speed, sprint time for the player and ship's battery for keeping the doors closed, along many others
    /// </summary>
    public abstract class GameAttributeTierUpgrade : TierUpgrade, IPlayerSync
    {
        #region Variables
        /// <summary>
        /// Previous state of the upgrade (for the purposes of "load LGU" not providing multiple increments)
        /// </summary>
        protected bool activeUpgrade;
        /// <summary>
        /// Previous level of the upgrade (for the purposes of "load LGU" not providing multiple increments)
        /// </summary>
        protected int currentUpgradeLevel;
        /// <summary>
        /// Initial value incremented to the selected attribute when first purchasing the selected upgrade
        /// </summary>
        protected float initialValue;
        /// <summary>
        /// Incremental value added to the selected attribute when purchasing further levels of the selected upgrade
        /// </summary>
        protected float incrementalValue;
        /// <summary>
        /// Attribute which the selected upgrade affects when being purchased/upgraded
        /// </summary>
        protected GameAttribute changingAttribute;
        /// <summary>
        /// Logger associated with this class for logging purposes (incase an attribute change is not occuring)
        /// </summary>
        protected LguLogger logger;
        #endregion
        #region Attribute Getters
        /// <summary>
        /// Enumerator used to distinguish between the player's attributes (this wouldn't be necessary if we could have reference to variables but for now, this will have to work)
        /// </summary>
        protected enum GameAttribute
        {
            /// <summary>
            /// None of the player attributes was selected (default case and act as failsafe when no attribute was selected)
            /// </summary>
            NONE,
            /// <summary>
            /// Affects the player's PLAYER_HEALTH
            /// </summary>
            PLAYER_HEALTH,
        }
        PlayerControllerB GetLocalPlayer()
        {
            return UpgradeBus.Instance.GetLocalPlayer();
        }
        #endregion
        #region Attribute Setters
        /// <summary>
        /// Initializes the upgrade to be applying to the local player by changing its selected attribute's value<para></para>
        /// Will apply incremental values based on the delta of levels between last saved and provided
        /// </summary>
        protected void LoadUpgradeAttribute()
        {
            if (!activeUpgrade) AddInitialValue();
            activeUpgrade = true;

            int upgradeLevel = GetUpgradeLevel(upgradeName);
            AddPossibleIncrementalValues(upgradeLevel);
            currentUpgradeLevel = upgradeLevel;
        }
        /// <summary>
        /// Adds the initial value stored in the upgrade to the selected attribute<para></para>
        /// An error will be logged when an attribute was not chosen for this upgrade
        /// </summary>
        void AddInitialValue()
        {
            PlayerControllerB localPlayer = GetLocalPlayer();
            switch (changingAttribute)
            {
                case GameAttribute.PLAYER_HEALTH: localPlayer.health += (int)initialValue; break;
                default: logger.LogError("No attribute was set for this upgrade to add the initial value"); break;
            }
        }
        /// <summary>
        /// Adds the incremental value stored in the upgrade to the selected attribute<para></para>
        /// An error will be logged when an attribute was not chosen for this upgrade
        /// </summary>
        void AddIncrementalValue()
        {
            PlayerControllerB localPlayer = GetLocalPlayer();
            switch (changingAttribute)
            {
                case GameAttribute.PLAYER_HEALTH: localPlayer.health += (int)incrementalValue; break;
                default: logger.LogError("No attribute was set for this upgrade to add the incremental value"); break;
            }
        }
        /// <summary>
        /// Adds the remaining incremental values needed when initializing the upgrade with a new level<para></para>
        /// </summary>
        /// <param name="upgradeLevel">New level of the upgrade</param>
        void AddPossibleIncrementalValues(int upgradeLevel = 0)
        {
            float amountToIncrement = 0;
            for (int i = 1; i < upgradeLevel + 1; i++)
            {
                if (i <= currentUpgradeLevel) continue;
                amountToIncrement += incrementalValue;
            }

            PlayerControllerB localPlayer = GetLocalPlayer();
            switch (changingAttribute)
            {
                case GameAttribute.PLAYER_HEALTH: localPlayer.health += (int)amountToIncrement; break;
                default: logger.LogError("No attribute was set for this upgrade to add the incremental values"); break;
            }
        }
        /// <summary>
        /// Removes the values introduced into the selected attribute to make them normalized to the vanilla standard<para></para>
        /// and resets the active status and level of the upgrade to turned off values
        /// </summary>
        internal void UnloadUpgradeAttribute()
        {
            bool upgradeActive = GetActiveUpgrade(upgradeName);
            int upgradeLevel = GetUpgradeLevel(upgradeName);
            if (upgradeActive) RemoveInitialValue();
            RemovePossibleIncrementalValues(upgradeLevel);
            activeUpgrade = false;
            currentUpgradeLevel = 0;
        }
        /// <summary>
        /// Removes the initial value introduced into the selected attribute
        /// </summary>
        void RemoveInitialValue()
        {
            PlayerControllerB localPlayer = GetLocalPlayer();
            switch (changingAttribute)
            {
                case GameAttribute.PLAYER_HEALTH: localPlayer.health -= (int)initialValue; break;
                default: logger.LogError("No attribute was set for this upgrade to remove the initial value"); break;
            }
        }
        /// <summary>
        /// Removes incremenetal values that were inserted into the selected attribute based on the provided level of the upgrade
        /// </summary>
        /// <param name="upgradeLevel">Current level of the upgrade</param>
        protected void RemovePossibleIncrementalValues(int upgradeLevel = 0)
        {
            PlayerControllerB localPlayer = GetLocalPlayer();
            float amountToIncrement = 0;
            for (int i = 0; i < upgradeLevel; i++)
            {
                amountToIncrement += incrementalValue;
            }
            if (amountToIncrement <= 0) return;
            switch (changingAttribute)
            {
                case GameAttribute.PLAYER_HEALTH: localPlayer.health -= (int)amountToIncrement; break;
                default: logger.LogError("No attribute was set for this upgrade to remove the incremental values"); break;
            }
        }
        #endregion
        #region Overriden Methods
        public override void Increment()
        {
            base.Increment();
            AddIncrementalValue();
            currentUpgradeLevel++;
        }
        public override void Load()
        {
            LoadUpgradeAttribute();
            base.Load();
        }
        public override void Unwind()
        {
            UnloadUpgradeAttribute();
            base.Unwind();
        }

        public void ResetPlayerAttribute()
        {
            UnloadUpgradeAttribute();
        }
        #endregion
    }
}
