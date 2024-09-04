using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class KeySetIcon
{    
    public Image ActionBarIcon;
    public Image KeyIcon;
}
public class UI_SkillPanel : MonoBehaviour
{
    [Header("스킬")]
    public GameObject skillWindowContent;
    public GameObject skillInfo;
    public GameObject keySetPanel;
    public List<KeySetIcon> iconPanelList;
    public List<SkillData> skillDataList = new List<SkillData>();

    private string tableName = "warrior_skills";

    private void Awake()
    {
        for (int i = 0; i < iconPanelList.Count; i++)
        {
            // TODO : 스킬 데이터 Manager
        }
        
    }

    private void Start()
    {
        //GetSkillFromDatabaseData();
        InitSkillWindow();
    }
    private void InitSkillWindow()
    {
        if (GameManager.Instance.userSkillDataList != null)
        {
            foreach (SkillData skillData in GameManager.Instance.userSkillDataList)
            {
                GameObject skillInfoObj = Instantiate(skillInfo, skillWindowContent.transform);
                UI_SkillEntry skillEntry = skillInfoObj.GetComponent<UI_SkillEntry>();

                // TODO : 직업 추가 시, tableName 바꾸기            
                skillEntry.Initialize(skillData, this.tableName, keySetPanel);
            }
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
