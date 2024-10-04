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
        // 로컬 플레이어 체크
        NetworkIdentity networkIdentity = transform.root.GetComponent<NetworkIdentity>();
        if (networkIdentity == null || !networkIdentity.isLocalPlayer)
        {
            return; // 로컬 플레이어가 아닐 경우 UI를 실행하지 않음
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
    /// <para>플레이어가 장착 중인 아이템에 맞춰 슬롯 UI에 세팅</para>
    /// ID는 기본적으로 1 이상의 정수. 0이면 아이템을 장착하지 않았다는 것
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
            // 장비를 장착하지 않은 경우            
            slot.Initialize();
        }
    }

    private void OnClickUnEquipAll()
    {
        PlayerEquipManager.Instance.UnEquipAll(this.userId);
    }
}
