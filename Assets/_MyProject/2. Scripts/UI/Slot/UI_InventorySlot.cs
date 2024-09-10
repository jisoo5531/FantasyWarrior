using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_InventorySlot : UI_ItemSlot
{    

    /// <summary>
    /// 슬롯 아이템 수량을 위한 텍스트
    /// </summary>
    public TMP_Text itemQuantityText;    
    /// <summary>
    /// 현재 슬롯에 있는 아이템의 종류(장비, 소비, 기타)
    /// </summary>
    private Item_Type Item_Type;

    public override void Initialize(int itemID, Sprite sprite = null, UI_ItemInfo itemInfo = null)
    {
        base.Initialize(itemID, sprite, itemInfo);
        this.Item_Type = ItemManager.Instance.GetInventoryItemTypeFromDB(itemID);
        int quantity = InventoryManager.Instance.GetItemQuantity(itemID);
        Debug.Log($"ID:{itemID}, item_Type:{Item_Type}");
        itemImage.ImageTransparent(1);
        itemQuantityText.text = quantity.ToString();        
        itemQuantityText.gameObject.SetActive(true);        
    }

    /// <summary>
    /// <para>더블클릭했을 때 장비 아이템이면 장착</para>
    /// <para>기타 아이템이나 슬롯에 아무것도 없으면 return</para>
    /// TODO : 소비 아이템의 경우도 후에 구현.
    /// </summary>
    protected override void OnMouseDoubleClick()
    {
        if (this.itemID == 0)
        {
            return;
        }
        Debug.Log("더블클릭했다.");
        Debug.Log($"현재 아이템 ID : {this.itemID}");
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
    /// <para>장비 장착</para> 
    /// 장비를 장착하면 인벤토리에서 사라지게끔.
    /// </summary>
    private void Equip_EquipItem()
    {
        EquipItemData equipItem = ItemManager.Instance.GetEquipItemFromDB(itemID);

        PlayerEquipManager.Instance.EquipItem(PlayerEquipManager.EquipParts[(int)equipItem.Equip_Type], itemID);        

        InventoryManager.Instance.EquipItemUpdateInventory(itemID);

        itemInfoWindow.gameObject.SetActive(false);

        SlotClear();
    }   
    /// <summary>
    /// 아이템이 없는 슬롯 초기화
    /// </summary>
    public void SlotClear()
    {        
        this.itemID = 0;        
        this.itemImage.sprite = null;
        this.itemImage.ImageTransparent(0);
        this.itemQuantityText.gameObject.SetActive(false);
    }
}
