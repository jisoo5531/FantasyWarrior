using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UI_InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{    

    private UI_ItemInfo equipitemInfoWindow;

    /// <summary>
    /// ���� ������ �̹���
    /// </summary>
    public Image itemIcon;
    /// <summary>
    /// ���� ������ ������ ���� �ؽ�Ʈ
    /// </summary>
    public TMP_Text itemQuantityText;
    /// <summary>
    /// ���� �κ��丮 ���Կ� �ִ� �������� ID
    /// </summary>
    public int itemID = 0;
    /// <summary>
    /// ���� ���Կ� �ִ� �������� ����(���, �Һ�, ��Ÿ)
    /// </summary>
    private Item_Type Item_Type;
    /// <summary>
    /// ���� Ŭ�� Ȯ���� ���� ����
    /// </summary>
    private float clickTime = 0;

    public void Initialize(int itemID, Item_Type item_Type, Sprite sprite, int quantity, UI_ItemInfo itemInfo = null)
    {        
        this.itemID = itemID;
        this.Item_Type = item_Type;
        this.itemIcon.sprite = sprite;
        this.equipitemInfoWindow = itemInfo;
        itemQuantityText.text = quantity.ToString();
        itemIcon.ImageTransparent(1);
        itemQuantityText.gameObject.SetActive(true);        
    }

    /// <summary>
    /// IPointerClickHandler �������̽�
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
    /// <para>����Ŭ������ �� ��� �������̸� ����</para>
    /// <para>��Ÿ �������̳� ���Կ� �ƹ��͵� ������ return</para>
    /// TODO : �Һ� �������� ��쵵 �Ŀ� ����.
    /// </summary>
    private void OnMouseDoubleClick()
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
    /// <para>IPointerEnterHandler �������̽�</para>
    /// <para>���Կ� Ŀ���� ��� �������� ������ �����Բ�</para>
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (equipitemInfoWindow == null || itemID == 0)
        {
            return;
        }
        switch (Item_Type)
        {
            case Item_Type.Equipment:
                equipitemInfoWindow.gameObject.SetActive(true);
                equipitemInfoWindow.Initialize(this.itemID);
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
    /// <para>IPointerExitHandler �������̽�</para>
    /// <para>���Կ� Ŀ���� ���� �������� ������ ������Բ�</para>
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (equipitemInfoWindow == null || itemID == 0)
        {
            return;
            
        }
        switch (Item_Type)
        {
            case Item_Type.Equipment:
                equipitemInfoWindow.gameObject.SetActive(false);
                break;
            case Item_Type.Consump:
                break;
            case Item_Type.Other:
                break;
            default:
                break;
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (this.itemID == 0)
        {
            return;
        }
        switch (Item_Type)
        {
            case Item_Type.Equipment:
                equipitemInfoWindow.gameObject.GetComponent<RectTransform>().position = eventData.position + new Vector2(250, -250);
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

        equipitemInfoWindow.gameObject.SetActive(false);

        SlotClear();
    }   
    /// <summary>
    /// �������� ���� ���� �ʱ�ȭ
    /// </summary>
    public void SlotClear()
    {        
        this.itemID = 0;        
        this.itemIcon.sprite = null;
        this.itemIcon.ImageTransparent(0);
        this.itemQuantityText.gameObject.SetActive(false);
    }
}
