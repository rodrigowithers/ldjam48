using DTO;
using Tile;
using System;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _body;
    
        [SerializeField] private float _drillRadius = 1;
        [SerializeField] private float _drillOffset = 1;

        [SerializeField] private LayerMask _tileLayer;

        public ParticleSystem DrillBit;
    
        private Vector2 DrillPoint => transform.position - transform.up * _drillOffset;
        
        public event Action OnPaused = () => {};
        public event Action OnResumed = () => {};

        private float _movementRotation;
        private bool _moving;
        
        private Collider2D[] _hits;

        private Animator _animator;
        public Drill Drill;

        private float _drillParticlesCount;
        
        public void ResetPlayer()
        {
            Drill = Drill.GetDrill(PlayerDatabase.Info.DrillIdentifier);
            Drill.Health = Drill.TotalHealth;
        }

        private void OnEnable()
        {
            switch (PlayerDatabase.Info.DrillIdentifier)
            {
                case 0:
                    _animator.Play("Stone", 0);
                    break;
                case 1:
                    _animator.Play("Iron", 0);
                    break;
                case 2:
                    _animator.Play("Gold", 0);
                    break;
            }
        }

        private void HandleInput()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
                GameController.Instance.SetGameState(GameController.GameState.Menu);
            
            _movementRotation = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.W))
            {
                if(!_moving)
                    OnResumed?.Invoke();
                
                _moving = true;
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                if(_moving)
                    OnPaused?.Invoke();

                _moving = false;
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_moving)
                {
                    _moving = false;
                    OnPaused?.Invoke();
                }
                else
                {
                    _moving = true;
                    OnResumed?.Invoke();
                }
            }
        }

        private void HandleCollisions()
        {
            if (Drill.Health <= 0)
                return;
            
            DebugExtension.DebugCircle(DrillPoint, Vector3.forward, Color.red, _drillRadius);
            
            var hits = Physics2D.OverlapCircleNonAlloc(DrillPoint, _drillRadius, _hits, _tileLayer);

            for (int i = 0; i < hits; i++)
            {
                _drillParticlesCount += 0.1f;
                
                if (_drillParticlesCount >= 1f)
                {
                    DrillBit.Emit(Mathf.FloorToInt(1));
                    _drillParticlesCount = 0;
                }
                
                if (_hits[i].GetComponent<BreakableTile>().Damage(Drill.Strength))
                {
                    Drill.Health--;
                }
            }
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _hits = new Collider2D[9];
            
            ResetPlayer();
        }

        private void Update()
        {
            Shader.SetGlobalVector("_PlayerPosition", transform.position);
            Shader.SetGlobalFloat("_Radius", Mathf.Lerp(20, 3, Mathf.InverseLerp(7, -20, transform.position.y)));
            
            HandleInput();
            HandleCollisions();

            transform.Rotate(Vector3.forward, _movementRotation * 0.5f);
        
            if (_moving)
                _body.velocity = -transform.up * Drill.Speed;
            else
                _body.velocity = Vector2.zero;
        
            DebugExtension.DebugArrow(transform.position, -transform.up, Color.red);
        }
    }
}
