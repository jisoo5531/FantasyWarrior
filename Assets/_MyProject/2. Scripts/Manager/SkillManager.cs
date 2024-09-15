using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    /// <summary>
    /// 현재 유저 직업에 맞는 모든 스킬 리스트
    /// </summary>
    public List<SkillData> ClassSkillDataList { get; private set; }
    /// <summary>
    /// 현재 유저 레벨에 사용 가능한 스킬들의 리스트
    /// </summary>
    public List<SkillData> userAvailableSkillList { get; private set; }
    /// <summary>
    /// 현재 유저가 보유(배운) 스킬 리스트
    /// </summary>
    public List<UserSkillData> UserSkillList { get; private set; }
    /// <summary>
    /// 저장할 때 비교를 위한 원본 스킬 리스트
    /// </summary>
    public List<UserSkillData> UserSkillOrigin { get; private set; }
    public SkillKeyBind UserSkillKeyBInd { get; private set; }

    /// <summary>
    /// 레벨업을 할때마다 스킬 언락 실행 이벤트
    /// </summary>
    public event Action OnUnlockSkillEvent;    

    private void Awake()
    {
        Instance = this;
        EventHandler.managerEvent.RegisterStatManagerInit(SkillManagerInit);
    }
    public void SkillManagerInit()
    {
        UserStatManager.Instance.OnLevelUpUpdateStat += OnLevelUp_UnlockSkill;

        UserSkillKeyBInd = GetSkillKeyBind();

        _ = GetSkillFromDB();
        _ = GetUserSkillFromDB();

        userAvailableSkillList = new List<SkillData>();
        UserStatClient userStatClient = UserStatManager.Instance.userStatClient;
        Debug.Log($"플레이어 현재 레벨 : {userStatClient.Level}");
        foreach (SkillData skillData in ClassSkillDataList)
        {
            if (userStatClient.Level >= skillData.Unlock_Level)
            {
                userAvailableSkillList.Add(skillData);
            }
        }
        EventHandler.managerEvent.TriggerSkillManagerInit();
    }
    private SkillKeyBind GetSkillKeyBind()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        string query =
            $"SELECT *\n" +
            $"FROM userskillkeybinds\n" +
            $"WHERE userskillkeybinds.User_ID={user_ID}";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;
        if (isGetData)
        {
            DataRow row = dataSet.Tables[0].Rows[0];
            return new SkillKeyBind(row);
        }
        else
        {
            //  실패
            return null;
        }
    }
    /// <summary>
    /// 데이터베이스에서 스킬 데이터 가져오기
    /// </summary>
    private List<SkillData> GetSkillFromDB()
    {
        ClassSkillDataList = new List<SkillData>();

        UserStatClient userStat = UserStatManager.Instance.userStatClient;
        int userId = DatabaseManager.Instance.userData.UID;
        CharClass userClass = userStat.charClass;

        string query =
            $"SELECT *\n" +
            $"FROM skills\n" +
            $"WHERE skills.Class={(int)userClass};";

        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;
        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                SkillData data = new SkillData(row);
                ClassSkillDataList.Add(data);
                //skillTable.Add(data.Skill_ID - 1, data.Skill_Name);
            }
            return ClassSkillDataList;
        }
        else
        {
            //  실패
            return null;
        }
    }
    /// <summary>
    /// 유저가 배운 스킬 목록 리스트 가져오기
    /// </summary>
    /// <returns></returns>
    private List<UserSkillData> GetUserSkillFromDB()
    {
        UserSkillOrigin = new List<UserSkillData>();
        UserSkillList = new List<UserSkillData>();

        string query =
            $"SELECT *\n" +
            $"FROM userskills;";
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;
        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                UserSkillData data = new UserSkillData(row);
                UserSkillOrigin.Add(data);
                UserSkillList.Add(data);
                //skillTable.Add(data.Skill_ID - 1, data.Skill_Name);
            }
            return UserSkillList;
        }
        else
        {
            //  실패
            return null;
        }

    }

    /// <summary>
    /// 유저 레벨에 따라 스킬 해제를 할 수 있는 메서드
    /// </summary>
    private void OnLevelUp_UnlockSkill()
    {
        UserStatClient userStat = UserStatManager.Instance.userStatClient;
        int userLevel = userStat.Level;
        CharClass userClass = userStat.charClass;
        userAvailableSkillList = ClassSkillDataList.FindAll(x => userLevel >= x.Unlock_Level && x.CharClass.Equals(userClass));
        OnUnlockSkillEvent?.Invoke();
    }
    /// <summary>
    /// 스킬 습득 메서드
    /// </summary>
    /// <param name="skillID"></param>
    public void LearnSkill(SkillData skill)
    {
        int userID = DatabaseManager.Instance.userData.UID;
        CharClass charClass = UserStatManager.Instance.userStatClient.charClass;
        UserSkillList.Add(new UserSkillData(userID, charClass, skill.Skill_ID));        
    }
    /// <summary>
    /// 스킬 레벨업을 할 때 
    /// </summary>
    /// <param name="skill"></param>
    public void SkillLevelUP(SkillData skill)
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        int index = UserSkillList.FindIndex(x => x.User_ID.Equals(user_ID) && x.Skill_ID.Equals(skill.Skill_ID));
        if (index >= 0)
        {
            UserSkillList[index].Skill_Level += 1;
        }
    }

    #region 스킬 저장
    /// <summary>
    /// DB에 아직 넣지 않고 클라이언트에 임의로 저장해놓은 데이터들을 DB로 저장 (userquestList, userquestOBJList)
    /// <para>(게임 종료 전 또는 일정 시간마다)</para>
    /// </summary>
    public void SaveSkill()
    {
        Debug.Log("스킬 저장.");
        int user_ID = DatabaseManager.Instance.userData.UID;
        CharClass charClass = UserStatManager.Instance.userStatClient.charClass;

        var differences = Extensions.GetDifferences(
            UserSkillOrigin,
            UserSkillList,
            (original, updated) => original.Skill_ID == updated.Skill_ID,
            (original, updated) => original.Skill_Level != updated.Skill_Level
        );        
        foreach (var addedSkill in differences.Added)
        {
            string query =
            $"INSERT INTO userskills (userskills.User_ID, userskills.Job_ID, userskills.Skill_ID)\n" +
            $"VALUES ({user_ID}, {(int)charClass}, {addedSkill.Skill_ID});";
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
        foreach (var skill in UserSkillList)
        {
            string query =
                $"UPDATE userskills\n" +
                $"SET userskills.Skill_Level={skill.Skill_Level}\n" +
                $"WHERE userskills.User_ID={user_ID} " +
                $"AND userskills.Job_ID={(int)charClass} " +
                $"AND userskills.Skill_ID={skill.Skill_ID};";            
            _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
        }
    }
    private void SaveSkillKeyBind()
    {
        int user_ID = DatabaseManager.Instance.userData.UID;
        string skill_1 = PlayerSkill.EquipSkills[0] != 0 ? PlayerSkill.EquipSkills[0].ToString() : "NULL";
        string skill_2 = PlayerSkill.EquipSkills[1] != 0 ? PlayerSkill.EquipSkills[1].ToString() : "NULL";
        string skill_3 = PlayerSkill.EquipSkills[2] != 0 ? PlayerSkill.EquipSkills[2].ToString() : "NULL";
        string skill_4 = PlayerSkill.EquipSkills[3] != 0 ? PlayerSkill.EquipSkills[3].ToString() : "NULL";
        string query =
            $"UPDATE userskillkeybinds\n" +
            $"SET userskillkeybinds.Skill_ID_1={skill_1}," +
            $"userskillkeybinds.Skill_ID_2={skill_2}," +
            $"userskillkeybinds.Skill_ID_3={skill_3}," +
            $"userskillkeybinds.Skill_ID_4={skill_4}\n" +
            $"WHERE userskillkeybinds.User_ID={user_ID};";
        _ = DatabaseManager.Instance.OnInsertOrUpdateRequest(query);
    }
    private void AutoSave()
    {
        SaveSkill();
        SaveSkillKeyBind();
    }
    private void OnApplicationQuit()
    {
        SaveSkill();
        SaveSkillKeyBind();
    }
    #endregion
}
