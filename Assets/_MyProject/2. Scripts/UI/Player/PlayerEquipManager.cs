using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


public class PlayerEquipManager : MonoBehaviour
{
    public static PlayerEquipManager Instance { get; private set; }
    public PlayerEquipData playerEquip { get; private set; }
    private void Awake()
    {
        Instance = this;
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
        Debug.Log(query);

        Debug.Log($"성공? : {DatabaseManager.Instance.OnInsertOrUpdateRequest(query)}");
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
}
