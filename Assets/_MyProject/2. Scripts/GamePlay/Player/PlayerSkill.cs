using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkill : MonoBehaviour
{    
    /// <summary>
    /// 전체 스킬 정보
    /// </summary>
    public Dictionary<int, string> skillTable = new Dictionary<int, string>();
    /// <summary>
    /// 현재 장착하고 있는 스킬
    /// </summary>
    [HideInInspector] public List<int> equipSkills;
    /// <summary>
    /// 데이터베이스 where 쿼리문
    /// </summary>
    protected Dictionary<string, object> whereQuery;

    protected List<SkillData> skillDataList = new List<SkillData>();

    protected int currentSkillNum;
    private PlayerAnimation playerAnimation;

    private void OnEnable()
    {
        PlayerController.inputActions.PlayerActions.Skill_1.performed += OnSkill_1;
        PlayerController.inputActions.PlayerActions.Skill_2.performed += OnSkill_2;
        PlayerController.inputActions.PlayerActions.Skill_3.performed += OnSkill_3;
        PlayerController.inputActions.PlayerActions.Skill_4.performed += OnSkill_4;
    }
    private void OnDisable()
    {
        PlayerController.inputActions.PlayerActions.Skill_1.performed -= OnSkill_1;
        PlayerController.inputActions.PlayerActions.Skill_2.performed -= OnSkill_2;
        PlayerController.inputActions.PlayerActions.Skill_3.performed -= OnSkill_3;
        PlayerController.inputActions.PlayerActions.Skill_4.performed -= OnSkill_4;
    }

    private void Awake()
    {
        Initialize();
    }        

    protected virtual void Initialize()
    {
        equipSkills = new List<int>
        {
            //0, 1, 2, 3
            4,5,6,7
            //8
        };
    }
    private void Start()
    {
        GetSkillFromDatabaseData();
        playerAnimation = GetComponent<PlayerAnimation>();
    }

    #region Database
    /// <summary>
    /// 데이터베이스에서 스킬 데이터 가져오기
    /// </summary>
    protected virtual void GetSkillFromDatabaseData()
    {
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest("skills");        

        bool isGetData = dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0;

        if (isGetData)
        {                        
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {                
                SkillData data = new SkillData(row);                
                skillDataList.Add(data);
                skillTable.Add(data.Skill_ID - 1, data.Skill_Name);
            }                        
        }
        else
        {
            //  실패
        }
    }
    #endregion

    #region Input System
    private void OnSkill_1(InputAction.CallbackContext context)
    {
        currentSkillNum = equipSkills[0];
        playerAnimation.SkillAnimation($"{skillTable[equipSkills[0]]}");        
    }
    private void OnSkill_2(InputAction.CallbackContext context)
    {
        currentSkillNum = equipSkills[1];
        playerAnimation.SkillAnimation($"{skillTable[equipSkills[1]]}");
    }
    private void OnSkill_3(InputAction.CallbackContext context)
    {
        currentSkillNum = equipSkills[2];
        playerAnimation.SkillAnimation($"{skillTable[equipSkills[2]]}");
    }
    private void OnSkill_4(InputAction.CallbackContext context)
    {
        currentSkillNum = equipSkills[3];
        playerAnimation.SkillAnimation($"{skillTable[equipSkills[3]]}");
    }
    #endregion

    #region TODO List
    public virtual void SkillChagne()
    {
        // TODO : 스킬 교체
        //equipSkills.RemoveAt(1);
        //equipSkills.Insert(1, 5)
    }
    #endregion
}

#region 스킬 데이터베이스
public enum Skill_Type
{
    Attack,
    Deffense,
    Buff,
    Debuff
}
public enum Status_Effect
{
    None,
    Slow,
    Stun
}

public class SkillData
{
    public int Skill_ID { get; set; }
    public string Skill_Name { get; set; }
    public int Level { get; set; }
    public Skill_Type SkillType { get; set; }
    public int Damage { get; set; }
    public int Mana_Cost { get; set; }
    public float CoolTime { get; set; }
    public Status_Effect Status_Effect { get; set; }
    public int Unlock_Level { get; set; }
    public string Skill_Desc { get; set; }

    public SkillData(DataRow row) : this
        (
            int.Parse(row["skill_id"].ToString()),
            row["skill_name"].ToString(),
            int.Parse(row["level"].ToString()),
            (Skill_Type)int.Parse(row["skill_type"].ToString()),
            int.Parse(row["damage"].ToString()),
            int.Parse(row["mana_cost"].ToString()),
            float.Parse(row["cooltime"].ToString()),
            (Status_Effect)int.Parse(row["status_effects"].ToString()),
            int.Parse(row["unlock_level"].ToString()),
            row["DESCRIPTION"].ToString()
        )
    { }

    public SkillData(int skill_ID, string skill_Name, int level, Skill_Type skillType, int damage, int mana_Cost, float coolTime, Status_Effect status_Effect, int unlock_Level, string skill_Desc)
    {
        this.Skill_ID = skill_ID;
        this.Skill_Name = skill_Name;
        this.Level = level;
        this.SkillType = skillType;
        this.Damage = damage;
        this.Mana_Cost = mana_Cost;
        this.CoolTime = coolTime;
        this.Status_Effect = status_Effect;
        this.Unlock_Level = unlock_Level;
        this.Skill_Desc = skill_Desc;
    }
}
#endregion