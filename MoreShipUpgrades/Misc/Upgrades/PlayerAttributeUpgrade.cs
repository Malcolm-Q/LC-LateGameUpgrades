using GameNetcodeStuff;
using LethalLib.Modules;
using MoreShipUpgrades.Managers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MoreShipUpgrades.Misc.Upgrades
{
    internal class PlayerAttributeTierUpgrade : TierUpgrade
    {
        protected bool activeUpgrade;
        protected int currentUpgradeLevel;
        protected float initialValue;
        protected float incrementalValue;
        protected PlayerAttribute changingAttribute;
        protected LGULogger logger;

        internal enum PlayerAttribute
        {
            NONE,
            HEALTH,
            SPRINT_TIME,
            MOVEMENT_SPEED,
            JUMP_FORCE,
        }

        protected void LoadUpgradeAttribute(ref bool upgradeActive, int upgradeLevel = 0)
        {
            if (!activeUpgrade) AddInitialValue();
            upgradeActive = true;
            activeUpgrade = true;

            AddPossibleIncrementalValues(upgradeLevel);
            currentUpgradeLevel = upgradeLevel;
        }
        void AddInitialValue()
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            logger.LogDebug($"Adding {initialValue} to the player's {changingAttribute}...");
            switch (changingAttribute)
            {
                case PlayerAttribute.HEALTH: player.health += (int)initialValue; break;
                case PlayerAttribute.MOVEMENT_SPEED: player.movementSpeed += initialValue; break;
                case PlayerAttribute.SPRINT_TIME: player.sprintTime += initialValue; break;
                case PlayerAttribute.JUMP_FORCE: player.jumpForce += initialValue; break;
                default: logger.LogError("No attribute was set for this upgrade to add the initial value"); break;
            }
        }
        void AddIncrementalValue()
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            logger.LogDebug($"Adding {incrementalValue} to the player's {changingAttribute}...");
            switch (changingAttribute)
            {
                case PlayerAttribute.HEALTH: player.health += (int)incrementalValue; break;
                case PlayerAttribute.MOVEMENT_SPEED: player.movementSpeed += incrementalValue; break;
                case PlayerAttribute.SPRINT_TIME: player.sprintTime += incrementalValue; break;
                case PlayerAttribute.JUMP_FORCE: player.jumpForce += incrementalValue; break;
                default: logger.LogError("No attribute was set for this upgrade to add the incremental value"); break;
            }
        }
        void AddPossibleIncrementalValues(int upgradeLevel = 0)
        {
            float amountToIncrement = 0;
            for (int i = 1; i < upgradeLevel + 1; i++)
            {
                if (i <= currentUpgradeLevel) continue;
                logger.LogDebug($"Adding {incrementalValue} to the player's {changingAttribute}...");
                amountToIncrement += incrementalValue;
            }

            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            switch (changingAttribute)
            {
                case PlayerAttribute.HEALTH: player.health += (int)amountToIncrement; break;
                case PlayerAttribute.MOVEMENT_SPEED: player.movementSpeed += amountToIncrement; break;
                case PlayerAttribute.SPRINT_TIME: player.sprintTime += amountToIncrement; break;
                case PlayerAttribute.JUMP_FORCE: player.jumpForce += amountToIncrement; break;
                default: logger.LogError("No attribute was set for this upgrade to add the incremental values"); break;
            }
        }
        public void UnloadUpgradeAttribute(ref bool upgradeActive, ref int upgradeLevel)
        {
            if (upgradeActive) RemoveInitialValue();
            RemovePossibleIncrementalValues(upgradeLevel);
            upgradeActive = false;
            upgradeLevel = 0;
            activeUpgrade = upgradeActive;
            currentUpgradeLevel = upgradeLevel;
        }
        void RemoveInitialValue()
        {
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            logger.LogDebug($"Removing {initialValue} to the player's {changingAttribute}...");
            switch (changingAttribute)
            {
                case PlayerAttribute.HEALTH: player.health -= (int)initialValue; break;
                case PlayerAttribute.MOVEMENT_SPEED: player.movementSpeed -= initialValue; break;
                case PlayerAttribute.SPRINT_TIME: player.sprintTime -= initialValue; break;
                case PlayerAttribute.JUMP_FORCE: player.jumpForce -= initialValue; break;
                default: logger.LogError("No attribute was set for this upgrade to remove the initial value"); break;
            }
        }
        protected void RemovePossibleIncrementalValues(int upgradeLevel = 0)
        {
            float amountToIncrement = 0;
            for (int i = 0; i < upgradeLevel; i++)
            {
                logger.LogDebug($"Removing {incrementalValue} to the player's {changingAttribute}...");
                amountToIncrement += incrementalValue;
            }
            if (amountToIncrement <= 0) return;
            PlayerControllerB player = UpgradeBus.instance.GetLocalPlayer();
            switch (changingAttribute)
            {
                case PlayerAttribute.HEALTH: player.health -= (int)amountToIncrement; break;
                case PlayerAttribute.MOVEMENT_SPEED: player.movementSpeed -= amountToIncrement; break;
                case PlayerAttribute.SPRINT_TIME: player.sprintTime -= amountToIncrement; break;
                case PlayerAttribute.JUMP_FORCE: player.jumpForce -= amountToIncrement; break;
                default: logger.LogError("No attribute was set for this upgrade to remove the incremental values"); break;
            }
        }
        public override void Increment()
        {
            AddIncrementalValue();
            currentUpgradeLevel++;
        }
    }
}
