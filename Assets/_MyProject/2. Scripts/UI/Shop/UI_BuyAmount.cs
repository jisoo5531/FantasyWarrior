using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuyAmount : UI_BuyOrSellAmount
{
    public Button buyButton;

    private NPC_Shop_Item_Data shopItem;
    
    private int userID;

    protected override void ButtonInitialize()
    {
        base.ButtonInitialize();
        buyButton.onClick.AddListener(OnClickBuyButton);
    }
    /// <summary>
    /// 사고자 할 때의 UI 세팅
    /// </summary>
    /// <param name="shopItemID"></param>
    public void SetUI_Buy(int userId, int shopItemID)
    {
        this.userID = userId;
        itemQuantity = 0;
        NPC_Shop_Item_Data shopItem = ShopManager.Instance.GetShopItem(shopItemID);
        this.shopItem = shopItem;
        ItemData itemData = ItemManager.Instance.GetItemData(shopItem.Item_ID);
        Initialize(itemData);

        List<InventoryData> inventoryList = InventoryManager.Instance.inventoryDataList;
        InventoryData invenItemData = inventoryList.Find(x => x.Item_ID == itemData.Item_ID);

        UpdateQuantityText();
        UpdateItemGoldText();
    }

    protected override void OnClickAmountDownButton()
    {
        base.OnClickAmountDownButton();
        UpdateItemGoldText();
        UpdateQuantityText();
    }
    protected override void OnClickAmountUpButton()
    {
        base.OnClickAmountUpButton();
        UpdateItemGoldText();
        UpdateQuantityText();
    }
    protected override void OnValueChangedInputQuantity(string value)
    {
        base.OnValueChangedInputQuantity(value);
        UpdateItemGoldText();
        UpdateQuantityText();
    }
    protected override void UpdateItemGoldText()
    {
        itemGoldText.text = (shopItem.Price * itemQuantity).ToString();
    }
    /// <summary>
    /// 구매 버튼을 누르면 해당 수량만큼 아이템 구매
    /// </summary>
    private void OnClickBuyButton()
    {
        Action buySuccess = PanelManager.Instance.ShopPanel.OnSuccessBuySell;
        Action sellSuccess = PanelManager.Instance.ShopPanel.OnFailureBuy;
        buySuccess += SuccessBuy;        
        ShopManager.Instance.BuyItem(userID, this.shopItem.NPC_Shop_Item_ID, itemQuantity, buySuccess, sellSuccess);
    }    
    private void SuccessBuy()
    {
        gameObject.SetActive(false);
    }
}
