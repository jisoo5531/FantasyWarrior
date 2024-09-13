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
    /// ������ �����ϰ� �ִ� ������ ��� ����Ʈ
    /// <para>Ŭ���̾�Ʈ�� �ӽ÷� ������ �д�.</para>
    /// <para>�����ͺ��̽��� ������ ����� �Ѵ�.</para>
    /// </summary>
    public Dictionary<string, int> UserEquipTable { get; private set; }

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

    private void Awake()
    {
        Instance = this;
        unEquipButton.onClick.AddListener(UnEquipAll);        
        EventHandler.managerEvent.RegisterStatManagerInit(Initialize);
    }
    /// <summary>
    /// UserStatManager�� �ʱ�ȭ ���� �ʱ�ȭ�� �ȴ�.
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
    /// �÷��̾ ������ ��� ��� ��������
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

        UserEquipTable[part] = item_ID;
        UserStatManager.Instance.EquipItemUpdateStat(isEquip: true, itemID: item_ID);
        OnEquipItem?.Invoke();
    }
    /// <summary>
    /// <para>��� ��� ����</para>
    /// <para>HP ���� ���� ��ȭ�� ���� ����</para>
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
    /// ��� ���Կ��� ��� ������ ��, Ư�� ��� ���� ��, �κ��丮�� �ֱ�
    /// <para>HP ���� ���� ��ȭ�� ���� ����</para>
    /// </summary>
    /// <param name="part">������ �κ�</param>
    /// <param name="itemID">������ �������� ID</param>
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
    #region ���� ��� ����
    /// <summary>
    /// DB�� ���� ���� �ʰ� Ŭ���̾�Ʈ�� ���Ƿ� �����س��� �����͵��� DB�� ���� (userquestList, userquestOBJList)
    /// <para>(���� ���� �� �Ǵ� ���� �ð�����)</para>
    /// </summary>
    public void SaveQuestProgress()
    {
        Debug.Log("���� ��� ����.");
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
