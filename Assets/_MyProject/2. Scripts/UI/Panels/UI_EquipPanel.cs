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
        // �÷��̾ �κ��丮 �������� ���� �� ������ �Լ�
        PlayerEquipManager.Instance.OnEquipItem += SetItemToSlot;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick += SetItemToSlot;
        SetItemToSlot();
    }

    /// <summary>
    /// <para>�÷��̾ ���� ���� �����ۿ� ���� ���� UI�� ����</para>
    /// ID�� �⺻������ 1 �̻��� ����. 0�̸� �������� �������� �ʾҴٴ� ��
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
            // ��� �������� ���� ���            
            slot.Initialize();
        }
    }

    
}
