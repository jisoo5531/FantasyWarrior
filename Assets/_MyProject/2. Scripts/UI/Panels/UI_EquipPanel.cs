using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EquipPanel : MonoBehaviour
{
    // TODO : 플레이어가 아이템 장착 시 UI 반영
    // 인벤토리에서 더블 클릭 시, 장착하면서 인벤토리에 -> 장착 아이템으로
    // 
    public UI_EquipSlot HeadArmorSlot;    
    public UI_EquipSlot BodyArmorSlot;    
    public UI_EquipSlot GloveSlot;    
    public UI_EquipSlot BootsSlot;    
    public UI_EquipSlot WeaponSlot;    
    public UI_EquipSlot PendantSlot;    
    public UI_EquipSlot RingSlot;

    /// <summary>
    /// <para>플레이어가 장착 중인 아이템에 맞춰 슬롯 UI에 세팅</para>
    /// ID는 기본적으로 1 이상의 정수. 0이면 아이템을 장착하지 않았다는 것
    /// </summary>
    private void SetItemToSlot()
    {
        PlayerEquipData playerEquipData = PlayerEquipManager.Instance.GetPlayerEquipFromDB();


        if (playerEquipData.HeadItem_ID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(playerEquipData.HeadItem_ID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
        }
        
    }        
}
