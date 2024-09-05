using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    public List<SkillData> userSkillDataList = new List<SkillData>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //string query =

        GetSkillFromDatabaseData();        
    }

    #region Database
    /// <summary>
    /// 데이터베이스에서 스킬 데이터 가져오기
    /// </summary>
    private void GetSkillFromDatabaseData()
    {
        //int userId = DatabaseManager.Instance.userData.UID;

        string query =
            $"SELECT skills.Skill_ID, skills.Skill_Name, userskills.Skill_Level, skills.Damage, skills.Mana_Cost, skills.Cooltime, skills.Unlock_Level, skills.Skill_Order, skills.Description, skills.Icon_Name\n" +
            $"FROM UserSkills\n" +
            $"JOIN Skills ON UserSkills.Skill_ID = Skills.Skill_ID\n" +
            $"WHERE UserSkills.User_ID = {1 /* TODO : 임시 */} AND UserSkills.Job_ID = 1;";

        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
        
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;
        if (isGetData)
        {
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                SkillData data = new SkillData(row);
                userSkillDataList.Add(data);
                //skillTable.Add(data.Skill_ID - 1, data.Skill_Name);
            }
        }
        else
        {
            //  실패
        }
    }
    #endregion
}
