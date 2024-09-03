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
            
            // TODO : ���� �߰� ��, tableName �ٲٱ�            
            skillEntry.Initialize(skillData, this.tableName);
        }
    }
    #region Database
    /// <summary>
    /// �����ͺ��̽����� ��ų ������ ��������
    /// </summary>
    private void GetSkillFromDatabaseData()
    {
        Dictionary<string, object> whereQuery = new Dictionary<string, object>
        {
            { "char_class", 1 }
        };
        // TODO : ���� �߰� ��, ����
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
            //  ����
        }
    }
    #endregion
}
