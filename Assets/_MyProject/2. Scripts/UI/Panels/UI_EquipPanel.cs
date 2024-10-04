using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class UI_EquipPanel : MonoBehaviour
{

    [Header("Slot")]
    public List<UI_EquipSlot> EquipItemParts;

    [Header("Item Info")]
    public UI_EquipItemInfo EquipItemInfo;

    public Button unEquipAllButton;

    private Dictionary<string, int> userEquip;

    private int userId;

    private void Start()
    {
        // ���� �÷��̾� üũ
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // ���� �÷��̾ �ƴ� ��� UI�� �������� ����
        }

        this.userId = transform.root.GetComponent<PlayerController>().userID;
        Debug.LogError($"{userId}");
        unEquipAllButton.onClick.AddListener(OnClickUnEquipAll);
        PlayerEquipManager.Instance.OnEquipItem += SetItemToSlot;
        PlayerEquipManager.Instance.OnAllUnEquipButtonClick += SetItemToSlot;
        SetItemToSlot(userId);
    }

    public void Initialize()
    {        
        
    }

    /// <summary>
    /// <para>�÷��̾ ���� ���� �����ۿ� ���� ���� UI�� ����</para>
    /// ID�� �⺻������ 1 �̻��� ����. 0�̸� �������� �������� �ʾҴٴ� ��
    /// </summary>
    private void SetItemToSlot(int userid)
    {
        if (userId != userid)
        {
            return;
        }
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

    private void OnClickUnEquipAll()
    {
        PlayerEquipManager.Instance.UnEquipAll(this.userId);
    }
}
