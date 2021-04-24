using Player;
using UnityEngine;

namespace UI.PlayPauseVisualizer
{
    public class PlayPauseVisualizer : MonoBehaviour
    {
        public PlayerController PlayerController;
        private Animator _animator;
        
        /// <summary>
        /// 0 - Paused
        /// 1 - Playing
        /// </summary>
        public void ChangeState(int state)
        {
            if(state == 0) _animator.Play("PlayingToPaused", 0);
            if(state == 1) _animator.Play("PausedToPlaying", 0);
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