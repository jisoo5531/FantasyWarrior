using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_ShopOrCraftItemPrefab : MonoBehaviour
{    
    public Image itemIcon;
    [Header("아이템 이름")]
    public TMP_Text itemNameText;
    
    [Header("아이템 상세정보 열람 버튼")]
    public Button ItemInfoButton;    
    

    private void Awake()
    {
        Debug.Log("되지?");
        ItemInfoButton.onClick.AddListener(OnClickItemInfoButton);
    }
    protected void Initialize(ItemData item)
    {
        ItemData itemInfo = ItemManager.Instance.GetItemData(item.Item_ID);        
        itemIcon.sprite = Resources.Load<Sprite>($"Items/Icon/{itemInfo.Item_Name}");
        itemNameText.text = itemInfo.Item_Name;        
    }

    /// <summary>
    /// 이 아이템(오브젝트)를 클릭했을 때 상세 정보가 나오게 하기
    /// </summary>
    protected virtual void OnClickItemInfoButton()
    {
        // override
    }        
}
