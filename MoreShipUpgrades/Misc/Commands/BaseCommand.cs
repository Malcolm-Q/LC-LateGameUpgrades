using MoreShipUpgrades.Misc.Util;
using System;
using Unity.Netcode;
using UnityEngine;

namespace MoreShipUpgrades.Misc.Commands
{
    public abstract class BaseCommand : NetworkBehaviour
    {
        /// <summary>
        /// Registers the associated command to the game to be initialized correctly
        /// <para></para>
        /// This method is to be overriden by their subclasses through "new"
        /// </summary>
        /// <exception cref="NotSupportedException"></exception>
        public static void RegisterCommand() { throw new NotSupportedException(); }

        /// <summary>
        /// Generic function where it adds a script (specificed through the type) into an GameObject asset
        /// which is present in a provided asset bundle in a given path and registers it as a network prefab.
        /// </summary>
        /// <typeparam name="T"> The script we wish to include into the GameObject asset</typeparam>
        internal static void SetupGenericCommand<T>(string commandName) where T : Component
        {
            Tools.SetupGameObject<T>(commandName);
        }
    }
}
