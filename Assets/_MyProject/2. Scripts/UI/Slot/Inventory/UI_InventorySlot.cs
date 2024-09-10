using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_InventorySlot : UI_ItemSlot
{    

    /// <summary>
    /// ���� ������ ������ ���� �ؽ�Ʈ
    /// </summary>
    public TMP_Text itemQuantityText;    
    /// <summary>
    /// ���� ���Կ� �ִ� �������� ����(���, �Һ�, ��Ÿ)
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
    /// <para>����Ŭ������ �� ��� �������̸� ����</para>
    /// <para>��Ÿ �������̳� ���Կ� �ƹ��͵� ������ return</para>
    /// TODO : �Һ� �������� ��쵵 �Ŀ� ����.
    /// </summary>
    protected override void OnMouseDoubleClick()
    {
        if (this.itemID == 0)
        {
            return;
        }
        Debug.Log("����Ŭ���ߴ�.");
        Debug.Log($"���� ������ ID : {this.itemID}");
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
    /// <para>��� ����</para> 
    /// ��� �����ϸ� �κ��丮���� ������Բ�.
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
    /// �������� ���� ���� �ʱ�ȭ
    /// </summary>
    public void SlotClear()
    {        
        this.itemID = 0;        
        this.itemImage.sprite = null;
        this.itemImage.ImageTransparent(0);
        this.itemQuantityText.gameObject.SetActive(false);
    }
}
