using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_ShopOrCraftItemPrefab : MonoBehaviour
{    
    public Image itemIcon;
    [Header("������ �̸�")]
    public TMP_Text itemNameText;
    
    [Header("������ ������ ���� ��ư")]
    public Button ItemInfoButton;    
    

    private void Awake()
    {
        Debug.Log("����?");
        ItemInfoButton.onClick.AddListener(OnClickItemInfoButton);
    }
    protected void Initialize(ItemData item)
    {
        ItemData itemInfo = ItemManager.Instance.GetItemData(item.Item_ID);        
        itemIcon.sprite = Resources.Load<Sprite>($"Items/Icon/{itemInfo.Item_Name}");
        itemNameText.text = itemInfo.Item_Name;        
    }

    /// <summary>
    /// �� ������(������Ʈ)�� Ŭ������ �� �� ������ ������ �ϱ�
    /// </summary>
    protected virtual void OnClickItemInfoButton()
    {
        // override
    }        
}
