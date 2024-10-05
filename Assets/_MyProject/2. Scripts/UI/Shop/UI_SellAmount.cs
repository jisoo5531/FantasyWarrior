using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SellAmount : UI_BuyOrSellAmount
{
    public Button sellButton;

    private InventoryData invenItem;    
    private ItemData Item;

    protected override void ButtonInitialize()
    {
        base.ButtonInitialize();
        sellButton.onClick.AddListener(OnClickSellButton);
    }
    public void SetUI_Sell(int userId, ItemData item)
    {                
        this.Item = item;

        GameManager.Instance.invenManger[userId].GetInventoryItem(item.Item_ID);        
        Initialize(userId, item);
        itemGoldText.text = item.SellPrice.ToString();
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
        if (itemQuantity > invenItem.Quantity)
        {
            itemQuantity = invenItem.Quantity;
        }
        UpdateItemGoldText();
        UpdateQuantityText();
    }
    protected override void OnValueChangedInputQuantity(string value)
    {
        base.OnValueChangedInputQuantity(value);
        if (itemQuantity > invenItem.Quantity)
        {
            itemQuantity = invenItem.Quantity;
        }
        UpdateItemGoldText();
        UpdateQuantityText();
    }
    protected override void UpdateItemGoldText()
    {
        itemGoldText.text = (Item.SellPrice * itemQuantity).ToString();
    }
    private void OnClickSellButton()
    {
        ShopManager.Instance.SellItem(this.Item, itemQuantity, OnSuccessSell);
    }
    private void OnSuccessSell()
    {
        gameObject.SetActive(false);
        PanelManager.Instance.ShopPanel.OnSuccessBuySell();
    }
}
