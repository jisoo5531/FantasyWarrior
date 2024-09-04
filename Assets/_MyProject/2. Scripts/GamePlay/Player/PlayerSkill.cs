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
    [HideInInspector] public static List<int> equipSkills;
    /// <summary>
    /// 데이터베이스 where 쿼리문
    /// </summary>
    protected Dictionary<string, object> whereQuery;

    public List<SkillData> skillDataList = new List<SkillData>();

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
            5,6,7,8
            //8
        };
    }
    private void Start()
    {        
        playerAnimation = GetComponent<PlayerAnimation>();        
        if (GameManager.Instance.userSkillDataList != null)
        {
            foreach (SkillData skillData in GameManager.Instance.userSkillDataList)
            {
                skillTable.Add(skillData.Skill_ID - 1, skillData.Skill_Name);
            }
        }
    }

    

    #region Input System
    private void OnSkill_1(InputAction.CallbackContext context)
    {
        currentSkillNum = equipSkills[0];
        playerAnimation.SkillAnimation($"{skillTable[equipSkills[0]]}");
        Debug.Log(currentSkillNum);
    }
    private void OnSkill_2(InputAction.CallbackContext context)
    {
        currentSkillNum = equipSkills[1];
        playerAnimation.SkillAnimation($"{skillTable[equipSkills[1]]}");
        Debug.Log(currentSkillNum);
    }
    private void OnSkill_3(InputAction.CallbackContext context)
    {
        currentSkillNum = equipSkills[2];
        playerAnimation.SkillAnimation($"{skillTable[equipSkills[2]]}");
        Debug.Log(currentSkillNum);
    }
    private void OnSkill_4(InputAction.CallbackContext context)
    {
        currentSkillNum = equipSkills[3];
        playerAnimation.SkillAnimation($"{skillTable[equipSkills[3]]}");
        Debug.Log(currentSkillNum);
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

