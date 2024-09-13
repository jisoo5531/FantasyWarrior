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
    private Item_Type? item_Type;

    public override void Initialize(int itemID, Sprite sprite = null, UI_ItemInfo itemInfo = null)
    {
        base.Initialize(itemID, sprite, itemInfo);
        this.item_Type = ItemManager.Instance.GetInventoryItemTypeFromDB(itemID);
        int? quantity = InventoryManager.Instance.GetItemQuantity(itemID);
        if (quantity == null)
        {
            Debug.Log("�߸��� ID");
            return;
        }        
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
        switch (item_Type)
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
        List<EquipItemData> equipItemDataList = ItemManager.Instance.equipItemList;
        EquipItemData equipItem = equipItemDataList.Find(x => x.Item_ID.Equals(this.itemID));   // ���� ���Կ� �ִ� ������

        PlayerEquipManager.Instance.EquipItem(PlayerEquipManager.Instance.EquipParts[(int)equipItem.Equip_Type], itemID);        

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
