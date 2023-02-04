using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class TentacleSpawn : MonoBehaviour
{
    private PlayerController _playerController;
    // Start is called before the first frame update
    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController.Rooted)
        {
            
        }
    }
}
