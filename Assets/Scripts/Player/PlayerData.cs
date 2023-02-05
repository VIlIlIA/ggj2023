using System;
using System.Collections;
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
        private SpriteRenderer _spriteRenderer;
        
        private int _counter;
        private int _iFrame;
        private bool _invincible;

        public bool CanHit() => !_invincible;

        public void Hit()
        {
            _controller.UnRoot();
            _invincible = true;
            StartCoroutine(Flash());
        }

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
        
        private IEnumerator Flash()
        {
            const float flashDelay = 0.0833f;
            for (var i = 0; i < 10; i++)
            {
                _spriteRenderer.color = new Color(1,1,1,0);
                yield return new WaitForSeconds(flashDelay);
                _spriteRenderer.color = new Color(1,1,1,1);
                yield return new WaitForSeconds(flashDelay);
            }
            _invincible = false;
        }


        private void Start()
        {
            _controller = GetComponent<PlayerController>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
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