using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static PlayerInputAction inputActions;

    public List<SkillData> userSkillDataList = new List<SkillData>();

    public GameObject goblin;
    public GameObject mummy;
    public GameObject Item;
    private void Awake()
    {
        Instance = this;
        inputActions = new();
    }
    private void OnEnable()
    {
        Debug.Log(inputActions == null);
        inputActions.PlayerActions.Enable();
    }
    private void OnDisable()
    {
        
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
        // TODO : 스킬 쿼리 불러오기 수정, GameManager와 DatabaseManager 호출 순서 파악하기

        string query =
            $"SELECT skills.Skill_ID, skills.Skill_Name, userskills.Skill_Level, skills.Damage, skills.Mana_Cost, skills.Cooltime, skills.Unlock_Level, skills.Skill_Order, skills.Skill_Description, skills.Icon_Name" +
            $"FROM UserSkills" +
            $"JOIN Skills ON UserSkills.Skill_ID = Skills.Skill_ID" +
            $"WHERE UserSkills.User_ID = 1 AND UserSkills.Job_ID = 1;";

        // TODO : 직업 추가 시, 수정
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest(query);
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
