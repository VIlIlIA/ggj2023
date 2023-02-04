using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerData : MonoBehaviour
    {
        [field: SerializeField] public int WaterLimit { get; private set; }


        [SerializeField] private int growthCoolDown;
        [SerializeField] private int invincibleFrame;

        [Header("On Win")] [SerializeField] private UnityEvent winEvent;

        [Header("Read only")]
        public int waterAmount;
        public int growthPercentage;

        private PlayerController _controller;
        private int _counter;
        private int _iFrame;

        public bool CanHit() => _iFrame <= 0;

        public void Hit() => _iFrame = invincibleFrame;

        public void AddWater(int amount) => waterAmount = waterAmount + amount > 100 ? 100 : waterAmount + amount;

        public int RemoveWater(int amount)
        {
            if (waterAmount - amount < 0)
            {
                var result = amount + (waterAmount - amount);
                waterAmount = 0;
                return result;
            }

            waterAmount -= amount;
            return amount;
        }

        public void ClearWater() => waterAmount = 0;

        private void Grow(int percentage)
        {
            if (waterAmount <= 0) return;
            waterAmount -= percentage * 4;
            growthPercentage += percentage;
            if (growthPercentage >= 100)
            {
                Debug.Log("You won");
                // trigger win condition
            }
        }

        private void Start()
        {
            _controller = GetComponent<PlayerController>();
            _counter = growthCoolDown;
        }

        private void Update()
        {
            var deltaMillis = Mathf.RoundToInt(Time.deltaTime * 1000);
            _iFrame = _iFrame > 0 ? _iFrame - deltaMillis : 0;

            switch (_controller.Rooted)
            {
                case false when _counter == growthCoolDown:
                    return;
                case false:
                    _counter = growthCoolDown;
                    return;
            }
            _counter -= deltaMillis;
            if (_counter > 0) return;
            Grow(1);
            _counter = growthCoolDown;
        }

    }
}