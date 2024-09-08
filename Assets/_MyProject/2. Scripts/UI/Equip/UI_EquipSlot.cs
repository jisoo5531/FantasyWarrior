using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipSlot : MonoBehaviour, IPointerClickHandler
{
    public Image itemImage;
   
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
    public void Initialize(int itemID = 0, Sprite sprite = null)
    {
        this.itemID = itemID;
        Debug.Log(sprite == null);        
        if (itemID == 0)
        {            
            this.itemImage.ImageTransparent(0);
        }
        else
        {            
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

        this.itemImage.ImageTransparent(0);
        this.itemID = 0;
    }
}
