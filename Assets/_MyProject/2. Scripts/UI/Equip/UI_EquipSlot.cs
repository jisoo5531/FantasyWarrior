using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public Image itemImage;
    private UI_ItemInfo itemInfoWindow;
   
    /// <summary>
    /// ���� ���� ���Կ� �ִ� �������� ID
    /// </summary>
    private int itemID;
    /// <summary>
    /// ���� Ŭ�� Ȯ���� ���� ����
    /// </summary>
    private float clickTime;   

    /// <summary>
    /// <para>���� �ʱ�ȭ</para>
    /// ���� �������� ID�� 0�̶�� �������� ���� �� ����.
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="sprite"></param>
    public void Initialize(int itemID = 0, Sprite sprite = null, UI_EquipItemInfo itemInfo = null)
    {
        this.itemID = itemID;        
        if (itemID == 0)
        {            
            this.itemImage.ImageTransparent(0);
        }
        else
        {
            this.itemInfoWindow = itemInfo;
            this.itemImage.sprite = sprite;
            this.itemImage.ImageTransparent(1);
        }
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
    /// <para>IPointerEnterHandler �������̽�</para>
    /// <para>���Կ� Ŀ���� ��� �������� ������ �����Բ�</para>
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInfoWindow != null && itemID != 0)
        {
            Item_Type item_Type = ItemManager.Instance.GetInventoryItemTypeFromDB(itemID);

            itemInfoWindow.gameObject.SetActive(true);
            itemInfoWindow.Initialize(this.itemID);

            
        }   
    }
    /// <summary>
    /// <para>IPointerExitHandler �������̽�</para>
    /// <para>���Կ� Ŀ���� ���� �������� ������ ������Բ�</para>
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemInfoWindow != null && itemID != 0)
        {
            itemInfoWindow.gameObject.SetActive(false);            
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (this.itemID == 0)
        {
            return;
        }
        itemInfoWindow.gameObject.GetComponent<RectTransform>().position = eventData.position + new Vector2(250, -50);
    }

    /// <summary>
    /// ����Ŭ������ ��, ��� ����. ������ return
    /// </summary>
    private void OnMouseDoubleClick()
    {
        if (this.itemID == 0)
        {
            return;
        }
        Debug.Log("����Ŭ��. ������ ����");
        Debug.Log($"���� ������ ID : {itemID}");
        EquipItemData equipItem = ItemManager.Instance.GetEquipItemFromDB(itemID);
        string part = PlayerEquipManager.EquipParts[(int)equipItem.Equip_Type];

        PlayerEquipManager.Instance.UnEquip(part, this.itemID);

        itemInfoWindow.gameObject.SetActive(false);
        this.itemImage.ImageTransparent(0);
        this.itemID = 0;
    }
}
