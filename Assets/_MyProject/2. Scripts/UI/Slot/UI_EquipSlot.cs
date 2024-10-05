using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_EquipSlot : UI_ItemSlot
{

    /// <summary>
    /// <para>슬롯 초기화</para>
    /// 만약 아이템의 ID가 0이라면 장착하지 않은 빈 슬롯.
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="sprite"></param>
    public override void Initialize(int userId, int itemID = 0, Sprite sprite = null, UI_ItemInfo itemInfo = null)
    {
        base.Initialize(userId, itemID, sprite, itemInfo);        
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
    /// <para>IPointerEnterHandler 인터페이스</para>
    /// <para>슬롯에 커서를 대면 아이템의 정보가 나오게끔</para>
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInfoWindow != null && itemID != 0)
        {            
            itemInfoWindow.gameObject.SetActive(true);
            itemInfoWindow.Initialize(this.userId, this.itemID);
        }   
    }

    /// <summary>
    /// 더블클릭했을 때, 장비 해제. 없으면 return
    /// </summary>
    protected override void OnMouseDoubleClick()
    {
        if (this.itemID == 0)
        {
            return;
        }                
        EquipItemData equipItem = ItemManager.Instance.GetEquipItemData(this.itemID);

        string part = PlayerEquipManager.Instance.EquipParts[(int)equipItem.Equip_Type];

        PlayerEquipManager.Instance.UnEquip(part, this.itemID);

        itemInfoWindow.gameObject.SetActive(false);
        this.itemImage.ImageTransparent(0);
        this.itemID = 0;
    }
}
