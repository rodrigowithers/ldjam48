using System.Linq;
using System.Threading.Tasks;
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

        public Sprite[] Tiers;
        
        private async void InstantiateShop()
        {
            for (int i = 0; i < ButtonParent.childCount; i++)
            {
                Destroy(ButtonParent.GetChild(i).gameObject);
            }
            
            for (var i = 0; i < Shop.ShopItems.Count; i++)
            {
                var shopItem = Shop.ShopItems[i];
                GameObject item = Instantiate(ShopButtonPrefab, ButtonParent, false);
                Transform tiers = item.transform.GetChild(2);

                item.transform.GetChild(0).GetComponent<Image>().sprite = shopItem.Sprite;
                item.transform.GetChild(1).GetComponent<Text>().text = 
                    $"Stone: {shopItem.StonePrice}\n" +
                    $"Iron: {shopItem.IronPrice}\n" +
                    $"Gold: {shopItem.GoldPrice}";

                PlayerInfo info = PlayerDatabase.Info;
                
                // Fill tiers from player
                if (shopItem.Id == info.DrillIdentifier)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if(info.DrillTier >= j)
                            tiers.GetChild(j).GetComponent<Image>().sprite = Tiers[1];
                    }   
                }

                var button = item.GetComponent<Button>();
                
                if (info.StoneCount < shopItem.StonePrice || 
                    info.IronCount < shopItem.IronPrice ||
                    info.GoldCount < shopItem.GoldPrice)
                {
                    button.image.color = Color.gray;
                    button.GetComponent<FMODUnity.StudioEventEmitter>().Event = "event:/UPGRADE UNAVAILABLE";
                }
                else
                {
                    button.onClick.AddListener(() => Buy(shopItem.Id));
                }

                await Task.Delay(100);
            }
        }
        
        public void Buy(int id)
        {
            PlayerInfo i = PlayerDatabase.Info;
            ShopItem item = Shop.ShopItems.First(x => x.Id == id);
            
            // Check money
            if (i.StoneCount < item.StonePrice || i.IronCount < item.IronPrice || i.GoldCount < item.GoldPrice)
                return;

            // Check if its the same drill we already have
            if (item.Id == i.DrillIdentifier)
            {
                if (i.DrillTier < 5)
                    i.DrillTier++;
                else
                    return;
            }

            if (item.Id != i.DrillIdentifier)
            {
                i.DrillTier = 0;
            }

            i.StoneCount -= item.StonePrice;
            i.IronCount -= item.IronPrice;
            i.GoldCount -= item.GoldPrice;
                
            i.DrillIdentifier = id;
            PlayerDatabase.Save(i);
            ResourceStorage.Load();
            
            GameController.Instance.Player.ResetPlayer();
            InstantiateShop();
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