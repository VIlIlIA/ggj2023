using Player;
using UnityEngine;

namespace Water
{
    public class WaterHit : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;
    
        private void OnCollisionEnter2D(Collision2D col)
        {
            // Debug.Log("OnCollisionEnter2D");
            if ((groundLayer.value & (1 << col.transform.gameObject.layer)) > 0)
            {
                Destroy(gameObject);
            }

            if (!col.gameObject.CompareTag("Player")) return;
            col.gameObject.GetComponent<PlayerData>().AddWater(6);
            Destroy(gameObject);
        }
    }
}
