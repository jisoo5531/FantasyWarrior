using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;


public class PlayerEquipManager : MonoBehaviour
{
    public static PlayerEquipManager Instance { get; private set; }
    public PlayerEquipData playerEquip { get; private set; }

    [Header("장비 해제 버튼")]
    public Button unEquipButton;
    /// <summary>
    /// 플레이어가 장비를 장착했을 때, 발생하는 이벤트
    /// </summary>
    public event Action OnEquipItem;
    public event Action OnAllUnEquipButtonClick;

    private void Awake()
    {
        Instance = this;
        unEquipButton.onClick.AddListener(UnEquip);
    }

    private void Start()
    {
        GetPlayerEquipFromDB();
    }

    /// <summary>
    /// 인벤토리 슬롯에 있는 장비 아이템을 더블 클릭 시 장착을 구현할 메서드
    /// </summary>
    public void EquipItem(string part, int item_ID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"UPDATE playerequipment\n" +
            $"SET playerequipment.{part}={item_ID}\n" +
            $"WHERE user_id={user_ID};";

        if (DatabaseManager.Instance.OnInsertOrUpdateRequest(query))
        {
            Debug.Log("장착 성공");
            OnEquipItem?.Invoke();
        }        
        //Debug.Log($"장착 성공? : {DatabaseManager.Instance.OnInsertOrUpdateRequest(query)}");
        //DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
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
            playerEquip = new PlayerEquipData(row);
            return playerEquip;
        }
        else
        {
            // 실패
            return null;
        }
    }
    /// <summary>
    /// 모든 장비 해제
    /// </summary>
    private void UnEquip()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        PlayerEquipData equipData = PlayerEquipManager.Instance.GetPlayerEquipFromDB();
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

        query =
            $"INSERT INTO inventory (inventory.User_ID, inventory.Item_ID)\n" +
            $"VALUES ({user_ID}, {equipData.HeadItem_ID})," +
            $"({user_ID}, {equipData.ArmorItem_ID})," +
            $"({user_ID}, {equipData.GloveItem_ID})," +
            $"({user_ID}, {equipData.BootItem_ID})," +
            $"({user_ID}, {equipData.WeaponItem_ID})," +
            $"({user_ID}, {equipData.Pendant_ID})," +
            $"({user_ID}, {equipData.Ring_ID});";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        InventoryManager.Instance.GetDataFromDatabase();
        OnAllUnEquipButtonClick?.Invoke();
    }
}
