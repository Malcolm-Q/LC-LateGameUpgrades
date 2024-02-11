using GameNetcodeStuff;
using MoreShipUpgrades.Managers;

namespace MoreShipUpgrades.Misc.Upgrades
{
    /// <summary>
    /// Class responsible to handle game attributes such as health, jump force, movement speed, sprint time for the player and ship's battery for keeping the doors closed, along many others
    /// </summary>
    abstract class GameAttributeTierUpgrade : TierUpgrade
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
        /// Instance of the ship's doors which holds attributes such as time it can stay closed, etc..
        /// </summary>
        private HangarShipDoor doorControls;
        /// <summary>
        /// Instance of the local player's controller which holds attributes such as PLAYER_HEALTH, movement speed, etc..
        /// </summary>
        private PlayerControllerB localPlayer;
        /// <summary>
        /// Logger associated with this class for logging purposes (incase an attribute change is not occuring)
        /// </summary>
        protected LGULogger logger;
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
            /// <summary>
            /// Affects the stamina variable used when sprinting
            /// </summary>
            PLAYER_SPRINT_TIME,
            /// <summary>
            /// Affects the speed of the player's movement
            /// </summary>
            PLAYER_MOVEMENT_SPEED,
            /// <summary>
            /// Affects the height of the player's jump
            /// </summary>
            PLAYER_JUMP_FORCE,
            /// <summary>
            /// Affects the total amount of time the ship's doors can hold shut
            /// </summary>
            SHIP_DOOR_BATTERY,
            /// <summary>
            /// Affects the global time speed multiplier used to change the time in a moon
            /// </summary>
            TIME_GLOBAL_TIME_MULTIPLIER,
        }
        PlayerControllerB GetLocalPlayer()
        {
            if (localPlayer == null) localPlayer = UpgradeBus.instance.GetLocalPlayer();
            return localPlayer;
        }
        HangarShipDoor GetShipDoors()
        {
            if (doorControls == null) doorControls = UpgradeBus.instance.GetShipDoors();
            return doorControls;
        }
        #endregion
        #region Attribute Setters
        /// <summary>
        /// Initializes the upgrade to be applying to the local player by changing its selected attribute's value<para></para>
        /// Will apply incremental values based on the delta of levels between last saved and provided
        /// </summary>
        /// <param name="upgradeActive">Represents the state of the upgrade being applied on the player</param>
        /// <param name="upgradeLevel">Current level of the upgrade when loading the upgrade</param>
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
            HangarShipDoor doorControls = GetShipDoors();
            logger.LogDebug($"Adding {initialValue} to {changingAttribute}...");
            switch (changingAttribute)
            {
                case GameAttribute.PLAYER_HEALTH: localPlayer.health += (int)initialValue; break;
                case GameAttribute.PLAYER_MOVEMENT_SPEED: localPlayer.movementSpeed += initialValue; break;
                case GameAttribute.PLAYER_SPRINT_TIME: localPlayer.sprintTime += initialValue; break;
                case GameAttribute.PLAYER_JUMP_FORCE: localPlayer.jumpForce += initialValue; break;
                case GameAttribute.SHIP_DOOR_BATTERY: doorControls.doorPowerDuration += initialValue; break;
                case GameAttribute.TIME_GLOBAL_TIME_MULTIPLIER: TimeOfDay.Instance.globalTimeSpeedMultiplier -= initialValue; break;
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
            HangarShipDoor doorControls = GetShipDoors();
            logger.LogDebug($"Adding {incrementalValue} to {changingAttribute}...");
            switch (changingAttribute)
            {
                case GameAttribute.PLAYER_HEALTH: localPlayer.health += (int)incrementalValue; break;
                case GameAttribute.PLAYER_MOVEMENT_SPEED: localPlayer.movementSpeed += incrementalValue; break;
                case GameAttribute.PLAYER_SPRINT_TIME: localPlayer.sprintTime += incrementalValue; break;
                case GameAttribute.PLAYER_JUMP_FORCE: localPlayer.jumpForce += incrementalValue; break;
                case GameAttribute.SHIP_DOOR_BATTERY: doorControls.doorPowerDuration += incrementalValue; break;
                case GameAttribute.TIME_GLOBAL_TIME_MULTIPLIER: TimeOfDay.Instance.globalTimeSpeedMultiplier -= incrementalValue; break;
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
                logger.LogDebug($"Adding {incrementalValue} to {changingAttribute}...");
                amountToIncrement += incrementalValue;
            }

            PlayerControllerB localPlayer = GetLocalPlayer();
            HangarShipDoor doorControls = GetShipDoors();
            switch (changingAttribute)
            {
                case GameAttribute.PLAYER_HEALTH: localPlayer.health += (int)amountToIncrement; break;
                case GameAttribute.PLAYER_MOVEMENT_SPEED: localPlayer.movementSpeed += amountToIncrement; break;
                case GameAttribute.PLAYER_SPRINT_TIME: localPlayer.sprintTime += amountToIncrement; break;
                case GameAttribute.PLAYER_JUMP_FORCE: localPlayer.jumpForce += amountToIncrement; break;
                case GameAttribute.SHIP_DOOR_BATTERY: doorControls.doorPowerDuration += amountToIncrement; break;
                case GameAttribute.TIME_GLOBAL_TIME_MULTIPLIER: TimeOfDay.Instance.globalTimeSpeedMultiplier -= amountToIncrement; break;
                default: logger.LogError("No attribute was set for this upgrade to add the incremental values"); break;
            }
        }
        /// <summary>
        /// Removes the values introduced into the selected attribute to make them normalized to the vanilla standard<para></para>
        /// and resets the active status and level of the upgrade to turned off values
        /// </summary>
        /// <param name="upgradeActive">Current status of the upgrade</param>
        /// <param name="upgradeLevel">Current level of the upgrade</param>
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
            HangarShipDoor doorControls = GetShipDoors();
            logger.LogDebug($"Removing {initialValue} from {changingAttribute}...");
            switch (changingAttribute)
            {
                case GameAttribute.PLAYER_HEALTH: localPlayer.health -= (int)initialValue; break;
                case GameAttribute.PLAYER_MOVEMENT_SPEED: localPlayer.movementSpeed -= initialValue; break;
                case GameAttribute.PLAYER_SPRINT_TIME: localPlayer.sprintTime -= initialValue; break;
                case GameAttribute.PLAYER_JUMP_FORCE: localPlayer.jumpForce -= initialValue; break;
                case GameAttribute.SHIP_DOOR_BATTERY: doorControls.doorPowerDuration -= initialValue; break;
                case GameAttribute.TIME_GLOBAL_TIME_MULTIPLIER: TimeOfDay.Instance.globalTimeSpeedMultiplier += initialValue; break;
                default: logger.LogError("No attribute was set for this upgrade to remove the initial value"); break;
            }
        }
        /// <summary>
        /// Removes incremenetal values that were inserted into the selected attribute based on the provided level of the upgrade
        /// </summary>
        /// <param name="upgradeLevel">Current level of the upgrade</param>
        protected void RemovePossibleIncrementalValues(int upgradeLevel = 0)
        {
            float amountToIncrement = 0;
            for (int i = 0; i < upgradeLevel; i++)
            {
                logger.LogDebug($"Removing {incrementalValue} from {changingAttribute}...");
                amountToIncrement += incrementalValue;
            }
            if (amountToIncrement <= 0) return;
            switch (changingAttribute)
            {
                case GameAttribute.PLAYER_HEALTH: localPlayer.health -= (int)amountToIncrement; break;
                case GameAttribute.PLAYER_MOVEMENT_SPEED: localPlayer.movementSpeed -= amountToIncrement; break;
                case GameAttribute.PLAYER_SPRINT_TIME: localPlayer.sprintTime -= amountToIncrement; break;
                case GameAttribute.PLAYER_JUMP_FORCE: localPlayer.jumpForce -= amountToIncrement; break;
                case GameAttribute.SHIP_DOOR_BATTERY: doorControls.doorPowerDuration -= amountToIncrement; break;
                case GameAttribute.TIME_GLOBAL_TIME_MULTIPLIER: TimeOfDay.Instance.globalTimeSpeedMultiplier += amountToIncrement; break;
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
        #endregion
    }
}
