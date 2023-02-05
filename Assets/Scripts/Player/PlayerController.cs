using System;
using System.Collections;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Setting")] [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float wallSlidingSpeed;
        [Header("Detector")] [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform leftCheck;
        [SerializeField] private Transform rightCheck;
        [SerializeField] private LayerMask groundLayer;
        [Header("Camera")] [SerializeField] private CinemachineVirtualCamera vCam;
        [SerializeField] private float zoomSpeed;

        public bool Rooted { get; private set; }

        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private PlayerKeybinding _playerKeybinding;
        private PlayerData _playerData;

        private const float WallJumpingTime = 0.2f;
        private float _wallJumpingCounter;
        private float _wallJumpingDirection;
        private Vector2 _wallJumpingPower;

        private int _jumpLeft = 2;

        private bool _grounded;
        private float _horizontal;
        private bool _isWallSliding;
        private bool _isWallJumping;

        private float _zoom;

        public void UnRoot() => Rooted = false;

        // Start is called before the first frame update
        private void Start()
        {
            _wallJumpingPower = new Vector2(speed, jumpForce);
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _rigidbody2D.freezeRotation = true;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _playerKeybinding = GetComponent<PlayerKeybinding>();
            _playerData = GetComponent<PlayerData>();
        }

        // Update is called once per frame
        private void Update()
        {
            _rigidbody2D.isKinematic = Rooted;
            var position = transform.position;
            var x = Mathf.FloorToInt(position.x);
            var y = Mathf.FloorToInt(position.y);
            _horizontal = _playerKeybinding.GetHorizontalAxis();
            if (!Rooted)
            {
                if (Input.GetKeyDown(_playerKeybinding.UpKey) && GroundCheck())
                {
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
                    _jumpLeft = 1;
                }
                else if (Input.GetKeyDown(_playerKeybinding.UpKey) && _jumpLeft > 0)
                {
                    _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
                    _jumpLeft--;
                }

                if (Input.GetKeyUp(_playerKeybinding.UpKey) && _rigidbody2D.velocity.y > 0f)
                {
                    var velocity = _rigidbody2D.velocity;
                    velocity = new Vector2(velocity.x, velocity.y * 0.5f);
                    _rigidbody2D.velocity = velocity;
                }

                if (Input.GetKeyDown(_playerKeybinding.AttackKey))
                {
                    Attack();
                }
            }

            if (Input.GetKeyDown(_playerKeybinding.RootKey) && GroundCheck() && _playerData.CanHit())
            {
                _rigidbody2D.velocity = new Vector2(0, 0);
                Rooted = !Rooted;
            }

            if (Input.GetKeyDown(_playerKeybinding.LeftKey))
            {
                _spriteRenderer.flipX = true;
            }

            if (Input.GetKeyDown(_playerKeybinding.RightKey))
            {
                _spriteRenderer.flipX = false;
            }

            // _spriteRenderer.flipX = _horizontal switch
            // {
            //     < 0f => true,
            //     > 0f => false,
            //     _ => _spriteRenderer.flipX
            // };

            WallSlide();
            WallJump();

            vCam.m_Lens.OrthographicSize = Rooted
                ? Mathf.MoveTowards(vCam.m_Lens.OrthographicSize, 2, zoomSpeed * Time.deltaTime)
                : Mathf.MoveTowards(vCam.m_Lens.OrthographicSize, 8, zoomSpeed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (Rooted) return;
            _rigidbody2D.velocity = new Vector2(_horizontal * speed, _rigidbody2D.velocity.y);
        }

        private bool GroundCheck()
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        }

        private bool WallCheck()
        {
            return Physics2D.OverlapCircle(leftCheck.position, 0.2f, groundLayer) ||
                   Physics2D.OverlapCircle(rightCheck.position, 0.2f, groundLayer);
        }

        private void WallJump()
        {
            if (_isWallSliding)
            {
                _wallJumpingDirection = _spriteRenderer.flipX ? 1 : -1;
                _wallJumpingCounter = WallJumpingTime;

                // CancelInvoke(nameof(StopWallJumping));
            }
            else
            {
                _wallJumpingCounter -= Time.deltaTime;
            }

            if (!Input.GetKeyDown(_playerKeybinding.UpKey) || !(_wallJumpingCounter > 0f)) return;
            _rigidbody2D.velocity = new Vector2(_wallJumpingDirection * _wallJumpingPower.x, _wallJumpingPower.y);
            _wallJumpingCounter = 0f;
            _jumpLeft = 1;

            if (Math.Abs((_spriteRenderer.flipX ? 1 : -1) - _wallJumpingDirection) > 0.1)
            {
                _spriteRenderer.flipX = !_spriteRenderer.flipX;
            }
        }

        private void WallSlide()
        {
            if (WallCheck() && !GroundCheck() && _horizontal != 0f)
            {
                _isWallSliding = true;
                var velocity = _rigidbody2D.velocity;
                _rigidbody2D.velocity = new Vector2(velocity.x,
                    Mathf.Clamp(velocity.y, -wallSlidingSpeed, float.MaxValue));
            }
            else
            {
                _isWallSliding = false;
            }
        }

        private void Attack()
        {
            var check = _spriteRenderer.flipX ? leftCheck.position : rightCheck.position;
            var hit = Physics2D.OverlapCircleAll(check, 0.5f)
                .Where(col =>
                {
                    GameObject o;
                    return (o = col.gameObject).CompareTag("Player") && o != gameObject;
                }).FirstOrDefault();
            if (!hit) return;
            var hitData = hit.GetComponent<PlayerData>();
            if (!hitData.CanHit()) return;
            _playerData.AddWater(hitData.RemoveWater(15));
            hitData.Hit();
        }
    }
}