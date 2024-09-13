using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


public class PlayerEquipManager : MonoBehaviour
{
    public static PlayerEquipManager Instance { get; private set; }

    public readonly List<string> EquipParts = new List<string>
    {
        "HeadItem_ID", "ArmorItem_ID", "GlovesItem_ID",
        "BootsItem_ID", "WeaponItem_ID", "PendantItem_ID", "RingItem_ID"
    };
    /// <summary>
    /// 유저가 장착하고 있는 장비들을 담는 리스트
    /// <para>클라이언트에 임시로 저장해 둔다.</para>
    /// <para>데이터베이스에 저장을 해줘야 한다.</para>
    /// </summary>
    public Dictionary<string, int> UserEquipTable { get; private set; }

    [Header("장비 해제 버튼")]
    public Button unEquipButton;
    /// <summary>
    /// 플레이어가 장비를 장착했을 때, 발생하는 이벤트
    /// </summary>
    public event Action OnEquipItem;
    /// <summary>
    /// 플레이어가 장비를 해제했을 때, 발생하는 이벤트
    /// </summary>
    public event Action OnUnEquipItem;
    /// <summary>
    /// 모든 장비 해제 버튼을 눌렀을 때, 발생하는 이벤트
    /// </summary>
    public event Action OnAllUnEquipButtonClick;    

