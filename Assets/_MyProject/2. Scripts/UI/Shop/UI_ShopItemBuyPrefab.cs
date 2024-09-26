using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_ShopItemBuyPrefab : MonoBehaviour
{    
    public Image itemIcon;
    [Header("아이템 이름")]
    public TMP_Text itemNameText;
    [Header("아이템 가격")]
    public TMP_Text itemPriceText;
    [Header("아이템 상세정보 열람 버튼")]
    public Button ItemInfoButton;

    private NPC_Shop_Item_Data shopItem;
    private ItemData itemData;

    private void Awake()
    {
        Debug.Log("되지?");
        ItemInfoButton.onClick.AddListener(OnClickItemInfoButton);
    }
    public void Initialize(NPC_Shop_Item_Data item)
    {
        this.shopItem = item;

        ItemData itemInfo = ItemManager.Instance.GetItemData(item.Item_ID);
        this.itemData = itemInfo;
        itemIcon.sprite = Resources.Load<Sprite>($"Items/Icon/{itemInfo.Item_Name}");
        itemNameText.text = itemInfo.Item_Name;
        itemPriceText.text = item.Price.ToString();
    }
    /// <summary>
    /// 이 아이템(오브젝트)를 클릭했을 때 상세 정보가 나오게 하기
    /// </summary>
    private void OnClickItemInfoButton()
    {
        // TODO : 임시 아이템 사기        
        PanelManager.Instance.ShopPanel.UI_BuyAmount.SetUI_Buy(this.shopItem.NPC_Shop_Item_ID);
        PanelManager.Instance.ShopPanel.UI_BuyAmount.gameObject.SetActive(true);
        //ShopManager.Instance.BuyItem(this.shopItem.NPC_Shop_Item_ID, successBuy, failureBuy);
        Debug.Log("상점 아이템 상세정보 나오자.");
    }        
}
