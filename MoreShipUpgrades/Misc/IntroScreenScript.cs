using UnityEngine;
using UnityEngine.InputSystem;

namespace MoreShipUpgrades.Misc
{
    internal class IntroScreenScript : MonoBehaviour
    {
        void Update()
        {
            if(Keyboard.current[Key.Escape].wasPressedThisFrame)
            {
                Destroy(gameObject);
            }
        }
    }
}
