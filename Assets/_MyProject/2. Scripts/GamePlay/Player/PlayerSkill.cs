using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkill : MonoBehaviour
{
    /// <summary>
    /// ���� �����ϰ� �ִ� ��ų
    /// </summary>
    [HideInInspector] public static List<int> EquipSkills { get; private set; }
    /// <summary>
    /// ��ü ��ų ����
    /// </summary>
    public Dictionary<int, string> skillTable = new Dictionary<int, string>();
    
    /// <summary>
    /// �����ͺ��̽� where ������
    /// </summary>
    protected Dictionary<string, object> whereQuery;

    public List<SkillData> skillDataList = new List<SkillData>();

    protected int currentSkillNum;
    private PlayerAnimation playerAnimation;

    private void OnEnable()
    {
        Debug.Log(GameManager.inputActions == null);
        GameManager.inputActions.PlayerActions.Skill_1.performed += OnSkill_1;
        GameManager.inputActions.PlayerActions.Skill_2.performed += OnSkill_2;
        GameManager.inputActions.PlayerActions.Skill_3.performed += OnSkill_3;
        GameManager.inputActions.PlayerActions.Skill_4.performed += OnSkill_4;
    }
    private void OnDisable()
    {
        GameManager.inputActions.PlayerActions.Skill_1.performed -= OnSkill_1;
        GameManager.inputActions.PlayerActions.Skill_2.performed -= OnSkill_2;
        GameManager.inputActions.PlayerActions.Skill_3.performed -= OnSkill_3;
        GameManager.inputActions.PlayerActions.Skill_4.performed -= OnSkill_4;
    }

    private void Awake()
    {
        Initialize();
    }        

    protected virtual void Initialize()
    {
        EquipSkills = new List<int>
        {
            //1,2,3,4
            5,6,7,8
            //9
        };
    }
    private void Start()
    {        
        playerAnimation = GetComponent<PlayerAnimation>();
        
        if (GameManager.Instance.userSkillDataList != null)
        {
            for (int i = 0; i < GameManager.Instance.userSkillDataList.Count; i++)
            {
                SkillData skillData = GameManager.Instance.userSkillDataList[i];
                skillTable.Add(i + 1, skillData.Skill_Name);
            }            
        }
    }

    

    #region Input System
    private void OnSkill_1(InputAction.CallbackContext context)
    {
        currentSkillNum = EquipSkills[0];
        playerAnimation.SkillAnimation($"{skillTable[EquipSkills[0]]}");
        Debug.Log($"���� ��ų ��ȣ : {currentSkillNum}");
    }
    private void OnSkill_2(InputAction.CallbackContext context)
    {
        currentSkillNum = EquipSkills[1];
        playerAnimation.SkillAnimation($"{skillTable[EquipSkills[1]]}");
        Debug.Log($"���� ��ų ��ȣ : {currentSkillNum}");
    }
    private void OnSkill_3(InputAction.CallbackContext context)
    {
        currentSkillNum = EquipSkills[2];
        playerAnimation.SkillAnimation($"{skillTable[EquipSkills[2]]}");
        Debug.Log($"���� ��ų ��ȣ : {currentSkillNum}");
    }
    private void OnSkill_4(InputAction.CallbackContext context)
    {
        currentSkillNum = EquipSkills[3];
        playerAnimation.SkillAnimation($"{skillTable[EquipSkills[3]]}");
        Debug.Log($"���� ��ų ��ȣ : {currentSkillNum}");
    }
    #endregion

    #region TODO List
    public virtual void SkillChagne()
    {
        // TODO : ��ų ��ü
        //equipSkills.RemoveAt(1);
        //equipSkills.Insert(1, 5)
    }
    #endregion
}

