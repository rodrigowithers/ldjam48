using System.Linq;
using DTO;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public class ShopPanel : MonoBehaviour
    {
        public ShopResource Shop;
        public GameObject ShopButtonPrefab;

        public Transform ButtonParent;
        
        private void InstantiateShop()
        {
            for (int i = 0; i < ButtonParent.childCount; i++)
            {
                Destroy(ButtonParent.GetChild(i).gameObject);
            }
            
            for (var i = 0; i < Shop.ShopItems.Count; i++)
            {
                var shopItem = Shop.ShopItems[i];
                GameObject item = Instantiate(ShopButtonPrefab, ButtonParent, false);

                item.transform.GetChild(0).GetComponent<Image>().sprite = shopItem.Sprite;
                item.transform.GetChild(1).GetComponent<Text>().text = 
                    $"Stone: {shopItem.StonePrice}\n" +
                    $"Iron: {shopItem.IronPrice}\n" +
                    $"Gold: {shopItem.GoldPrice}";
                
                PlayerInfo info = PlayerDatabase.Info;
                var button = item.GetComponent<Button>();
                
                if (info.StoneCount < shopItem.StonePrice || 
                    info.IronCount < shopItem.IronPrice ||
                    info.GoldCount < shopItem.GoldPrice)
                {
                    button.image.color = Color.gray;
                    button.enabled = false;
                }
                else
                {
                    button.onClick.AddListener(() => Buy(shopItem.Id));
                }
            }
        }
        
        public void Buy(int id)
        {
            PlayerInfo i = PlayerDatabase.Info;
            ShopItem item = Shop.ShopItems.First(x => x.Id == id);
            
            // Check money
            if (i.StoneCount < item.StonePrice || i.IronCount < item.IronPrice || i.GoldCount < item.GoldPrice)
                return;

            i.StoneCount -= item.StonePrice;
            i.IronCount -= item.IronPrice;
            i.GoldCount -= item.GoldPrice;
            
                
            i.DrillIdentifier = id;
            PlayerDatabase.Save(i);
            ResourceStorage.Load();
            
            GameController.Instance.Player.ResetPlayer();
        }
        
        public void Play()
        {
            GameController.Instance.SetGameState(GameController.GameState.Game);
        }

        private void Start()
        {
            GameController.Instance.OnShopState += InstantiateShop;
        }
    }
}