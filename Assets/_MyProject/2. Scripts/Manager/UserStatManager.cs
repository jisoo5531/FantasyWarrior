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
    private OriginUserStat originUserStat = new();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _ = GetUserStatDataFromDB();

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
            userStatData = new UserStatData(row);
            return userStatData;
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
    public void UpdateUserStat(bool isEquip, int itemID = 0)
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
            strAmount = originUserStat.O_STR;
            dexAmount = originUserStat.O_DEX;
            intAmount = originUserStat.O_INT;
            lukAmount = originUserStat.O_LUK;
            defAmount = originUserStat.O_DEF;
            hpAmount = originUserStat.O_MaxHP;
            mpAmount = originUserStat.O_MaxMP;
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
}
