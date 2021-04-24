using UnityEngine;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        public Transform Target;

        private Vector3 _position;
        
        private void LateUpdate()
        {
            if (!Target) return;
            
            _position = Target.position;
            _position.x = 0;
            
            transform.position = Vector3.MoveTowards(transform.position, _position, 0.5f);
        }
    }
}
