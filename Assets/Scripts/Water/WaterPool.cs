using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterPool : MonoBehaviour
{
    private const int PoolLimit = 200;

    private readonly List<GameObject> _waterObjects = new();

    [SerializeField] private GameObject waterPrefab;
    // Start is called before the first frame update


    private void Start()
    {
        for (var i = 0; i < PoolLimit; i++)
        {
            var water = Instantiate(waterPrefab);
            water.SetActive(false);
            _waterObjects.Add(water);
        }
    }

    public GameObject GetPooledGameObject() => _waterObjects.FirstOrDefault(i => !i.activeInHierarchy);
}