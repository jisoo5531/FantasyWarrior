using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class SkillPanel : MonoBehaviour
{
    public GameObject skillWindowContent;
    public GameObject skillInfo;
    public List<SkillData> skillDataList = new List<SkillData>();

    private string tableName = "warrior_skills";

    private void Start()
    {
        GetSkillFromDatabaseData();
        InitSkillWindow();
    }
    private void InitSkillWindow()
    {        
        foreach (SkillData skillData in skillDataList)
        {
            GameObject skillInfoObj = Instantiate(skillInfo, skillWindowContent.transform);
            SkillEntry skillEntry = skillInfoObj.GetComponent<SkillEntry>();            
            
            // TODO : 직업 추가 시, tableName 바꾸기            
            skillEntry.Initialize(skillData, this.tableName);
        }
    }
    #region Database
    /// <summary>
    /// 데이터베이스에서 스킬 데이터 가져오기
    /// </summary>
    private void GetSkillFromDatabaseData()
    {
        Dictionary<string, object> whereQuery = new Dictionary<string, object>
        {
            { "char_class", 1 }
        };
        // TODO : 직업 추가 시, 수정
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest("skills", whereQuery);
        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;
        if (isGetData)
        {
            Debug.Log(dataSet.Tables[0].Rows.Count);
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                SkillData data = new SkillData(row);                
                skillDataList.Add(data);
            }
        }
        else
        {
            //  실패
        }
    }
    #endregion
}