    private void Awake()
    {
        Instance = this;
        unEquipButton.onClick.AddListener(UnEquipAll);        
        EventHandler.managerEvent.RegisterStatManagerInit(Initialize);
    }
    /// <summary>
    /// UserStatManager의 초기화 이후 초기화가 된다.
    /// </summary>
    private void Initialize()
    {        
        PlayerEquipData playerEquipData = GetPlayerEquipFromDB();
        UserEquipTable = new Dictionary<string, int>
        {
            { EquipParts[0], playerEquipData.HeadItem_ID },
            { EquipParts[1], playerEquipData.ArmorItem_ID },
            { EquipParts[2], playerEquipData.GloveItem_ID },
            { EquipParts[3], playerEquipData.BootItem_ID },
            { EquipParts[4], playerEquipData.WeaponItem_ID },
            { EquipParts[5], playerEquipData.Pendant_ID },
            { EquipParts[6], playerEquipData.Ring_ID }
        };

        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, playerEquipData.HeadItem_ID);
        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, playerEquipData.ArmorItem_ID);
        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, playerEquipData.GloveItem_ID);
        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, playerEquipData.BootItem_ID);
        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, playerEquipData.WeaponItem_ID);
        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, playerEquipData.Pendant_ID);
        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, playerEquipData.Ring_ID);
        
        EventHandler.managerEvent.TriggerEquipManagerInit();
        //UserStatManager.Instance.UpdateUserStat();
    }
    private void Start()
    {
        InvokeRepeating("AutoSave", 300f, 300f);
    }

    /// <summary>
    /// 플레이어가 장착한 장비 목록 가져오기
    /// </summary>
    private PlayerEquipData GetPlayerEquipFromDB()
    {
        int user_id = DatabaseManager.Instance.userData.UID;
        string query =
            $"SELECT *\n" +
            $"FROM playerequipment\n" +
            $"WHERE user_id={user_id};";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isExist = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isExist)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return new PlayerEquipData(row);
        }
        else
        {
            // 실패
            return null;
        }
    }

    /// <summary>
    /// <para>인벤토리 슬롯에 있는 장비 아이템을 더블 클릭 시 장착을 구현할 메서드</para>
    /// HP 등의 스탯 변화도 같이 동작
    /// </summary>
    public void EquipItem(string part, int item_ID)
    {
        Debug.Log($"아이템 ID : {item_ID}");

        UserEquipTable[part] = item_ID;
        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, itemID: item_ID);
        OnEquipItem?.Invoke();
    }
    /// <summary>
    /// <para>모든 장비 해제</para>
    /// <para>HP 등의 스탯 변화도 같이 동작</para>
    /// </summary>
    private void UnEquipAll()
    {
        for (int i = 0; i < UserEquipTable.Count; i++)
        {
            int itemID = UserEquipTable[EquipParts[i]];
            if (itemID > 0)
            {
                UserEquipTable[EquipParts[i]] = 0;
                UserStatManager.Instance.EquipItemUpdateStat(isEquip: false, itemID: itemID);
                InventoryManager.Instance.GetItemUnEquip(itemID, 1);
                InventoryManager.Instance.AddWhichItem(new AddItemClassfiy(itemID, 1, false));
            }
        }
        OnAllUnEquipButtonClick?.Invoke();
    }
    /// <summary>
    /// 장비 슬롯에서 장비를 해제할 시, 특정 장비 해제 후, 인벤토리에 넣기
    /// <para>HP 등의 스탯 변화도 같이 동작</para>
    /// </summary>
    /// <param name="part">해제할 부분</param>
    /// <param name="itemID">해제할 아이템의 ID</param>
    public void UnEquip(string part, int itemID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;

        //query =
        //    $"INSERT INTO inventory (inventory.User_ID, inventory.Item_ID)\n" +
        //    $"VALUES ({user_ID}, {itemID});";
        //DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        UserEquipTable[part] = 0;

        UserStatManager.Instance.EquipItemUpdateStat(isEquip: false, itemID);
        InventoryManager.Instance.GetItemUnEquip(itemID, 1);
        InventoryManager.Instance.AddWhichItem(new AddItemClassfiy(itemID, 1, false));

        OnUnEquipItem?.Invoke();
    }
    #region 장착 장비 저장
    /// <summary>
    /// DB에 아직 넣지 않고 클라이언트에 임의로 저장해놓은 데이터들을 DB로 저장 (userquestList, userquestOBJList)
    /// <para>(게임 종료 전 또는 일정 시간마다)</para>
    /// </summary>
    public void SaveQuestProgress()
    {
        Debug.Log("장착 장비 저장.");
        int user_ID = DatabaseManager.Instance.userData.UID;

        string head = UserEquipTable[EquipParts[0]] != 0 ? UserEquipTable[EquipParts[0]].ToString() : "NULL";
        string armor = UserEquipTable[EquipParts[1]] != 0 ? UserEquipTable[EquipParts[1]].ToString() : "NULL";
        string glove = UserEquipTable[EquipParts[2]] != 0 ? UserEquipTable[EquipParts[2]].ToString() : "NULL";
        string boots = UserEquipTable[EquipParts[3]] != 0 ? UserEquipTable[EquipParts[3]].ToString() : "NULL";
        string weapon = UserEquipTable[EquipParts[4]] != 0 ? UserEquipTable[EquipParts[4]].ToString() : "NULL";
        string pendant = UserEquipTable[EquipParts[5]] != 0 ? UserEquipTable[EquipParts[5]].ToString() : "NULL";
        string ring = UserEquipTable[EquipParts[6]] != 0 ? UserEquipTable[EquipParts[6]].ToString() : "NULL";

        string query =
            $"UPDATE playerequipment\n" +
            $"SET playerequipment.HeadItem_ID={head}," +
            $"playerequipment.ArmorItem_ID={armor}," +
            $"playerequipment.GlovesItem_ID={glove}," +
            $"playerequipment.BootsItem_ID={boots}," +
            $"playerequipment.WeaponItem_ID={weapon}," +
            $"playerequipment.PendantItem_ID={pendant}," +
            $"playerequipment.RingItem_ID={ring}\n" +
            $"WHERE playerequipment.User_ID={user_ID};";
        Debug.Log(query);
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
    }
    private void AutoSave()
    {
        SaveQuestProgress();
    }
    private void OnApplicationQuit()
    {
        SaveQuestProgress();
    }
    #endregion
}
