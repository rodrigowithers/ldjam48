using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Logger : MonoBehaviour
    {
        public static Logger Instance { get; private set; }
        
        [SerializeField] private TMP_Text _text;

        public void Log(string s)
        {
            _text.DOText(_text.text + s + "\n", 0.25f);
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}