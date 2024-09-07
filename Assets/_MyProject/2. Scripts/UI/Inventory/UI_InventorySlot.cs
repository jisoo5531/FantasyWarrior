using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_InventorySlot : MonoBehaviour, IPointerClickHandler
{
    private readonly List<string> EquipParts = new List<string>
    {
        "HeadItem_ID", "ArmorItem_ID", "GlovesItem_ID",
        "BootsItem_ID", "WeaponItem_ID", "PendantItem_ID", "RingItem_ID"
    };

    /// <summary>
    /// 슬롯 아이템 이미지
    /// </summary>
    public Image itemIcon;
    /// <summary>
    /// 슬롯 아이템 수량을 위한 텍스트
    /// </summary>
    public TMP_Text itemQuantityText;
    /// <summary>
    /// 현재 슬롯에 있는 아이템의 ID
    /// </summary>
    private int itemID;
    /// <summary>
    /// 현재 슬롯에 있는 아이템의 종류(장비, 소비, 기타)
    /// </summary>
    private Item_Type Item_Type;
    /// <summary>
    /// 더블 클릭 확인을 위한 변수
    /// </summary>
    private float clickTime = 0;

    public void Initialize(int itemID, Item_Type item_Type, Sprite sprite, int quantity)
    {
        this.itemID = itemID;
        this.Item_Type = item_Type;
        this.itemIcon.sprite = sprite;
        itemQuantityText.text = quantity.ToString();
        itemIcon.ImageTransparent(1);
        itemQuantityText.gameObject.SetActive(true);
    }

    /// <summary>
    /// IPointerClickHandler 인터페이스
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Time.time - clickTime) < 0.3f)
        {
            OnMouseDoubleClick();
            clickTime = -1;
        }
        else
        {
            clickTime = Time.time;
        }
    }
    /// <summary>
    /// 더블클릭했을 때 장비 아이템이면 장착
    /// </summary>
    private void OnMouseDoubleClick()
    {
        Debug.Log("더블클릭했다.");
        switch (Item_Type)
        {
            case Item_Type.Equipment:
                Equip_EquipItem();
                break;
            case Item_Type.Consump:
                break;
            case Item_Type.Other:
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 장비 장착
    /// </summary>
    private void Equip_EquipItem()
    {
        EquipItemData equipItem = ItemManager.Instance.GetEquipItemFromDB(itemID);

        PlayerEquipManager.Instance.EquipItem(EquipParts[(int)equipItem.Equip_Type], itemID);
    }    
}
