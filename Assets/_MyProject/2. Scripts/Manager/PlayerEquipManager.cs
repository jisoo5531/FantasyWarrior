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


    [Header("��� ���� ��ư")]
    public Button unEquipButton;
    /// <summary>
    /// �÷��̾ ��� �������� ��, �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event Action OnEquipItem;
    /// <summary>
    /// �÷��̾ ��� �������� ��, �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event Action OnUnEquipItem;
    /// <summary>
    /// ��� ��� ���� ��ư�� ������ ��, �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event Action OnAllUnEquipButtonClick;
    /// <summary>
    /// PlayerEquipManager�� �ʱ�ȭ ���� ��, �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event Action OnEquipManagerInit;

    private void Awake()
    {
        Instance = this;
        unEquipButton.onClick.AddListener(UnEquipAll);
        UserStatManager.Instance.OnInitStatManager += Initialize;
    }
    /// <summary>
    /// UserStatManager�� �ʱ�ȭ ���� �ʱ�ȭ�� �ȴ�.
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
            return new PlayerEquipData(row);
        }
        else
        {
            // ����
            return null;
        }
    }

    /// <summary>
    /// <para>�κ��丮 ���Կ� �ִ� ��� �������� ���� Ŭ�� �� ������ ������ �޼���</para>
    /// HP ���� ���� ��ȭ�� ���� ����
    /// </summary>
    public void EquipItem(string part, int item_ID)
    {
        Debug.Log($"������ ID : {item_ID}");
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"UPDATE playerequipment\n" +
            $"SET playerequipment.{part}={item_ID}\n" +
            $"WHERE user_id={user_ID};";

        if (DatabaseManager.Instance.OnInsertOrUpdateRequest(query))
        {
            Debug.Log("���� ����");
            UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, item_ID);
            OnEquipItem?.Invoke();
        }

        //Debug.Log($"���� ����? : {DatabaseManager.Instance.OnInsertOrUpdateRequest(query)}");
        //DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
    }
    /// <summary>
    /// <para>��� ��� ����</para>
    /// <para>HP ���� ���� ��ȭ�� ���� ����</para>
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

        // �׸��� ����Ʈ�� ����
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

        // values ����Ʈ�� ��� ���� �ʴٸ� ������ ����
        if (values.Count > 0)
        {
            queryBuilder.Append(string.Join(",", values));
            queryBuilder.Append(";");

            query = queryBuilder.ToString();
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
        else
        {
            // ���� ��� �������� 0�̸� ������ �������� �����Ƿ� ���� ó��
            Debug.Log("�������� �����ϴ�.");
        }


        OnAllUnEquipButtonClick?.Invoke();
    }
    /// <summary>
    /// ��� ���Կ��� ��� ������ ��, Ư�� ��� ���� ��, �κ��丮�� �ֱ�
    /// <para>HP ���� ���� ��ȭ�� ���� ����</para>
    /// </summary>
    /// <param name="part">������ �κ�</param>
    /// <param name="itemID">������ �������� ID</param>
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
