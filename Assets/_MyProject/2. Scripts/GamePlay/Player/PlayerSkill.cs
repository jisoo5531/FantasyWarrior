using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{    
    /// <summary>
    /// ��ü ��ų ����
    /// </summary>
    public Dictionary<int, string> skillTable;
    /// <summary>
    /// ���� �����ϰ� �ִ� ��ų
    /// </summary>
    [HideInInspector] public List<int> equipSkills;
    /// <summary>
    /// �����ͺ��̽� where ������
    /// </summary>
    protected Dictionary<string, object> whereQuery;

    protected List<SkillData> skillDataList;

    private void Awake()
    {
        Initialize();
    }    

    protected virtual void Initialize()
    {
        equipSkills = new List<int>
        {
            0, 1, 2, 3
        };
    }
    private void Start()
    {
        GetSkillFromDatabaseData();
    }

    public virtual void SKill_Play(int skillNum)
    {
        
    }    
    public virtual void SkillChagne()
    {
        // TODO : ��ų ��ü
        //equipSkills.RemoveAt(1);
        //equipSkills.Insert(1, 5)
    }
    /// <summary>
    /// �����ͺ��̽����� ��ų ������ ��������
    /// </summary>
    protected virtual void GetSkillFromDatabaseData()
    {
        DataSet dataSet = DatabaseManager.Instance.OnSelectRequest("skills", whereQuery);

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
            //  ����
        }
    }
}

#region ��ų �����ͺ��̽�
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