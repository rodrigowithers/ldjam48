using System;
using TMPro;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UI
{
    public class Logger : MonoBehaviour
    {
        public static Logger Instance { get; private set; }
        
        [SerializeField] private TMP_Text _text;

        private Queue<string> _log = new Queue<string>();
        
        public void Log(string s)
        {
            _log.Enqueue(s + "\n");
        }

        private async void UpdateLog()
        {
            while (true)
            {
                if (_log.Count > 0)
                {
                    _text.DOText(_text.text + _log.Dequeue(), 0.15f);
                }   
                
                await Task.Delay(150);
            }
        }

        private void Clear()
        {
            _text.text = String.Empty;
        }
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            GameController.Instance.OnShopState += Clear;
            
            UpdateLog();
        }
    }
}