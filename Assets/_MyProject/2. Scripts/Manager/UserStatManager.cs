using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class UserStatManager : MonoBehaviour
{
    public readonly string STR = "str";
    public readonly string DEX = "dex";
    public readonly string INT = "Intelligence";
    public readonly string LUK = "luk";
    public readonly string DEF = "defense";
    public readonly string MaxHP = "maxhp";
    public readonly string MaxMP = "maxmana";

    public static UserStatManager Instance { get; private set; }    
    /// <summary>
    /// ���� �߿� ����� ���� ������ �����ϴ� ����
    /// </summary>
    public UserStatClient userStatClient { get; private set; }
    /// <summary>
    /// ������ ��, ������ �̺�Ʈ
    /// </summary>
    public event Action OnLevelUpUpdateStat;
    /// <summary>
    /// ���͸� ��ų� ���� ����ġ�� ������ ���� �� �߻�
    /// </summary>
    public event Action OnChangeExpStat;

    private void Awake()
    {
        Instance = this;
        EventHandler.managerEvent.RegisterItemManagerInit(StatManagerInit);
    }
    private void Start()
    {                  
        InvokeRepeating("AutoSave", 300f, 300f);
    }
    /// <summary>
    /// ���� �����ϸ� ���� ������ ��������
    /// </summary>
    private void StatManagerInit()
    {
        UserStatData userStatData = GetUserStatDataFromDB();
        userStatClient = new UserStatClient(userStatData);                
        
        EventHandler.managerEvent.TriggerStatManagerInit();
    }

    /// <summary>
    /// ���� ���� ��������
    /// </summary>
    /// <returns></returns>
    private UserStatData GetUserStatDataFromDB()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"SELECT *\n" +
            $"FROM userstats\n" +
            $"WHERE user_id={user_ID};";

        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return new UserStatData(row);
        }
        else
        {
            //  ����
            return null;
        }
    }
    
    /// <summary>
    /// �������� �����ϰų� ������ �� ������ ���ȿ� �ݿ��ϴ� �޼���
    /// </summary>
    /// <param name="itemID"></param>
    public void EquipItemUpdateStat(bool isEquip, int itemID = 0)
    {        
        int user_ID = DatabaseManager.Instance.userData.UID;
        int atkAmount = 0;
        int strAmount = 0;
        int dexAmount = 0;
        int intAmount = 0;
        int lukAmount = 0;
        int defAmount = 0;
        int hpAmount = 0;
        int mpAmount = 0;
        
        
        if (itemID != 0)
        {
            // �������� �����ϰ� ���� ��
            EquipItemData equipItemData = ItemManager.Instance.GetEquipItemData(itemID);
            atkAmount = isEquip ? equipItemData.ATK_Boost : -equipItemData.ATK_Boost;
            strAmount = isEquip ? equipItemData.STR_Boost : -equipItemData.STR_Boost;
            dexAmount = isEquip ? equipItemData.DEX_Boost : -equipItemData.DEX_Boost;
            intAmount = isEquip ? equipItemData.INT_Boost : -equipItemData.INT_Boost;
            lukAmount = isEquip ? equipItemData.LUK_Boost : -equipItemData.LUK_Boost;
            defAmount = isEquip ? equipItemData.DEF_Boost : -equipItemData.DEF_Boost;
            hpAmount = isEquip ? equipItemData.Hp_Boost : -equipItemData.Hp_Boost;
            mpAmount = isEquip ? equipItemData.Mp_Boost : -equipItemData.Mp_Boost;
            userStatClient.UpdateATK(atkAmount);
            userStatClient.UpdateSTR(strAmount);
            userStatClient.UpdateDEX(dexAmount);
            userStatClient.UpdateINT(intAmount);
            userStatClient.UpdateLUK(lukAmount);
            userStatClient.UpdateDEF(defAmount);
            userStatClient.UpdateMaxHP(hpAmount);
            userStatClient.UpdateMaxMP(mpAmount);
        }
    }
    /// <summary>
    /// ������ �� ���� ���� ������ ������Ʈ
    /// <para>�������ϰ� ���� ����ġ�� �ݿ�</para>
    /// </summary>
    public void LevelUpUpdateStat(int remainExp)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;

        userStatClient.UpdateLv(1);
        userStatClient.Exp = remainExp;
        userStatClient.UpdateMaxExp(userStatClient.levelUpStat.MaxExpAmount);
        userStatClient.UpdateSTR(userStatClient.levelUpStat.STRAmount);
        userStatClient.UpdateDEX(userStatClient.levelUpStat.DEXAmount);
        userStatClient.UpdateINT(userStatClient.levelUpStat.INTAmount);
        userStatClient.UpdateLUK(userStatClient.levelUpStat.LUKAmount);
        userStatClient.UpdateDEF(userStatClient.levelUpStat.DEFAmount);
        userStatClient.UpdateMaxHP(userStatClient.levelUpStat.MaxhpAmount);
        userStatClient.UpdateMaxMP(userStatClient.levelUpStat.MaxmpAmount);

        OnLevelUpUpdateStat?.Invoke();
        EventHandler.playerEvent.TriggerPlayerLevelUp();
    }
    /// <summary>
    /// ���Ͱ� ���� �� ����� ����ġ�� ������Ʈ �Ǵ� ��Ȳ���� ȣ��Ǵ� �޼���
    /// </summary>
    /// <param name="expAmount">�ö󰡴� ����ġ ��</param>
    public void UpdateExp(int expAmount)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;

        float ExpAmount = userStatClient.UpdateExp(expAmount);

        if (userStatClient.Exp >= userStatClient.MaxExp)
        {
            LevelUpUpdateStat(userStatClient.Exp - userStatClient.MaxExp);
        }

        OnChangeExpStat?.Invoke();
    }

    #region ���� ����
    /// <summary>
    /// DB�� ���� ���� �ʰ� Ŭ���̾�Ʈ�� ���Ƿ� �����س��� �����͵��� DB�� ���� (userquestList, userquestOBJList)
    /// <para>(���� ���� �� �Ǵ� ���� �ð�����)</para>
    /// </summary>
    public void SaveStat()
    {
        Debug.Log("Stat ����.");
        int user_ID = DatabaseManager.Instance.userData.UID;

        string query =
            $"UPDATE userstats\n" +
            $"SET userstats.level={userStatClient.Level}," +
            $"userstats.maxexp={userStatClient.MaxExp}," +
            $"userstats.exp={userStatClient.Exp}," +
            $"userstats.STR={userStatClient.STR}," +
            $"userstats.DEX={userStatClient.DEX}," +
            $"userstats.Intelligence={userStatClient.INT}," +
            $"userstats.LuK={userStatClient.LUK}," +
            $"userstats.defense={userStatClient.DEF}," +
            $"userstats.MaxHp={userStatClient.MaxHP}," +
            $"userstats.MaxMana={userStatClient.MaxMP}\n" +
            $"WHERE userstats.User_ID={user_ID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
    }
    private void AutoSave()
    {
        SaveStat();
    }
    private void OnApplicationQuit()
    {        
        SaveStat();
    }
    #endregion
}
