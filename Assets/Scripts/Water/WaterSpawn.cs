using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Water
{
    public class WaterSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject[] players;
        [SerializeField] private GameObject waterPrefab;
        [SerializeField] private int cd;

        private int _counter;


        private void SpawnWater(GameObject player)
        {
            var x = player.transform.position.x;
            var sp = Random.Range(x - 10, x + 10);
            Instantiate(waterPrefab, new Vector3(sp, transform.position.y), Quaternion.identity);
        }

        private void Awake()
        {
            _counter = cd;
        }

        private void Update()
        {
            _counter -= Mathf.RoundToInt(Time.fixedDeltaTime*1000);
            if (_counter > 0) return;
            foreach (var player in players)
            {
                SpawnWater(player);
            }

            _counter = cd;
        }
    }
}