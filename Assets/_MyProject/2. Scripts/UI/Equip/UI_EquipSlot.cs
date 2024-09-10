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
    /// 현재 장착 슬롯에 있는 아이템의 ID
    /// </summary>
    private int itemID;
    /// <summary>
    /// 더블 클릭 확인을 위한 변수
    /// </summary>
    private float clickTime;   

    /// <summary>
    /// <para>슬롯 초기화</para>
    /// 만약 아이템의 ID가 0이라면 장착하지 않은 빈 슬롯.
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
    /// <para>IPointerEnterHandler 인터페이스</para>
    /// <para>슬롯에 커서를 대면 아이템의 정보가 나오게끔</para>
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
    /// <para>IPointerExitHandler 인터페이스</para>
    /// <para>슬롯에 커서를 떼면 아이템의 정보가 사라지게끔</para>
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
    /// 더블클릭했을 때, 장비 해제. 없으면 return
    /// </summary>
    private void OnMouseDoubleClick()
    {
        if (this.itemID == 0)
        {
            return;
        }
        Debug.Log("더블클릭. 아이템 해제");
        Debug.Log($"현재 아이템 ID : {itemID}");
        EquipItemData equipItem = ItemManager.Instance.GetEquipItemFromDB(itemID);
        string part = PlayerEquipManager.EquipParts[(int)equipItem.Equip_Type];

        PlayerEquipManager.Instance.UnEquip(part, this.itemID);

        itemInfoWindow.gameObject.SetActive(false);
        this.itemImage.ImageTransparent(0);
        this.itemID = 0;
    }
}
