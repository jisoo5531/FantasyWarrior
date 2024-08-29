using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[
    RequireComponent(typeof(CharacterController)), 
    RequireComponent(typeof(PlayerInput)), 
    RequireComponent(typeof(PlayerMovement)), 
    RequireComponent(typeof(PlayerAnimation))
]
public class PlayerController : MonoBehaviour
{
    protected CharacterController controller;
    protected PlayerInput playerInput;
    protected PlayerMovement playerMovement;
    protected PlayerAnimation playerAnimation;
    protected UIComponent playerUI;
    protected PlayerSkill playerSkill;
    
    protected Damagable damagable;
    protected Attackable attackable;    

    private void Awake()
    {
        controller = GetComponent<CharacterController>();        
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();        
        playerUI = GetComponentInChildren<UIComponent>();

        damagable = gameObject.AddComponent<Damagable>();
        attackable = gameObject.AddComponent<Attackable>();

        PlayerInit();
    }
    protected virtual void PlayerInit()
    {

    }
    private void Start()
    {        
        damagable.Initialize(maxHp: 1000, hp: 1000);
        attackable.Initialize(damage: 10, range: 2);

        playerUI?.Initialize(damagable);        
        damagable.OnDeath += () => { Debug.Log("�÷��̾� �׾���."); };        
    }        

    private void OnEnable()
    {
        playerInput.OnAttack += playerAnimation.AttackAnimation;

        damagable.OnHpChange += OnHpChange;
        damagable.OnDeath += OnDeath;        
    }

    private void Update()
    {
        playerInput?.Keyinput();
        
        float moveValue = Mathf.Max(Mathf.Abs(playerInput.Vertical), Mathf.Abs(playerInput.Horizontal));
        playerAnimation?.MoveAnimation(moveValue, playerInput.IsRun);
        playerAnimation?.SkillAnimation(playerInput.IsSkills, playerSkill.skillTable);
    }
    
    private void FixedUpdate()
    {
        playerMovement?.Move(transform, controller, playerInput.Horizontal, playerInput.Vertical, playerInput.IsRun);
    }    

    private void OnDisable()
    {        
        playerInput.OnAttack -= playerAnimation.AttackAnimation;

        damagable.OnHpChange -= OnHpChange;
        damagable.OnDeath -= OnDeath;        
    }
    //private void OnDestroy()
    //{
    //    playerInput.OnAttack -= playerAnimation.AttackAnimation;
    //    EventHandler.actionEvent.UnRegisterHpChange(OnHpChange);
    //    EventHandler.actionEvent.UnRegisterDeath(OnDeath);
    //}
    private void OnHpChange(int damage)
    {
        // TODO : ���ָ��� ���� ������ �ٸ��Բ� (��? ����)
        damagable.Hp -= damage;
        Debug.Log($"������ ���� {damage} ��ŭ");
    }
    private void OnDeath()
    {
        playerAnimation.DeathAnimation();

        Destroy(gameObject, 3f);
    }
}