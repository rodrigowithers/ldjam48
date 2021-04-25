using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AlwaysKeepScrollViewAtBottom : MonoBehaviour
    {
        private ScrollRect _scrollView;

        private void Start()
        {
            _scrollView = GetComponent<ScrollRect>();
        }

        private void Update()
        {
            _scrollView.verticalNormalizedPosition = 0;
        }
    }
}
