using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : MonoBehaviour
{
    [HideInInspector] public Animator anim;
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
        equipSkills = PlayerSkill.EquipSkills;
    }

    private void OnEnable()
    {        
        GameManager.inputActions.PlayerActions.Move.performed += MoveAnimation;
        GameManager.inputActions.PlayerActions.Move.canceled += MoveAnimation;
        GameManager.inputActions.PlayerActions.Run.performed += OnRunAction;
        GameManager.inputActions.PlayerActions.Run.canceled += OnRunAction;

        GameManager.inputActions.PlayerActions.Attack.performed += AttackAnimation;


    }
    private void OnDisable()
    {
        GameManager.inputActions.PlayerActions.Move.performed -= MoveAnimation;
        GameManager.inputActions.PlayerActions.Move.canceled -= MoveAnimation;
        GameManager.inputActions.PlayerActions.Run.performed -= OnRunAction;
        GameManager.inputActions.PlayerActions.Run.canceled -= OnRunAction;

        GameManager.inputActions.PlayerActions.Attack.performed -= AttackAnimation;
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
