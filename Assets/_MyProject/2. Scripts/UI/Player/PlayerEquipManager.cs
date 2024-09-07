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
    /// �κ��丮 ���Կ� �ִ� ��� �������� ���� Ŭ�� �� ������ ������ �޼���
    /// </summary>
    public void EquipItem(string part, int item_ID)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"UPDATE playerequipment\n" +
            $"SET playerequipment.{part}={item_ID}\n" +
            $"WHERE user_id={user_ID};";
        Debug.Log(query);

        Debug.Log($"����? : {DatabaseManager.Instance.OnInsertOrUpdateRequest(query)}");
    }
    /// <summary>
    /// �÷��̾ ������ ��� ��� ��������
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
            // ����
            return null;
        }
    }
}
