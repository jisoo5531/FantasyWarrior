using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_ShopSellItemInfo : UI_ItemInfo
{
    public Button SellButton;
    public TMP_Text descText;
    public TMP_Text sellPriceText;    

    private void Awake()
    {
        SellButton.onClick.AddListener(OnClickSellButton);
    }
    public override void Initialize(int itemID)
    {
        base.Initialize(itemID);
        descText.text = itemData.Item_Description;
        sellPriceText.text = itemData.SellPrice.ToString();
    }
    /// <summary>
    /// 수량 체크 UI 활성화
    /// </summary>
    private void OnClickSellButton()
    {
        UI_SellAmount UI_setAmount = PanelManager.Instance.ShopPanel.uI_SellAmount;        
        UI_setAmount.SetUI_Sell(this.itemData);
        gameObject.SetActive(false);
        UI_setAmount.gameObject.SetActive(true);
    }
}
