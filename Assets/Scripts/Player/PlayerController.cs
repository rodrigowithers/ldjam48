using Tile;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _body;
        [SerializeField] private float _speed = 1;
    
        [SerializeField] private float _drillRadius = 1;
        [SerializeField] private float _drillOffset = 1;

        [SerializeField] private LayerMask _tileLayer;
    
        private Vector2 DrillPoint => transform.position - transform.up * _drillOffset;
    
        private float _movementRotation;
        private bool _throttle;

        private Collider2D[] _hits;

        public Drill Drill;
        
        private void HandleInput()
        {
            _movementRotation = Input.GetAxisRaw("Horizontal");
            _throttle = Input.GetKey(KeyCode.Space);
        }

        private void HandleCollisions()
        {
            if (Drill.Health <= 0)
                return;
            
            DebugExtension.DebugCircle(DrillPoint, Vector3.forward, Color.red, _drillRadius);
            
            var hits = Physics2D.OverlapCircleNonAlloc(DrillPoint, _drillRadius, _hits, _tileLayer);

            for (int i = 0; i < hits; i++)
            {
                if (_hits[i].GetComponent<BreakableTile>().Damage(Drill.Strength))
                {
                    Drill.Health--;
                }
            }
        }

        private void Start()
        {
            _hits = new Collider2D[9];
            Drill = new Drill
            {
                TotalHealth = 50,
                Health = 50,
                Strength = 5
            };
        }

        private void Update()
        {
            Shader.SetGlobalVector("_PlayerPosition", transform.position);
            
            HandleInput();
            HandleCollisions();

            transform.Rotate(Vector3.forward, _movementRotation);
        
            if (_throttle)
                _body.velocity = -transform.up * _speed;
            else
                _body.velocity = Vector2.zero;
        
            DebugExtension.DebugArrow(transform.position, -transform.up, Color.red);
        }
    }
}
