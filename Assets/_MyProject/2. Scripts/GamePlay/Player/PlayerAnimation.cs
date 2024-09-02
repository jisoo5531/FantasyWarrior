using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;
    private bool isRun = false;
    public Dictionary<int, string> skillTable;
    public List<int> equipSkills;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        PlayerSkill skill = GetComponent<PlayerSkill>();
        skillTable = skill.skillTable;
        equipSkills = skill.equipSkills;
    }

    private void OnEnable()
    {
        PlayerController.inputActions.PlayerActions.Move.performed += MoveAnimation;
        PlayerController.inputActions.PlayerActions.Move.canceled += MoveAnimation;
        PlayerController.inputActions.PlayerActions.Run.performed += OnRunAction;
        PlayerController.inputActions.PlayerActions.Run.canceled += OnRunAction;

        PlayerController.inputActions.PlayerActions.Attack.performed += AttackAnimation;

        PlayerController.inputActions.PlayerActions.Skill_1.performed += OnSkill_1;
        PlayerController.inputActions.PlayerActions.Skill_2.performed += OnSkill_2;
        PlayerController.inputActions.PlayerActions.Skill_3.performed += OnSkill_3;
        PlayerController.inputActions.PlayerActions.Skill_4.performed += OnSkill_4;

    }
    private void OnDisable()
    {
        PlayerController.inputActions.PlayerActions.Move.performed -= MoveAnimation;
        PlayerController.inputActions.PlayerActions.Move.canceled -= MoveAnimation;
        PlayerController.inputActions.PlayerActions.Run.performed -= OnRunAction;
        PlayerController.inputActions.PlayerActions.Run.canceled -= OnRunAction;

        PlayerController.inputActions.PlayerActions.Attack.performed -= AttackAnimation;

        PlayerController.inputActions.PlayerActions.Skill_1.performed -= OnSkill_1;
        PlayerController.inputActions.PlayerActions.Skill_2.performed -= OnSkill_2;
        PlayerController.inputActions.PlayerActions.Skill_3.performed -= OnSkill_3;
        PlayerController.inputActions.PlayerActions.Skill_4.performed -= OnSkill_4;
    }

    #region Move
    public void MoveAnimation(InputAction.CallbackContext context)
    {
        Vector2 move = context.ReadValue<Vector2>();
        float speed = Mathf.Max(Mathf.Abs(move.x), Mathf.Abs(move.y));
        speed = isRun ? speed * 2 : speed;        
        anim.SetFloat("Speed", speed);
    }
    public void OnRunAction(InputAction.CallbackContext context)
    {
        isRun = context.ReadValueAsButton();
    }
    #endregion

    #region Skill
    public void OnSkill_1(InputAction.CallbackContext context)
    {
        anim.SetTrigger($"{skillTable[equipSkills[0]]}");
    }
    public void OnSkill_2(InputAction.CallbackContext context)
    {
        anim.SetTrigger($"{skillTable[equipSkills[1]]}");
    }
    public void OnSkill_3(InputAction.CallbackContext context)
    {
        anim.SetTrigger($"{skillTable[equipSkills[2]]}");
    }
    public void OnSkill_4(InputAction.CallbackContext context)
    {
        anim.SetTrigger($"{skillTable[equipSkills[3]]}");
    }
    #endregion

    #region Attack
    public void AttackAnimation(InputAction.CallbackContext context)
    {        
        anim.SetTrigger("Attack");
    }
    #endregion
    public void DeathAnimation()
    {
        anim.SetTrigger("Death");
    }
}
