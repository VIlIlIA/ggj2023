using Cinemachine;
using ScriptableObject;
using UnityEngine;
using UnityEngine.Serialization;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private PlayerManager _playerManager;
        private CinemachineVirtualCamera _camera;

        private void Start()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
        }

        public void SetCameraTarget(Component sender, object data)
        {
            if (data is not int id) return;
            var player = _playerManager.GetPlayer(id);
            _camera.Follow = player.transform;
            _camera.LookAt = player.transform;
        }
    }
}
