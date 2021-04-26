﻿using DTO;
using Tile;
using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Logger = UI.Logger;
using Random = UnityEngine.Random;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Required Components")]
        [SerializeField] private Rigidbody2D _body;

        [SerializeField] private Animator _animator;

        [Header("FMOD")]
        [SerializeField] private FMODUnity.StudioEventEmitter BGM;
        [SerializeField] private FMODUnity.StudioEventEmitter DrillSFX;
        [SerializeField] private FMODUnity.StudioEventEmitter RockColliderSFX;

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

        public Drill Drill;

        private float _drillParticlesCount;
        private float _turningAngle;

        private Vector2 _velocity;

        private float _depth;
        private float _drillHealth;
        
        public void ResetPlayer()
        {
            Drill = Drill.GetDrill(PlayerDatabase.Info.DrillIdentifier);
            Drill.Tier = PlayerDatabase.Info.DrillTier;
            Drill.SetHealth();

            _drillHealth = 1;
            
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
                if (!_moving)
                {
                    DrillSFX.Play();  
                    OnResumed?.Invoke();
                }
                
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
                    DrillSFX.Play();  

                    _moving = true;
                    OnResumed?.Invoke();
                }
            }

            DrillSFX.SetParameter("DRILL INPUT", _moving ? 1 : 0);
        }

        private void HandleCollisions()
        {
            if (Drill.Health <= 0 || !_moving)
            {
                RockColliderSFX.SetParameter("DRILL INPUT", 0);
                return;
            }
            
            DebugExtension.DebugCircle(DrillPoint, Vector3.forward, Color.red, _drillRadius);
            
            var hits = Physics2D.OverlapCircleNonAlloc(DrillPoint, _drillRadius, _hits, _tileLayer);
            if (hits > 0)
            {
                // Shake player
                _animator.transform.localPosition = Random.insideUnitCircle * 0.05f;
                
                if(!RockColliderSFX.IsPlaying())
                    RockColliderSFX.Play();

                RockColliderSFX.SetParameter("DRILL INPUT", 1);
            }
            else
            {
                RockColliderSFX.SetParameter("DRILL INPUT", 0);
            }
            
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
            
            RockColliderSFX.Stop();
            DrillSFX.SetParameter("DRILL INPUT", 0);

            _moving = false;
            _broken = true;
            
            StopAnimation();
            RecoilMovement();

            await Task.Delay(1000);
            
            GameController.Instance.SetGameState(GameController.GameState.Menu);
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
            _animator.transform.localPosition = Vector3.MoveTowards(_animator.transform.localPosition, Vector3.zero, 0.1f);
            AchievementSystem.AchievementHandler.SetAchievementCounter(6, (int) Mathf.Abs(transform.position.y));

            Shader.SetGlobalVector("_PlayerPosition", transform.position);
            Shader.SetGlobalFloat("_Radius", Mathf.Lerp(20, 3, Mathf.InverseLerp(7, -40, transform.position.y)));

            _depth = Mathf.MoveTowards(_depth, Mathf.InverseLerp(5, -70, transform.position.y) * 100, 0.5f);
            float drillHealth = Mathf.InverseLerp(0, Drill.TotalHealth, Drill.Health) * 100;

            _drillHealth = Mathf.MoveTowards(_drillHealth, Mathf.InverseLerp(0, Drill.TotalHealth, Drill.Health), 0.001f);
            
            BGM.SetParameter("DEPTH", _depth);
            BGM.SetParameter("Life", drillHealth);
            
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
