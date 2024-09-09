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

    public static readonly List<string> EquipParts = new List<string>
    {
        "HeadItem_ID", "ArmorItem_ID", "GlovesItem_ID",
        "BootsItem_ID", "WeaponItem_ID", "PendantItem_ID", "RingItem_ID"
    };


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
    /// <summary>
    /// PlayerEquipManager가 초기화 됐을 때, 발생하는 이벤트
    /// </summary>
    public event Action OnEquipManagerInit;

    private void Awake()
    {
        Instance = this;
        unEquipButton.onClick.AddListener(UnEquipAll);
        UserStatManager.Instance.OnInitStatManager += Initialize;
    }
    /// <summary>
    /// UserStatManager의 초기화 이후 초기화가 된다.
    /// </summary>
    private void Initialize()
    {
        GetPlayerEquipFromDB();
        PlayerEquipData playerEquipData = GetPlayerEquipFromDB();

        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, playerEquipData.HeadItem_ID);
        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, playerEquipData.ArmorItem_ID);
        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, playerEquipData.GloveItem_ID);
        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, playerEquipData.BootItem_ID);
        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, playerEquipData.WeaponItem_ID);
        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, playerEquipData.Pendant_ID);
        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, playerEquipData.Ring_ID);

        OnEquipManagerInit?.Invoke();
        //UserStatManager.Instance.UpdateUserStat();
    }
    /// <summary>
    /// 플레이어가 장착한 장비 목록 가져오기
    /// </summary>
    public PlayerEquipData GetPlayerEquipFromDB()
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
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"UPDATE playerequipment\n" +
            $"SET playerequipment.{part}={item_ID}\n" +
            $"WHERE user_id={user_ID};";

        if (DatabaseManager.Instance.OnInsertOrUpdateRequest(query))
        {
            Debug.Log("장착 성공");
            UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, item_ID);
            OnEquipItem?.Invoke();
        }

        //Debug.Log($"장착 성공? : {DatabaseManager.Instance.OnInsertOrUpdateRequest(query)}");
        //DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
    }
    /// <summary>
    /// <para>모든 장비 해제</para>
    /// <para>HP 등의 스탯 변화도 같이 동작</para>
    /// </summary>
    private void UnEquipAll()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        PlayerEquipData equipData = GetPlayerEquipFromDB();

        string query =
            $"UPDATE playerequipment\n" +
            $"SET playerequipment.HeadItem_ID=NULL,\n" +
            $"playerequipment.ArmorItem_ID=NULL,\n" +
            $"playerequipment.GlovesItem_ID=NULL,\n" +
            $"playerequipment.BootsItem_ID=NULL,\n" +
            $"playerequipment.WeaponItem_ID=NULL,\n" +
            $"playerequipment.PendantItem_ID=NULL,\n" +
            $"playerequipment.RingItem_ID=NULL\n" +
            $"WHERE user_id=1;";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        StringBuilder queryBuilder = new StringBuilder();
        queryBuilder.Append("INSERT INTO inventory (inventory.User_ID, inventory.Item_ID) VALUES");

        // 항목을 리스트로 구성
        List<string> values = new List<string>();

        if (equipData.HeadItem_ID != 0)
        {
            values.Add($"({user_ID}, {equipData.HeadItem_ID})");
            UserStatManager.Instance.EquipItemUpdateStat(isEquip: false, equipData.HeadItem_ID);
            InventoryManager.Instance.AddWhichItem(equipData.HeadItem_ID);
        }
        if (equipData.ArmorItem_ID != 0)
        {
            values.Add($"({user_ID}, {equipData.ArmorItem_ID})");
            UserStatManager.Instance.EquipItemUpdateStat(isEquip: false, equipData.ArmorItem_ID);
            InventoryManager.Instance.AddWhichItem(equipData.ArmorItem_ID);
        }
        if (equipData.GloveItem_ID != 0)
        {
            values.Add($"({user_ID}, {equipData.GloveItem_ID})");
            UserStatManager.Instance.EquipItemUpdateStat(isEquip: false, equipData.GloveItem_ID);
            InventoryManager.Instance.AddWhichItem(equipData.GloveItem_ID);
        }
        if (equipData.BootItem_ID != 0)
        {
            values.Add($"({user_ID}, {equipData.BootItem_ID})");
            UserStatManager.Instance.EquipItemUpdateStat(isEquip: false, equipData.BootItem_ID);
            InventoryManager.Instance.AddWhichItem(equipData.BootItem_ID);
        }
        if (equipData.WeaponItem_ID != 0)
        {
            values.Add($"({user_ID}, {equipData.WeaponItem_ID})");
            UserStatManager.Instance.EquipItemUpdateStat(isEquip: false, equipData.WeaponItem_ID);
            InventoryManager.Instance.AddWhichItem(equipData.WeaponItem_ID);
        }
        if (equipData.Pendant_ID != 0)
        {
            values.Add($"({user_ID}, {equipData.Pendant_ID})");
            UserStatManager.Instance.EquipItemUpdateStat(isEquip: false, equipData.Pendant_ID);
            InventoryManager.Instance.AddWhichItem(equipData.Pendant_ID);
        }
        if (equipData.Ring_ID != 0)
        {
            values.Add($"({user_ID}, {equipData.Ring_ID})");
            UserStatManager.Instance.EquipItemUpdateStat(isEquip: false, equipData.Ring_ID);
            InventoryManager.Instance.AddWhichItem(equipData.Ring_ID);
        }

        // values 리스트가 비어 있지 않다면 쿼리를 생성
        if (values.Count > 0)
        {
            queryBuilder.Append(string.Join(",", values));
            queryBuilder.Append(";");

            query = queryBuilder.ToString();
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
        else
        {
            // 만약 모든 아이템이 0이면 쿼리가 생성되지 않으므로 별도 처리
            Debug.Log("아이템이 없습니다.");
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

        string query =
            $"UPDATE playerequipment\n" +
            $"SET playerequipment.{part}=null\n" +
            $"WHERE playerequipment.User_ID={user_ID};";
        DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        query =
            $"INSERT INTO inventory (inventory.User_ID, inventory.Item_ID)\n" +
            $"VALUES ({user_ID}, {itemID});";
        DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        UserStatManager.Instance.EquipItemUpdateStat(isEquip: false, itemID);
        InventoryManager.Instance.AddWhichItem(itemID);

        OnUnEquipItem?.Invoke();
    }
}
