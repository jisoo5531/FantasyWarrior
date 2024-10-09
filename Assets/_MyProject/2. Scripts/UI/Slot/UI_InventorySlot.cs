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

    private int item_ID;
    /// <summary>
    /// ���� ���Կ� �ִ� �������� ����(���, �Һ�, ��Ÿ)
    /// </summary>
    private Item_Type? item_Type;
    private int? itemQuantity;


    public override void Initialize(int itemID, Sprite sprite = null, UI_ItemInfo itemInfo = null)
    {
        this.item_ID = itemID;
        base.Initialize(itemID, sprite, itemInfo);
        this.item_Type = ItemManager.Instance.GetInventoryItemTypeFromDB(itemID);
        itemQuantity = InventoryManager.Instance.GetItemQuantity(itemID);
        if (itemQuantity == null)
        {
            Debug.Log("�߸��� ID");
            return;
        }
        itemImage.ImageTransparent(1);
        itemQuantityText.text = itemQuantity.ToString();
        itemQuantityText.gameObject.SetActive(true);
    }
    private void Start()
    {
        InventoryManager.Instance.OnSubtractItem += UpdateQuantityText;
        InventoryManager.Instance.OnDeleteItem += EventSlotClear;
    }

    private void OnDestroy()
    {
        InventoryManager.Instance.OnSubtractItem -= UpdateQuantityText;
        InventoryManager.Instance.OnDeleteItem -= EventSlotClear;
    }
    private void UpdateQuantityText(ItemData item)
    {
        if (item_ID == 0 || this.item_ID != item.Item_ID)
        {
            return;
        }        
        itemQuantityText.text = InventoryManager.Instance.GetInventoryItem(item.Item_ID).Quantity.ToString();
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
        EquipItemData equipItem = ItemManager.Instance.GetEquipItemData(this.itemID);   // ���� ���Կ� �ִ� ������

        PlayerEquipManager.Instance.EquipItem(PlayerEquipManager.Instance.EquipParts[(int)equipItem.Equip_Type], itemID);

        InventoryManager.Instance.EquipItemUpdateInventory(itemID);

        itemInfoWindow.gameObject.SetActive(false);

        SlotClear();

        SoundManager.Instance.PlaySound("EquipItemSkill");
    }
    public void EventSlotClear(ItemData item)
    {
        if (item.Item_ID == this.itemID)
        {
            this.itemID = 0;
            this.itemImage.sprite = null;
            this.itemImage.ImageTransparent(0);
            this.itemQuantityText.gameObject.SetActive(false);
        }        
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
