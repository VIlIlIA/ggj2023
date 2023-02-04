using Cinemachine.PostFX;
using UnityEngine;

namespace Player
{
    public class PlayerKeybinding : MonoBehaviour
    {
        [field:SerializeField] public KeyCode UpKey { get; private set; }
        [field:SerializeField] public KeyCode DownKey { get; private set; }
        [field:SerializeField] public KeyCode LeftKey { get; private set; }
        [field:SerializeField] public KeyCode RightKey { get; private set; }
        [field:SerializeField] public KeyCode AttackKey { get; private set; }
        [field:SerializeField] public KeyCode RootKey { get; private set; }

        public float GetHorizontalAxis()
        {
            if (Input.GetKey(RightKey))
            {
                return 1;
            }

            if (Input.GetKey(LeftKey))
            {
                return -1;
            }

            return 0;
        }
    }
}