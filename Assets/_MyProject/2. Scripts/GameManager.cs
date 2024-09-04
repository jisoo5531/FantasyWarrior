using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public List<SkillData> userSkillDataList = new List<SkillData>();

    public GameObject goblin;
    public GameObject mummy;
    public GameObject Item;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GetSkillFromDatabaseData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Instantiate(goblin, new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10)), goblin.transform.rotation);
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            Instantiate(mummy, new Vector3(Random.Range(0, 10), 0, Random.Range(0, 10)), mummy.transform.rotation);
        }
    }


    #region Database
    /// <summary>
    /// 데이터베이스에서 스킬 데이터 가져오기
    /// </summary>
    private void GetSkillFromDatabaseData()
    {
        // TODO : 직업 추가 시, 수정
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest("warrior_skills");
        //DataSet dataSet = DatabaseManager.Instance.OnSelectRequest("archer_skills");
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
