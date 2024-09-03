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
        equipSkills = PlayerSkill.equipSkills;
    }

    private void OnEnable()
    {
        PlayerController.inputActions.PlayerActions.Move.performed += MoveAnimation;
        PlayerController.inputActions.PlayerActions.Move.canceled += MoveAnimation;
        PlayerController.inputActions.PlayerActions.Run.performed += OnRunAction;
        PlayerController.inputActions.PlayerActions.Run.canceled += OnRunAction;

        PlayerController.inputActions.PlayerActions.Attack.performed += AttackAnimation;


    }
    private void OnDisable()
    {
        PlayerController.inputActions.PlayerActions.Move.performed -= MoveAnimation;
        PlayerController.inputActions.PlayerActions.Move.canceled -= MoveAnimation;
        PlayerController.inputActions.PlayerActions.Run.performed -= OnRunAction;
        PlayerController.inputActions.PlayerActions.Run.canceled -= OnRunAction;

        PlayerController.inputActions.PlayerActions.Attack.performed -= AttackAnimation;
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
    public void SkillAnimation(string skillName)
    {
        anim.SetTrigger(skillName);
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
