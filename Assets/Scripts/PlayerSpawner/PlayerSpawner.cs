using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerSpawner
{
    public class PlayerSpawner : MonoBehaviour
    {
        [FormerlySerializedAs("_worldGeneratorGameObject")] [SerializeField]
        private GameObject worldGeneratorGameObject;

        [FormerlySerializedAs("_playerPrefab")] [SerializeField]
        private GameObject playerPrefab;

        // [SerializeField] private PlayerManager playerManager;
        // [SerializeField] private GameEvent onPlayerSpawn;
        [SerializeField] private GameObject playerGameObject;
        [SerializeField] private GameObject cameraGameObject;

        private WorldGenerator _worldGenerator;


        // Start is called before the first frame update
        private void Start()
        {
            _worldGenerator = worldGeneratorGameObject.GetComponent<WorldGenerator>();
        }

        public void Spawn()
        {
            playerGameObject.transform.position = _worldGenerator.GetSpawnableTile();
            cameraGameObject.GetComponent<CinemachineVirtualCamera>().Follow = playerGameObject.transform;
        }
    }
}