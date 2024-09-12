using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EquipPanel : MonoBehaviour
{

    [Header("Slot")]
    public List<UI_EquipSlot> EquipItemParts;

    [Header("Item Info")]
    public UI_EquipItemInfo EquipItemInfo;

    private Dictionary<string, int> userEquip;

    private void Start()
    {
        // 플레이어가 인벤토리 아이템을 장착 시 실행할 함수
        PlayerEquipManager.Instance.OnEquipItem += SetItemToSlot;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick += SetItemToSlot;
        SetItemToSlot();
    }

    /// <summary>
    /// <para>플레이어가 장착 중인 아이템에 맞춰 슬롯 UI에 세팅</para>
    /// ID는 기본적으로 1 이상의 정수. 0이면 아이템을 장착하지 않았다는 것
    /// </summary>
    private void SetItemToSlot()
    {        
        userEquip = PlayerEquipManager.Instance.UserEquipTable;

        for (int i = 0; i < PlayerEquipManager.Instance.EquipParts.Count; i++)
        {
            int itemID = userEquip[PlayerEquipManager.Instance.EquipParts[i]];
            SlotsSetting(itemID, EquipItemParts[i]);
        }
    }

    private void SlotsSetting(int itemID, UI_EquipSlot slot)
    {
        if (itemID != 0)
        {
            string itemName = ItemManager.Instance.GetInventoryItemNameFromDB(itemID);
            Sprite sprite = Resources.Load<Sprite>($"Items/Icon/{itemName}");
            slot.Initialize(itemID, sprite, EquipItemInfo);
        }
        else
        {
            // 장비를 장착하지 않은 경우            
            slot.Initialize();
        }
    }

    
}
