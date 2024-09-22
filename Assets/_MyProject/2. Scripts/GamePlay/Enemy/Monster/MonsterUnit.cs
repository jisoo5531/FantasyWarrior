using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(UnitAnimation))]
public class MonsterUnit : Enemy
{
    // TODO : ���͸� ������ ����(����ġ, ��ȭ, ������) ���� ��´�.    
    protected MonsterData monsterData;

    public MonsterStateMachine M_StateMachine;
    public PlayerController player;
    public UnitAnimation unitAnim;    
    public NavMeshAgent nav;

    [HideInInspector] public Damagable damagable;
    [HideInInspector] public Attackable attackable;
    [HideInInspector] public Followable followable;

    protected UIComponent monsterUI;    

    [Tooltip("��� ������ ����� Ž���Ÿ���")]
    public int detectionRange = 10;

    protected int monsterID;

    private void Awake()
    {        
        player = FindObjectOfType<PlayerController>();
        attackable = gameObject.AddComponent<Attackable>();
        damagable = gameObject.AddComponent<Damagable>();
        followable = gameObject.AddComponent<Followable>();

        monsterUI = GetComponentInChildren<UIComponent>();
        unitAnim = GetComponent<UnitAnimation>();
        nav = GetComponent<NavMeshAgent>();        
    }

    protected virtual void Initialize()
    {        
        this.monsterData = MonsterManager.Instance.GetMonsterData(monsterID);
        Debug.Log(monsterData.MonsterName);
        damagable.Initialize(unitID: monsterData.MonsterID, maxHp: monsterData.MaxHp, hp: monsterData.Hp);
        attackable.Initialize(damage: monsterData.Damage, range: monsterData.AttackRange);
        followable.Initialize(moveSpeed: monsterData.MoveSpeed);
        nav.speed = followable.MoveSpeed;

        monsterUI.Initialize(damagable);
        M_StateMachine = new MonsterStateMachine(this);
        M_StateMachine.Initialize(M_StateMachine.idleState);
    }
    private void Update()
    {
        if (damagable.isStunned)
        {
            Debug.Log("�� �����̴�. �� ������");
            M_StateMachine.StateTransition(M_StateMachine.idleState);
            return;
        }
        M_StateMachine.Excute();
    }
    private void LateUpdate()
    {
        followable.CalculateDistance(transform.position, player.transform.position);
    }    
    private void OnEnable()
    {
        Initialize();
        damagable.OnHpChange += OnHpChange;
        damagable.OnDeath += OnDeath;    
    }
    private void OnDisable()
    {        
        damagable.OnHpChange -= OnHpChange;
        damagable.OnDeath -= OnDeath;
    }
    //private void OnDestroy()
    //{
    //    Debug.Log("���� �ı�");
    //    EventHandler.actionEvent.UnRegisterHpChange(OnHpChange);
    //    EventHandler.actionEvent.UnRegisterDeath(OnDeath);
    //}
    
    protected virtual void OnHpChange(int damage)
    {
        // TODO : ���ָ��� ���� ������ �ٸ��Բ� (��? ����)
        damagable.Hp -= damage;        
    }
    protected virtual void OnDeath()
    {        
        unitAnim.DeathAnimPlay();
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 3f);
    }
}