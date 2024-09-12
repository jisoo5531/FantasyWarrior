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
    private UserStatData userStatData;
    public UserStatClient userStatClient;

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
        StatManagerInit();
        InvokeRepeating("AutoSave", 300f, 300f);
    }
    /// <summary>
    /// ���� �����ϸ� ���� ������ ��������
    /// </summary>
    private void StatManagerInit()
    {
        userStatData = GetUserStatDataFromDB();
        userStatClient = new UserStatClient(userStatData);
        Debug.Log(userStatClient.MaxHP);
        int user_ID = DatabaseManager.Instance.userData.UID;

        OnInitStatManager?.Invoke();
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
        int dexAmount = 0;
        int intAmount = 0;
        int lukAmount = 0;
        int defAmount = 0;
        int hpAmount = 0;
        int mpAmount = 0;

        int strAmount;
        if (itemID != 0)
        {
            // �������� �����ϰ� ���� ��
            EquipItemData equipItemData = ItemManager.Instance.GetEquipItemFromDB(itemID);

            strAmount = isEquip ? equipItemData.STR_Boost : -equipItemData.STR_Boost;
            dexAmount = isEquip ? equipItemData.DEX_Boost : -equipItemData.DEX_Boost;
            intAmount = isEquip ? equipItemData.INT_Boost : -equipItemData.INT_Boost;
            lukAmount = isEquip ? equipItemData.LUK_Boost : -equipItemData.LUK_Boost;
            defAmount = isEquip ? equipItemData.DEF_Boost : -equipItemData.DEF_Boost;
            hpAmount = isEquip ? equipItemData.Hp_Boost : -equipItemData.Hp_Boost;
            mpAmount = isEquip ? equipItemData.Mp_Boost : -equipItemData.Mp_Boost;
            userStatClient.UpdateSTR(strAmount);
            userStatClient.UpdateDEX(dexAmount);
            userStatClient.UpdateINT(intAmount);
            userStatClient.UpdateLUK(lukAmount);
            userStatClient.UpdateDEF(defAmount);
            userStatClient.UpdateMaxHP(hpAmount);
            userStatClient.UpdateMaxMP(mpAmount);
        }
        //else
        //{
        //    // �������� �����ϰ� ���� ���� ��
        //    strAmount = userStatClient.STR;
        //    dexAmount = userStatClient.DEX;
        //    intAmount = userStatClient.INT;
        //    lukAmount = userStatClient.LUK;
        //    defAmount = userStatClient.DEF;
        //    hpAmount = userStatClient.MaxHP;
        //    mpAmount = userStatClient.MaxMP;
        //}
        //UserStatData userStatData = GetUserStatDataFromDB();        

        //string query =
        //    $"UPDATE userstats\n" +
        //    $"SET userstats.STR={strAmount}," +
        //    $"userstats.DEX={dexAmount}," +
        //    $"userstats.Intelligence={intAmount}," +
        //    $"userstats.LuK={lukAmount}," +
        //    $"userstats.defense={defAmount}," +
        //    $"userstats.MaxHp={hpAmount}," +
        //    $"userstats.MaxMana={mpAmount}\n" +
        //    $"WHERE userstats.User_ID={user_ID};";
        //_ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
    }
    /// <summary>
    /// ������ �� ���� ���� ������ ������Ʈ
    /// <para>�������ϰ� ���� ����ġ�� �ݿ�</para>
    /// </summary>
    public void LevelUpUpdateStat(int remainExp)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;

        int lvAmount = userStatClient.UpdateLv(1);
        int expAmount = userStatClient.Exp = remainExp;
        int MaxExpAmount = userStatClient.UpdateMaxExp(userStatClient.levelUpStat.MaxExpAmount);
        int strAmount = userStatClient.UpdateSTR(userStatClient.levelUpStat.STRAmount);
        int dexAmount = userStatClient.UpdateDEX(userStatClient.levelUpStat.DEXAmount);
        int intAmount = userStatClient.UpdateINT(userStatClient.levelUpStat.INTAmount);
        int lukAmount = userStatClient.UpdateLUK(userStatClient.levelUpStat.LUKAmount);
        int defAmount = userStatClient.UpdateDEF(userStatClient.levelUpStat.DEFAmount);
        int hpAmount = userStatClient.UpdateMaxHP(userStatClient.levelUpStat.MaxhpAmount);
        int mpAmount = userStatClient.UpdateMaxMP(userStatClient.levelUpStat.MaxmpAmount);
        //string query =
        //    $"UPDATE userstats\n" +
        //    $"SET userstats.level={lvAmount}," +                                  
        //    $"userstats.exp={expAmount}," +
        //    $"userstats.maxexp={MaxExpAmount}," +
        //    $"userstats.STR={strAmount}," +
        //    $"userstats.DEX={dexAmount}," +
        //    $"userstats.Intelligence={intAmount}," +
        //    $"userstats.LuK={lukAmount}," +
        //    $"userstats.defense={defAmount}," +
        //    $"userstats.MaxHp={hpAmount}," +
        //    $"userstats.hp={hpAmount}," +
        //    $"userstats.MaxMana={mpAmount}," +
        //    $"userstats.mana={mpAmount}\n" +
        //    $"WHERE userstats.User_ID={user_ID};"; 
        //_ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);


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
        //string query =
        //    $"UPDATE userstats\n" +
        //    $"SET userstats.`Exp`={ExpAmount}\n" +
        //    $"WHERE user_id={user_ID};";
        //_ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);

        //UserStatData userStat = GetUserStatDataFromDB();

        //if (userStat.EXP >= userStat.MaxExp)
        //{
        //    LevelUpUpdateStat(userStat.EXP - userStat.MaxExp);            
        //}
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
    public void SaveQuestProgress()
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
        SaveQuestProgress();
    }
    private void OnApplicationQuit()
    {        
        SaveQuestProgress();
    }
    #endregion
}
