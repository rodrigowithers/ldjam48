using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ShopItem
{
    public int Id;
    
    public int StonePrice;
    public int IronPrice;
    public int GoldPrice;
    
    public Sprite Sprite;
}

[CreateAssetMenu(menuName = "Shop/CreateShop")]
public class ShopResource : ScriptableObject
{
    public List<ShopItem> ShopItems;
}
