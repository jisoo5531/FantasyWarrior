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
    /// UserStatManager 초기화 됐을 때, 발생하는 이벤트
    /// </summary>
    public event Action OnInitStatManager;
    /// <summary>
    /// 레벨업 시, 실행할 이벤트
    /// </summary>
    public event Action OnLevelUpUpdateStat;
    /// <summary>
    /// 몬스터를 잡거나 등의 경험치가 변동이 있을 때 발생
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
    /// 유저 스탯 가져오기
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
            //  실패
            return null;
        }
    }
    
    /// <summary>
    /// 아이템을 장착하거나 해제할 때 유저의 스탯에 반영하는 메서드
    /// </summary>
    /// <param name="itemID"></param>
    public void EquipItemUpdateStat(bool isEquip, int itemID = 0)
    {
        Debug.Log($"아이템 ID : {itemID}, 장착하는 것이야? : {isEquip}");
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
            // 아이템을 장착하고 있을 때
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
            // 아이템을 장착하고 있지 않을 때
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
    /// 레벨업 시 유저 스탯 데이터 업데이트
    /// <para>레벨업하고 남은 경험치도 반영</para>
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
    /// 몬스터가 죽을 때 등등의 경험치가 업데이트 되는 상황에서 호출되는 메서드
    /// </summary>
    /// <param name="expAmount">올라가는 경험치 양</param>
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
