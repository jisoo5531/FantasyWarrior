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
    public UserStatData userStatData { get; private set; }
    private OriginUserStat originUserStat;

    /// <summary>
    /// UserStatManager �ʱ�ȭ ���� ��, �߻��ϴ� �̺�Ʈ
    /// </summary>
    public event Action OnInitStatManager;
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
    }

    private void Start()
    {        
        userStatData = GetUserStatDataFromDB();
        originUserStat = new OriginUserStat(userStatData);
        StatManagerInit();
        OnInitStatManager?.Invoke();
    }
    /// <summary>
    /// 
    /// </summary>
    private void StatManagerInit()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;

        int lvAmount = originUserStat.Lv;
        int expAmount = originUserStat.Exp;
        int maxExpAmount = originUserStat.MaxExp;
        int strAmount = originUserStat.STR;
        int dexAmount = originUserStat.DEX;
        int intAmount = originUserStat.INT;
        int lukAmount = originUserStat.LUK;
        int defAmount = originUserStat.DEF;
        int hpAmount = originUserStat.MaxHP;
        int mpAmount = originUserStat.MaxMP;
        string query =
            $"UPDATE userstats\n" +
            $"SET userstats.level={lvAmount}," +
            $"userstats.maxexp={maxExpAmount}," +
            $"userstats.exp={expAmount}," +
            $"userstats.STR={strAmount}," +
            $"userstats.DEX={dexAmount}," +
            $"userstats.Intelligence={intAmount}," +
            $"userstats.LuK={lukAmount}," +
            $"userstats.defense={defAmount}," +
            $"userstats.MaxHp={hpAmount}," +
            $"userstats.MaxMana={mpAmount}\n" +
            $"WHERE userstats.User_ID={user_ID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
    }

    /// <summary>
    /// ���� ���� ��������
    /// </summary>
    /// <returns></returns>
    public UserStatData GetUserStatDataFromDB()
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
        Debug.Log($"������ ID : {itemID}, �����ϴ� ���̾�? : {isEquip}");
        int user_ID = DatabaseManager.Instance.userData.UID;

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
            EquipItemData equipItemData = ItemManager.Instance.GetEquipItemFromDB(itemID);

            strAmount = isEquip ? originUserStat.UpdateSTR(equipItemData.STR_Boost) : originUserStat.UpdateSTR(-equipItemData.STR_Boost);
            dexAmount = isEquip ? originUserStat.UpdateDEX(equipItemData.DEX_Boost) : originUserStat.UpdateDEX(-equipItemData.DEX_Boost);
            intAmount = isEquip ? originUserStat.UpdateINT(equipItemData.INT_Boost) : originUserStat.UpdateINT(-equipItemData.INT_Boost);
            lukAmount = isEquip ? originUserStat.UpdateLUK(equipItemData.LUK_Boost) : originUserStat.UpdateLUK(-equipItemData.LUK_Boost);
            defAmount = isEquip ? originUserStat.UpdateDEF(equipItemData.DEF_Boost) : originUserStat.UpdateDEF(-equipItemData.DEF_Boost);
            hpAmount = isEquip ? originUserStat.UpdateMaxHP(equipItemData.Hp_Boost) : originUserStat.UpdateMaxHP(-equipItemData.Hp_Boost);
            mpAmount = isEquip ? originUserStat.UpdateMaxMP(equipItemData.Mp_Boost) : originUserStat.UpdateMaxMP(-equipItemData.Mp_Boost);
        }
        else
        {
            // �������� �����ϰ� ���� ���� ��
            strAmount = originUserStat.STR;
            dexAmount = originUserStat.DEX;
            intAmount = originUserStat.INT;
            lukAmount = originUserStat.LUK;
            defAmount = originUserStat.DEF;
            hpAmount = originUserStat.MaxHP;
            mpAmount = originUserStat.MaxMP;
        }

        //UserStatData userStatData = GetUserStatDataFromDB();        

        string query =
            $"UPDATE userstats\n" +
            $"SET userstats.STR={strAmount}," +
            $"userstats.DEX={dexAmount}," +
            $"userstats.Intelligence={intAmount}," +
            $"userstats.LuK={lukAmount}," +
            $"userstats.defense={defAmount}," +
            $"userstats.MaxHp={hpAmount}," +
            $"userstats.MaxMana={mpAmount}\n" +
            $"WHERE userstats.User_ID={user_ID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
    }
    /// <summary>
    /// ������ �� ���� ���� ������ ������Ʈ
    /// <para>�������ϰ� ���� ����ġ�� �ݿ�</para>
    /// </summary>
    public void LevelUpUpdateStat(int remainExp)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;

        int lvAmount = originUserStat.UpdateLv(1);
        int expAmount = originUserStat.Exp = remainExp;
        int MaxExpAmount = originUserStat.UpdateMaxExp(originUserStat.levelUpStat.MaxExpAmount);
        int strAmount = originUserStat.UpdateSTR(originUserStat.levelUpStat.STRAmount);
        int dexAmount = originUserStat.UpdateDEX(originUserStat.levelUpStat.DEXAmount);
        int intAmount = originUserStat.UpdateINT(originUserStat.levelUpStat.INTAmount);
        int lukAmount = originUserStat.UpdateLUK(originUserStat.levelUpStat.LUKAmount);
        int defAmount = originUserStat.UpdateDEF(originUserStat.levelUpStat.DEFAmount);
        int hpAmount = originUserStat.UpdateMaxHP(originUserStat.levelUpStat.MaxhpAmount);
        int mpAmount = originUserStat.UpdateMaxMP(originUserStat.levelUpStat.MaxmpAmount);
        string query =
            $"UPDATE userstats\n" +
            $"SET userstats.level={lvAmount}," +                                  
            $"userstats.exp={expAmount}," +
            $"userstats.maxexp={MaxExpAmount}," +
            $"userstats.STR={strAmount}," +
            $"userstats.DEX={dexAmount}," +
            $"userstats.Intelligence={intAmount}," +
            $"userstats.LuK={lukAmount}," +
            $"userstats.defense={defAmount}," +
            $"userstats.MaxHp={hpAmount}," +
            $"userstats.hp={hpAmount}," +
            $"userstats.MaxMana={mpAmount}," +
            $"userstats.mana={mpAmount}\n" +
            $"WHERE userstats.User_ID={user_ID};"; 
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

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

        float ExpAmount = originUserStat.UpdateExp(expAmount);
        string query =
            $"UPDATE userstats\n" +
            $"SET userstats.`Exp`={ExpAmount}\n" +
            $"WHERE user_id={user_ID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        UserStatData userStat = GetUserStatDataFromDB();
        
        if (userStat.EXP >= userStat.MaxExp)
        {
            LevelUpUpdateStat(userStat.EXP - userStat.MaxExp);            
        }
        OnChangeExpStat?.Invoke();
    }
}
