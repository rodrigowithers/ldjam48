using DTO;
using Tile;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Logger = UI.Logger;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Required Components")]
        [SerializeField] private Rigidbody2D _body;

        [Header("Configuration")] 
        [SerializeField, Range(0, 90)] private float MaxTurningAngle = 60;
        [SerializeField] private float _drillRadius = 1;
        [SerializeField] private float _drillOffset = 1;

        [SerializeField] private LayerMask _tileLayer;

        public ParticleSystem DrillBit;
    
        private Vector2 DrillPoint => transform.position - transform.up * _drillOffset;
        
        public event Action OnPaused = () => {};
        public event Action OnResumed = () => {};
        public event Action OnDrillBroken = () => {};
        public event Action OnDrillDamage = () => {};

        private float _movementRotation;
        private bool _moving;
        private bool _broken;
        
        private Collider2D[] _hits;

        private Animator _animator;
        public Drill Drill;

        private float _drillParticlesCount;
        private float _turningAngle;

        private Vector2 _velocity;
        
        public void ResetPlayer()
        {
            Drill = Drill.GetDrill(PlayerDatabase.Info.DrillIdentifier);
            Drill.Tier = PlayerDatabase.Info.DrillTier;
            Drill.SetHealth();

            _turningAngle = 0;
            _broken = false;
            
            StopAnimation();
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
            if (Drill.Health <= 0 || !_moving)
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
                
                if (_hits[i].GetComponent<BreakableTile>().Damage(Drill.GetDamage(), Drill.Strength))
                {
                    OnDrillDamage.Invoke();
                }
            }
        }

        private void StopAnimation()
        {
            if(_animator != null)
                _animator.speed = 0;
        }
        
        private void ResumeAnimation()
        {
            _animator.speed = 1;
        }

        private void RecoilMovement()
        {
            _body.velocity = -_body.velocity * 1.5f;
        }

        private void OnDrillDamageListener()
        {
            Drill.Health--;

            if (Drill.Health <= 0)
            {
                AchievementSystem.AchievementHandler.IncreaseAchievementCounter(10);
                
                OnDrillBroken.Invoke();
            }
        }

        private async void OnDrillBrokenListener()
        {
            Logger.Instance.Log("Your drill broke");

            _moving = false;
            _broken = true;
            
            StopAnimation();
            RecoilMovement();

            await Task.Delay(1000);
            
            GameController.Instance.SetGameState(GameController.GameState.Menu);
        }
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _hits = new Collider2D[9];
            
            ResetPlayer();

            OnPaused += StopAnimation;
            OnPaused += RecoilMovement;
            
            OnResumed += ResumeAnimation;

            OnDrillBroken += OnDrillBrokenListener;
            OnDrillDamage += OnDrillDamageListener;
        }

        private void Update()
        {
            Shader.SetGlobalVector("_PlayerPosition", transform.position);
            Shader.SetGlobalFloat("_Radius", Mathf.Lerp(20, 3, Mathf.InverseLerp(7, -20, transform.position.y)));
            
            if(Input.GetKeyDown(KeyCode.Escape))
                GameController.Instance.SetGameState(GameController.GameState.Menu);
            
            if (!_broken)
            {
                HandleInput();
                HandleCollisions();   
            }

            _turningAngle = Mathf.Clamp(_turningAngle + _movementRotation * 0.7f, -MaxTurningAngle, MaxTurningAngle);                
            transform.rotation = Quaternion.Euler(0,0,_turningAngle);

            if (_moving)
                _body.velocity = Vector2.MoveTowards(_body.velocity, -transform.up * Drill.Speed, 0.2f);
            else
                _body.velocity = Vector2.MoveTowards(_body.velocity, Vector2.zero, 0.1f);
        
            DebugExtension.DebugArrow(transform.position, _body.velocity, Color.red);
        }
    }
}
