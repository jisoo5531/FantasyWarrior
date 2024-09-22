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
    /// 게임 중에 사용할 유저 스탯을 관리하는 변수
    /// </summary>
    public UserStatClient userStatClient { get; private set; }
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
        EventHandler.managerEvent.RegisterItemManagerInit(StatManagerInit);
    }
    private void Start()
    {                  
        InvokeRepeating("AutoSave", 300f, 300f);
    }
    /// <summary>
    /// 게임 시작하면 스탯 데이터 가져오기
    /// </summary>
    private void StatManagerInit()
    {
        UserStatData userStatData = GetUserStatDataFromDB();
        userStatClient = new UserStatClient(userStatData);                
        
        EventHandler.managerEvent.TriggerStatManagerInit();
    }

    /// <summary>
    /// 유저 스탯 가져오기
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
            // 아이템을 장착하고 있을 때
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
    /// 레벨업 시 유저 스탯 데이터 업데이트
    /// <para>레벨업하고 남은 경험치도 반영</para>
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
    /// 몬스터가 죽을 때 등등의 경험치가 업데이트 되는 상황에서 호출되는 메서드
    /// </summary>
    /// <param name="expAmount">올라가는 경험치 양</param>
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

    #region 스탯 저장
    /// <summary>
    /// DB에 아직 넣지 않고 클라이언트에 임의로 저장해놓은 데이터들을 DB로 저장 (userquestList, userquestOBJList)
    /// <para>(게임 종료 전 또는 일정 시간마다)</para>
    /// </summary>
    public void SaveStat()
    {
        Debug.Log("Stat 저장.");
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
