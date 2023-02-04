using System.Collections;
using UnityEngine;

namespace ScriptableObject
{
    [CreateAssetMenu(fileName = "Player Manager")]
    public class PlayerManager : UnityEngine.ScriptableObject
    {
        private readonly Hashtable _player = new();

        public void AddPlayer(int id, GameObject pl) => _player[id] = pl;

        public void RemovePlayer(int id) => _player.Remove(id);

        public GameObject GetPlayer(int id) => (GameObject)_player[id];
    }
}