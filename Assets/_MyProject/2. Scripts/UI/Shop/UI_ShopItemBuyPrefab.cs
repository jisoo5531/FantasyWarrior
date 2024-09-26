using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_ShopItemBuyPrefab : MonoBehaviour
{    
    public Image itemIcon;
    [Header("������ �̸�")]
    public TMP_Text itemNameText;
    [Header("������ ����")]
    public TMP_Text itemPriceText;
    [Header("������ ������ ���� ��ư")]
    public Button ItemInfoButton;

    private NPC_Shop_Item_Data shopItem;
    private ItemData itemData;

    private void Awake()
    {
        Debug.Log("����?");
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
    /// �� ������(������Ʈ)�� Ŭ������ �� �� ������ ������ �ϱ�
    /// </summary>
    private void OnClickItemInfoButton()
    {
        // TODO : �ӽ� ������ ���        
        PanelManager.Instance.ShopPanel.UI_BuyAmount.SetUI_Buy(this.shopItem.NPC_Shop_Item_ID);
        PanelManager.Instance.ShopPanel.UI_BuyAmount.gameObject.SetActive(true);
        //ShopManager.Instance.BuyItem(this.shopItem.NPC_Shop_Item_ID, successBuy, failureBuy);
        Debug.Log("���� ������ ������ ������.");
    }        
}
