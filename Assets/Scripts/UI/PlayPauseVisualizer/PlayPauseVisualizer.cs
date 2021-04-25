using Player;
using UnityEngine;

namespace UI.PlayPauseVisualizer
{
    public class PlayPauseVisualizer : MonoBehaviour
    {
        public PlayerController PlayerController;
        private Animator _animator;

        private int _state;
        
        /// <summary>
        /// 0 - Paused
        /// 1 - Playing
        /// </summary>
        public void ChangeState(int state)
        {
            if(state == 0)
            {
                _animator.Play("PlayingToPaused", 0);
                _state = 0;
            }
            
            if(state == 1)
            {
                _animator.Play("PausedToPlaying", 0);
                _state = 1;
            }
        }
        
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            PlayerController.OnPaused += () => ChangeState(0);
            PlayerController.OnResumed += () => ChangeState(1);
        }
    }
}