using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    /// <summary>
    /// 슬롯 아이템 이미지
    /// </summary>
    public Image itemImage;
    public UI_ItemInfo itemInfoWindow;
    /// <summary>
    /// 현재 인벤토리 슬롯에 있는 아이템의 ID
    /// </summary>
    public int itemID = 0;
    /// <summary>
    /// 더블 클릭 확인을 위한 변수
    /// </summary>
    protected float clickTime = 0;

    protected int userId;

    public virtual void Initialize(int userId, int itemID, Sprite sprite = null, UI_ItemInfo itemInfo = null)
    {
        this.userId = userId;
        this.itemID = itemID;
        this.itemImage.sprite = sprite;
        this.itemInfoWindow = itemInfo;        
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
    /// <para>더블클릭했을 때 장비 아이템이면 장착, 해제</para>
    /// <para>기타 아이템이나 슬롯에 아무것도 없으면 return</para>
    /// TODO : 소비 아이템의 경우도 후에 구현.
    /// </summary>
    protected virtual void OnMouseDoubleClick()
    {
        // override
    }
    /// <summary>
    /// <para>IPointerEnterHandler 인터페이스</para>
    /// <para>슬롯에 커서를 대면 아이템의 정보가 나오게끔</para>
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInfoWindow == null || itemID == 0)
        {
            return;
        }

        itemInfoWindow.gameObject.SetActive(true);
        itemInfoWindow.Initialize(this.userId, this.itemID);
    }
    /// <summary>
    /// <para>IPointerExitHandler 인터페이스</para>
    /// <para>슬롯에 커서를 떼면 아이템의 정보가 사라지게끔</para>
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemInfoWindow == null || itemID == 0)
        {
            return;
        }
        itemInfoWindow.gameObject.SetActive(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (this.itemID == 0)
        {
            return;
        }
        itemInfoWindow.gameObject.GetComponent<RectTransform>().position = eventData.position + new Vector2(250, -50);
    } 
}
