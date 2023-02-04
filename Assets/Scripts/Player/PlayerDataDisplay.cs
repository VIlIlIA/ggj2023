using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerDataDisplay : MonoBehaviour
    {
        [SerializeField] private Image waterBar;
        [SerializeField] private Image growthBar;

        private PlayerData _playerData;

        // Start is called before the first frame update
        private void Start()
        {
            _playerData = GetComponent<PlayerData>();
        }

        // Update is called once per frame
        private void Update()
        {
            waterBar.fillAmount = _playerData.waterAmount * 0.01f;
            growthBar.fillAmount = _playerData.growthPercentage * 0.01f;
        }
    }
}