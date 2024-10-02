using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ShopItemBuyPrefab : UI_ShopOrCraftItemPrefab
{
    [Header("아이템 가격")]
    public TMP_Text itemPriceText;

    private NPC_Shop_Item_Data shopItem;
    private ItemData itemData;
    
    /// <summary>
    /// 해당 상점이 팔 수 있는 아이템 리스트 나열하기
    /// </summary>
    /// <param name="item"></param>
    public void SetBuyShopItem(NPC_Shop_Item_Data item)
    {
        this.shopItem = item;
        ItemData itemInfo = ItemManager.Instance.GetItemData(item.Item_ID);
        this.itemData = itemInfo;
        itemPriceText.text = item.Price.ToString();
        Initialize(ItemManager.Instance.GetItemData(item.Item_ID));
    }

    protected override void OnClickItemInfoButton()
    {
        UI_ShopPanel shopPanel = FindObjectOfType<PanelManager>(true).ShopPanel;
        shopPanel.UI_BuyAmount.SetUI_Buy(this.shopItem.NPC_Shop_Item_ID);
        shopPanel.UI_BuyAmount.gameObject.SetActive(true);
    }
}
