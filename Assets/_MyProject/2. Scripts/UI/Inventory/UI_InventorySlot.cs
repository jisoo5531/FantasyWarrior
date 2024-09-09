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
    /// 슬롯 아이템 이미지
    /// </summary>
    public Image itemIcon;
    /// <summary>
    /// 슬롯 아이템 수량을 위한 텍스트
    /// </summary>
    public TMP_Text itemQuantityText;
    /// <summary>
    /// 현재 인벤토리 슬롯에 있는 아이템의 ID
    /// </summary>
    public int itemID = 0;
    /// <summary>
    /// 현재 슬롯에 있는 아이템의 종류(장비, 소비, 기타)
    /// </summary>
    private Item_Type Item_Type;
    /// <summary>
    /// 더블 클릭 확인을 위한 변수
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
    /// <para>더블클릭했을 때 장비 아이템이면 장착</para>
    /// <para>기타 아이템이나 슬롯에 아무것도 없으면 return</para>
    /// TODO : 소비 아이템의 경우도 후에 구현.
    /// </summary>
    private void OnMouseDoubleClick()
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
    /// <para>IPointerEnterHandler 인터페이스</para>
    /// <para>슬롯에 커서를 대면 아이템의 정보가 나오게끔</para>
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
    /// <para>IPointerExitHandler 인터페이스</para>
    /// <para>슬롯에 커서를 떼면 아이템의 정보가 사라지게끔</para>
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
    /// <para>장비 장착</para> 
    /// 장비를 장착하면 인벤토리에서 사라지게끔.
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
    /// 아이템이 없는 슬롯 초기화
    /// </summary>
    public void SlotClear()
    {        
        this.itemID = 0;        
        this.itemIcon.sprite = null;
        this.itemIcon.ImageTransparent(0);
        this.itemQuantityText.gameObject.SetActive(false);
    }
}
