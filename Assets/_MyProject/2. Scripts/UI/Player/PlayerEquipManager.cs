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

    [Header("��� ���� ��ư")]
    public Button unEquipButton;
    /// <summary>
    /// �÷��̾ ��� �������� ��, �߻��ϴ� �̺�Ʈ
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
    /// �κ��丮 ���Կ� �ִ� ��� �������� ���� Ŭ�� �� ������ ������ �޼���
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
            Debug.Log("���� ����");
            OnEquipItem?.Invoke();
        }        
        //Debug.Log($"���� ����? : {DatabaseManager.Instance.OnInsertOrUpdateRequest(query)}");
        //DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
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
    /// <summary>
    /// ��� ��� ����
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
