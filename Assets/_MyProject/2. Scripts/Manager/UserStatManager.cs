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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {        
        userStatData = GetUserStatDataFromDB();
        originUserStat = new OriginUserStat(userStatData.Level);
        InitStat();
        OnInitStatManager?.Invoke();
    }
    private void InitStat()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;

        int lvAmount = originUserStat.Lv;
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
            hpAmount = isEquip ? originUserStat.UpdateHP(equipItemData.Hp_Boost) : originUserStat.UpdateHP(-equipItemData.Hp_Boost);
            mpAmount = isEquip ? originUserStat.UpdateMP(equipItemData.Mp_Boost) : originUserStat.UpdateMP(-equipItemData.Mp_Boost);
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
    /// </summary>
    public void LevelUpUpdateStat()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;

        int lvAmount = originUserStat.UpdateLv(1);
        int strAmount = originUserStat.UpdateSTR(originUserStat.levelUpStat.STRAmount);
        int dexAmount = originUserStat.UpdateDEX(originUserStat.levelUpStat.STRAmount);
        int intAmount = originUserStat.UpdateINT(originUserStat.levelUpStat.STRAmount);
        int lukAmount = originUserStat.UpdateLUK(originUserStat.levelUpStat.STRAmount);
        int defAmount = originUserStat.UpdateDEF(originUserStat.levelUpStat.STRAmount);
        int hpAmount = originUserStat.UpdateHP(originUserStat.levelUpStat.STRAmount);
        int mpAmount = originUserStat.UpdateMP(originUserStat.levelUpStat.STRAmount);
        string query =
            $"UPDATE userstats\n" +
            $"SET userstats.level={lvAmount}," +
            $"userstats.STR={strAmount}," +
            $"userstats.DEX={dexAmount}," +
            $"userstats.Intelligence={intAmount}," +
            $"userstats.LuK={lukAmount}," +
            $"userstats.defense={defAmount}," +
            $"userstats.MaxHp={hpAmount}," +
            $"userstats.MaxMana={mpAmount}\n" +
            $"WHERE userstats.User_ID={user_ID};"; 
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        OnLevelUpUpdateStat?.Invoke();
    }
}
