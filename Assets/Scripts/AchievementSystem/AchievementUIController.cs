using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace AchievementSystem
{
    public class AchievementUIController : MonoBehaviour
    {
        public static AchievementUIController Instance { get; private set; }

        public RectTransform Achievement;
        public Text Title;
        public Text Description;
        public Image AchievementImage;

        [ContextMenu("Test")]
        public void TestShow()
        {
            Show("Teste", "O Matheus é um Corno", "");
        }
        
        public void Show(string title, string description, string imagePath)
        {
            Title.text = title;
            Description.text = description;

            AchievementImage.sprite = Resources.Load<Sprite>(imagePath);

            Sequence showSequence = DOTween.Sequence();
            showSequence.Append(Achievement.DOAnchorPos(Vector2.zero, 1));
            showSequence.AppendInterval(2);
            showSequence.Append(Achievement.DOAnchorPos(Vector2.up * 32f, 1));
            showSequence.Play();
        }
        
        private void Awake()
        {
            Instance = this;
            
            DontDestroyOnLoad(this.gameObject);
        }
    }
}