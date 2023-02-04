using UnityEngine;

namespace Camera
{
    public class UniversalCameraSetting : MonoBehaviour
    {
        [SerializeField] private Color background;
        [SerializeField] private UnityEngine.Camera[] cameras;
        // Start is called before the first frame update
        private void Start()
        {
            foreach (var cam in cameras)
            {
                cam.backgroundColor = background;
            }
        }
    }
}
